FROM mcr.microsoft.com/dotnet/sdk:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

RUN apt-get update && apt-get install -y gosu && rm -rf /var/lib/apt/lists/*

FROM base AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["WebApi/WebApi.csproj", "WebApi/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "WebApi/WebApi.csproj"

COPY . .
WORKDIR "/src/WebApi"
RUN dotnet build "WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src/WebApi/entrypoint.sh .

RUN chmod +x ./entrypoint.sh
RUN chown -R $APP_UID:$APP_UID /app

ENTRYPOINT ["./entrypoint.sh"]