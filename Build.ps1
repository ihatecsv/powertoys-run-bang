$ErrorActionPreference = "Stop"

[xml]$xml = Get-Content -Path "$PSScriptRoot\Directory.Build.Props"
$version = $xml.Project.PropertyGroup.Version

foreach ($platform in "ARM64", "x64")
{
    if (Test-Path -Path "$PSScriptRoot\Community.PowerToys.Run.Plugin.Bang\bin")
    {
        Remove-Item -Path "$PSScriptRoot\Community.PowerToys.Run.Plugin.Bang\bin\*" -Recurse
    }
    if (Test-Path -Path "$PSScriptRoot\Bang-$version-$platform.zip")
    {
        Remove-Item -Path "$PSScriptRoot\Bang-$version-$platform.zip"
    }

    dotnet build $PSScriptRoot\PowerToys-Run-Bang.sln -c Release /p:Platform=$platform

    Remove-Item -Path "$PSScriptRoot\Community.PowerToys.Run.Plugin.Bang\bin\*" -Recurse -Include *.xml, *.pdb, PowerToys.*, Wox.*
    Rename-Item -Path "$PSScriptRoot\Community.PowerToys.Run.Plugin.Bang\bin\$platform\Release" -NewName "Bang"

    Compress-Archive -Path "$PSScriptRoot\Community.PowerToys.Run.Plugin.Bang\bin\$platform\Bang" -DestinationPath "$PSScriptRoot\Bang-$version-$platform.zip"
}
