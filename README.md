---

# 🧳 Travel\_agency

---

## 📌 Опис

**Travel\_agency** — це повноцінний веб-додаток для управління турами, бронюваннями, готелями та транспортом. Він включає **бекенд (RESTful API на .NET 7)** та **фронтенд (React + Tailwind CSS, Vite)**, повністю докеризовані для зручного розгортання та масштабування.

### Ключові особливості реалізації:

* **Бекенд:**

  * **Платформа:** .NET 7
  * **ORM:** Entity Framework Core
  * **Архітектурні патерни:** Repository та Unit of Work
  * **Безпека:** підтримка JWT-аутентифікації
  * **Мапінг об'єктів:** AutoMapper
  * **Документування API:** Swagger

* **Фронтенд:**

  * **Бібліотека:** React
  * **CSS Framework:** Tailwind CSS
  * **Білд-система:** Vite
  * **Лінтинг:** ESLint
  * **Розгортання:** Docker + Nginx

* **Інфраструктура:**

  * **Docker Compose:** для запуску бекенду, фронтенду та бази даних як єдиного середовища
  * **База даних:** Microsoft SQL Server

---

## 🎯 Основні можливості

* **CRUD-операції** для турів, бронювань, готелів і транспорту
* **Пошук та фільтрація** турів за різними параметрами
* **JWT-аутентифікація** з підтримкою ролей: `Admin`, `Manager`, `User`
* **Інтерактивна документація API** через Swagger
* **Користувацький інтерфейс** для зручної взаємодії з API через браузер

---

## 🧰 Технології

### Бекенд

* .NET 7 / ASP.NET Core Web API
* Entity Framework Core (Code First)
* AutoMapper
* JWT Authentication
* Swagger
* xUnit, AutoFixture, NSubstitute
* Docker

### Фронтенд

* React + Vite
* Tailwind CSS
* ESLint
* Nginx
* Docker

---

## 🚀 Встановлення та запуск

1. **Клонуйте репозиторій:**

   ```bash
   git clone https://github.com/Artemian21/PI-224-1-2-Travel_agency.git
   cd PI-224-1-2-Travel_agency
   ```

2. **Запустіть усі сервіси через Docker Compose:**

   ```bash
   docker-compose up --build
   ```

   * **API буде доступне за адресою:** `http://localhost:8080`
   * **Swagger UI:** `http://localhost:8080/swagger`
   * **Фронтенд (інтерфейс користувача):** `http://localhost:5173` *(або інший порт, зазначений у `docker-compose.yml`)*

> Усі змінні оточення можна налаштувати у `.env` файлі або через `appsettings.json`/`nginx.conf` у відповідних сервісах.

---

## 📁 Структура проєкту

```
Travel_agency/
├── Travel_agency_frontend/   # React + Vite фронтенд-додаток
│   ├── public/
│   ├── src/
│   ├── Dockerfile
│   ├── nginx.conf
│   └── ...
│
├── Travel_agency.BLL/        # Бізнес-логіка
├── Travel_agency.Core/       # Спільні сутності та інтерфейси
├── Travel_agency.DataAccess/ # Репозиторії, EF Core
├── Travel_agency.PL/         # ASP.NET Core Web API (Presentation Layer)
├── Travel_agency.Tests/      # Юніт-тести для бекенду
├── Travel_agency.UI/         # (опційно) можливий інший представницький шар
│
├── docker-compose.yml        # Контейнеризація фронтенду, бекенду і БД
├── .env                      # Змінні середовища
├── Travel_agency.sln         # Файл рішення Visual Studio
└── README.md
```

---

## 🧪 Тестування

* **Фреймворк:** xUnit
* **Генерація даних:** AutoFixture
* **Мокування:** NSubstitute

Для запуску тестів:

```bash
cd Travel_agency.Tests
dotnet test
```

---

## 🔮 Плани на майбутнє

* ✅ **Додано фронтенд-додаток (React + Docker + Nginx)**
* 🔜 Розширити можливості фільтрації турів
* 🔜 Впровадити гнучке керування правами доступу через UI
* 🔜 CI/CD інтеграція для автоматичного деплою

---

## 📬 Контакти

* **Email:** [artemyaremenko21@gmail.com](mailto:artemyaremenko21@gmail.com)
* **GitHub:** [github.com/Artemian21](https://github.com/Artemian21)

---
