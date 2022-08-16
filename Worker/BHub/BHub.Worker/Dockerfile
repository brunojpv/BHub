#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["BHub.Worker/BHub.Worker.csproj", "BHub.Worker/"]
RUN dotnet restore "BHub.Worker/BHub.Worker.csproj"
COPY . .
WORKDIR "/src/BHub.Worker"
RUN dotnet build "BHub.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BHub.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BHub.Worker.dll"]