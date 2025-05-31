# Travel_agency

  Опис
Цей проєкт — це RESTful API для управління турами, бронюваннями, готелями та транспортом. Реалізовано з використанням .NET 7, Entity Framework Core, патернів Repository та Unit of Work, із підтримкою JWT-аутентифікації, AutoMapper, та Swagger для документування API. Проєкт розгортається у Docker-контейнерах разом із базою даних Microsoft SQL Server.

  Основні можливості
Управління турами: створення, редагування, видалення, перегляд.
Управління бронюваннями, готелями та транспортом.
Пошук та фільтрація турів за різними критеріями.
Аутентифікація та авторизація користувачів з ролями (Admin, Manager, User).
CRUD операції для основних ресурсів через окремі контролери.
Документування API за допомогою Swagger із підтримкою автентифікації.

  Технології
.NET 7 / ASP.NET Core Web API
Entity Framework Core (Code First, міграції)
Microsoft SQL Serve
AutoMapper
JWT Authentication
xUnit, AutoFixture, NSubstitute (тестування)
Docker, Docker Compose
Swagger (OpenAPI)

  Встановлення та запуск
Клонуйте репозиторій: git clone https://github.com/Artemian21/PI-224-1-2-Travel_agency.git
Налаштуйте змінні оточення у файлі appsettings.json або через Docker Compose для підключення до бази даних та JWT.

  Запустіть Docker-контейнери:
docker-compose up --build
API буде доступне за адресою http://localhost:8080 (або порт, вказаний у конфігурації).

Перейдіть до Swagger UI для тестування API: http://localhost:8080/swagger

  Використання
Використовуйте стандартні HTTP методи (GET, POST, PUT, DELETE) для роботи з ресурсами.
Для захищених ендпоінтів передавайте JWT токен у заголовку Authorization: Bearer <token>.
Авторизація реалізована через ролі: Administrator, Manager, User.

  Тестування
Виконуйте юніт-тести через xUnit. Використовуються AutoFixture для генерації тестових даних та NSubstitute для мокування залежностей.

  Структура проєкту
API — контролери та точки входу для HTTP запитів.
BLL — бізнес-логіка, сервіси.
DAL — робота з базою даних, репозиторії, конфігурація Entity Framework.
DTO — моделі передачі даних між шарами.
Tests — юніт-тести.

  Плани на майбутнє
Додати фронтенд інтерфейс для користувачів.
Розширити функціонал пошуку та фільтрації.
Впровадити більш гнучку систему ролей та прав доступу.

  Контакти
Якщо у вас виникли питання або пропозиції, будь ласка, зв’яжіться зі мною:
Email: artemyaremenko21@gmail.com
GitHub: https://github.com/Artemian21
