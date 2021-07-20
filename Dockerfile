FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.17
WORKDIR /app
COPY ./ ./

ENTRYPOINT ["dotnet", "TIKSN.Lionize.HabiticaTaskProviderService.WebAPI.dll"]