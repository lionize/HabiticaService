using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Text;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Business
{
    public class BusinessMappingProfile : Profile
    {
        public BusinessMappingProfile(IDataProtectionProvider provider)
        {
            protector = provider.CreateProtector("Contoso.MyClass.v1");
        }
    }
}
