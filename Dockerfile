FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.5
WORKDIR /app
COPY ./ ./

ENTRYPOINT ["dotnet", "TIKSN.Lionize.HabiticaTaskProviderService.WebAPI.dll"]