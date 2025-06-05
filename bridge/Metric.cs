using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;

namespace BridgePayload
{
    public sealed partial class Metric : IMessage<Metric>
    {
        private static readonly MessageParser<Metric> _parser = new MessageParser<Metric>(() => new Metric());
        public static MessageParser<Metric> Parser { get { return _parser; } }

        // Lägg till Descriptor så att IMessage implementeras korrekt
        public static MessageDescriptor Descriptor
        {
            get { throw new NotImplementedException("Descriptor is not implemented in this manual class."); }
        }

        MessageDescriptor IMessage.Descriptor => Descriptor;

        public string Name { get; set; } = "";
        public double DoubleValue { get; set; }
        public long IntValue { get; set; }
        public string StringValue { get; set; } = "";

        public object Value
        {
            get
            {
                if (!string.IsNullOrEmpty(StringValue)) return StringValue;
                if (DoubleValue != 0) return DoubleValue;
                if (IntValue != 0) return IntValue;
                return null;
            }
        }

        public Metric() { }
        public Metric(Metric other)
        {
            Name = other.Name;
            DoubleValue = other.DoubleValue;
            IntValue = other.IntValue;
            StringValue = other.StringValue;
        }

        public Metric Clone() => new Metric(this);

        public void MergeFrom(CodedInputStream input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 10: Name = input.ReadString(); break;
                    case 17: DoubleValue = input.ReadDouble(); break;
                    case 24: IntValue = input.ReadInt64(); break;
                    case 34: StringValue = input.ReadString(); break;
                    default: input.SkipLastField(); break;
                }
            }
        }

        public void MergeFrom(Metric other)
        {
            if (!string.IsNullOrEmpty(other.Name)) Name = other.Name;
            if (other.DoubleValue != 0) DoubleValue = other.DoubleValue;
            if (other.IntValue != 0) IntValue = other.IntValue;
            if (!string.IsNullOrEmpty(other.StringValue)) StringValue = other.StringValue;
        }

        public void WriteTo(CodedOutputStream output)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                output.WriteRawTag(10);
                output.WriteString(Name);
            }
            if (DoubleValue != 0)
            {
                output.WriteRawTag(17);
                output.WriteDouble(DoubleValue);
            }
            if (IntValue != 0)
            {
                output.WriteRawTag(24);
                output.WriteInt64(IntValue);
            }
            if (!string.IsNullOrEmpty(StringValue))
            {
                output.WriteRawTag(34);
                output.WriteString(StringValue);
            }
        }

        public int CalculateSize()
        {
            int size = 0;
            if (!string.IsNullOrEmpty(Name))
                size += 1 + CodedOutputStream.ComputeStringSize(Name);
            if (DoubleValue != 0)
                size += 1 + 8;
            if (IntValue != 0)
                size += 1 + CodedOutputStream.ComputeInt64Size(IntValue);
            if (!string.IsNullOrEmpty(StringValue))
                size += 1 + CodedOutputStream.ComputeStringSize(StringValue);
            return size;
        }

        public bool Equals(Metric other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return Name == other.Name && DoubleValue == other.DoubleValue &&
                   IntValue == other.IntValue && StringValue == other.StringValue;
        }

        public override bool Equals(object obj) => Equals(obj as Metric);
        public override int GetHashCode() => Name.GetHashCode() ^ DoubleValue.GetHashCode() ^ IntValue.GetHashCode() ^ StringValue.GetHashCode();
    }
}
