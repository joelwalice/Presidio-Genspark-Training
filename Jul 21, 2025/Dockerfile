# Set .NET version
ARG DOTNET_VERSION=9.0

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
WORKDIR /src

# Copy only the csproj first
COPY src/AppDeployment/AppDeployment.csproj ./AppDeployment/
RUN dotnet restore ./AppDeployment/AppDeployment.csproj

# Copy the entire source
COPY src/AppDeployment ./AppDeployment/

WORKDIR /src/AppDeployment
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS final
WORKDIR /app
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "AppDeployment.dll"]
