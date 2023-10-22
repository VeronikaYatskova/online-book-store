FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["/EmailService/EmailService.csproj", "src/EmailService/"]

RUN dotnet restore "src/EmailService/EmailService.csproj"
COPY . .

WORKDIR "/src/EmailService/"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app/publish .
RUN ls -l

ENTRYPOINT ["dotnet", "EmailService.dll"]
