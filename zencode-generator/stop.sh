#!/bin/bash

# Navigate to the generator directory
cd "$(dirname "$0")"

echo "🛑 Stopping ZenCode Generator Environment..."

# Kill Bridge Server (node bridge.js)
echo "🌉 Stopping Backend Bridge..."
pkill -f "node bridge.js" 2>/dev/null && echo "   ✅ Bridge stopped" || echo "   ⚠️  Bridge was not running"

# Kill Vite dev server
echo "💻 Stopping Frontend Designer..."
pkill -f "vite" 2>/dev/null && echo "   ✅ Vite stopped" || echo "   ⚠️  Vite was not running"

# Also kill any process on ports 3001 and 5173
lsof -ti:3001 | xargs kill -9 2>/dev/null
lsof -ti:5173 | xargs kill -9 2>/dev/null

echo ""
echo "✅ ZenCode Generator Environment stopped!"
