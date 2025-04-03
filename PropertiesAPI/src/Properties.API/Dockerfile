# Base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# Build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia los archivos .csproj con rutas corregidas
COPY Properties.API/Properties.API.csproj Properties.API/
COPY Properties.Application/Properties.Application.csproj Properties.Application/
COPY Properties.Core/Properties.Core.csproj Properties.Core/
COPY Properties.Infrastructure/Properties.Infrastructure.csproj Properties.Infrastructure/

# Restaura las dependencias
RUN dotnet restore "Properties.API/Properties.API.csproj"

# Copia el resto de los archivos
COPY . .

# Compila el proyecto
RUN dotnet build "Properties.API/Properties.API.csproj" -c Release -o /app/build

# Publish image
FROM build AS publish
RUN dotnet publish "Properties.API/Properties.API.csproj" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Properties.API.dll"]