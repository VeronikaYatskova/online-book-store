FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["/BookStore/.", "src/"]

RUN dotnet restore "src/BookStore.Api/BookStore.Api.csproj"
COPY . .

WORKDIR "/src/BookStore/"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "BookStore.Api.dll"]
