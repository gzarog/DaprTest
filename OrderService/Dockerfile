# Set the base image to use for the container
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

# Set the build environment
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OrderService.csproj", "./"]
RUN dotnet restore "OrderService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "OrderService.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "OrderService.csproj" -c Release -o /app/publish

# Set the final base image for the published application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderService.dll"]