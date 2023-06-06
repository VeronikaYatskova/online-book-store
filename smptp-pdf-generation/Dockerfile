FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["smptp-pdf-generation.csproj", "src/smptp-pdf-generation.csproj"]

RUN dotnet restore "src/smptp-pdf-generation.csproj"
COPY . .

RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

RUN dotnet tool install -g dotnet-ef
ENV PATH $PATH:/root/.dotnet/tools

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "smptp-pdf-generation.csproj"]
