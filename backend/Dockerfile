FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY *.slnx ./
COPY Identity/*.csproj Identity/
COPY Profile/*.csproj Profile/
COPY Shared/*.csproj Shared/
COPY Content/*.csproj Content/
COPY Study/*.csproj Study/
COPY Fenixa.Api/*.csproj Fenixa.Api/

RUN dotnet restore

COPY . .

RUN dotnet publish Fenixa.Api/Fenixa.Api.csproj -c Release -o /app/out


FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/out .

# Відкриваємо порт, на якому працює API
EXPOSE 8080

ENTRYPOINT ["dotnet", "Fenixa.Api.dll"]