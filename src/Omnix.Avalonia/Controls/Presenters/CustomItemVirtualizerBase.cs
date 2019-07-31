// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Utils;
using Avalonia.Input;
using Omnix.Base.Extensions;

namespace Omnix.Avalonia.Controls.Presenters
{
    /// <summary>
    /// Base class for classes which handle virtualization for an <see cref="ItemsPresenter"/>.
    /// </summary>
    public abstract class CustomItemVirtualizerBase : IVirtualizingController, IDisposable
    {
        private double _crossAxisOffset;
        private IDisposable _subscriptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomItemVirtualizerBase"/> class.
        /// </summary>
        /// <param name="owner"></param>
        public CustomItemVirtualizerBase(CustomItemsPresenter owner)
        {
            this.Owner = owner;
            this.Items = owner.Items;
            this.ItemCount = owner.Items.Count();

            var panel = this.VirtualizingPanel;

            if (panel != null)
            {
                _subscriptions = panel.GetObservable(Panel.BoundsProperty)
                    .Skip(1)
                    .Subscribe(_ => this.InvalidateScroll());
            }
        }

        /// <summary>
        /// Gets the <see cref="ItemsPresenter"/> which owns the virtualizer.
        /// </summary>
        public CustomItemsPresenter Owner { get; }

        /// <summary>
        /// Gets the <see cref="IVirtualizingPanel"/> which will host the items.
        /// </summary>
        public IVirtualizingPanel VirtualizingPanel => this.Owner.Panel as IVirtualizingPanel;

        /// <summary>
        /// Gets the items to display.
        /// </summary>
        public IEnumerable Items { get; private set; }

        /// <summary>
        /// Gets the number of items in <see cref="Items"/>.
        /// </summary>
        public int ItemCount { get; private set; }

        /// <summary>
        /// Gets or sets the index of the first item displayed in the panel.
        /// </summary>
        public int FirstIndex { get; protected set; }

        /// <summary>
        /// Gets or sets the index of the first item beyond those displayed in the panel.
        /// </summary>
        public int NextIndex { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the items should be scroll horizontally or vertically.
        /// </summary>
        public bool Vertical => this.VirtualizingPanel?.ScrollDirection == Orientation.Vertical;

        /// <summary>
        /// Gets a value indicating whether logical scrolling is enabled.
        /// </summary>
        public abstract bool IsLogicalScrollEnabled { get; }

        /// <summary>
        /// Gets the value of the scroll extent.
        /// </summary>
        public abstract double ExtentValue { get; }

        /// <summary>
        /// Gets or sets the value of the current scroll offset.
        /// </summary>
        public abstract double OffsetValue { get; set; }

        /// <summary>
        /// Gets the value of the scrollable viewport.
        /// </summary>
        public abstract double ViewportValue { get; }

        /// <summary>
        /// Gets the <see cref="ExtentValue"/> as a <see cref="Size"/>.
        /// </summary>
        public Size Extent
        {
            get
            {
                return this.Vertical ?
                    new Size(this.Owner.Panel.DesiredSize.Width, this.ExtentValue) :
                    new Size(this.ExtentValue, this.Owner.Panel.DesiredSize.Height);
            }
        }

        /// <summary>
        /// Gets the <see cref="ViewportValue"/> as a <see cref="Size"/>.
        /// </summary>
        public Size Viewport
        {
            get
            {
                return this.Vertical ?
                    new Size(this.Owner.Panel.Bounds.Width, this.ViewportValue) :
                    new Size(this.ViewportValue, this.Owner.Panel.Bounds.Height);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="OffsetValue"/> as a <see cref="Vector"/>.
        /// </summary>
        public Vector Offset
        {
            get
            {
                return this.Vertical ? new Vector(_crossAxisOffset, this.OffsetValue) : new Vector(this.OffsetValue, _crossAxisOffset);
            }

            set
            {
                var oldCrossAxisOffset = _crossAxisOffset;

                if (this.Vertical)
                {
                    this.OffsetValue = value.Y;
                    _crossAxisOffset = value.X;
                }
                else
                {
                    this.OffsetValue = value.X;
                    _crossAxisOffset = value.Y;
                }

                if (_crossAxisOffset != oldCrossAxisOffset)
                {
                    this.Owner.InvalidateArrange();
                }
            }
        }

        /// <summary>
        /// Creates an <see cref="CustomItemVirtualizerBase"/> based on an item presenter's
        /// <see cref="ItemVirtualizationMode"/>.
        /// </summary>
        /// <param name="owner">The items presenter.</param>
        /// <returns>An <see cref="CustomItemVirtualizerBase"/>.</returns>
        public static CustomItemVirtualizerBase Create(CustomItemsPresenter owner)
        {
            if (owner.Panel == null)
            {
                return null;
            }

            var virtualizingPanel = owner.Panel as IVirtualizingPanel;
            var scrollable = (ILogicalScrollable)owner;
            CustomItemVirtualizerBase result = new CustomItemVirtualizer(owner);

            if (virtualizingPanel != null)
            {
                virtualizingPanel.Controller = result;
            }

            return result;
        }

        /// <summary>
        /// Carries out a measure for the related <see cref="ItemsPresenter"/>.
        /// </summary>
        /// <param name="availableSize">The size available to the control.</param>
        /// <returns>The desired size for the control.</returns>
        public virtual Size MeasureOverride(Size availableSize)
        {
            this.Owner.Panel.Measure(availableSize);
            return this.Owner.Panel.DesiredSize;
        }

        /// <summary>
        /// Carries out an arrange for the related <see cref="ItemsPresenter"/>.
        /// </summary>
        /// <param name="finalSize">The size available to the control.</param>
        /// <returns>The actual size used.</returns>
        public virtual Size ArrangeOverride(Size finalSize)
        {
            if (this.VirtualizingPanel != null)
            {
                this.VirtualizingPanel.CrossAxisOffset = _crossAxisOffset;
                this.Owner.Panel.Arrange(new Rect(finalSize));
            }
            else
            {
                var origin = this.Vertical ? new Point(-_crossAxisOffset, 0) : new Point(0, _crossAxisOffset);
                this.Owner.Panel.Arrange(new Rect(origin, finalSize));
            }

            return finalSize;
        }

        /// <inheritdoc/>
        public virtual void UpdateControls()
        {
        }

        /// <summary>
        /// Gets the next control in the specified direction.
        /// </summary>
        /// <param name="direction">The movement direction.</param>
        /// <param name="from">The control from which movement begins.</param>
        /// <returns>The control.</returns>
        public virtual IControl GetControlInDirection(NavigationDirection direction, IControl from)
        {
            return null;
        }

        /// <summary>
        /// Called when the items for the presenter change, either because
        /// <see cref="ItemsPresenterBase.Items"/> has been set, the items collection has been
        /// modified, or the panel has been created.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="e">A description of the change.</param>
        public virtual void ItemsChanged(IEnumerable items, NotifyCollectionChangedEventArgs e)
        {
            this.Items = items;
            this.ItemCount = items.Count();
        }

        /// <summary>
        /// Scrolls the specified item into view.
        /// </summary>
        /// <param name="item">The item.</param>
        public virtual void ScrollIntoView(object item)
        {
        }

        /// <inheritdoc/>
        public virtual void Dispose()
        {
            _subscriptions?.Dispose();
            _subscriptions = null;

            if (this.VirtualizingPanel != null)
            {
                this.VirtualizingPanel.Controller = null;
                this.VirtualizingPanel.Children.Clear();
            }

            this.Owner.ItemContainerGenerator.Clear();
        }

        /// <summary>
        /// Invalidates the current scroll.
        /// </summary>
        protected void InvalidateScroll() => ((ILogicalScrollable)this.Owner).InvalidateScroll?.Invoke();
    }
}
