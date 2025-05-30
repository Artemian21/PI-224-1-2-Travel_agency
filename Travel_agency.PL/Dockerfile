FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Travel_agency.PL/Travel_agency.PL.csproj", "Travel_agency.PL/"]
RUN dotnet restore "./Travel_agency.PL/Travel_agency.PL.csproj"
COPY . .
WORKDIR "/src/Travel_agency.PL"
RUN dotnet build "./Travel_agency.PL.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Travel_agency.PL.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Travel_agency.PL.dll"]