FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base

RUN apt-get update &&\
    apt-get install --no-install-recommends -y libc6 libicu-dev libfontconfig1 libgdiplus libharfbuzz0b libfreetype6 libpango-1.0-0 libpangocairo-1.0-0

COPY sharedsettings-docker.json .
ADD sharedsettings-docker.json sharedsettings.json
WORKDIR /appstation/backend/Netsoftware.Xanthos.Api
EXPOSE 60356
ENV ASPNETCORE_URLS=http://+:60356

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
COPY sharedsettings-docker.json .
ADD sharedsettings-docker.json sharedsettings.json
WORKDIR /appstation/backend
COPY ./appstation/backend .
WORKDIR /appstation/backend/Netsoftware.Xanthos.Api

RUN dotnet build Netsoftware.Xanthos.Api.csproj -o /app/build

# Pick up where a previous build stage left off
FROM build AS publish
# Publish the application and its dependencies to a folder for deployment
# UseAppHost=false to disable the generation of the native executable.
RUN dotnet publish Netsoftware.Xanthos.Api.csproj -o /app/publish /p:UseAppHost=false --no-restore

# Pick up where a previous base stage left off
FROM base AS final
# Set the working directory for the subsequent COPY command
WORKDIR /appstation/backend/Netsoftware.Xanthos.Api
# Copy from the publish image /app/publish directory to the container filesystem
COPY --from=publish /app/publish .

# Specify the command executed when the container is started
ENTRYPOINT ["dotnet", "Netsoftware.Xanthos.Api.dll"]
