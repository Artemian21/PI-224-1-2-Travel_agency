# Використовуйте стандартне зображення для .NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Створення етапу для складання
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Travel_agency.PL/Travel_agency.PL.csproj", "Travel_agency.PL/"]
RUN dotnet restore "./Travel_agency.PL/Travel_agency.PL.csproj"
COPY . . 
WORKDIR "/src/Travel_agency.PL"
RUN dotnet build "./Travel_agency.PL.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Етап публікації
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Travel_agency.PL.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Робочий етап
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish . 

# Встановлюємо змінні середовища для підключення до бази даних
ENV ConnectionStrings__DefaultConnection "Server=mssql-server;Database=TravelAgencyDB;User=sa;Password=My21252006AA!;TrustServerCertificate=True"

# Запуск вашого застосунку
ENTRYPOINT ["dotnet", "Travel_agency.PL.dll"]