FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
COPY "game-server" "./game-server"

EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY ["Acrobatt/Src/External/Acrobatt.API/Acrobatt.API.csproj", "Src/External/Acrobatt.API/"]
COPY ["Acrobatt/Src/External/Acrobatt.Infrastructure/Acrobatt.Infrastructure.csproj", "Src/External/Acrobatt.Infrastructure/"]
COPY ["Acrobatt/Src/Core/Acrobatt.Domain/Acrobatt.Domain.csproj", "Src/Core/Acrobatt.Domain/"]
COPY ["Acrobatt/Src/Core/Acrobatt.Application/Acrobatt.Application.csproj", "Src/Core/Acrobatt.Application/"]
RUN dotnet restore "Src/External/Acrobatt.API/Acrobatt.API.csproj"
COPY "./Acrobatt" "./Acrobatt"
WORKDIR "Acrobatt/Src/External/Acrobatt.API"
RUN dotnet build "Acrobatt.API.csproj" -c Release -o /app/Acrobatt/build

FROM build AS publish
RUN dotnet publish "Acrobatt.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app/Acrobatt
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Acrobatt.API.dll"]
