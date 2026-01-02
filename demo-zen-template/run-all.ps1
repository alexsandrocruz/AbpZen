param (
    [switch]$setup = $false
)

if ($setup) {
    dotnet tool update -g Microsoft.Tye --prerelease

    docker run --name tmp-lpx-mongo -p 27017:27017 -d mongo:latest
    abp install-libs
}

tye run --watch