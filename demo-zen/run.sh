#!/bin/bash

# Demo-Zen Run Script for macOS
# Usage: ./run.sh [--setup]

SETUP=false

# Parse arguments
while [[ "$#" -gt 0 ]]; do
    case $1 in
        --setup) SETUP=true ;;
        *) echo "Unknown parameter: $1"; exit 1 ;;
    esac
    shift
done

if [ "$SETUP" = true ]; then
    echo "🔧 Running setup..."
    
    # Install/Update Tye
    echo "📦 Installing Microsoft Tye..."
    dotnet tool update -g Microsoft.Tye --prerelease
    
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

# Check if Docker is running
if ! docker info &> /dev/null; then
    echo "❌ Docker is not running. Please start Docker Desktop first."
    exit 1
fi

# Check if containers are running, start if not
if [ ! "$(docker ps -q -f name=demo-zen-mongodb)" ]; then
    echo "🍃 Starting MongoDB..."
    if [ "$(docker ps -aq -f name=demo-zen-mongodb)" ]; then
        docker start demo-zen-mongodb
    else
        docker run --name demo-zen-mongodb -p 27017:27017 -d mongo:latest
    fi
fi

if [ ! "$(docker ps -q -f name=demo-zen-redis)" ]; then
    echo "📮 Starting Redis..."
    if [ "$(docker ps -aq -f name=demo-zen-redis)" ]; then
        docker start demo-zen-redis
    else
        docker run --name demo-zen-redis -p 6379:6379 -d redis:latest
    fi
fi

echo ""
echo "🚀 Starting demo-zen with Tye..."
echo "   Dashboard: http://localhost:8000"
echo "   Web:       https://localhost:44360"
echo "   API:       https://localhost:44322"
echo "   Angular:   http://localhost:4200"
echo ""

# Run Tye
tye run --watch
