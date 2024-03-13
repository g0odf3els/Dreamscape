FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Dreamscape/Dreamscape.csproj", "Dreamscape/"]
RUN dotnet restore "Dreamscape/Dreamscape.csproj"
COPY . .
WORKDIR "/src/Dreamscape"
RUN dotnet build "Dreamscape.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dreamscape.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dreamscape.dll"]
