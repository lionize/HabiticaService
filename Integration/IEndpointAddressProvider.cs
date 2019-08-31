using System;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Integration
{
    public interface IEndpointAddressProvider
    {
        Uri GetEndpointAddress(string queueName);
    }
}