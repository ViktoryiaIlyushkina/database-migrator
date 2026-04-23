# Используем SDK на всех этапах, так как для генерации скриптов миграций 
# EF Core нужно компилировать проект в процессе работы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем проекты с учетом вашей вложенности папок
COPY ["HybridMigrator/TestMigrator.HybridEfCoreMigrator/TestMigrator.HybridEfCoreMigrator.csproj", "HybridMigrator/TestMigrator.HybridEfCoreMigrator/"]
COPY ["HybridMigrator/TestMigrator.HybridShared/TestMigrator.HybridShared.csproj", "HybridMigrator/TestMigrator.HybridShared/"]

# Восстанавливаем зависимости
RUN dotnet restore "./HybridMigrator/TestMigrator.HybridEfCoreMigrator/TestMigrator.HybridEfCoreMigrator.csproj"

# Копируем все исходники
COPY . .

# Переходим в папку проекта мигратора
WORKDIR "/src/HybridMigrator/TestMigrator.HybridEfCoreMigrator"

# Устанавливаем инструмент EF Core глобально внутри контейнера
RUN dotnet tool install --global dotnet-ef
# Добавляем путь к инструментам dotnet в переменную PATH
ENV PATH="$PATH:/root/.dotnet/tools"

# Собираем проект
RUN dotnet build "./TestMigrator.HybridEfCoreMigrator.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Финальный этап (используем SDK, а не runtime)
FROM build AS final
WORKDIR /app
COPY --from=build /app/build .

# Создаем папку для скриптов и даем права (на случай запуска не от root)
RUN mkdir /scripts && chmod 777 /scripts

# Команда генерации скрипта.
# --idempotent позволяет запускать скрипт многократно без ошибок
# --output указывает путь внутри контейнера (который мы примонтируем к Volume)
ENTRYPOINT dotnet ef migrations script \
    --idempotent \
    --output /scripts/init.sql \
    --project /src/HybridMigrator/TestMigrator.HybridEfCoreMigrator/TestMigrator.HybridEfCoreMigrator.csproj \
    --context AppDbContext