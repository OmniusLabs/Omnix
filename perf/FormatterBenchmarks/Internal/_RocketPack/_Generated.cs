
#nullable enable

namespace FormatterBenchmarks.Internal
{

    internal sealed partial class RocketPack_BytesElementsList : global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_BytesElementsList>
    {
        public static global::Omnius.Core.RocketPack.IRocketPackObjectFormatter<global::FormatterBenchmarks.Internal.RocketPack_BytesElementsList> Formatter => global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_BytesElementsList>.Formatter;
        public static global::FormatterBenchmarks.Internal.RocketPack_BytesElementsList Empty => global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_BytesElementsList>.Empty;

        static RocketPack_BytesElementsList()
        {
            global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_BytesElementsList>.Formatter = new ___CustomFormatter();
            global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_BytesElementsList>.Empty = new global::FormatterBenchmarks.Internal.RocketPack_BytesElementsList(global::System.Array.Empty<RocketPack_BytesElements>());
        }

        private readonly global::System.Lazy<int> ___hashCode;

        public static readonly int MaxListCount = 1048576;

        public RocketPack_BytesElementsList(RocketPack_BytesElements[] list)
        {
            if (list is null) throw new global::System.ArgumentNullException("list");
            if (list.Length > 1048576) throw new global::System.ArgumentOutOfRangeException("list");
            foreach (var n in list)
            {
                if (n is null) throw new global::System.ArgumentNullException("n");
            }

            this.List = new global::Omnius.Core.Collections.ReadOnlyListSlim<RocketPack_BytesElements>(list);

            ___hashCode = new global::System.Lazy<int>(() =>
            {
                var ___h = new global::System.HashCode();
                foreach (var n in list)
                {
                    if (n != default) ___h.Add(n.GetHashCode());
                }
                return ___h.ToHashCode();
            });
        }

        public global::Omnius.Core.Collections.ReadOnlyListSlim<RocketPack_BytesElements> List { get; }

        public static global::FormatterBenchmarks.Internal.RocketPack_BytesElementsList Import(global::System.Buffers.ReadOnlySequence<byte> sequence, global::Omnius.Core.IBytesPool bytesPool)
        {
            var reader = new global::Omnius.Core.RocketPack.RocketPackObjectReader(sequence, bytesPool);
            return Formatter.Deserialize(ref reader, 0);
        }
        public void Export(global::System.Buffers.IBufferWriter<byte> bufferWriter, global::Omnius.Core.IBytesPool bytesPool)
        {
            var writer = new global::Omnius.Core.RocketPack.RocketPackObjectWriter(bufferWriter, bytesPool);
            Formatter.Serialize(ref writer, this, 0);
        }

        public static bool operator ==(global::FormatterBenchmarks.Internal.RocketPack_BytesElementsList? left, global::FormatterBenchmarks.Internal.RocketPack_BytesElementsList? right)
        {
            return (right is null) ? (left is null) : right.Equals(left);
        }
        public static bool operator !=(global::FormatterBenchmarks.Internal.RocketPack_BytesElementsList? left, global::FormatterBenchmarks.Internal.RocketPack_BytesElementsList? right)
        {
            return !(left == right);
        }
        public override bool Equals(object? other)
        {
            if (!(other is global::FormatterBenchmarks.Internal.RocketPack_BytesElementsList)) return false;
            return this.Equals((global::FormatterBenchmarks.Internal.RocketPack_BytesElementsList)other);
        }
        public bool Equals(global::FormatterBenchmarks.Internal.RocketPack_BytesElementsList? target)
        {
            if (target is null) return false;
            if (object.ReferenceEquals(this, target)) return true;
            if (!global::Omnius.Core.Helpers.CollectionHelper.Equals(this.List, target.List)) return false;

            return true;
        }
        public override int GetHashCode() => ___hashCode.Value;

        private sealed class ___CustomFormatter : global::Omnius.Core.RocketPack.IRocketPackObjectFormatter<global::FormatterBenchmarks.Internal.RocketPack_BytesElementsList>
        {
            public void Serialize(ref global::Omnius.Core.RocketPack.RocketPackObjectWriter w, in global::FormatterBenchmarks.Internal.RocketPack_BytesElementsList value, in int rank)
            {
                if (rank > 256) throw new global::System.FormatException();

                if (value.List.Count != 0)
                {
                    w.Write((uint)1);
                    w.Write((uint)value.List.Count);
                    foreach (var n in value.List)
                    {
                        global::FormatterBenchmarks.Internal.RocketPack_BytesElements.Formatter.Serialize(ref w, n, rank + 1);
                    }
                }
                w.Write((uint)0);
            }

            public global::FormatterBenchmarks.Internal.RocketPack_BytesElementsList Deserialize(ref global::Omnius.Core.RocketPack.RocketPackObjectReader r, in int rank)
            {
                if (rank > 256) throw new global::System.FormatException();

                RocketPack_BytesElements[] p_list = global::System.Array.Empty<RocketPack_BytesElements>();

                for (; ; )
                {
                    uint id = r.GetUInt32();
                    if (id == 0) break;
                    switch (id)
                    {
                        case 1:
                            {
                                var length = r.GetUInt32();
                                p_list = new RocketPack_BytesElements[length];
                                for (int i = 0; i < p_list.Length; i++)
                                {
                                    p_list[i] = global::FormatterBenchmarks.Internal.RocketPack_BytesElements.Formatter.Deserialize(ref r, rank + 1);
                                }
                                break;
                            }
                    }
                }

                return new global::FormatterBenchmarks.Internal.RocketPack_BytesElementsList(p_list);
            }
        }
    }
    internal sealed partial class RocketPack_BytesElements : global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_BytesElements>, global::System.IDisposable
    {
        public static global::Omnius.Core.RocketPack.IRocketPackObjectFormatter<global::FormatterBenchmarks.Internal.RocketPack_BytesElements> Formatter => global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_BytesElements>.Formatter;
        public static global::FormatterBenchmarks.Internal.RocketPack_BytesElements Empty => global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_BytesElements>.Empty;

        static RocketPack_BytesElements()
        {
            global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_BytesElements>.Formatter = new ___CustomFormatter();
            global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_BytesElements>.Empty = new global::FormatterBenchmarks.Internal.RocketPack_BytesElements(global::Omnius.Core.MemoryOwner<byte>.Empty, global::Omnius.Core.MemoryOwner<byte>.Empty, global::Omnius.Core.MemoryOwner<byte>.Empty, global::Omnius.Core.MemoryOwner<byte>.Empty, global::Omnius.Core.MemoryOwner<byte>.Empty, global::Omnius.Core.MemoryOwner<byte>.Empty, global::Omnius.Core.MemoryOwner<byte>.Empty, global::Omnius.Core.MemoryOwner<byte>.Empty, global::Omnius.Core.MemoryOwner<byte>.Empty, global::Omnius.Core.MemoryOwner<byte>.Empty);
        }

        private readonly global::System.Lazy<int> ___hashCode;

        public static readonly int MaxX0Length = 1048576;
        public static readonly int MaxX1Length = 1048576;
        public static readonly int MaxX2Length = 1048576;
        public static readonly int MaxX3Length = 1048576;
        public static readonly int MaxX4Length = 1048576;
        public static readonly int MaxX5Length = 1048576;
        public static readonly int MaxX6Length = 1048576;
        public static readonly int MaxX7Length = 1048576;
        public static readonly int MaxX8Length = 1048576;
        public static readonly int MaxX9Length = 1048576;

        public RocketPack_BytesElements(global::System.Buffers.IMemoryOwner<byte> x0, global::System.Buffers.IMemoryOwner<byte> x1, global::System.Buffers.IMemoryOwner<byte> x2, global::System.Buffers.IMemoryOwner<byte> x3, global::System.Buffers.IMemoryOwner<byte> x4, global::System.Buffers.IMemoryOwner<byte> x5, global::System.Buffers.IMemoryOwner<byte> x6, global::System.Buffers.IMemoryOwner<byte> x7, global::System.Buffers.IMemoryOwner<byte> x8, global::System.Buffers.IMemoryOwner<byte> x9)
        {
            if (x0 is null) throw new global::System.ArgumentNullException("x0");
            if (x0.Memory.Length > 1048576) throw new global::System.ArgumentOutOfRangeException("x0");
            if (x1 is null) throw new global::System.ArgumentNullException("x1");
            if (x1.Memory.Length > 1048576) throw new global::System.ArgumentOutOfRangeException("x1");
            if (x2 is null) throw new global::System.ArgumentNullException("x2");
            if (x2.Memory.Length > 1048576) throw new global::System.ArgumentOutOfRangeException("x2");
            if (x3 is null) throw new global::System.ArgumentNullException("x3");
            if (x3.Memory.Length > 1048576) throw new global::System.ArgumentOutOfRangeException("x3");
            if (x4 is null) throw new global::System.ArgumentNullException("x4");
            if (x4.Memory.Length > 1048576) throw new global::System.ArgumentOutOfRangeException("x4");
            if (x5 is null) throw new global::System.ArgumentNullException("x5");
            if (x5.Memory.Length > 1048576) throw new global::System.ArgumentOutOfRangeException("x5");
            if (x6 is null) throw new global::System.ArgumentNullException("x6");
            if (x6.Memory.Length > 1048576) throw new global::System.ArgumentOutOfRangeException("x6");
            if (x7 is null) throw new global::System.ArgumentNullException("x7");
            if (x7.Memory.Length > 1048576) throw new global::System.ArgumentOutOfRangeException("x7");
            if (x8 is null) throw new global::System.ArgumentNullException("x8");
            if (x8.Memory.Length > 1048576) throw new global::System.ArgumentOutOfRangeException("x8");
            if (x9 is null) throw new global::System.ArgumentNullException("x9");
            if (x9.Memory.Length > 1048576) throw new global::System.ArgumentOutOfRangeException("x9");

            _x0 = x0;
            _x1 = x1;
            _x2 = x2;
            _x3 = x3;
            _x4 = x4;
            _x5 = x5;
            _x6 = x6;
            _x7 = x7;
            _x8 = x8;
            _x9 = x9;

            ___hashCode = new global::System.Lazy<int>(() =>
            {
                var ___h = new global::System.HashCode();
                if (!x0.Memory.IsEmpty) ___h.Add(global::Omnius.Core.Helpers.ObjectHelper.GetHashCode(x0.Memory.Span));
                if (!x1.Memory.IsEmpty) ___h.Add(global::Omnius.Core.Helpers.ObjectHelper.GetHashCode(x1.Memory.Span));
                if (!x2.Memory.IsEmpty) ___h.Add(global::Omnius.Core.Helpers.ObjectHelper.GetHashCode(x2.Memory.Span));
                if (!x3.Memory.IsEmpty) ___h.Add(global::Omnius.Core.Helpers.ObjectHelper.GetHashCode(x3.Memory.Span));
                if (!x4.Memory.IsEmpty) ___h.Add(global::Omnius.Core.Helpers.ObjectHelper.GetHashCode(x4.Memory.Span));
                if (!x5.Memory.IsEmpty) ___h.Add(global::Omnius.Core.Helpers.ObjectHelper.GetHashCode(x5.Memory.Span));
                if (!x6.Memory.IsEmpty) ___h.Add(global::Omnius.Core.Helpers.ObjectHelper.GetHashCode(x6.Memory.Span));
                if (!x7.Memory.IsEmpty) ___h.Add(global::Omnius.Core.Helpers.ObjectHelper.GetHashCode(x7.Memory.Span));
                if (!x8.Memory.IsEmpty) ___h.Add(global::Omnius.Core.Helpers.ObjectHelper.GetHashCode(x8.Memory.Span));
                if (!x9.Memory.IsEmpty) ___h.Add(global::Omnius.Core.Helpers.ObjectHelper.GetHashCode(x9.Memory.Span));
                return ___h.ToHashCode();
            });
        }

        private readonly global::System.Buffers.IMemoryOwner<byte> _x0;
        public global::System.ReadOnlyMemory<byte> X0 => _x0.Memory;
        private readonly global::System.Buffers.IMemoryOwner<byte> _x1;
        public global::System.ReadOnlyMemory<byte> X1 => _x1.Memory;
        private readonly global::System.Buffers.IMemoryOwner<byte> _x2;
        public global::System.ReadOnlyMemory<byte> X2 => _x2.Memory;
        private readonly global::System.Buffers.IMemoryOwner<byte> _x3;
        public global::System.ReadOnlyMemory<byte> X3 => _x3.Memory;
        private readonly global::System.Buffers.IMemoryOwner<byte> _x4;
        public global::System.ReadOnlyMemory<byte> X4 => _x4.Memory;
        private readonly global::System.Buffers.IMemoryOwner<byte> _x5;
        public global::System.ReadOnlyMemory<byte> X5 => _x5.Memory;
        private readonly global::System.Buffers.IMemoryOwner<byte> _x6;
        public global::System.ReadOnlyMemory<byte> X6 => _x6.Memory;
        private readonly global::System.Buffers.IMemoryOwner<byte> _x7;
        public global::System.ReadOnlyMemory<byte> X7 => _x7.Memory;
        private readonly global::System.Buffers.IMemoryOwner<byte> _x8;
        public global::System.ReadOnlyMemory<byte> X8 => _x8.Memory;
        private readonly global::System.Buffers.IMemoryOwner<byte> _x9;
        public global::System.ReadOnlyMemory<byte> X9 => _x9.Memory;

        public static global::FormatterBenchmarks.Internal.RocketPack_BytesElements Import(global::System.Buffers.ReadOnlySequence<byte> sequence, global::Omnius.Core.IBytesPool bytesPool)
        {
            var reader = new global::Omnius.Core.RocketPack.RocketPackObjectReader(sequence, bytesPool);
            return Formatter.Deserialize(ref reader, 0);
        }
        public void Export(global::System.Buffers.IBufferWriter<byte> bufferWriter, global::Omnius.Core.IBytesPool bytesPool)
        {
            var writer = new global::Omnius.Core.RocketPack.RocketPackObjectWriter(bufferWriter, bytesPool);
            Formatter.Serialize(ref writer, this, 0);
        }

        public static bool operator ==(global::FormatterBenchmarks.Internal.RocketPack_BytesElements? left, global::FormatterBenchmarks.Internal.RocketPack_BytesElements? right)
        {
            return (right is null) ? (left is null) : right.Equals(left);
        }
        public static bool operator !=(global::FormatterBenchmarks.Internal.RocketPack_BytesElements? left, global::FormatterBenchmarks.Internal.RocketPack_BytesElements? right)
        {
            return !(left == right);
        }
        public override bool Equals(object? other)
        {
            if (!(other is global::FormatterBenchmarks.Internal.RocketPack_BytesElements)) return false;
            return this.Equals((global::FormatterBenchmarks.Internal.RocketPack_BytesElements)other);
        }
        public bool Equals(global::FormatterBenchmarks.Internal.RocketPack_BytesElements? target)
        {
            if (target is null) return false;
            if (object.ReferenceEquals(this, target)) return true;
            if (!global::Omnius.Core.BytesOperations.Equals(this.X0.Span, target.X0.Span)) return false;
            if (!global::Omnius.Core.BytesOperations.Equals(this.X1.Span, target.X1.Span)) return false;
            if (!global::Omnius.Core.BytesOperations.Equals(this.X2.Span, target.X2.Span)) return false;
            if (!global::Omnius.Core.BytesOperations.Equals(this.X3.Span, target.X3.Span)) return false;
            if (!global::Omnius.Core.BytesOperations.Equals(this.X4.Span, target.X4.Span)) return false;
            if (!global::Omnius.Core.BytesOperations.Equals(this.X5.Span, target.X5.Span)) return false;
            if (!global::Omnius.Core.BytesOperations.Equals(this.X6.Span, target.X6.Span)) return false;
            if (!global::Omnius.Core.BytesOperations.Equals(this.X7.Span, target.X7.Span)) return false;
            if (!global::Omnius.Core.BytesOperations.Equals(this.X8.Span, target.X8.Span)) return false;
            if (!global::Omnius.Core.BytesOperations.Equals(this.X9.Span, target.X9.Span)) return false;

            return true;
        }
        public override int GetHashCode() => ___hashCode.Value;

        public void Dispose()
        {
            _x0?.Dispose();
            _x1?.Dispose();
            _x2?.Dispose();
            _x3?.Dispose();
            _x4?.Dispose();
            _x5?.Dispose();
            _x6?.Dispose();
            _x7?.Dispose();
            _x8?.Dispose();
            _x9?.Dispose();
        }

        private sealed class ___CustomFormatter : global::Omnius.Core.RocketPack.IRocketPackObjectFormatter<global::FormatterBenchmarks.Internal.RocketPack_BytesElements>
        {
            public void Serialize(ref global::Omnius.Core.RocketPack.RocketPackObjectWriter w, in global::FormatterBenchmarks.Internal.RocketPack_BytesElements value, in int rank)
            {
                if (rank > 256) throw new global::System.FormatException();

                w.Write(value.X0.Span);
                w.Write(value.X1.Span);
                w.Write(value.X2.Span);
                w.Write(value.X3.Span);
                w.Write(value.X4.Span);
                w.Write(value.X5.Span);
                w.Write(value.X6.Span);
                w.Write(value.X7.Span);
                w.Write(value.X8.Span);
                w.Write(value.X9.Span);
            }

            public global::FormatterBenchmarks.Internal.RocketPack_BytesElements Deserialize(ref global::Omnius.Core.RocketPack.RocketPackObjectReader r, in int rank)
            {
                if (rank > 256) throw new global::System.FormatException();

                global::System.Buffers.IMemoryOwner<byte> p_x0 = global::Omnius.Core.MemoryOwner<byte>.Empty;
                global::System.Buffers.IMemoryOwner<byte> p_x1 = global::Omnius.Core.MemoryOwner<byte>.Empty;
                global::System.Buffers.IMemoryOwner<byte> p_x2 = global::Omnius.Core.MemoryOwner<byte>.Empty;
                global::System.Buffers.IMemoryOwner<byte> p_x3 = global::Omnius.Core.MemoryOwner<byte>.Empty;
                global::System.Buffers.IMemoryOwner<byte> p_x4 = global::Omnius.Core.MemoryOwner<byte>.Empty;
                global::System.Buffers.IMemoryOwner<byte> p_x5 = global::Omnius.Core.MemoryOwner<byte>.Empty;
                global::System.Buffers.IMemoryOwner<byte> p_x6 = global::Omnius.Core.MemoryOwner<byte>.Empty;
                global::System.Buffers.IMemoryOwner<byte> p_x7 = global::Omnius.Core.MemoryOwner<byte>.Empty;
                global::System.Buffers.IMemoryOwner<byte> p_x8 = global::Omnius.Core.MemoryOwner<byte>.Empty;
                global::System.Buffers.IMemoryOwner<byte> p_x9 = global::Omnius.Core.MemoryOwner<byte>.Empty;

                {
                    p_x0 = r.GetRecyclableMemory(1048576);
                }
                {
                    p_x1 = r.GetRecyclableMemory(1048576);
                }
                {
                    p_x2 = r.GetRecyclableMemory(1048576);
                }
                {
                    p_x3 = r.GetRecyclableMemory(1048576);
                }
                {
                    p_x4 = r.GetRecyclableMemory(1048576);
                }
                {
                    p_x5 = r.GetRecyclableMemory(1048576);
                }
                {
                    p_x6 = r.GetRecyclableMemory(1048576);
                }
                {
                    p_x7 = r.GetRecyclableMemory(1048576);
                }
                {
                    p_x8 = r.GetRecyclableMemory(1048576);
                }
                {
                    p_x9 = r.GetRecyclableMemory(1048576);
                }
                return new global::FormatterBenchmarks.Internal.RocketPack_BytesElements(p_x0, p_x1, p_x2, p_x3, p_x4, p_x5, p_x6, p_x7, p_x8, p_x9);
            }
        }
    }
    internal sealed partial class RocketPack_IntElementsList : global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_IntElementsList>
    {
        public static global::Omnius.Core.RocketPack.IRocketPackObjectFormatter<global::FormatterBenchmarks.Internal.RocketPack_IntElementsList> Formatter => global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_IntElementsList>.Formatter;
        public static global::FormatterBenchmarks.Internal.RocketPack_IntElementsList Empty => global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_IntElementsList>.Empty;

        static RocketPack_IntElementsList()
        {
            global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_IntElementsList>.Formatter = new ___CustomFormatter();
            global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_IntElementsList>.Empty = new global::FormatterBenchmarks.Internal.RocketPack_IntElementsList(global::System.Array.Empty<RocketPack_IntElements>());
        }

        private readonly global::System.Lazy<int> ___hashCode;

        public static readonly int MaxListCount = 1048576;

        public RocketPack_IntElementsList(RocketPack_IntElements[] list)
        {
            if (list is null) throw new global::System.ArgumentNullException("list");
            if (list.Length > 1048576) throw new global::System.ArgumentOutOfRangeException("list");
            foreach (var n in list)
            {
                if (n is null) throw new global::System.ArgumentNullException("n");
            }

            this.List = new global::Omnius.Core.Collections.ReadOnlyListSlim<RocketPack_IntElements>(list);

            ___hashCode = new global::System.Lazy<int>(() =>
            {
                var ___h = new global::System.HashCode();
                foreach (var n in list)
                {
                    if (n != default) ___h.Add(n.GetHashCode());
                }
                return ___h.ToHashCode();
            });
        }

        public global::Omnius.Core.Collections.ReadOnlyListSlim<RocketPack_IntElements> List { get; }

        public static global::FormatterBenchmarks.Internal.RocketPack_IntElementsList Import(global::System.Buffers.ReadOnlySequence<byte> sequence, global::Omnius.Core.IBytesPool bytesPool)
        {
            var reader = new global::Omnius.Core.RocketPack.RocketPackObjectReader(sequence, bytesPool);
            return Formatter.Deserialize(ref reader, 0);
        }
        public void Export(global::System.Buffers.IBufferWriter<byte> bufferWriter, global::Omnius.Core.IBytesPool bytesPool)
        {
            var writer = new global::Omnius.Core.RocketPack.RocketPackObjectWriter(bufferWriter, bytesPool);
            Formatter.Serialize(ref writer, this, 0);
        }

        public static bool operator ==(global::FormatterBenchmarks.Internal.RocketPack_IntElementsList? left, global::FormatterBenchmarks.Internal.RocketPack_IntElementsList? right)
        {
            return (right is null) ? (left is null) : right.Equals(left);
        }
        public static bool operator !=(global::FormatterBenchmarks.Internal.RocketPack_IntElementsList? left, global::FormatterBenchmarks.Internal.RocketPack_IntElementsList? right)
        {
            return !(left == right);
        }
        public override bool Equals(object? other)
        {
            if (!(other is global::FormatterBenchmarks.Internal.RocketPack_IntElementsList)) return false;
            return this.Equals((global::FormatterBenchmarks.Internal.RocketPack_IntElementsList)other);
        }
        public bool Equals(global::FormatterBenchmarks.Internal.RocketPack_IntElementsList? target)
        {
            if (target is null) return false;
            if (object.ReferenceEquals(this, target)) return true;
            if (!global::Omnius.Core.Helpers.CollectionHelper.Equals(this.List, target.List)) return false;

            return true;
        }
        public override int GetHashCode() => ___hashCode.Value;

        private sealed class ___CustomFormatter : global::Omnius.Core.RocketPack.IRocketPackObjectFormatter<global::FormatterBenchmarks.Internal.RocketPack_IntElementsList>
        {
            public void Serialize(ref global::Omnius.Core.RocketPack.RocketPackObjectWriter w, in global::FormatterBenchmarks.Internal.RocketPack_IntElementsList value, in int rank)
            {
                if (rank > 256) throw new global::System.FormatException();

                if (value.List.Count != 0)
                {
                    w.Write((uint)1);
                    w.Write((uint)value.List.Count);
                    foreach (var n in value.List)
                    {
                        global::FormatterBenchmarks.Internal.RocketPack_IntElements.Formatter.Serialize(ref w, n, rank + 1);
                    }
                }
                w.Write((uint)0);
            }

            public global::FormatterBenchmarks.Internal.RocketPack_IntElementsList Deserialize(ref global::Omnius.Core.RocketPack.RocketPackObjectReader r, in int rank)
            {
                if (rank > 256) throw new global::System.FormatException();

                RocketPack_IntElements[] p_list = global::System.Array.Empty<RocketPack_IntElements>();

                for (; ; )
                {
                    uint id = r.GetUInt32();
                    if (id == 0) break;
                    switch (id)
                    {
                        case 1:
                            {
                                var length = r.GetUInt32();
                                p_list = new RocketPack_IntElements[length];
                                for (int i = 0; i < p_list.Length; i++)
                                {
                                    p_list[i] = global::FormatterBenchmarks.Internal.RocketPack_IntElements.Formatter.Deserialize(ref r, rank + 1);
                                }
                                break;
                            }
                    }
                }

                return new global::FormatterBenchmarks.Internal.RocketPack_IntElementsList(p_list);
            }
        }
    }
    internal sealed partial class RocketPack_IntElements : global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_IntElements>
    {
        public static global::Omnius.Core.RocketPack.IRocketPackObjectFormatter<global::FormatterBenchmarks.Internal.RocketPack_IntElements> Formatter => global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_IntElements>.Formatter;
        public static global::FormatterBenchmarks.Internal.RocketPack_IntElements Empty => global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_IntElements>.Empty;

        static RocketPack_IntElements()
        {
            global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_IntElements>.Formatter = new ___CustomFormatter();
            global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_IntElements>.Empty = new global::FormatterBenchmarks.Internal.RocketPack_IntElements(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        }

        private readonly global::System.Lazy<int> ___hashCode;

        public RocketPack_IntElements(uint x0, uint x1, uint x2, uint x3, uint x4, uint x5, uint x6, uint x7, uint x8, uint x9)
        {
            this.X0 = x0;
            this.X1 = x1;
            this.X2 = x2;
            this.X3 = x3;
            this.X4 = x4;
            this.X5 = x5;
            this.X6 = x6;
            this.X7 = x7;
            this.X8 = x8;
            this.X9 = x9;

            ___hashCode = new global::System.Lazy<int>(() =>
            {
                var ___h = new global::System.HashCode();
                if (x0 != default) ___h.Add(x0.GetHashCode());
                if (x1 != default) ___h.Add(x1.GetHashCode());
                if (x2 != default) ___h.Add(x2.GetHashCode());
                if (x3 != default) ___h.Add(x3.GetHashCode());
                if (x4 != default) ___h.Add(x4.GetHashCode());
                if (x5 != default) ___h.Add(x5.GetHashCode());
                if (x6 != default) ___h.Add(x6.GetHashCode());
                if (x7 != default) ___h.Add(x7.GetHashCode());
                if (x8 != default) ___h.Add(x8.GetHashCode());
                if (x9 != default) ___h.Add(x9.GetHashCode());
                return ___h.ToHashCode();
            });
        }

        public uint X0 { get; }
        public uint X1 { get; }
        public uint X2 { get; }
        public uint X3 { get; }
        public uint X4 { get; }
        public uint X5 { get; }
        public uint X6 { get; }
        public uint X7 { get; }
        public uint X8 { get; }
        public uint X9 { get; }

        public static global::FormatterBenchmarks.Internal.RocketPack_IntElements Import(global::System.Buffers.ReadOnlySequence<byte> sequence, global::Omnius.Core.IBytesPool bytesPool)
        {
            var reader = new global::Omnius.Core.RocketPack.RocketPackObjectReader(sequence, bytesPool);
            return Formatter.Deserialize(ref reader, 0);
        }
        public void Export(global::System.Buffers.IBufferWriter<byte> bufferWriter, global::Omnius.Core.IBytesPool bytesPool)
        {
            var writer = new global::Omnius.Core.RocketPack.RocketPackObjectWriter(bufferWriter, bytesPool);
            Formatter.Serialize(ref writer, this, 0);
        }

        public static bool operator ==(global::FormatterBenchmarks.Internal.RocketPack_IntElements? left, global::FormatterBenchmarks.Internal.RocketPack_IntElements? right)
        {
            return (right is null) ? (left is null) : right.Equals(left);
        }
        public static bool operator !=(global::FormatterBenchmarks.Internal.RocketPack_IntElements? left, global::FormatterBenchmarks.Internal.RocketPack_IntElements? right)
        {
            return !(left == right);
        }
        public override bool Equals(object? other)
        {
            if (!(other is global::FormatterBenchmarks.Internal.RocketPack_IntElements)) return false;
            return this.Equals((global::FormatterBenchmarks.Internal.RocketPack_IntElements)other);
        }
        public bool Equals(global::FormatterBenchmarks.Internal.RocketPack_IntElements? target)
        {
            if (target is null) return false;
            if (object.ReferenceEquals(this, target)) return true;
            if (this.X0 != target.X0) return false;
            if (this.X1 != target.X1) return false;
            if (this.X2 != target.X2) return false;
            if (this.X3 != target.X3) return false;
            if (this.X4 != target.X4) return false;
            if (this.X5 != target.X5) return false;
            if (this.X6 != target.X6) return false;
            if (this.X7 != target.X7) return false;
            if (this.X8 != target.X8) return false;
            if (this.X9 != target.X9) return false;

            return true;
        }
        public override int GetHashCode() => ___hashCode.Value;

        private sealed class ___CustomFormatter : global::Omnius.Core.RocketPack.IRocketPackObjectFormatter<global::FormatterBenchmarks.Internal.RocketPack_IntElements>
        {
            public void Serialize(ref global::Omnius.Core.RocketPack.RocketPackObjectWriter w, in global::FormatterBenchmarks.Internal.RocketPack_IntElements value, in int rank)
            {
                if (rank > 256) throw new global::System.FormatException();

                w.Write(value.X0);
                w.Write(value.X1);
                w.Write(value.X2);
                w.Write(value.X3);
                w.Write(value.X4);
                w.Write(value.X5);
                w.Write(value.X6);
                w.Write(value.X7);
                w.Write(value.X8);
                w.Write(value.X9);
            }

            public global::FormatterBenchmarks.Internal.RocketPack_IntElements Deserialize(ref global::Omnius.Core.RocketPack.RocketPackObjectReader r, in int rank)
            {
                if (rank > 256) throw new global::System.FormatException();

                uint p_x0 = 0;
                uint p_x1 = 0;
                uint p_x2 = 0;
                uint p_x3 = 0;
                uint p_x4 = 0;
                uint p_x5 = 0;
                uint p_x6 = 0;
                uint p_x7 = 0;
                uint p_x8 = 0;
                uint p_x9 = 0;

                {
                    p_x0 = r.GetUInt32();
                }
                {
                    p_x1 = r.GetUInt32();
                }
                {
                    p_x2 = r.GetUInt32();
                }
                {
                    p_x3 = r.GetUInt32();
                }
                {
                    p_x4 = r.GetUInt32();
                }
                {
                    p_x5 = r.GetUInt32();
                }
                {
                    p_x6 = r.GetUInt32();
                }
                {
                    p_x7 = r.GetUInt32();
                }
                {
                    p_x8 = r.GetUInt32();
                }
                {
                    p_x9 = r.GetUInt32();
                }
                return new global::FormatterBenchmarks.Internal.RocketPack_IntElements(p_x0, p_x1, p_x2, p_x3, p_x4, p_x5, p_x6, p_x7, p_x8, p_x9);
            }
        }
    }
    internal sealed partial class RocketPack_StringElementsList : global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_StringElementsList>
    {
        public static global::Omnius.Core.RocketPack.IRocketPackObjectFormatter<global::FormatterBenchmarks.Internal.RocketPack_StringElementsList> Formatter => global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_StringElementsList>.Formatter;
        public static global::FormatterBenchmarks.Internal.RocketPack_StringElementsList Empty => global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_StringElementsList>.Empty;

        static RocketPack_StringElementsList()
        {
            global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_StringElementsList>.Formatter = new ___CustomFormatter();
            global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_StringElementsList>.Empty = new global::FormatterBenchmarks.Internal.RocketPack_StringElementsList(global::System.Array.Empty<RocketPack_StringElements>());
        }

        private readonly global::System.Lazy<int> ___hashCode;

        public static readonly int MaxListCount = 1048576;

        public RocketPack_StringElementsList(RocketPack_StringElements[] list)
        {
            if (list is null) throw new global::System.ArgumentNullException("list");
            if (list.Length > 1048576) throw new global::System.ArgumentOutOfRangeException("list");
            foreach (var n in list)
            {
                if (n is null) throw new global::System.ArgumentNullException("n");
            }

            this.List = new global::Omnius.Core.Collections.ReadOnlyListSlim<RocketPack_StringElements>(list);

            ___hashCode = new global::System.Lazy<int>(() =>
            {
                var ___h = new global::System.HashCode();
                foreach (var n in list)
                {
                    if (n != default) ___h.Add(n.GetHashCode());
                }
                return ___h.ToHashCode();
            });
        }

        public global::Omnius.Core.Collections.ReadOnlyListSlim<RocketPack_StringElements> List { get; }

        public static global::FormatterBenchmarks.Internal.RocketPack_StringElementsList Import(global::System.Buffers.ReadOnlySequence<byte> sequence, global::Omnius.Core.IBytesPool bytesPool)
        {
            var reader = new global::Omnius.Core.RocketPack.RocketPackObjectReader(sequence, bytesPool);
            return Formatter.Deserialize(ref reader, 0);
        }
        public void Export(global::System.Buffers.IBufferWriter<byte> bufferWriter, global::Omnius.Core.IBytesPool bytesPool)
        {
            var writer = new global::Omnius.Core.RocketPack.RocketPackObjectWriter(bufferWriter, bytesPool);
            Formatter.Serialize(ref writer, this, 0);
        }

        public static bool operator ==(global::FormatterBenchmarks.Internal.RocketPack_StringElementsList? left, global::FormatterBenchmarks.Internal.RocketPack_StringElementsList? right)
        {
            return (right is null) ? (left is null) : right.Equals(left);
        }
        public static bool operator !=(global::FormatterBenchmarks.Internal.RocketPack_StringElementsList? left, global::FormatterBenchmarks.Internal.RocketPack_StringElementsList? right)
        {
            return !(left == right);
        }
        public override bool Equals(object? other)
        {
            if (!(other is global::FormatterBenchmarks.Internal.RocketPack_StringElementsList)) return false;
            return this.Equals((global::FormatterBenchmarks.Internal.RocketPack_StringElementsList)other);
        }
        public bool Equals(global::FormatterBenchmarks.Internal.RocketPack_StringElementsList? target)
        {
            if (target is null) return false;
            if (object.ReferenceEquals(this, target)) return true;
            if (!global::Omnius.Core.Helpers.CollectionHelper.Equals(this.List, target.List)) return false;

            return true;
        }
        public override int GetHashCode() => ___hashCode.Value;

        private sealed class ___CustomFormatter : global::Omnius.Core.RocketPack.IRocketPackObjectFormatter<global::FormatterBenchmarks.Internal.RocketPack_StringElementsList>
        {
            public void Serialize(ref global::Omnius.Core.RocketPack.RocketPackObjectWriter w, in global::FormatterBenchmarks.Internal.RocketPack_StringElementsList value, in int rank)
            {
                if (rank > 256) throw new global::System.FormatException();

                if (value.List.Count != 0)
                {
                    w.Write((uint)1);
                    w.Write((uint)value.List.Count);
                    foreach (var n in value.List)
                    {
                        global::FormatterBenchmarks.Internal.RocketPack_StringElements.Formatter.Serialize(ref w, n, rank + 1);
                    }
                }
                w.Write((uint)0);
            }

            public global::FormatterBenchmarks.Internal.RocketPack_StringElementsList Deserialize(ref global::Omnius.Core.RocketPack.RocketPackObjectReader r, in int rank)
            {
                if (rank > 256) throw new global::System.FormatException();

                RocketPack_StringElements[] p_list = global::System.Array.Empty<RocketPack_StringElements>();

                for (; ; )
                {
                    uint id = r.GetUInt32();
                    if (id == 0) break;
                    switch (id)
                    {
                        case 1:
                            {
                                var length = r.GetUInt32();
                                p_list = new RocketPack_StringElements[length];
                                for (int i = 0; i < p_list.Length; i++)
                                {
                                    p_list[i] = global::FormatterBenchmarks.Internal.RocketPack_StringElements.Formatter.Deserialize(ref r, rank + 1);
                                }
                                break;
                            }
                    }
                }

                return new global::FormatterBenchmarks.Internal.RocketPack_StringElementsList(p_list);
            }
        }
    }
    internal sealed partial class RocketPack_StringElements : global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_StringElements>
    {
        public static global::Omnius.Core.RocketPack.IRocketPackObjectFormatter<global::FormatterBenchmarks.Internal.RocketPack_StringElements> Formatter => global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_StringElements>.Formatter;
        public static global::FormatterBenchmarks.Internal.RocketPack_StringElements Empty => global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_StringElements>.Empty;

        static RocketPack_StringElements()
        {
            global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_StringElements>.Formatter = new ___CustomFormatter();
            global::Omnius.Core.RocketPack.IRocketPackObject<global::FormatterBenchmarks.Internal.RocketPack_StringElements>.Empty = new global::FormatterBenchmarks.Internal.RocketPack_StringElements(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        }

        private readonly global::System.Lazy<int> ___hashCode;

        public static readonly int MaxX0Length = 8192;
        public static readonly int MaxX1Length = 8192;
        public static readonly int MaxX2Length = 8192;
        public static readonly int MaxX3Length = 8192;
        public static readonly int MaxX4Length = 8192;
        public static readonly int MaxX5Length = 8192;
        public static readonly int MaxX6Length = 8192;
        public static readonly int MaxX7Length = 8192;
        public static readonly int MaxX8Length = 8192;
        public static readonly int MaxX9Length = 8192;

        public RocketPack_StringElements(string x0, string x1, string x2, string x3, string x4, string x5, string x6, string x7, string x8, string x9)
        {
            if (x0 is null) throw new global::System.ArgumentNullException("x0");
            if (x0.Length > 8192) throw new global::System.ArgumentOutOfRangeException("x0");
            if (x1 is null) throw new global::System.ArgumentNullException("x1");
            if (x1.Length > 8192) throw new global::System.ArgumentOutOfRangeException("x1");
            if (x2 is null) throw new global::System.ArgumentNullException("x2");
            if (x2.Length > 8192) throw new global::System.ArgumentOutOfRangeException("x2");
            if (x3 is null) throw new global::System.ArgumentNullException("x3");
            if (x3.Length > 8192) throw new global::System.ArgumentOutOfRangeException("x3");
            if (x4 is null) throw new global::System.ArgumentNullException("x4");
            if (x4.Length > 8192) throw new global::System.ArgumentOutOfRangeException("x4");
            if (x5 is null) throw new global::System.ArgumentNullException("x5");
            if (x5.Length > 8192) throw new global::System.ArgumentOutOfRangeException("x5");
            if (x6 is null) throw new global::System.ArgumentNullException("x6");
            if (x6.Length > 8192) throw new global::System.ArgumentOutOfRangeException("x6");
            if (x7 is null) throw new global::System.ArgumentNullException("x7");
            if (x7.Length > 8192) throw new global::System.ArgumentOutOfRangeException("x7");
            if (x8 is null) throw new global::System.ArgumentNullException("x8");
            if (x8.Length > 8192) throw new global::System.ArgumentOutOfRangeException("x8");
            if (x9 is null) throw new global::System.ArgumentNullException("x9");
            if (x9.Length > 8192) throw new global::System.ArgumentOutOfRangeException("x9");

            this.X0 = x0;
            this.X1 = x1;
            this.X2 = x2;
            this.X3 = x3;
            this.X4 = x4;
            this.X5 = x5;
            this.X6 = x6;
            this.X7 = x7;
            this.X8 = x8;
            this.X9 = x9;

            ___hashCode = new global::System.Lazy<int>(() =>
            {
                var ___h = new global::System.HashCode();
                if (x0 != default) ___h.Add(x0.GetHashCode());
                if (x1 != default) ___h.Add(x1.GetHashCode());
                if (x2 != default) ___h.Add(x2.GetHashCode());
                if (x3 != default) ___h.Add(x3.GetHashCode());
                if (x4 != default) ___h.Add(x4.GetHashCode());
                if (x5 != default) ___h.Add(x5.GetHashCode());
                if (x6 != default) ___h.Add(x6.GetHashCode());
                if (x7 != default) ___h.Add(x7.GetHashCode());
                if (x8 != default) ___h.Add(x8.GetHashCode());
                if (x9 != default) ___h.Add(x9.GetHashCode());
                return ___h.ToHashCode();
            });
        }

        public string X0 { get; }
        public string X1 { get; }
        public string X2 { get; }
        public string X3 { get; }
        public string X4 { get; }
        public string X5 { get; }
        public string X6 { get; }
        public string X7 { get; }
        public string X8 { get; }
        public string X9 { get; }

        public static global::FormatterBenchmarks.Internal.RocketPack_StringElements Import(global::System.Buffers.ReadOnlySequence<byte> sequence, global::Omnius.Core.IBytesPool bytesPool)
        {
            var reader = new global::Omnius.Core.RocketPack.RocketPackObjectReader(sequence, bytesPool);
            return Formatter.Deserialize(ref reader, 0);
        }
        public void Export(global::System.Buffers.IBufferWriter<byte> bufferWriter, global::Omnius.Core.IBytesPool bytesPool)
        {
            var writer = new global::Omnius.Core.RocketPack.RocketPackObjectWriter(bufferWriter, bytesPool);
            Formatter.Serialize(ref writer, this, 0);
        }

        public static bool operator ==(global::FormatterBenchmarks.Internal.RocketPack_StringElements? left, global::FormatterBenchmarks.Internal.RocketPack_StringElements? right)
        {
            return (right is null) ? (left is null) : right.Equals(left);
        }
        public static bool operator !=(global::FormatterBenchmarks.Internal.RocketPack_StringElements? left, global::FormatterBenchmarks.Internal.RocketPack_StringElements? right)
        {
            return !(left == right);
        }
        public override bool Equals(object? other)
        {
            if (!(other is global::FormatterBenchmarks.Internal.RocketPack_StringElements)) return false;
            return this.Equals((global::FormatterBenchmarks.Internal.RocketPack_StringElements)other);
        }
        public bool Equals(global::FormatterBenchmarks.Internal.RocketPack_StringElements? target)
        {
            if (target is null) return false;
            if (object.ReferenceEquals(this, target)) return true;
            if (this.X0 != target.X0) return false;
            if (this.X1 != target.X1) return false;
            if (this.X2 != target.X2) return false;
            if (this.X3 != target.X3) return false;
            if (this.X4 != target.X4) return false;
            if (this.X5 != target.X5) return false;
            if (this.X6 != target.X6) return false;
            if (this.X7 != target.X7) return false;
            if (this.X8 != target.X8) return false;
            if (this.X9 != target.X9) return false;

            return true;
        }
        public override int GetHashCode() => ___hashCode.Value;

        private sealed class ___CustomFormatter : global::Omnius.Core.RocketPack.IRocketPackObjectFormatter<global::FormatterBenchmarks.Internal.RocketPack_StringElements>
        {
            public void Serialize(ref global::Omnius.Core.RocketPack.RocketPackObjectWriter w, in global::FormatterBenchmarks.Internal.RocketPack_StringElements value, in int rank)
            {
                if (rank > 256) throw new global::System.FormatException();

                w.Write(value.X0);
                w.Write(value.X1);
                w.Write(value.X2);
                w.Write(value.X3);
                w.Write(value.X4);
                w.Write(value.X5);
                w.Write(value.X6);
                w.Write(value.X7);
                w.Write(value.X8);
                w.Write(value.X9);
            }

            public global::FormatterBenchmarks.Internal.RocketPack_StringElements Deserialize(ref global::Omnius.Core.RocketPack.RocketPackObjectReader r, in int rank)
            {
                if (rank > 256) throw new global::System.FormatException();

                string p_x0 = string.Empty;
                string p_x1 = string.Empty;
                string p_x2 = string.Empty;
                string p_x3 = string.Empty;
                string p_x4 = string.Empty;
                string p_x5 = string.Empty;
                string p_x6 = string.Empty;
                string p_x7 = string.Empty;
                string p_x8 = string.Empty;
                string p_x9 = string.Empty;

                {
                    p_x0 = r.GetString(8192);
                }
                {
                    p_x1 = r.GetString(8192);
                }
                {
                    p_x2 = r.GetString(8192);
                }
                {
                    p_x3 = r.GetString(8192);
                }
                {
                    p_x4 = r.GetString(8192);
                }
                {
                    p_x5 = r.GetString(8192);
                }
                {
                    p_x6 = r.GetString(8192);
                }
                {
                    p_x7 = r.GetString(8192);
                }
                {
                    p_x8 = r.GetString(8192);
                }
                {
                    p_x9 = r.GetString(8192);
                }
                return new global::FormatterBenchmarks.Internal.RocketPack_StringElements(p_x0, p_x1, p_x2, p_x3, p_x4, p_x5, p_x6, p_x7, p_x8, p_x9);
            }
        }
    }


}