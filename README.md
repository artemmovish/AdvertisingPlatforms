# Advertising Platforms API

RESTful веб-сервис для управления и поиска рекламных площадок. Сервис предоставляет функционал для загрузки данных о площадках из txt-файла и их последующего поиска по географическому местоположению.

## 🚀 Технологический стек

*   **.NET 8** - Основной фреймворк
*   **ASP.NET Core** - Веб-фреймворк
*   **Docker & Docker Compose** - Контейнеризация и оркестрация
*   **MediatR** - Реализация паттерна Mediator/CQRS
*   **CQRS** - Архитектурный паттерн (Command Query Responsibility Segregation)
*   **Swagger/OpenAPI** - Документирование API
*   **Result Pattern** -  Паттерн для обработки ошибок и возврата состояний операций (Success/Failure) в слое Application
*   **ApiResponse Pattern** - Стандартизированный формат ответов API для успешных результатов и ошибок
## 📋 Функциональность

*   **Поиск рекламных площадок (`GET /api/AdPlatforms/search`)**: Поиск всех площадок, связанных с указанным местоположением (например, городом или страной).
*   **Загрузка данных (`POST /api/AdPlatforms/upload`)**: Загрузка и обработка CSV-файла с данными о рекламных площадках. Поддерживается частичная обработка: успешно обработанные строки сохраняются в БД, а ошибки валидации возвращаются в ответе.

## 🛠️ Запуск проекта с помощью Docker Compose

Это самый простой способ запустить всё необходимое окружение (приложение и базу данных) одной командой.

### Предварительные требования

1.  Установите [Docker Desktop](https://www.docker.com/products/docker-desktop/) (или Docker Engine + Docker Compose на Linux).
2.  Убедитесь, что Docker запущен.
3.  (Опционально) Клонируйте репозиторий:
    ```bash
    git clone https://github.com/artemmovish/AdvertisingPlatforms.git
    cd AdvertisingPlatforms
    ```

### Инструкция по запуску

1.  В корне проекта (где находится файл `docker-compose.yml`) выполните команду:
    ```bash
    docker-compose up -d
    ```
    Флаг `-d` запускает контейнеры в фоновом режиме.

2.  Дождитесь завершения сборки образов и запуска контейнеров. Это может занять несколько минут при первом запуске.

3.  После успешного запуска веб-сервис будет доступен по адресу:  
    **http://localhost:5050**

## 📁 Структура проекта

```
AdvertisingPlatforms/
├── AdvertisingPlatforms.API/          # Веб-API с контроллерами
├── AdvertisingPlatforms.Application/   # Слой приложения (CQRS, сервисы)
├── AdvertisingPlatforms.Domain/        # Доменный слой (сущности, интерфейсы)
├── AdvertisingPlatforms.Infrastructure/# Слой инфраструктуры (репозитории)
├── docker-compose.yml                  # Конфигурация Docker Compose
```
