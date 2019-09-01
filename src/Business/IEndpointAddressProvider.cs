using System;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Business
{
    public interface IEndpointAddressProvider
    {
        Uri GetEndpointAddress(string queueName);
    }
}