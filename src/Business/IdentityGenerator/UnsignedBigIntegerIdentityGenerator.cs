using Microsoft.Extensions.Options;
using System;
using System.Numerics;
using TIKSN.Serialization.Numerics;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Business.IdentityGenerator
{
    public class UnsignedBigIntegerIdentityGenerator : IIdentityGenerator<BigInteger>
    {
        private readonly Random _random;
        private readonly UnsignedBigIntegerBinaryDeserializer _unsignedBigIntegerBinaryDeserializer;
        private readonly IOptions<UnsignedBigIntegerIdentityGeneratorOptions> _unsignedBigIntegerIdentityGeneratorOptions;

        public UnsignedBigIntegerIdentityGenerator(Random random, IOptions<UnsignedBigIntegerIdentityGeneratorOptions> unsignedBigIntegerIdentityGeneratorOptions, UnsignedBigIntegerBinaryDeserializer unsignedBigIntegerBinaryDeserializer)
        {
            _unsignedBigIntegerBinaryDeserializer = unsignedBigIntegerBinaryDeserializer ?? throw new ArgumentNullException(nameof(unsignedBigIntegerBinaryDeserializer));
            _random = random ?? throw new ArgumentNullException(nameof(random));
            _unsignedBigIntegerIdentityGeneratorOptions = unsignedBigIntegerIdentityGeneratorOptions ?? throw new ArgumentNullException(nameof(unsignedBigIntegerIdentityGeneratorOptions));
        }

        public BigInteger Generate()
        {
            var buffer = new byte[_unsignedBigIntegerIdentityGeneratorOptions.Value.ByteLength];
            _random.NextBytes(buffer);
            return _unsignedBigIntegerBinaryDeserializer.Deserialize(buffer);
        }
    }
}