# ================================
# Build stage - .NET 9 SDK
# ================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files of ALL required projects
COPY ["UNIVERSITY.EDUCATION.PLATFORMS/UNIVERSITY.EDUCATION.PLATFORMS.csproj", "UNIVERSITY.EDUCATION.PLATFORMS/"]
COPY ["UNIVERSITY.EDUCATION.PLATFORMS.Application/UNIVERSITY.EDUCATION.PLATFORMS.Application.csproj", "UNIVERSITY.EDUCATION.PLATFORMS.Application/"]
COPY ["UNIVERSITY.EDUCATION.PLATFORMS.Domain/UNIVERSITY.EDUCATION.PLATFORMS.Domain.csproj", "UNIVERSITY.EDUCATION.PLATFORMS.Domain/"]
COPY ["UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure/UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.csproj", "UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure/"]
COPY ["UNIVERSITY.EDUCATION.PLATFORMS.ServiceDefaults/UNIVERSITY.EDUCATION.PLATFORMS.ServiceDefaults.csproj", "UNIVERSITY.EDUCATION.PLATFORMS.ServiceDefaults/"]
COPY ["UNIVERSITY.EDUCATION.PLATFORMS.Web/UNIVERSITY.EDUCATION.PLATFORMS.Web.csproj", "UNIVERSITY.EDUCATION.PLATFORMS.Web/"]
COPY ["UNIVERSITY.EDUCATION.PLATFORMS.AppHost/UNIVERSITY.EDUCATION.PLATFORMS.AppHost.csproj", "UNIVERSITY.EDUCATION.PLATFORMS.AppHost/"]

# Restore dependencies but target AppHost
RUN dotnet restore "UNIVERSITY.EDUCATION.PLATFORMS.AppHost/UNIVERSITY.EDUCATION.PLATFORMS.AppHost.csproj"

# Copy full source code
COPY . .

# Build AppHost project
WORKDIR "/src/UNIVERSITY.EDUCATION.PLATFORMS.AppHost"
RUN dotnet build "UNIVERSITY.EDUCATION.PLATFORMS.AppHost.csproj" -c Release -o /app/build

# ================================
# Publish stage
# ================================
FROM build AS publish
RUN dotnet publish "UNIVERSITY.EDUCATION.PLATFORMS.AppHost.csproj" -c Release -o /app/publish --no-restore /p:UseAppHost=false

# ================================
# Runtime stage (ASP.NET Core 9)
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Run AppHost, NOT the Web project
ENTRYPOINT ["dotnet", "UNIVERSITY.EDUCATION.PLATFORMS.AppHost.dll"]
