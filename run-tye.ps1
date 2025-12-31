<# Check development certificates #>

if (! (  Test-Path ".\etc\dev-cert\localhost.pfx" -PathType Leaf ) ){
   Write-Information "Creating dev certificates..."
   cd ".\etc\dev-cert"
   .\create-certificate.ps1
   cd..
   cd ..  
}

tye run --watch