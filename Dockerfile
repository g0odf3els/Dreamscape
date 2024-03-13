FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Dreamscape.UI/Dreamscape.UI.csproj", "Dreamscape.UI/"]
COPY ["Dreamscape.Application/Dreamscape.Application.csproj", "Dreamscape.Application/"]
COPY ["Dreamscape.Domain/Dreamscape.Domain.csproj", "Dreamscape.Domain/"]
COPY ["ImageTagging/Dreamscape.ImageRecognition.csproj", "ImageTagging/"]
COPY ["Dreamscape.Persistance/Dreamscape.Persistance.csproj", "Dreamscape.Persistance/"]
RUN dotnet restore "./Dreamscape.UI/Dreamscape.UI.csproj"
COPY . .
WORKDIR "/src/Dreamscape.UI"
RUN dotnet build "./Dreamscape.UI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Dreamscape.UI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dreamscape.UI.dll"]
