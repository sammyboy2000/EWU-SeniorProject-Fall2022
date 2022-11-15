FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY /Tutor.Api .
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
COPY --from=build-env /App/out .
ENTRYPOINT [ "dotnet", "Tutor.Api.dll" ]