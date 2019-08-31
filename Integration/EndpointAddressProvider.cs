using Microsoft.Extensions.Configuration;
using System;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Integration
{
    public class EndpointAddressProvider : IEndpointAddressProvider
    {
        private readonly IConfigurationRoot _configurationRoot;

        public EndpointAddressProvider(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot ?? throw new ArgumentNullException(nameof(configurationRoot));
        }

        public Uri GetEndpointAddress(string queueName)
        {
            var connectionString = _configurationRoot.GetConnectionString("RabbitMQ");
            return new Uri($"{connectionString}/{queueName}");
        }
    }
}