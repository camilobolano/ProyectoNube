# Usa la imagen oficial de .NET 8.0 ASP.NET Core como base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Usa la imagen oficial de .NET SDK 8.0 para compilar la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["bibliotecaAPI/bibliotecaAPI.csproj", "bibliotecaAPI/"]
COPY ["bibliotecaDataAccess/bibliotecaDataAccess.csproj", "bibliotecaDataAccess/"]
COPY ["bibliotecaModels/bibliotecaModels.csproj", "bibliotecaModels/"]
RUN dotnet restore "bibliotecaAPI/bibliotecaAPI.csproj"
COPY . .
WORKDIR "/src/bibliotecaAPI"
RUN dotnet build "bibliotecaAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "bibliotecaAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "bibliotecaAPI.dll"]