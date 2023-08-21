FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["Requests.BLL/Requests.BLL.csproj", "Requests.BLL/"]
COPY ["Requests.DAL/Requests.DAL.csproj", "Requests.DAL/"]
COPY ["Requests.API/Requests.API.csproj", "Requests.API/"]

RUN dotnet restore "Requests.API/Requests.API.csproj"
COPY . .

WORKDIR "/src/Requests.API"
RUN dotnet build "Requests.API.csproj" -c Release -o app/build

FROM build AS publish
RUN dotnet publish "Requests.API.csproj" -c Release -o /app/publish

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT [ "dotnet", "Requests.API.dll" ]
