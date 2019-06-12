﻿using System;
using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Omnix.Base;
using Omnix.Serialization.RocketPack.Internal;

namespace Omnix.Serialization.RocketPack
{
    /// <summary>
    /// RocketPackフォーマットのデシリアライズ機能を提供します。
    /// </summary>
    public sealed class RocketPackReader
    {
        private ReadOnlySequence<byte> _sequence;
        private BufferPool _bufferPool;

        private static readonly Lazy<Encoding> _encoding = new Lazy<Encoding>(() => new UTF8Encoding(false));

        public RocketPackReader(ReadOnlySequence<byte> sequence, BufferPool bufferPool)
        {
            _sequence = sequence;
            _bufferPool = bufferPool;
        }

        public long Available => _sequence.Length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IMemoryOwner<byte> GetRecyclableMemory(int limit)
        {
            if (!Varint.TryGetUInt32(_sequence, out uint length, out var consumed))
            {
                throw new FormatException();
            }

            _sequence = _sequence.Slice(consumed);

            if (length > limit)
            {
                throw new FormatException();
            }

            var memoryOwner = _bufferPool.Rent((int)length);

            _sequence.Slice(0, length).CopyTo(memoryOwner.Memory.Span);
            _sequence = _sequence.Slice(length);

            return memoryOwner;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyMemory<byte> GetMemory(int limit)
        {
            if (!Varint.TryGetUInt32(_sequence, out uint length, out var consumed))
            {
                throw new FormatException();
            }

            _sequence = _sequence.Slice(consumed);

            if (length > limit)
            {
                throw new FormatException();
            }

            var result = new byte[(int)length];

            _sequence.Slice(0, length).CopyTo(result.AsSpan());
            _sequence = _sequence.Slice(length);

            return new ReadOnlyMemory<byte>(result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetString(int limit)
        {
            if (!Varint.TryGetUInt32(_sequence, out uint length, out var consumed))
            {
                throw new FormatException();
            }

            _sequence = _sequence.Slice(consumed);

            if (length > limit)
            {
                throw new FormatException();
            }

            using (var memoryOwner = _bufferPool.Rent((int)length))
            {
                _sequence.Slice(0, length).CopyTo(memoryOwner.Memory.Span);
                _sequence = _sequence.Slice(length);

                return _encoding.Value.GetString(memoryOwner.Memory.Span);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Timestamp GetTimestamp()
        {
            long seconds = this.GetInt64();
            int nanos = (int)this.GetUInt64();

            return new Timestamp(seconds, nanos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GetBoolean()
        {
            if (!Varint.TryGetUInt64(_sequence, out ulong result, out var consumed))
            {
                throw new FormatException();
            }

            _sequence = _sequence.Slice(consumed);

            return (result != 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong GetUInt64()
        {
            if (!Varint.TryGetUInt64(_sequence, out ulong result, out var consumed))
            {
                throw new FormatException();
            }

            _sequence = _sequence.Slice(consumed);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetUInt8()
        {
            if (!Varint.TryGetUInt8(_sequence, out byte result, out var consumed))
            {
                throw new FormatException();
            }

            _sequence = _sequence.Slice(consumed);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ushort GetUInt16()
        {
            if (!Varint.TryGetUInt16(_sequence, out ushort result, out var consumed))
            {
                throw new FormatException();
            }

            _sequence = _sequence.Slice(consumed);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint GetUInt32()
        {
            if (!Varint.TryGetUInt32(_sequence, out uint result, out var consumed))
            {
                throw new FormatException();
            }

            _sequence = _sequence.Slice(consumed);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sbyte GetInt8()
        {
            if (!Varint.TryGetInt8(_sequence, out sbyte result, out var consumed))
            {
                throw new FormatException();
            }

            _sequence = _sequence.Slice(consumed);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public short GetInt16()
        {
            if (!Varint.TryGetInt16(_sequence, out short result, out var consumed))
            {
                throw new FormatException();
            }

            _sequence = _sequence.Slice(consumed);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetInt32()
        {
            if (!Varint.TryGetInt32(_sequence, out int result, out var consumed))
            {
                throw new FormatException();
            }

            _sequence = _sequence.Slice(consumed);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long GetInt64()
        {
            if (!Varint.TryGetInt64(_sequence, out long result, out var consumed))
            {
                throw new FormatException();
            }

            _sequence = _sequence.Slice(consumed);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetFloat32()
        {
            const int BytesLength = 4;
            Span<byte> bytes = stackalloc byte[BytesLength];

            _sequence.Slice(0, BytesLength).CopyTo(bytes);
            _sequence = _sequence.Slice(BytesLength);

            var f = new Float32Bits(bytes);

            return f.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetFloat64()
        {
            const int BytesLength = 8;
            Span<byte> bytes = stackalloc byte[BytesLength];

            _sequence.Slice(0, BytesLength).CopyTo(bytes);
            _sequence = _sequence.Slice(BytesLength);

            var f = new Float64Bits(bytes);

            return f.Value;
        }
    }
}
