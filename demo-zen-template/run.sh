#!/bin/bash

# Demo-Zen Run Script for macOS
# Usage: ./run.sh [--setup] [--build-abp]

SETUP=false
BUILD_ABP=false

# Parse arguments
while [[ "$#" -gt 0 ]]; do
    case $1 in
        --setup) SETUP=true ;;
        --build-abp) BUILD_ABP=true ;;
        *) echo "Unknown parameter: $1"; exit 1 ;;
    esac
    shift
done

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ABP_ROOT="$(dirname "$SCRIPT_DIR")"
NUPKGS_DIR="$SCRIPT_DIR/nupkgs"

if [ "$SETUP" = true ]; then
    echo "🔧 Running setup..."
    
    # Start MongoDB container
    echo "🍃 Starting MongoDB container..."
    docker run --name demo-zen-mongodb -p 27017:27017 -d mongo:latest
    
    # Start Redis container
    echo "📮 Starting Redis container..."
    docker run --name demo-zen-redis -p 6379:6379 -d redis:latest
    
    # Install ABP libs
    echo "📚 Installing ABP libs..."
    abp install-libs
    
    echo "✅ Setup complete!"
fi

# Build ABP packages if requested or if nupkgs folder is empty
if [ "$BUILD_ABP" = true ] || [ ! -d "$NUPKGS_DIR" ] || [ -z "$(ls -A "$NUPKGS_DIR" 2>/dev/null)" ]; then
    echo "🏗️  Building ABP Pro packages (this may take a while)..."
    
    # Run the build script
    if [ -f "$ABP_ROOT/build-abp-packages.sh" ]; then
        bash "$ABP_ROOT/build-abp-packages.sh"
        
        # Copy packages to demo-zen for Docker context
        echo "📋 Copying packages to demo-zen..."
        mkdir -p "$NUPKGS_DIR"
        cp "$ABP_ROOT/nupkgs"/*.nupkg "$NUPKGS_DIR/" 2>/dev/null || true
        echo "   Copied $(ls -1 "$NUPKGS_DIR"/*.nupkg 2>/dev/null | wc -l | tr -d ' ') packages"
    else
        echo "⚠️  build-abp-packages.sh not found at $ABP_ROOT"
        echo "   Run: ./run.sh --build-abp after creating the script"
    fi
fi

# Check if Docker is running
if ! docker info &> /dev/null; then
    echo "❌ Docker is not running. Please start Docker Desktop first."
    exit 1
fi

# Check if nupkgs folder exists and has packages
if [ ! -d "$NUPKGS_DIR" ] || [ -z "$(ls -A "$NUPKGS_DIR" 2>/dev/null)" ]; then
    echo "⚠️  No NuGet packages found in $NUPKGS_DIR"
    echo "   Run: ./run.sh --build-abp to build ABP Pro packages first"
    exit 1
fi

echo ""
echo "🚀 Starting demo-zen with Docker Compose..."
echo "   Web:       http://localhost:44360"
echo "   API:       http://localhost:44322"
echo "   Packages:  $(ls -1 "$NUPKGS_DIR"/*.nupkg 2>/dev/null | wc -l | tr -d ' ') NuGet packages"
echo ""

# Run Docker Compose
docker-compose up --build
