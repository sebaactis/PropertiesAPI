# Base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 90

# Build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar archivos de proyecto
COPY ["AuthService/AuthService.csproj", "AuthService/"]

# Restaurar dependencias
RUN dotnet restore "AuthService/AuthService.csproj"

# Copiar todo el código
COPY . .

# Build
WORKDIR "/src/AuthService"
RUN dotnet build "AuthService.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "AuthService.csproj" -c Release -o /app/publish

# Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthService.dll"]