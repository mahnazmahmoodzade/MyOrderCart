# Use SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything from solution folder
COPY . .

# Restore using the solution file
RUN dotnet restore

# Publish the Blazor UI project
RUN dotnet publish ./src/MyOrderCart.BlazorUI/MyOrderCart.BlazorUI.csproj -c Release -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 8080
ENTRYPOINT ["dotnet", "MyOrderCart.BlazorUI.dll"]
