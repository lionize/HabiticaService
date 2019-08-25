namespace TIKSN.Lionize.HabiticaTaskProviderService.WebAPI.Options
{
    public class ServiceDiscoveryOptions
    {
        public ServiceInfo Identity { get; set; }

        public class ServiceInfo
        {
            public string BaseAddress { get; set; }
        }
    }
}