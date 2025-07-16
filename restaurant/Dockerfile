FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5037

ENV ASPNETCORE_URLS=http://+:5037

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
ARG configuration=Release
WORKDIR /src
COPY ["Restaurant_API.csproj", "./"]
RUN dotnet restore "Restaurant_API.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Restaurant_API.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Restaurant_API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Restaurant_API.dll"]
