FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore "GLMS_V3/GLMS_V3.csproj"

RUN dotnet publish "GLMS_V3/GLMS_V3.csproj" \
    -c Release \
    -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8081

ENTRYPOINT ["dotnet", "GLMS_V3.dll"]