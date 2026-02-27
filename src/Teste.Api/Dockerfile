FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Teste.Api.csproj", "./"]
RUN dotnet restore "Teste.Api.csproj"

COPY . .
RUN dotnet publish "Teste.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Teste.Api.dll"]