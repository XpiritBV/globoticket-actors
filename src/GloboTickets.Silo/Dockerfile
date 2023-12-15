FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 8082

#Orleans Default Ports
EXPOSE 11111 30000 

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

RUN --mount=type=secret,id=github_token \
    export GITHUB_TOKEN=$(cat /run/secrets/github_token) \
    && dotnet nuget add source --username USERNAME --password $GITHUB_TOKEN --store-password-in-clear-text --name github "https://nuget.pkg.github.com/xpiritbv/index.json"

COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["src/GloboTickets.Silo/GloboTickets.Silo.csproj", "src/GloboTickets.Silo/"]
COPY ["src/GloboTickets.Grains/GloboTickets.Grains.csproj", "src/GloboTickets.Grains/"]
COPY ["src/GloboTickets.Abstractions/GloboTickets.Abstractions.csproj", "src/GloboTickets.Abstractions/"]
RUN dotnet restore "src/GloboTickets.Silo/GloboTickets.Silo.csproj"
COPY . .
WORKDIR "/src/src/GloboTickets.Silo"
RUN dotnet build "GloboTickets.Silo.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "GloboTickets.Silo.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GloboTickets.Silo.dll"]