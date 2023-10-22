FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["Auth/.", "./"]

RUN dotnet restore "Auth.API/Auth.API.csproj"
RUN dotnet build "Auth.API/Auth.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Auth.API/Auth.API.csproj" -c Release -o /app/publish

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Auth.API.dll"]
