# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Tests/Tests.csproj", "Tests/"]
COPY ["DB/DB.csproj", "DB/"]
COPY ["Model/Model.csproj", "Model/"]
COPY ["Processors.Browser/Processors.Browser.csproj", "Processors.Browser/"]
COPY ["SmartLinks/SmartLinks.csproj", "SmartLinks/"]
COPY ["Processors.Language/Processors.Language.csproj", "Processors.Language/"]
COPY ["Processors.Time/Processors.Time.csproj", "Processors.Time/"]
COPY ["WebAPI/WebAPI.csproj", "WebAPI/"]
RUN dotnet restore "./Tests/Tests.csproj"
COPY . .
WORKDIR "/src/Tests"
RUN dotnet build "./Tests.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tests.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tests.dll"]