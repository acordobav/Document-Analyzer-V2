#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["OffensiveContentAPI/OffensiveContentAPI.csproj", "OffensiveContentAPI/"]
COPY ["DataHandlerAzureBlob/DataHandlerAzureBlob.csproj", "DataHandlerAzureBlob/"]
COPY ["RabbitMQ/RabbitMQ.csproj", "RabbitMQ/"]
COPY ["FileHandler/FileHandler.csproj", "FileHandler/"]
RUN dotnet restore "OffensiveContentAPI/OffensiveContentAPI.csproj"
COPY . .
WORKDIR "/src/OffensiveContentAPI"
RUN dotnet build "OffensiveContentAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OffensiveContentAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OffensiveContentAPI.dll"]