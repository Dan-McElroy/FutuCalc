FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["FutuCalc.API/FutuCalc.API.csproj", "FutuCalc.API/"]
COPY ["FutuCalc.Tests/FutuCalc.Tests.csproj", "FutuCalc.Tests/"]
RUN dotnet restore "FutuCalc.API/FutuCalc.API.csproj"
COPY . .
WORKDIR "/src/FutuCalc.API"
RUN dotnet build "FutuCalc.API.csproj" -c Release -o /app/build

FROM build AS testing
WORKDIR "/src/FutuCalc.Tests"
RUN dotnet test "FutuCalc.Tests.csproj" -c Release -o /app/testing

FROM build AS publish
RUN dotnet publish "FutuCalc.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet FutuCalc.API.dll