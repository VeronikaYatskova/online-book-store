FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM base AS runtime
WORKDIR /app

COPY --from=base /app/out .

ENTRYPOINT ["dotnet", "Smtp.ConsoleApp.dll"]
