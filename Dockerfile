FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file (specify the exact name)
COPY TodoItem.csproj .

# Restore dependencies
RUN dotnet restore TodoItem.csproj

# Copy everything else
COPY . .

# Publish the application (specify the project file)
RUN dotnet publish TodoItem.csproj -c Release -o /app

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the published output
COPY --from=build /app .

# Set the port to 3087
ENV ASPNETCORE_URLS=http://+:3087

# Expose port 3087
EXPOSE 3087

# Start the application
ENTRYPOINT ["dotnet", "TodoItem.dll"]