FROM mcr.microsoft.com/dotnet/aspnet:8.0.3-jammy-amd64 AS base
USER $APP_UID
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy-amd64 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Controllers/Controllers.csproj", "Controllers/"]
COPY ["Models/Models.csproj", "Models/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["Repositories/Repositories.csproj", "Repositories/"]
COPY ["EXE_API_Backend.sln", "."]
RUN dotnet restore "EXE_API_Backend.sln"
COPY . .
WORKDIR "/src"
RUN dotnet build "EXE_API_Backend.sln" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Controllers/Controllers.csproj" -c $BUILD_CONFIGURATION --runtime linux-x64 -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Controllers.dll"]
