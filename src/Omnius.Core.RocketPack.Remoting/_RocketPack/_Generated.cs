
#nullable enable

namespace Omnius.Core.RocketPack.Remoting
{

    public sealed partial class RocketPackRpcErrorMessage : global::Omnius.Core.RocketPack.IRocketPackObject<global::Omnius.Core.RocketPack.Remoting.RocketPackRpcErrorMessage>
    {
        public static global::Omnius.Core.RocketPack.IRocketPackObjectFormatter<global::Omnius.Core.RocketPack.Remoting.RocketPackRpcErrorMessage> Formatter => global::Omnius.Core.RocketPack.IRocketPackObject<global::Omnius.Core.RocketPack.Remoting.RocketPackRpcErrorMessage>.Formatter;
        public static global::Omnius.Core.RocketPack.Remoting.RocketPackRpcErrorMessage Empty => global::Omnius.Core.RocketPack.IRocketPackObject<global::Omnius.Core.RocketPack.Remoting.RocketPackRpcErrorMessage>.Empty;

        static RocketPackRpcErrorMessage()
        {
            global::Omnius.Core.RocketPack.IRocketPackObject<global::Omnius.Core.RocketPack.Remoting.RocketPackRpcErrorMessage>.Formatter = new ___CustomFormatter();
            global::Omnius.Core.RocketPack.IRocketPackObject<global::Omnius.Core.RocketPack.Remoting.RocketPackRpcErrorMessage>.Empty = new global::Omnius.Core.RocketPack.Remoting.RocketPackRpcErrorMessage(string.Empty, string.Empty, null);
        }

        private readonly global::System.Lazy<int> ___hashCode;

        public static readonly int MaxTypeLength = 8192;
        public static readonly int MaxMessageLength = 8192;
        public static readonly int MaxStackTraceLength = 8192;

        public RocketPackRpcErrorMessage(string type, string message, string? stackTrace)
        {
            if (type is null) throw new global::System.ArgumentNullException("type");
            if (type.Length > 8192) throw new global::System.ArgumentOutOfRangeException("type");
            if (message is null) throw new global::System.ArgumentNullException("message");
            if (message.Length > 8192) throw new global::System.ArgumentOutOfRangeException("message");
            if (!(stackTrace is null) && stackTrace.Length > 8192) throw new global::System.ArgumentOutOfRangeException("stackTrace");

            this.Type = type;
            this.Message = message;
            this.StackTrace = stackTrace;

            ___hashCode = new global::System.Lazy<int>(() =>
            {
                var ___h = new global::System.HashCode();
                if (type != default) ___h.Add(type.GetHashCode());
                if (message != default) ___h.Add(message.GetHashCode());
                if (stackTrace != default) ___h.Add(stackTrace.GetHashCode());
                return ___h.ToHashCode();
            });
        }

        public string Type { get; }
        public string Message { get; }
        public string? StackTrace { get; }

        public static global::Omnius.Core.RocketPack.Remoting.RocketPackRpcErrorMessage Import(global::System.Buffers.ReadOnlySequence<byte> sequence, global::Omnius.Core.IBytesPool bytesPool)
        {
            var reader = new global::Omnius.Core.RocketPack.RocketPackObjectReader(sequence, bytesPool);
            return Formatter.Deserialize(ref reader, 0);
        }
        public void Export(global::System.Buffers.IBufferWriter<byte> bufferWriter, global::Omnius.Core.IBytesPool bytesPool)
        {
            var writer = new global::Omnius.Core.RocketPack.RocketPackObjectWriter(bufferWriter, bytesPool);
            Formatter.Serialize(ref writer, this, 0);
        }

        public static bool operator ==(global::Omnius.Core.RocketPack.Remoting.RocketPackRpcErrorMessage? left, global::Omnius.Core.RocketPack.Remoting.RocketPackRpcErrorMessage? right)
        {
            return (right is null) ? (left is null) : right.Equals(left);
        }
        public static bool operator !=(global::Omnius.Core.RocketPack.Remoting.RocketPackRpcErrorMessage? left, global::Omnius.Core.RocketPack.Remoting.RocketPackRpcErrorMessage? right)
        {
            return !(left == right);
        }
        public override bool Equals(object? other)
        {
            if (!(other is global::Omnius.Core.RocketPack.Remoting.RocketPackRpcErrorMessage)) return false;
            return this.Equals((global::Omnius.Core.RocketPack.Remoting.RocketPackRpcErrorMessage)other);
        }
        public bool Equals(global::Omnius.Core.RocketPack.Remoting.RocketPackRpcErrorMessage? target)
        {
            if (target is null) return false;
            if (object.ReferenceEquals(this, target)) return true;
            if (this.Type != target.Type) return false;
            if (this.Message != target.Message) return false;
            if (this.StackTrace != target.StackTrace) return false;

            return true;
        }
        public override int GetHashCode() => ___hashCode.Value;

        private sealed class ___CustomFormatter : global::Omnius.Core.RocketPack.IRocketPackObjectFormatter<global::Omnius.Core.RocketPack.Remoting.RocketPackRpcErrorMessage>
        {
            public void Serialize(ref global::Omnius.Core.RocketPack.RocketPackObjectWriter w, in global::Omnius.Core.RocketPack.Remoting.RocketPackRpcErrorMessage value, in int rank)
            {
                if (rank > 256) throw new global::System.FormatException();

                if (value.Type != string.Empty)
                {
                    w.Write((uint)1);
                    w.Write(value.Type);
                }
                if (value.Message != string.Empty)
                {
                    w.Write((uint)2);
                    w.Write(value.Message);
                }
                if (value.StackTrace != null)
                {
                    w.Write((uint)3);
                    w.Write(value.StackTrace);
                }
                w.Write((uint)0);
            }

            public global::Omnius.Core.RocketPack.Remoting.RocketPackRpcErrorMessage Deserialize(ref global::Omnius.Core.RocketPack.RocketPackObjectReader r, in int rank)
            {
                if (rank > 256) throw new global::System.FormatException();

                string p_type = string.Empty;
                string p_message = string.Empty;
                string? p_stackTrace = null;

                for (; ; )
                {
                    uint id = r.GetUInt32();
                    if (id == 0) break;
                    switch (id)
                    {
                        case 1:
                            {
                                p_type = r.GetString(8192);
                                break;
                            }
                        case 2:
                            {
                                p_message = r.GetString(8192);
                                break;
                            }
                        case 3:
                            {
                                p_stackTrace = r.GetString(8192);
                                break;
                            }
                    }
                }

                return new global::Omnius.Core.RocketPack.Remoting.RocketPackRpcErrorMessage(p_type, p_message, p_stackTrace);
            }
        }
    }


}