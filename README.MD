# TestMigrator — многоформатная система миграций и API (Dapper / EF Core / Hybrid)

Этот репозиторий демонстрирует три разных подхода к работе с миграциями и API в .NET 8:

1. **Dapper + FluentMigrator**
2. **EF Core Migrations**
3. **Hybrid (EF Core → SQL → Seed → Dapper API)**

Каждый подход полностью изолирован, имеет собственный API, собственный мигратор и собственный `docker-compose`.

---

## Структура решения

```text
Solution
│
├── src/
│   ├── DapperMigrator/
│   │   ├── TestMigrator.DapperMigrator/        # Web API на Dapper
│   │   └── TestMigrator.DbMigrator/            # FluentMigrator мигратор
│   │
│   ├── EfCoreMigrator/
│   │   └── TestMigrator.EfCoreDefault/         # Web API на EF Core
│   │
│   ├── HybridMigrator/
│   │   ├── TestMigrator.HybridEfCoreMigrator/  # Генерация SQL из EF Core
│   │   ├── TestMigrator.HybridDapperMigrator/  # API на Dapper
│   │   └── TestMigrator.HybridShared/          # Общие модели/логика
│
├── deploy/
│   ├── docker-compose.dapper.yml
│   ├── docker-compose.efcoredefault.yml
│   ├── docker-compose.hybrid.yml
│   └── sql-scripts/
│       ├── 01_seed_data.sql
│       ├── 02_seed_companies.sql
│       ├── 03_seed_employees.sql
│       └── 04_seed_test.sql
│
└── README.md

```

## Варианты запуска
Проект поддерживает три независимых сценария, каждый со своим docker-compose.

## 1.  Dapper + FluentMigrator

### Сервисы
- Postgres
- FluentMigrator мигратор (`TestMigrator.DbMigrator`)
- Dapper Web API (`TestMigrator.DapperMigrator`)

### Логика запуска
1. Поднимается Postgres.
2. Запускается мигратор.
3. После успешного завершения — стартует API.

### Запуск
```bash
cd deploy
docker compose -f docker-compose.dapper.yml up --build
```

API доступно по адресу: `http://localhost:8081`

## 2. EF Core Migrations

### Сервисы
- Postgres
- Web API на EF Core (`TestMigrator.EfCoreDefault`)

### Логика запуска
1. Поднимается Postgres.
2. API стартует после готовности БД.
3. EF Core миграции применяются автоматически при старте API (если включено в коде).
    
### Запуск
```bash
cd deploy
docker compose -f docker-compose.efcoredefault.yml up --build
```

API доступно по адресу: `http://localhost:8080`

## 3. Hybrid (EF Core → SQL → Seed → Dapper API)
Самый сложный и интересный сценарий.

### Сервисы
- Postgres
- EF Core 
- SQL Generator (`TestMigrator.HybridEfCoreMigrator`)
- Postgres контейнер для применения SQL
- Seed-скрипты
- Dapper API (`TestMigrator.HybridDapperMigrator`)
    
### Полный pipeline
1. Поднимается Postgres.
2. EF Core генерирует SQL (init.sql).
3. SQL применяется к базе.
4. Выполняются seed-скрипты.
5. Запускается Dapper API.
    
### Запуск
```bash
cd deploy
docker compose -f docker-compose.hybrid.yml up --build
```

API доступно по адресу: `http://localhost:8082`

---

##  Проекты и их назначение

- `TestMigrator.DapperMigrator`
  - Web API на Dapper
  - Использует Npgsql и Dapper
  - Swagger включён
  - Подключение через ConnectionStrings__DefaultConnection

- `TestMigrator.DbMigrator`
  - Консольный мигратор на FluentMigrator
  - Читает appsettings.json
  - Применяет миграции к Postgres

- `TestMigrator.EfCoreDefault`
  - Web API на EF Core
  - Использует Npgsql.EntityFrameworkCore.PostgreSQL
  - Поддерживает миграции через CLI

- `TestMigrator.HybridEfCoreMigrator`
  - Генерирует SQL‑скрипт миграций EF Core
  - Скрипт сохраняется в volume /scripts/init.sql

- `TestMigrator.HybridDapperMigrator`
  - Web API на Dapper
  - Работает поверх схемы, созданной EF Core

- `TestMigrator.HybridShared`
  - Общие модели и логика для Hybrid‑подхода

---

## Используемые технологии

| Компонент        | Используется в       | Версия   |
|------------------|----------------------|----------|
| .NET             | Все проекты          | net8.0 |
| Dapper           | Dapper API           | 2.1.35 |
| FluentMigrator   | Dapper мигратор      | 5.2.0  |
| EF Core          | EF API + Hybrid      | 8.0.x  |
| Npgsql           | Все проекты          | 8.0.x  |
| Swagger          | Все API              | 6.6.2  |
| Docker Compose   | Все сценарии         | 3.8    |

---

## Seed‑данные

В папке deploy/sql-scripts находятся SQL‑файлы:
- 01_seed_data.sql
- 02_seed_companies.sql
- 03_seed_employees.sql
- 04_seed_test.sql

Они автоматически применяются в Hybrid‑сценарии.

---

## Полезные команды

Создать EF Core миграцию
dotnet ef migrations add InitialCreate -p src/EfCoreMigrator/TestMigrator.EfCoreDefault

Применить миграции вручную
dotnet ef database update -p src/EfCoreMigrator/TestMigrator.EfCoreDefault

Пересобрать контейнеры
docker compose down -v
docker compose up --build

---

## Скрипт полного удаления Docker‑ресурсов

Чтобы полностью очистить окружение (контейнеры, образы, тома, сети), можно использовать скрипт:

`cleanup-all.sh`
#!/bin/bash

echo "Stopping and removing all containers, images, volumes and networks for this project..."

docker compose -f deploy/docker-compose.dapper.yml down -v --rmi all --remove-orphans
docker compose -f deploy/docker-compose.efcoredefault.yml down -v --rmi all --remove-orphans
docker compose -f deploy/docker-compose.hybrid.yml down -v --rmi all --remove-orphans

echo "Done."

### Как использовать

1. Сохраните файл в папку scripts/:
   ```bash
   scripts/cleanup-all.sh
   ```
2. Сделайте его исполняемым:
   ```bash
   chmod +x scripts/cleanup-all.sh
   ```
3. Запустите:
   ```bash
   ./scripts/cleanup-all.sh
   ```

Скрипт выполняет следующие действия:
- Остановит все контейнеры
- Удалит контейнеры
- Удалит связанные образы
- Удалит тома
- Удалит сети
- Удалит "осиротевшие" ресурсы

> Идеально для полной очистки окружения перед новым запуском.

---

##  Лицензия

Проект демонстрационный. Используйте свободно.