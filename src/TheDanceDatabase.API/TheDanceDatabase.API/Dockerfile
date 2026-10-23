# syntax=docker/dockerfile:1
# Build context is src/TheDanceDatabase.API, the folder that holds global.json and the shared props files.
# From that folder: docker build -f TheDanceDatabase.API/Dockerfile .
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Restore needs the SDK pin, the target framework and the central package versions, not only the csproj
COPY global.json Directory.Build.props Directory.Packages.props ./
COPY TheDanceDatabase.API/TheDanceDatabase.API.csproj TheDanceDatabase.API/
RUN dotnet restore TheDanceDatabase.API/TheDanceDatabase.API.csproj

COPY TheDanceDatabase.API/ TheDanceDatabase.API/
RUN dotnet publish TheDanceDatabase.API/TheDanceDatabase.API.csproj -c Release -o /app/publish --no-restore /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# The base image listens on 8080 (ASPNETCORE_HTTP_PORTS) and provides the non-root APP_UID user
USER $APP_UID
EXPOSE 8080
ENTRYPOINT ["dotnet", "TheDanceDatabase.API.dll"]
