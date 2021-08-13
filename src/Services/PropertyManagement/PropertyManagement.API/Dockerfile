#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
# It's important to keep lines from here down to "COPY . ." identical in all Dockerfiles
# to take advantage of Docker's build cache, to speed up local container builds
COPY ["SmartApartmentEnterprise.sln", "SmartApartmentEnterprise.sln"]
COPY ["Services/Identity/Identity.API/Identity.API.csproj", "Services/Identity/Identity.API/Identity.API.csproj"]
COPY ["Services/PropertyManagement/PropertyManagement.API/PropertyManagement.API.csproj", "Services/PropertyManagement/PropertyManagement.API/PropertyManagement.API.csproj"]
COPY ["Web/SmartApartmentEnterprise.SPA/SmartApartmentEnterprise.SPA.csproj", "Web/SmartApartmentEnterprise.SPA/SmartApartmentEnterprise.SPA.csproj"]
COPY ["docker-compose.dcproj", "docker-compose.dcproj"]

RUN dotnet restore "SmartApartmentEnterprise.sln"
COPY . .
WORKDIR /src/Services/PropertyManagement/PropertyManagement.API/
RUN dotnet publish --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/ .
ENTRYPOINT ["dotnet", "PropertyManagement.API.dll"]
