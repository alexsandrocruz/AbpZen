FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy local NuGet packages first
COPY nupkgs /nupkgs

# Copy source code
COPY . .
WORKDIR "/src/LeptonXDemoApp.HttpApi.Host"
RUN dotnet restore "LeptonXDemoApp.HttpApi.Host.csproj"
RUN dotnet build "LeptonXDemoApp.HttpApi.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LeptonXDemoApp.HttpApi.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LeptonXDemoApp.HttpApi.Host.dll"]
