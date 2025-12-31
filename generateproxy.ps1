# Define the base URL
$baseUrl = "https://localhost:44341/"

# Function to run a command in a specific directory and stop if it fails
function Run-Command {
    param (
        [string]$directory,
        [bool]$isJs,
        [bool]$isAdmin
    )
    $originalDir = Get-Location
    Set-Location $directory
    $command = "abp generate-proxy -t " + ($isJs ? "js" : "csharp") + " -u $baseUrl -m " + ($isAdmin ? "cms-kit-pro-admin" : "cms-kit-pro-common") + ($isJs ? "" : " --without-contracts")
    Invoke-Expression $command
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Command failed: $command"
        exit $LASTEXITCODE
    }
    Set-Location $originalDir
}

# Run commands in their respective directories
$commands = @(
    @{ directory = "src/Volo.CmsKit.Pro.Admin.Web" ; isAdmin = $true; isJs = $true },
    @{ directory = "src/Volo.CmsKit.Pro.Admin.HttpApi.Client" ; isAdmin = $true; isJs = $false },
    @{ directory = "src/Volo.CmsKit.Pro.Common.Web" ; isAdmin = $false; isJs = $true },
    @{ directory = "src/Volo.CmsKit.Pro.Common.HttpApi.Client" ; isAdmin = $false; isJs = $false }
)

foreach ($command in $commands) {
    Run-Command -directory $command.directory -isJs $command.isJs -isAdmin $command.isAdmin
}