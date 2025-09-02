# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy sln and all project files (adjust paths if your structure differs)
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /out

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out ./
# Trust dev certs if you expose HTTPS, otherwise stick to HTTP (ASPNETCORE_URLS)
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Vwr.Api.dll"]