FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["/Smtp.WebAPI/Smtp.WebAPI.csproj", "src/Smtp.WebAPI/"]

RUN dotnet restore "src/Smtp.WebAPI/Smtp.WebAPI.csproj"

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app .

ENTRYPOINT ["dotnet", "Smtp.WebAPI.dll"]
