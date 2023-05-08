FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WebSocketChat.Api/WebSocketChat.Api.csproj", "WebSocketChat.Api/"]
RUN dotnet restore "WebSocketChat.Api/WebSocketChat.Api.csproj"
COPY . .
WORKDIR "/src/WebSocketChat.Api"
RUN dotnet build "WebSocketChat.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebSocketChat.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebSocketChat.Api.dll"]
