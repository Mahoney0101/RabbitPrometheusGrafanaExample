# Use the official Microsoft .NET Core runtime as the base image
FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /app

COPY StreamSubscriber.csproj .
RUN dotnet restore StreamSubscriber.csproj

COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "StreamSubscriber.dll"]
