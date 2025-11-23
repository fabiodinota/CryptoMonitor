# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the Solution file and Project files first (for better caching)
COPY ["CryptoMonitor.sln", "./"]
COPY ["CryptoMonitor.UI.MVC/UI-MVC.csproj", "CryptoMonitor.UI.MVC/"]
COPY ["CryptoMonitor.BL/CryptoMonitor.BL.csproj", "CryptoMonitor.BL/"]
COPY ["CryptoMonitor.DAL/CryptoMonitor.DAL.csproj", "CryptoMonitor.DAL/"]
COPY ["CryptoMonitor.Domain/CryptoMonitor.Domain.csproj", "CryptoMonitor.Domain/"]
COPY ["CryptoMonitor.UI.CA/CryptoMonitor.UI.CA.csproj", "CryptoMonitor.UI.CA/"]

# Restore dependencies
RUN dotnet restore "CryptoMonitor.sln"

# Copy the rest of the source code
COPY . .

# Build and Publish the MVC project
WORKDIR "/src/CryptoMonitor.UI.MVC"
RUN dotnet publish "UI-MVC.csproj" -c Release -o /app/publish

# Stage 2: Run the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Create a directory for the database to ensure permissions are right
RUN mkdir -p /app/data

# Expose port 8080 (Standard for .NET containers)
EXPOSE 8080

ENTRYPOINT ["dotnet", "UI-MVC.dll"]