FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["/Smtp.WebApi/smptp-pdf-generation.csproj", "src/Smtp.WebApi/"]

RUN dotnet restore "src/Smtp.WebApi/smptp-pdf-generation.csproj"
COPY . .

RUN dotnet build "smptp-pdf-generation.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "smptp-pdf-generation.csproj" -c Release -o /app

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app .

ENTRYPOINT ["dotnet", "smptp-pdf-generation.csproj"]
