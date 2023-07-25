FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["/Profiles.Application/Profiles.Application.csproj", "src/Profiles.Application/"]
COPY ["/Profiles.Domain/Profiles.Domain.csproj", "src/Profiles.Domain/"]
COPY ["/Profiles.Infrastructure/Profiles.Infrastructure.csproj", "src/Profiles.Infrastructure/"]
COPY ["/Profiles.API/Profiles.API.csproj", "src/Profiles.API/"]

RUN dotnet restore "src/Profiles.API/Profiles.API.csproj"
COPY . .

WORKDIR "/src/Profiles.API"
RUN dotnet build -c Release -o app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT [ "dotnet", "Profiles.API.dll" ]