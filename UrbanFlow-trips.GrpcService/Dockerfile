FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UrbanFlow-trips.GrpcService/UrbanFlow-trips.GrpcService.csproj", "UrbanFlow-trips.GrpcService/"]
RUN dotnet restore "UrbanFlow-trips.GrpcService/UrbanFlow-trips.GrpcService.csproj"
COPY . .
WORKDIR "/src/UrbanFlow-trips.GrpcService"
RUN dotnet build "./UrbanFlow-trips.GrpcService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UrbanFlow-trips.GrpcService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UrbanFlow-trips.GrpcService.dll"]
