using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System.Numerics;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Data.Serializers
{
    public class BigIntegerSerializer : SerializerBase<BigInteger>
    {
        public override BigInteger Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            string val = context.Reader.ReadString();
            return BigInteger.Parse(val);
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, BigInteger value)
        {
            context.Writer.WriteString(value.ToString());
        }
    }
}