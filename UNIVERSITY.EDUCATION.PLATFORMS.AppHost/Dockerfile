# ====================================
# 1. Build stage
# ====================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy all csproj files
COPY ["UNIVERSITY.EDUCATION.PLATFORMS.AppHost/UNIVERSITY.EDUCATION.PLATFORMS.AppHost.csproj", "UNIVERSITY.EDUCATION.PLATFORMS.AppHost/"]
COPY ["UNIVERSITY.EDUCATION.PLATFORMS/UNIVERSITY.EDUCATION.PLATFORMS.csproj", "UNIVERSITY.EDUCATION.PLATFORMS/"]
COPY ["UNIVERSITY.EDUCATION.PLATFORMS.Application/UNIVERSITY.EDUCATION.PLATFORMS.Application.csproj", "UNIVERSITY.EDUCATION.PLATFORMS.Application/"]
COPY ["UNIVERSITY.EDUCATION.PLATFORMS.Domain/UNIVERSITY.EDUCATION.PLATFORMS.Domain.csproj", "UNIVERSITY.EDUCATION.PLATFORMS.Domain/"]
COPY ["UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure/UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.csproj", "UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure/"]
COPY ["UNIVERSITY.EDUCATION.PLATFORMS.ServiceDefaults/UNIVERSITY.EDUCATION.PLATFORMS.ServiceDefaults.csproj", "UNIVERSITY.EDUCATION.PLATFORMS.ServiceDefaults/"]

# Restore dependencies
RUN dotnet restore "UNIVERSITY.EDUCATION.PLATFORMS.AppHost/UNIVERSITY.EDUCATION.PLATFORMS.AppHost.csproj"

# Copy full source code
COPY . .

# Build AppHost
WORKDIR "/src/UNIVERSITY.EDUCATION.PLATFORMS.AppHost"
RUN dotnet build -c Release -o /app/build

# ====================================
# 2. Publish stage
# ====================================
FROM build AS publish
WORKDIR "/src/UNIVERSITY.EDUCATION.PLATFORMS.AppHost"
RUN dotnet publish -c Release -o /app/publish --no-restore

# ====================================
# 3. Final runtime image
# ====================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "UNIVERSITY.EDUCATION.PLATFORMS.AppHost.dll"]
