FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["Comments/.", "./"]

RUN dotnet restore "Comments.API/Comments.API.csproj"
RUN dotnet build "Comments.API/Comments.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Comments.API/Comments.API.csproj" -c Release -o /app/publish

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app/publish .
RUN ls -l

ENTRYPOINT ["dotnet", "Comments.API.dll"]
