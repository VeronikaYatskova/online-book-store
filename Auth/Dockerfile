FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["/Auth.API/Auth.API.csproj", "src/Auth.API/"]
COPY ["/Auth.Application/Auth.Application.csproj", "src/Auth.Application/"]
COPY ["/Auth.Domain/Auth.Domain.csproj", "src/Auth.Domain/"]
COPY ["/Auth.Infrastructure/Auth.Infrastructure.csproj", "src/Auth.Infrastructure/"]

RUN dotnet restore "src/Auth.API/Auth.API.csproj"
COPY . .

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

RUN dotnet tool install -g dotnet-ef
ENV PATH $PATH:/root/.dotnet/tools

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app/publish .
RUN ls -l

CMD dotnet ef database update && dotnet Auth.API.dll
