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

# Kill process listening on port 3005 (Bridge)
lsof -ti:3005 | xargs kill -9 2>/dev/null
lsof -ti:5173 | xargs kill -9 2>/dev/null

echo ""
echo "✅ ZenCode Generator Environment stopped!"
