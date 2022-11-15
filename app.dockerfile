FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /Tutor.Api
COPY ["Tutor.Api.csproj", "."]
RUN dotnet restore "./Tutor.Api.csproj"
COPY . .
WORKDIR "/Tutor.Api/."
RUN dotnet build "Tutor.Api.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "Tutor.Api.csproj" -c Release -o /app/publish
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tutor.Api.dll"]