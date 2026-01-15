#!/bin/bash

# Sapienza.Sapienza.Dominus Local Run Script with Blazor support
# Usage: ./run-local-blazor.sh [--setup] [--api-only] [--blazor]

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

SETUP=false
API_ONLY=false
BLAZOR=false

while [[ "$#" -gt 0 ]]; do
    case $1 in
        --setup) SETUP=true ;;
        --api-only) API_ONLY=true ;;
        --blazor) BLAZOR=true ;;
        *) echo "Unknown parameter: $1"; exit 1 ;;
    esac
    shift
done

# Check if Docker is running
if ! docker info &> /dev/null; then
    echo "❌ Docker is not running. Please start Docker Desktop first."
    exit 1
fi

if [ "$SETUP" = true ]; then
    echo "🔧 Running setup..."
    
    # Install ABP libs
    echo "📚 Installing ABP libs..."
    cd "$SCRIPT_DIR/Sapienza.Sapienza.Dominus.Web" && abp install-libs
    cd "$SCRIPT_DIR/Sapienza.Sapienza.Dominus.Blazor.Server" && abp install-libs
    
    echo "✅ Setup complete!"
fi

# Start infrastructure containers
echo "🐳 Starting infrastructure (MongoDB + Redis)..."
cd "$SCRIPT_DIR"
docker-compose -f docker-compose.infra.yml up -d

# Wait for services to be ready
echo "⏳ Waiting for services to be ready..."
sleep 3

echo ""
echo "🚀 Starting sapienza-zen locally..."
echo ""
echo "   MongoDB: localhost:27017"
echo "   Redis:   localhost:6379"
echo ""

echo "📡 Starting API (https://localhost:44322)..."
cd "$SCRIPT_DIR/Sapienza.Sapienza.Dominus.HttpApi.Host"
dotnet run --urls="https://localhost:44322" &
API_PID=$!

if [ "$API_ONLY" = false ]; then
    if [ "$BLAZOR" = true ]; then
        echo "🌐 Starting Blazor Server (https://localhost:44307)..."
        cd "$SCRIPT_DIR/Sapienza.Sapienza.Dominus.Blazor.Server"
        dotnet run --urls="https://localhost:44307" &
        WEB_PID=$!
    else
        echo "🌐 Starting Web (https://localhost:44360)..."
        cd "$SCRIPT_DIR/Sapienza.Sapienza.Dominus.Web"
        dotnet run --urls="https://localhost:44360" &
        WEB_PID=$!
    fi
fi

echo "🚀 sapienza-zen is running!"
if [ "$BLAZOR" = true ]; then
    echo "   API:    https://localhost:44322/swagger"
    echo "   Blazor: https://localhost:44307"
else
    echo "   API:    https://localhost:44322/swagger"
    echo "   Web:    https://localhost:44360"
fi
echo ""
echo "Press Ctrl+C to stop all services"

# Handle shutdown
trap "kill $API_PID ${WEB_PID:-}; exit" SIGINT SIGTERM

wait
