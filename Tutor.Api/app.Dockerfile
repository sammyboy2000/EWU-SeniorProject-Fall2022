#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 7125
ENV DOTNET_URLS=http://+:7125

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Tutor.Api/Tutor.Api.csproj", "Tutor.Api/"]
RUN dotnet restore "Tutor.Api/Tutor.Api.csproj"
COPY . .
WORKDIR "/src/Tutor.Api"
RUN dotnet build "Tutor.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tutor.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:7125

ENV ASPNETCORE_HTTP_PORT=7125
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "Tutor.Api.dll"]