FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

COPY src/api/RecipeCosting.Api/RecipeCosting.Api.csproj src/api/RecipeCosting.Api/
RUN dotnet restore src/api/RecipeCosting.Api/RecipeCosting.Api.csproj

COPY src/api/RecipeCosting.Api/ src/api/RecipeCosting.Api/
RUN dotnet publish src/api/RecipeCosting.Api/RecipeCosting.Api.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app .

EXPOSE 8080
ENTRYPOINT ["dotnet", "RecipeCosting.Api.dll"]
