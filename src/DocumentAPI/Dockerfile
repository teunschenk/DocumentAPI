FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/DocumentAPI/DocumentAPI.csproj", "src/DocumentAPI/"]
COPY ["src/Application/DocumentAPI.Application.csproj", "src/Application/"]
COPY ["src/Domain/DocumentAPI.Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure/DocumentAPI.Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Presentation/DocumentAPI.RestAPI.csproj", "src/Presentation/"]
RUN dotnet restore "src/DocumentAPI/DocumentAPI.csproj"
COPY . .
WORKDIR "/src/src/DocumentAPI"
RUN dotnet build "DocumentAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DocumentAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DocumentAPI.dll"]
