# Используем базовый образ ASP.NET 10.0
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

# Рабочая директория внутри контейнера
WORKDIR /app

# Используем .NET 10 SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Копируем файлы проекта в контейнер
COPY . /src

# Рабочая директория для сборки
WORKDIR /src

# Восстанавливаем зависимости
RUN dotnet restore

# Собираем приложение
RUN dotnet build "./7_Calculator.csproj" -c Release -o /app/build

# Публикуем приложение
FROM build AS publish
RUN dotnet publish "./7_Calculator.csproj" -c Release -o /app/publish

# Финальный образ
FROM base AS final
WORKDIR /app

# Копируем опубликованное приложение
COPY --from=publish /app/publish ./

# Порт приложения внутри контейнера
EXPOSE 5007

# Запускаем приложение
ENTRYPOINT ["dotnet", "7_Calculator.dll"]