FROM mcr.microsoft.com/dotnet/sdk:5.0
WORKDIR /src
COPY ["Space.ImdbWatchList.Api/Space.ImdbWatchList.Api.csproj", "Space.ImdbWatchList.Api/"]
COPY ["Space.ImdbWatchList.Models/Space.ImdbWatchList.Models.csproj", "Space.ImdbWatchList.Models/"]
COPY ["Space.ImdbWatchList.Infrastructure/Space.ImdbWatchList.Infrastructure.csproj", "Space.ImdbWatchList.Infrastructure/"]
COPY ["Space.ImdbWatchList.Services/Space.ImdbWatchList.Services.csproj", "Space.ImdbWatchList.Services/"]
COPY ["Space.ImdbWatchList.Data/Space.ImdbWatchList.Data.csproj", "Space.ImdbWatchList.Data/"]
COPY ["migrate.sh", "./"]
RUN dotnet tool install -g dotnet-ef
RUN dotnet restore "Space.ImdbWatchList.Api/Space.ImdbWatchList.Api.csproj"
COPY . .
WORKDIR "/src/."

RUN /root/.dotnet/tools/dotnet-ef migrations add InitialMigrations -p ./Space.ImdbWatchList.Data/ -s ./Space.ImdbWatchList.Api/

RUN chmod +x ./migrate.sh
CMD /bin/bash ./migrate.sh