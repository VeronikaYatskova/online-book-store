FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["/Application/Application.csproj", "src/Application/"]
COPY ["/Domain/Domain.csproj", "src/Domain/"]
COPY ["/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY ["/WebApi/WebApi.csproj", "src/WebApi/"]

RUN dotnet restore "WebApi/WebApi.csproj"
COPY . .

WORKDIR "/src/WebApi/"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app/publish .
RUN ls -l

ENTRYPOINT ["dotnet", "WebApi.dll"]
