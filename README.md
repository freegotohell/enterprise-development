# Разработка корпоративных приложений — лабораторная работа №3

## «ORM» — реализация объектно-реляционной модели и оркестрация

В рамках третьей лабораторной работы хранилище автосервиса переведено с InMemory-коллекций на реляционную базу данных **MySQL** с использованием **Entity Framework Core**. Для создания схемы и добавления начальных справочных данных используются миграции EF Core.

Запуск API и базы данных организован с помощью **.NET Aspire**. Aspire AppHost управляет жизненным циклом сервисов, передаёт API строку подключения к MySQL и ожидает готовности базы данных.

Вариант: 57.

---

## Предметная область

Проект моделирует работу автомобильного сервиса.

### Основные сущности

| Класс | Назначение |
|---|---|
| `Client` | Клиент автосервиса |
| `Car` | Автомобиль клиента |
| `Mechanic` | Механик автосервиса |
| `WorkType` | Вид выполняемой работы |
| `RepairOrder` | Заказ на ремонт |
| `OrderWork` | Связь заказа с выполняемой работой |
| `OrderMechanic` | Связь заказа с механиком |

## Архитектура решения

| Проект | Назначение |
|---|---|
| `AutoService.Api` | ASP.NET Core Web API: контроллеры, HTTP-маршруты, регистрация сервисов и запуск приложения |
| `AutoService.AppHost` | Aspire AppHost: оркестрация API и MySQL |
| `AutoService.ServiceDefaults` | Общие настройки Aspire: OpenTelemetry, health checks, service discovery и HTTP resilience |
| `AutoService.Application` | Сервисы и бизнес-логика приложения |
| `AutoService.Contracts` | DTO и интерфейсы сервисов |
| `AutoService.Domain` | Сущности предметной области и интерфейсы репозиториев |
| `AutoService.Domain.Shared` | Общие перечисления и типы |
| `AutoService.Infrastructure.EfCore` | `DbContext`, конфигурации EF Core, миграции и реализации репозиториев для MySQL |
| `AutoService.Infrastructure.InMemory` | InMemory-реализация репозиториев, сохранённая с лабораторной №2 |
| `AutoService.Tests` | Модульные тесты предметной области и сервисных запросов |

### Хранение данных

При запуске API используется `AutoService.Infrastructure.EfCore`. Регистрация `AutoServiceDbContext` и EF Core-репозиториев выполняется методом `AddEfCoreInfrastructure()`.

InMemory-инфраструктура не удалена: она сохранена для совместимости с предыдущей лабораторной работой и использования в тестах.

### Entity Framework Core

Контекст `AutoServiceDbContext` содержит наборы сущностей:

- `Clients`;
- `Cars`;
- `Mechanics`;
- `WorkTypes`;
- `RepairOrders`;
- `OrderMechanics`;
- `OrderWorks`.

Конфигурации сущностей вынесены в отдельные классы `IEntityTypeConfiguration<T>`. Для таблиц и столбцов MySQL используются имена в стиле `snake_case` (например, `repair_orders`, `full_name`, `client_id`). Продолжительность работ хранится в БД в минутах и преобразуется в `TimeSpan` на уровне EF Core.

Миграции в `AutoService.Infrastructure.EfCore/Migrations` создают схему, добавляют начальные виды работ и задают ограничения базы данных.

При запуске API вызывается `Database.MigrateAsync()`, поэтому миграции применяются автоматически. После этого `EfCoreDataSeeder` создаёт дополнительные тестовые данные с помощью Bogus, если таблица клиентов ещё пуста.

### Начальные тестовые данные

Начальная миграция добавляет десять предопределённых видов работ. `EfCoreDataSeeder` дополняет базу следующими данными:

- 10 клиентов;
- 10 автомобилей;
- 10 механиков — по два механика каждой специализации;
- 20 заказов на ремонт с работами и назначенными механиками.

Для категорий `Engine`, `Transmission`, `Electrical`, `Diagnostics` и `BodyRepair` выбирается механик соответствующей специализации. Для общих работ категории `Maintenance` может быть выбран любой механик.

## Связи между сущностями

```text
Client
 └── Car
      └── RepairOrder
           ├── OrderWork ─── WorkType
           └── OrderMechanic ─── Mechanic
```

- один `Client` может иметь несколько автомобилей;
- один `Car` может иметь несколько заказов на ремонт;
- `RepairOrder` связан с клиентом и автомобилем;
- заказ может содержать несколько работ через `OrderWork`;
- заказ может быть связан с несколькими механиками через `OrderMechanic`.

---

## REST API

Для основных сущностей реализован полный CRUD.

### Clients

```text
GET    /api/clients
GET    /api/clients/{id}
POST   /api/clients
PUT    /api/clients/{id}
DELETE /api/clients/{id}
```

Дополнительные запросы:

```text
GET /api/clients/{clientId}/cars
GET /api/clients/{clientId}/repairorders
GET /api/clients/repeated
```

### Cars

```text
GET    /api/cars
GET    /api/cars/{id}
POST   /api/cars
PUT    /api/cars/{id}
DELETE /api/cars/{id}
```

### Mechanics

```text
GET    /api/mechanics
GET    /api/mechanics/{id}
POST   /api/mechanics
PUT    /api/mechanics/{id}
DELETE /api/mechanics/{id}
```

Дополнительные запросы:

```text
GET /api/mechanics/{mechanicId}/clients
GET /api/mechanics/{mechanicId}/repairorders
GET /api/worktypes/{workTypeId}/mechanics
GET /api/mechanics/by-specialization?specialization=Engine
```

### WorkTypes

```text
GET    /api/worktypes
GET    /api/worktypes/{id}
POST   /api/worktypes
PUT    /api/worktypes/{id}
DELETE /api/worktypes/{id}
```

Дополнительные запросы:

```text
GET /api/worktypes/{workTypeId}/repairorders
GET /api/worktypes/top5
```

### RepairOrders

```text
GET    /api/repairorders
GET    /api/repairorders/{id}
POST   /api/repairorders
PUT    /api/repairorders/{id}
DELETE /api/repairorders/{id}
```

Дополнительные запросы:

```text
GET /api/clients/{clientId}/repairorders
GET /api/cars/{carId}/repairorders
GET /api/mechanics/{mechanicId}/repairorders
GET /api/worktypes/{workTypeId}/repairorders
GET /api/repairorders/{id}/totalcost
```

---

## Aspire и телеметрия

`AutoService.AppHost` объявляет ресурс MySQL и базу данных `autoservice`, после чего добавляет API как отдельный проект. Через `WithReference(database)` строка подключения передаётся API автоматически, а `WaitFor(database)` задаёт зависимость от готовности базы.

В `AutoService.Api` вызывается `builder.AddServiceDefaults()`. Общие настройки включают OpenTelemetry (логи, метрики и трассировки), health checks, service discovery и HTTP resilience. В среде разработки доступны health endpoints `/health` и `/alive`.

## Запуск проекта

Для запуска необходимы .NET SDK 10 и Docker-совместимая среда контейнеров.

Из корня решения выполните:

```powershell
dotnet run --project .\AutoService.AppHost
```

Aspire запустит MySQL и API. Состояние ресурсов, логи и телеметрию можно просматривать в Aspire Dashboard. При первом запуске API применит миграции и заполнит базу начальными данными.

Swagger UI доступен в среде разработки по адресу API, путь `/swagger`.

## Unit-тесты

Тесты находятся в проекте `AutoService.Tests`.

### `DomainTests`

Проверяют корректность тестовых данных и основных связей предметной области.

### `QueriesTests`

Сравнивают результаты аналитических LINQ-запросов и результаты их реализации через сервисы.