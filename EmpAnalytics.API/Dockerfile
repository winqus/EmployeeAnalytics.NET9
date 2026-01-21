FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EmpAnalytics.API/EmpAnalytics.API.csproj", "EmpAnalytics.API/"]
RUN dotnet restore "EmpAnalytics.API/EmpAnalytics.API.csproj"
COPY . .
WORKDIR "/src/EmpAnalytics.API"
RUN dotnet build "./EmpAnalytics.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EmpAnalytics.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmpAnalytics.API.dll"]
