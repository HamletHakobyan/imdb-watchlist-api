#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Space.ImdbWatchList.Api/Space.ImdbWatchList.Api.csproj", "Space.ImdbWatchList.Api/"]
COPY ["Space.ImdbWatchList.Models/Space.ImdbWatchList.Models.csproj", "Space.ImdbWatchList.Models/"]
COPY ["Space.ImdbWatchList.Infrastructure/Space.ImdbWatchList.Infrastructure.csproj", "Space.ImdbWatchList.Infrastructure/"]
COPY ["Space.ImdbWatchList.Services/Space.ImdbWatchList.Services.csproj", "Space.ImdbWatchList.Services/"]
RUN dotnet restore "Space.ImdbWatchList.Api/Space.ImdbWatchList.Api.csproj"
COPY . .
WORKDIR "/src/Space.ImdbWatchList.Api"
RUN dotnet build "Space.ImdbWatchList.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Space.ImdbWatchList.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Space.ImdbWatchList.Api.dll"]