//https://github.com/ahopper/Avalonia.IconPacks/blob/master/VirtualizingWrapPanel.cs
// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Omnix.Avalonia.Controls.Primitives;
using static System.Math;

namespace Omnix.Avalonia.Controls
{
    public class VirtualizingWrapPanel : WrapPanel, IVirtualizingPanel, IScrollQuantum
    {
        private Size _availableSpace;
        private double _takenSpace;
        private int _canBeRemoved;
        private double _averageItemSize;
        private int _averageCount;
        private double _pixelOffset;
        private double _crossAxisOffset;
        private bool _forceRemeasure;
        private readonly List<double> _lineLengths = new List<double>();

        private double _takenLineSpace;
        public int ScrollQuantum => this.Orientation == Orientation.Horizontal ? (int)Math.Max(1, _availableSpace.Width / _averageItemSize) : (int)Math.Max(1, _availableSpace.Height / _averageItemSize);

        bool IVirtualizingPanel.IsFull
        {
            get
            {
                return this.Orientation == Orientation.Vertical ?
                    _takenSpace >= _availableSpace.Width :
                    _takenSpace >= _availableSpace.Height;
            }
        }

        public IVirtualizingController Controller { get; set; }

        int IVirtualizingPanel.OverflowCount => _canBeRemoved;
        Orientation IVirtualizingPanel.ScrollDirection => this.Orientation == Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal;
        double IVirtualizingPanel.AverageItemSize => _averageItemSize;

        double IVirtualizingPanel.PixelOverflow
        {
            get
            {
                var bounds = this.Orientation == Orientation.Vertical ?
                    _availableSpace.Width : _availableSpace.Height;
                return Math.Max(0, _takenSpace - bounds);
            }
        }

        double IVirtualizingPanel.PixelOffset
        {
            get { return _pixelOffset; }

            set
            {
                if (_pixelOffset != value)
                {
                    _pixelOffset = value;
                    this.InvalidateArrange();
                }
            }
        }

        double IVirtualizingPanel.CrossAxisOffset
        {
            get { return _crossAxisOffset; }

            set
            {
                if (_crossAxisOffset != value)
                {
                    _crossAxisOffset = value;
                    this.InvalidateArrange();
                }
            }
        }

        void IVirtualizingPanel.ForceInvalidateMeasure()
        {
            this.InvalidateMeasure();
            _forceRemeasure = true;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (availableSize.Width == double.PositiveInfinity)
            {
                availableSize = new Size(5, availableSize.Height);
            }

            if (_forceRemeasure || availableSize != ((ILayoutable)this).PreviousMeasure)
            {
                _forceRemeasure = false;
                _availableSpace = availableSize;
                this.Controller?.UpdateControls();
            }

            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _availableSpace = finalSize;
            _canBeRemoved = 0;
            _takenSpace = 0;
            _takenLineSpace = 0;
            _lineLengths.Clear();
            _averageItemSize = 0;
            _averageCount = 0;
            var result = this.Arrange(finalSize);

            _takenSpace += _pixelOffset;
            this.Controller?.UpdateControls();
            return result;
        }

        protected Size Arrange(Size finalSize)
        {
            double accumulatedV = 0;
            var uvFinalSize = this.CreateUVSize(finalSize);
            var lineSize = this.CreateUVSize();
            int firstChildInLineindex = 0;
            for (int index = 0; index < this.Children.Count; index++)
            {
                var child = this.Children[index];
                var childSize = this.CreateUVSize(child.DesiredSize);
                if (lineSize.U + childSize.U <= uvFinalSize.U) // same line
                {
                    lineSize.U += childSize.U;
                    lineSize.V = Max(lineSize.V, childSize.V);
                    _takenLineSpace += childSize.U;
                }
                else // moving to next line
                {
                    var controlsInLine = this.GetContolsBetween(firstChildInLineindex, index);
                    this.ArrangeLine(accumulatedV, lineSize.V, controlsInLine);
                    accumulatedV += lineSize.V;
                    lineSize = childSize;
                    firstChildInLineindex = index;
                    _takenLineSpace = childSize.U;
                }
            }

            if (firstChildInLineindex < this.Children.Count)
            {
                var controlsInLine = this.GetContolsBetween(firstChildInLineindex, this.Children.Count);
                this.ArrangeLine(accumulatedV, lineSize.V, controlsInLine);
            }
            return finalSize;
        }
        private IEnumerable<IControl> GetContolsBetween(int first, int last)
        {
            return this.Children.Skip(first).Take(last - first);
        }

        private void ArrangeLine(double v, double lineV, IEnumerable<IControl> contols)
        {
            double u = 0;
            bool isHorizontal = (this.Orientation == Orientation.Horizontal);
            foreach (var child in contols)
            {
                var childSize = this.CreateUVSize(child.DesiredSize);
                var x = isHorizontal ? u : v;
                var y = isHorizontal ? v : u;
                var width = isHorizontal ? childSize.U : lineV;
                var height = isHorizontal ? lineV : childSize.U;

                var rect = new Rect(
                   x - _crossAxisOffset,
                   y - _pixelOffset,
                   width,
                   height);
                child.Arrange(rect);

                u += childSize.U;
                this.AddToAverageItemSize(childSize.V);

                if (rect.Bottom >= _takenSpace)
                {
                    _takenSpace = rect.Bottom;
                }
                if (rect.Y >= _availableSpace.Height)
                {
                    ++_canBeRemoved;
                }

            }
            _lineLengths.Add(u);
        }
        protected override void ChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.ChildrenChanged(sender, e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (IControl control in e.NewItems)
                    {
                        this.UpdateAdd(control);
                    }

                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (IControl control in e.OldItems)
                    {
                        this.UpdateRemove(control);
                    }

                    break;
            }
        }


        private void UpdateAdd(IControl child)
        {
            var bounds = this.Bounds;
            var gap = 0;

            child.Measure(_availableSpace);
            //     ++_averageCount;
            var height = child.DesiredSize.Height;
            var width = child.DesiredSize.Width;

            if (this.Orientation == Orientation.Horizontal)
            {
                if (_takenLineSpace + width > _availableSpace.Width)
                {
                    _takenSpace += height + gap;
                    _takenLineSpace = width;
                    _lineLengths.Add(width);
                }
                else
                {
                    _takenLineSpace += width;
                    if (_lineLengths.Count == 0)
                    {
                        _lineLengths.Add(0);
                    }

                    _lineLengths[_lineLengths.Count - 1] += width;
                }
                this.AddToAverageItemSize(height);
            }
            else
            {
                if (_takenLineSpace + height > _availableSpace.Height)
                {
                    _takenSpace += width + gap;
                    _takenLineSpace = height;
                }
                else
                {
                    _takenLineSpace += height;
                }
                this.AddToAverageItemSize(width);
            }
        }

        private void UpdateRemove(IControl child)
        {
            var bounds = this.Bounds;
            var gap = 0;

            var height = child.DesiredSize.Height;
            var width = child.DesiredSize.Width;

            if (this.Orientation == Orientation.Horizontal)
            {
                if (_takenLineSpace - width <= 0)
                {
                    _takenSpace -= height + gap;
                    _lineLengths.RemoveAt(_lineLengths.Count - 1);
                    _takenLineSpace = _lineLengths.Count > 0 ? _lineLengths.Last() : 0;
                }
                else
                {
                    _takenLineSpace -= width;
                }
                this.AddToAverageItemSize(height);
            }
            else
            {
                if (_takenLineSpace - height <= 0)
                {
                    _takenSpace -= width + gap;
                    _takenLineSpace = _availableSpace.Height;
                }
                else
                {
                    _takenLineSpace -= height;
                }
                this.AddToAverageItemSize(width);
            }

            if (_canBeRemoved > 0)
            {
                --_canBeRemoved;
            }
        }

        private void AddToAverageItemSize(double value)
        {
            ++_averageCount;
            _averageItemSize += (value - _averageItemSize) / _averageCount;
        }

        private void RemoveFromAverageItemSize(double value)
        {
            _averageItemSize = ((_averageItemSize * _averageCount) - value) / (_averageCount - 1);
            --_averageCount;
        }

        private UVSize CreateUVSize(Size size) => new UVSize(this.Orientation, size);

        private UVSize CreateUVSize() => new UVSize(this.Orientation);

        /// <summary>
        /// Used to not not write sepearate code for horizontal and vertical orientation.
        /// U is direction in line. (x if orientation is horizontal)
        /// V is direction of lines. (y if orientation is horizonral)
        /// </summary>
        [DebuggerDisplay("U = {U} V = {V}")]
        private struct UVSize
        {
            private readonly Orientation _orientation;

            internal double U;

            internal double V;

            private UVSize(Orientation orientation, double width, double height)
            {
                U = V = 0d;
                _orientation = orientation;
                this.Width = width;
                this.Height = height;
            }

            internal UVSize(Orientation orientation, Size size)
                : this(orientation, size.Width, size.Height)
            {
            }

            internal UVSize(Orientation orientation)
            {
                U = V = 0d;
                _orientation = orientation;
            }

            private double Width
            {
                get { return (_orientation == Orientation.Horizontal ? U : V); }
                set
                {
                    if (_orientation == Orientation.Horizontal)
                    {
                        U = value;
                    }
                    else
                    {
                        V = value;
                    }
                }
            }

            private double Height
            {
                get { return (_orientation == Orientation.Horizontal ? V : U); }
                set
                {
                    if (_orientation == Orientation.Horizontal)
                    {
                        V = value;
                    }
                    else
                    {
                        U = value;
                    }
                }
            }

            public Size ToSize()
            {
                return new Size(this.Width, this.Height);
            }
        }
    }
}
