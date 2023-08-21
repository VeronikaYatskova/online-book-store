FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["BookStore/.", "./"]

RUN dotnet restore "BookStore.Api/BookStore.Api.csproj"
RUN dotnet build "BookStore.Api/BookStore.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookStore.Api/BookStore.Api.csproj" -c Release -o /app/publish

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app/publish .
RUN ls -l

ENTRYPOINT ["dotnet", "BookStore.Api.dll"]
