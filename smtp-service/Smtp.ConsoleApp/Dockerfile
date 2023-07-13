FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY Smtp.ConsoleApp.csproj Smtp.ConsoleApp/Smtp.ConsoleApp.csproj
RUN dotnet restore Smtp.ConsoleApp.csproj
COPY . .
WORKDIR /src/Smtp.ConsoleApp
RUN dotnet build Smtp.ConsoleApp.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Smtp.ConsoleApp.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Smtp.ConsoleApp.dll"]
