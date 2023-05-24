# Set the base image to use for the container
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8081

# Set the build environment
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["InventoryService.csproj", "./"]
RUN dotnet restore "InventoryService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "InventoryService.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "InventoryService.csproj" -c Release -o /app/publish

# Set the final base image for the published application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InventoryService.dll"]