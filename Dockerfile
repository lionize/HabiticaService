FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.9
WORKDIR /app
COPY ./ ./

ENTRYPOINT ["dotnet", "TIKSN.Lionize.HabiticaTaskProviderService.WebAPI.dll"]