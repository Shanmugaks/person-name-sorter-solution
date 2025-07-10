#!/bin/bash

# 📂 Step 0: Determine input file based on optional size argument
# Usage: ./build-pipeline.sh [2K|5K|10K|100K|1M]
SIZE=${1:-default}
case "$SIZE" in
  2K)    INPUT_FILE="./data/unsorted-names-list-2K.txt" ;;
  5K)    INPUT_FILE="./data/unsorted-names-list-5K.txt" ;;
  10K)   INPUT_FILE="./data/unsorted-names-list-10K.txt" ;;
  100K)  INPUT_FILE="./data/unsorted-names-list-100K.txt" ;;
  1M)    INPUT_FILE="./data/unsorted-names-list-1M.txt" ;;
  *)     INPUT_FILE="./data/unsorted-names-list.txt" ;;  # default fallback
esac

echo "📁 Using input file: $INPUT_FILE"

# 🔄 Step 1: Restore Dependencies
echo "🔄 Restoring project dependencies..."
dotnet restore

# 🛠️ Step 2: Build the Project
echo "🛠️ Building the project in Release configuration..."
dotnet build --configuration Release

# ✅ Step 3: Run Unit Tests
echo "✅ Running unit tests..."
dotnet test --no-build --verbosity normal

# 🚀 Step 4: Execute the Application
echo "🚀 Running the application with input: $INPUT_FILE"
dotnet run --project PersonNameSorter.Console "$INPUT_FILE"

echo "🎉 Build, test, and execution completed successfully."
