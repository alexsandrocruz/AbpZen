# Base image for production
FROM mcr.microsoft.com/dotnet/aspnet:9.0-noble AS base
USER app
WORKDIR /app
EXPOSE 8080
ENV DOTNET_EnableDiagnostics=0

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0-noble AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy local NuGet packages first
COPY nupkgs /nupkgs

# Copy source code
COPY . .
WORKDIR "/src/LeptonXDemoApp.Web"
RUN dotnet restore "LeptonXDemoApp.Web.csproj"
RUN dotnet build "LeptonXDemoApp.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LeptonXDemoApp.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LeptonXDemoApp.Web.dll"]
