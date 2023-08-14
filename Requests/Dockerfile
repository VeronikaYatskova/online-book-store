FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["/Requests.BLL/Requests.BLL.csproj", "src/Requests.BLL/"]
COPY ["/Requests.DAL/Requests.DAL.csproj", "src/Requests.DAL/"]
COPY ["/Requests.API/Requests.API.csproj", "src/Requests.API/"]

RUN dotnet restore "src/Requests.API/Requests.API.csproj"
COPY . .

WORKDIR "/src/Requests.API"
RUN dotnet build -c Release -o app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT [ "dotnet", "Requests.API.dll" ]
