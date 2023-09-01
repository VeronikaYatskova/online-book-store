FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["Requests/.", "./"]

RUN dotnet restore "Requests.API/Requests.API.csproj"
RUN dotnet build "Requests.API/Requests.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Requests.API/Requests.API.csproj" -c Release -o /app/publish

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app/publish .
RUN ls -l

ENTRYPOINT ["dotnet", "Requests.API.dll"]
