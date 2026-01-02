#!/bin/bash

# Navigate to the generator directory
cd "$(dirname "$0")"

echo "🚀 Starting ZenCode Generator Environment..."

# Kill background processes on exit
trap "kill 0" EXIT

# Start the Bridge Server in the background
echo "🌉 Starting Backend Bridge (localhost:3001)..."
node bridge.js &

# Wait a moment for bridge to start
sleep 2

# Start the Vite Frontend
echo "💻 Starting Frontend Designer (localhost:5173)..."
npm run dev
