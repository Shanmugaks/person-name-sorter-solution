FROM mcr.microsoft.com/dotnet/sdk:9.0-preview

# Set working directory
WORKDIR /app

# Copy everything into container
COPY . .

# Restore dependencies
RUN dotnet restore

# Build in Debug to ensure test DLLs are created
RUN dotnet build --configuration Debug

# Explicitly publish test project to ensure runtime availability
RUN dotnet publish PersonNameSorter.UnitTest/PersonNameSorter.UnitTest.csproj -c Debug -o out

# Run tests from published output
CMD ["dotnet", "test", "--no-build", "--verbosity", "normal"]
