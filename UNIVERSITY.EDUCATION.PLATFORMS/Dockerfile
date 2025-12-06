# ====================================
# 1. Build stage
# ====================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["UNIVERSITY.EDUCATION.PLATFORMS/UNIVERSITY.EDUCATION.PLATFORMS.csproj", "UNIVERSITY.EDUCATION.PLATFORMS/"]
COPY ["UNIVERSITY.EDUCATION.PLATFORMS.Application/UNIVERSITY.EDUCATION.PLATFORMS.Application.csproj", "UNIVERSITY.EDUCATION.PLATFORMS.Application/"]
COPY ["UNIVERSITY.EDUCATION.PLATFORMS.Domain/UNIVERSITY.EDUCATION.PLATFORMS.Domain.csproj", "UNIVERSITY.EDUCATION.PLATFORMS.Domain/"]
COPY ["UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure/UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.csproj", "UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure/"]
COPY ["UNIVERSITY.EDUCATION.PLATFORMS.ServiceDefaults/UNIVERSITY.EDUCATION.PLATFORMS.ServiceDefaults.csproj", "UNIVERSITY.EDUCATION.PLATFORMS.ServiceDefaults/"]

RUN dotnet restore "UNIVERSITY.EDUCATION.PLATFORMS/UNIVERSITY.EDUCATION.PLATFORMS.csproj"
COPY . .

WORKDIR "/src/UNIVERSITY.EDUCATION.PLATFORMS"
RUN dotnet publish -c Release -o /app/publish

# ====================================
# Runtime
# ====================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "UNIVERSITY.EDUCATION.PLATFORMS.dll"]
