FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Копируем csproj файлы и восстанавливаем зависимости
COPY ["Tests/Tests.csproj", "Tests/"]
COPY ["DB/DB.csproj", "DB/"]
COPY ["Model/Model.csproj", "Model/"]
COPY ["Processors.Browser/Processors.Browser.csproj", "Processors.Browser/"]
COPY ["SmartLinks/SmartLinks.csproj", "SmartLinks/"]
COPY ["Processors.Language/Processors.Language.csproj", "Processors.Language/"]
COPY ["Processors.Time/Processors.Time.csproj", "Processors.Time/"]
COPY ["WebAPI/WebAPI.csproj", "WebAPI/"]
RUN dotnet restore "Tests/Tests.csproj"

# Копируем весь проект
COPY . .

# Собираем проект
RUN dotnet build "Tests/Tests.csproj" -c Release -o /build

# Запускаем тесты с покрытием
RUN dotnet test "Tests/Tests.csproj" \
    --configuration Release \
    /p:CollectCoverage=true \
    /p:CoverletOutputFormat=opencover \
    /p:CoverletOutput=/coverage/

# Устанавливаем reportgenerator глобально
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS reportgen

WORKDIR /app

RUN dotnet tool install --global dotnet-reportgenerator-globaltool

# Команда по умолчанию — генерировать отчет (можно переопределить)
CMD ["bash", "-c", "export PATH=\"$PATH:/root/.dotnet/tools\" && reportgenerator -reports:\"/coverage/coverage.opencover.xml\" -targetdir:\"/output\" -reporttypes:HtmlSummary"]