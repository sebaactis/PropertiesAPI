# Base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 90

# Build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar archivos de proyecto
COPY ["Gateway/Gateway.csproj", "Gateway/"]

# Restaurar dependencias
RUN dotnet restore "Gateway/Gateway.csproj"

# Copiar todo el código
COPY . .

# Build
WORKDIR "/src/Gateway"
RUN dotnet build "Gateway.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "Gateway.csproj" -c Release -o /app/publish

# Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gateway.dll"]