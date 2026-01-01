#!/bin/bash

# Build ABP Pro Packages Script
# This script builds all ABP Pro solutions and creates NuGet packages in /nupkgs

set -e

ABP_ROOT="/Users/alexsandrocruz/Documents/dev/AbpZen"
NUPKGS_DIR="$ABP_ROOT/nupkgs"
ABP_VERSION="9.0.4"

echo "🏗️  Building ABP Pro Packages"
echo "   Output: $NUPKGS_DIR"
echo ""

# Create output directory
mkdir -p "$NUPKGS_DIR"

# List of solutions to build
SOLUTIONS=(
    "Volo.Abp.Identity.Pro.sln"
    "Volo.Abp.Account.Pro.sln"
    "Volo.Abp.AuditLogging.sln"
    "Volo.Abp.Gdpr.sln"
    "Volo.Abp.LanguageManagement.sln"
    "Volo.Abp.LeptonXTheme.sln"
    "Volo.Abp.OpenIddict.Pro.sln"
    "Volo.Abp.TextTemplateManagement.sln"
    "Volo.Chat.sln"
    "Volo.CmsKit.Pro.sln"
    "Volo.FileManagement.sln"
    "Volo.Forms.sln"
    "Volo.Saas.sln"
)

# Build each solution
for sln in "${SOLUTIONS[@]}"; do
    SLN_PATH="$ABP_ROOT/$sln"
    if [ -f "$SLN_PATH" ]; then
        echo "📦 Building $sln..."
        dotnet build "$SLN_PATH" -c Release --no-incremental -v q || {
            echo "⚠️  Warning: Failed to build $sln, skipping..."
            continue
        }
        
        # Pack all projects in the solution
        echo "   Packing NuGet packages..."
        SLN_DIR=$(dirname "$SLN_PATH")
        find "$SLN_DIR" -name "*.csproj" -type f | while read proj; do
            # Skip test projects
            if [[ "$proj" != *".Tests"* ]] && [[ "$proj" != *".Test."* ]]; then
                dotnet pack "$proj" -c Release -o "$NUPKGS_DIR" --no-build -v q 2>/dev/null || true
            fi
        done
        echo "   ✅ Done"
    else
        echo "⚠️  Solution not found: $sln"
    fi
done

echo ""
echo "✅ Build complete!"
echo "   Packages in: $NUPKGS_DIR"
echo "   Total packages: $(ls -1 "$NUPKGS_DIR"/*.nupkg 2>/dev/null | wc -l)"
