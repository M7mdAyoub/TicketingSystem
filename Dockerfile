# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY HelpdeskApp.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# Create data directory for SQLite
RUN mkdir -p /app/Data

ENV ASPNETCORE_URLS=http://+:7860
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ConnectionStrings__DefaultConnection="Data Source=/app/Data/helpdesk.db"

EXPOSE 7860

ENTRYPOINT ["dotnet", "HelpdeskApp.dll"]
