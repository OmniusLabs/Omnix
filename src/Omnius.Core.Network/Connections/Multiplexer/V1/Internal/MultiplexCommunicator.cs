using System;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;
using Omnius.Core;

namespace Omnius.Core.Network.Connections.Multiplexer.V1.Internal
{
    /*
    internal sealed class MultiplexCommunicator
    {
        private readonly IConnection _connection;
        private readonly IBytesPool _bytesPool;

        private enum PacketType : byte
        {
            Connect,
            Accept,
            UpdateWindowSize,
            Data,
            Close,
            Error,
        }

        public MultiplexCommunicator(IConnection connection, IBytesPool bytesPool)
        {
            _connection = connection;
            _bytesPool = bytesPool;
        }

        public void DoEvents()
        {
            _connection.DoEvents();
        }

        public async ValueTask SendConnectMessageAsync(SessionConnectMessage connectMessage, CancellationToken cancellationToken = default)
        {
            await _connection.EnqueueAsync((bufferWriter) =>
            {
                OmniVarint.SetUInt64((byte)PacketType.Connect, bufferWriter);
                connectMessage.Export(bufferWriter, _bytesPool);
            }, cancellationToken);
        }

        public async ValueTask SendAcceptMessageAsync(SessionAcceptMessage acceptMessage, CancellationToken cancellationToken = default)
        {
            await _connection.EnqueueAsync((bufferWriter) =>
            {
                OmniVarint.SetUInt64((byte)PacketType.Connect, bufferWriter);
                acceptMessage.Export(bufferWriter, _bytesPool);
            }, cancellationToken);
        }

        public async ValueTask SendUpdateWindowSizeMessageAsync(SessionUpdateWindowSizeMessage updateWindowSizeMessage, CancellationToken cancellationToken = default)
        {
            await _connection.EnqueueAsync((bufferWriter) =>
            {
                OmniVarint.SetUInt64((byte)PacketType.Connect, bufferWriter);
                updateWindowSizeMessage.Export(bufferWriter, _bytesPool);
            }, cancellationToken);
        }

        public async ValueTask SendDataMessageAsync(SessionDataMessage dataMessage, CancellationToken cancellationToken = default)
        {
            await _connection.EnqueueAsync((bufferWriter) =>
            {
                OmniVarint.SetUInt64((byte)PacketType.Connect, bufferWriter);
                dataMessage.Export(bufferWriter, _bytesPool);
            }, cancellationToken);
        }

        public async ValueTask SendCloseMessageAsync(SessionCloseMessage closeMessage, CancellationToken cancellationToken = default)
        {
            await _connection.EnqueueAsync((bufferWriter) =>
            {
                OmniVarint.SetUInt64((byte)PacketType.Connect, bufferWriter);
                closeMessage.Export(bufferWriter, _bytesPool);
            }, cancellationToken);
        }

        public async ValueTask SendErrorMessageAsync(SessionErrorMessage errorMessage, CancellationToken cancellationToken = default)
        {
            await _connection.EnqueueAsync((bufferWriter) =>
            {
                OmniVarint.SetUInt64((byte)PacketType.Connect, bufferWriter);
                errorMessage.Export(bufferWriter, _bytesPool);
            }, cancellationToken);
        }

        public async ValueTask<MultiplexReceiveMessageResult?> ReceiveMessageAsync(CancellationToken cancellationToken = default)
        {
            MultiplexReceiveMessageResult? result = default;

            await _connection.DequeueAsync((sequence) =>
            {
                var reader = new SequenceReader<byte>(sequence);

                if (!OmniVarint.TryGetUInt64(ref reader, out var type))
                {
                    throw new FormatException();
                }

                sequence = sequence.Slice(reader.Position);

                switch ((PacketType)type)
                {
                    case PacketType.Connect:
                        result = new MultiplexReceiveMessageResult(connectMessage: SessionConnectMessage.Import(sequence, _bytesPool));
                        break;
                    case PacketType.Accept:
                        result = new MultiplexReceiveMessageResult(acceptMessage: SessionAcceptMessage.Import(sequence, _bytesPool));
                        break;
                    case PacketType.UpdateWindowSize:
                        result = new MultiplexReceiveMessageResult(updateWindowSizeMessage: SessionUpdateWindowSizeMessage.Import(sequence, _bytesPool));
                        break;
                    case PacketType.Data:
                        result = new MultiplexReceiveMessageResult(dataMessage: SessionDataMessage.Import(sequence, _bytesPool));
                        break;
                    case PacketType.Close:
                        result = new MultiplexReceiveMessageResult(closeMessage: SessionCloseMessage.Import(sequence, _bytesPool));
                        break;
                    case PacketType.Error:
                        result = new MultiplexReceiveMessageResult(errorMessage: SessionErrorMessage.Import(sequence, _bytesPool));
                        break;
                }
            }, cancellationToken);

            return result;
        }
    }
    */
}
