#!/bin/bash

# Sapienza.Dominus Local Run Script (without Docker for app, only infra)
# Usage: ./run-local.sh [--setup]

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

SETUP=false

while [[ "$#" -gt 0 ]]; do
    case $1 in
        --setup) SETUP=true ;;
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
    cd "$SCRIPT_DIR"
    abp install-libs
    
    echo "✅ Setup complete!"
fi

# Start infrastructure containers
echo "🐳 Starting infrastructure (MongoDB + Redis)..."
docker-compose -f docker-compose.infra.yml up -d

# Wait for services to be ready
echo "⏳ Waiting for MongoDB to be ready..."
sleep 3

echo ""
echo "🚀 Starting sapienza-zen locally..."
echo ""
echo "   MongoDB: localhost:27017"
echo "   Redis:   localhost:6379"
echo ""

# Check if --api-only flag exists
API_ONLY=false
for arg in "$@"; do
    if [ "$arg" == "--api-only" ]; then
        API_ONLY=true
    fi
done

echo "📡 Starting API (https://localhost:44322)..."
cd "$SCRIPT_DIR/Sapienza.Dominus.HttpApi.Host"
dotnet run --urls="https://localhost:44322" &
API_PID=$!

if [ "$API_ONLY" = false ]; then
    echo "🌐 Starting Web (https://localhost:44360)..."
    cd "$SCRIPT_DIR/Sapienza.Dominus.Web"
    dotnet run --urls="https://localhost:44360" &
    WEB_PID=$!
fi

echo "🚀 sapienza-zen is running!"
echo "   API: https://localhost:44322/swagger"
echo "   Web: https://localhost:44360"
echo ""
echo "Press Ctrl+C to stop all services"

# Handle shutdown
trap "kill $API_PID ${WEB_PID:-}; exit" SIGINT SIGTERM

wait
