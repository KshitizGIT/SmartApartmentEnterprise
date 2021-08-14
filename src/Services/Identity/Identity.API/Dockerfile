#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
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

WORKDIR "/src/Services/Identity/Identity.API"
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet run /seed
RUN dotnet publish --no-restore -c Release -o /app
RUN mv AspIdUsers.db /app/


FROM build as publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/ .
ENTRYPOINT ["dotnet", "Identity.API.dll"]
