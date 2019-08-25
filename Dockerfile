FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./ ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out WebAPI/WebAPI.csproj

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build-env /app/WebAPI/out .
ENTRYPOINT ["dotnet", "TIKSN.Lionize.HabiticaTaskProviderService.WebAPI.dll"]