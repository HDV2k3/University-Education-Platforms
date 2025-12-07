# ================================
# Build stage - .NET 9 SDK
# ================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["UNIVERSITY.EDUCATION.PLATFORMS/UNIVERSITY.EDUCATION.PLATFORMS.csproj", "UNIVERSITY.EDUCATION.PLATFORMS/"]
COPY ["UNIVERSITY.EDUCATION.PLATFORMS.Application/UNIVERSITY.EDUCATION.PLATFORMS.Application.csproj", "UNIVERSITY.EDUCATION.PLATFORMS.Application/"]
COPY ["UNIVERSITY.EDUCATION.PLATFORMS.Domain/UNIVERSITY.EDUCATION.PLATFORMS.Domain.csproj", "UNIVERSITY.EDUCATION.PLATFORMS.Domain/"]
COPY ["UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure/UNIVERSITY.EDUCATION.PLATFORMS.Service.csproj", "UNIVERSITY.EDUCATION.PLATFORMS.Service/"]
COPY ["UNIVERSITY.EDUCATION.PLATFORMS.ServiceDefaults/UNIVERSITY.EDUCATION.PLATFORMS.ServiceDefaults.csproj", "UNIVERSITY.EDUCATION.PLATFORMS.ServiceDefaults/"]
COPY ["UNIVERSITY.EDUCATION.PLATFORMS.AppHost/UNIVERSITY.EDUCATION.PLATFORMS.AppHost.csproj", "UNIVERSITY.EDUCATION.PLATFORMS.AppHost/"]
COPY ["UNIVERSITY.EDUCATION.PLATFORMS.IntegrationTest/UNIVERSITY.EDUCATION.PLATFORMS.IntegrationTest.csproj", "UNIVERSITY.EDUCATION.PLATFORMS.IntegrationTest/"]
COPY ["UNIVERSITY.EDUCATION.PLATFORMS.UnitTests/UNIVERSITY.EDUCATION.PLATFORMS.UnitTests.csproj", "UNIVERSITY.EDUCATION.PLATFORMS.UnitTests/"]

# Restore dependencies
RUN dotnet restore "UNIVERSITY.EDUCATION.PLATFORMS/UNIVERSITY.EDUCATION.PLATFORMS.csproj" --disable-parallel

# Copy source code
COPY . .

# Build the project
WORKDIR "/src/UNIVERSITY.EDUCATION.PLATFORMS"
RUN dotnet build "UNIVERSITY.EDUCATION.PLATFORMS.csproj" -c Release -o /app/build

# ================================
# Publish stage
# ================================
FROM build AS publish
RUN dotnet publish "UNIVERSITY.EDUCATION.PLATFORMS.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ================================
# Runtime stage
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Install dependencies if needed (optional)
# RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "UNIVERSITY.EDUCATION.PLATFORMS.dll"]
