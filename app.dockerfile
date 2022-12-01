FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as build
WORKDIR /app

#remove when done debugging
EXPOSE 7125

COPY /Tutor.Api .
RUN dotnet restore
RUN dotnet publish -o /app/published-app

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine as runtime
WORKDIR /app
COPY --from=build /app/published-app /app

ENV ASPNETCORE_URLS=http://+:7125
ENV ASPNETCORE_HTTPS_PORT=7125

ENTRYPOINT [ "dotnet", "/app/Tutor.Api.dll" ]