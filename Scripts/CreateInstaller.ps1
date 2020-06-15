
Write-Host "Grimoire Installer generator version 1.2.0" -ForegroundColor Green
Write-Host "Requires dotnet 3.1 or greater and Inno Setup 6" -ForegroundColor Yellow
Write-Host "-----------------------------------------------"

Write-Host "Running Unit Tests..." -ForegroundColor Green
dotnet test ..\Grimoire.Tests\ -l "trx;LogFileName=TestOutput.xml"
$TestResult = "..\Grimoire.Tests\TestResults\TestOutput.xml"
$Ns = @{
    ns = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010"
}
$SummaryNode = Select-Xml -Path $TestResult -XPath "//ns:TestRun/ns:ResultSummary/ns:Counters" -Namespace $Ns
$Fail = $SummaryNode.Node | Select-Object failed
Write-Host "Unit tests finished" -ForegroundColor Green

if($Fail.failed -eq 0) {

    $InnoSetup = "C:\Program Files (x86)\Inno Setup 6\"
    if (-Not (Test-Path -Path $InnoSetup -PathType Any)) 
    { 
        Write-Host "Inno setup compiler not found" -ForegroundColor Red
        $InnoSetup = Read-Host -Prompt 'Input the path for ISCC.exe'
    }

    if (Test-Path -Path $InnoSetup -PathType Any) 
    {    
        Set-Location "..\"

        Write-Host "Geting Version number..." -ForegroundColor Green
        $project = ".\Grimoire\Grimoire.csproj"
        $XPath = "/Project/PropertyGroup/Version"
        $versionNode = Select-Xml -Path $project -XPath $XPath
        $version = $versionNode.Node.InnerXML
        if (-Not $version){
            $version = "1.0.0"
        }
        $nameSufix = "_$($version -replace '\.', '_')"
        Write-Host "Version $($version)" -ForegroundColor Green
        
        if (Test-Path -Path '.\publish' -PathType Any) 
        { 
            Remove-Item -Path ".\publish" -Recurse
        }
        $installScriptTemplate = Resolve-Path -Path '.\Scripts\GrimoireSetupTemplate.txt'
        
        Write-Host "Unzip CEF dependencies..." -ForegroundColor Green
        Expand-Archive -LiteralPath '.\CEF_Dependencies.zip' -DestinationPath '.\CEF_Dependencies'
        Write-Host "Unzip finished" -ForegroundColor Green

        Write-Host "Creating install script..." -ForegroundColor Green
        $currentPath = Resolve-Path -Path '.\'
        $ScriptContent = Get-Content -Path $installScriptTemplate.Path
        $ScriptContent = $ScriptContent -replace '<version>',$version
        $ScriptContent = $ScriptContent -replace '<nameSufix>',$nameSufix
        $ScriptContent = $ScriptContent -replace '<location>',($currentPath.Path)
        Set-Content -Path '.\Scripts\GrimoireSetup_temp.iss' -Value $ScriptContent
        $installScript = Resolve-Path -Path '.\Scripts\GrimoireSetup_temp.iss'
        Write-Host "Install script created: .\Scripts\GrimoireSetup_temp.iss" -ForegroundColor Green

        Write-Host "Publishing..." -ForegroundColor Green
        dotnet publish -c Release -r win10-x64 -o .\publish
        Write-Host "Published to .\publish" -ForegroundColor Green

        Write-Host "Creating instaler with version: $($version)" -ForegroundColor Green
        New-Item -ItemType Directory -Force -Path '.\Release'
        Set-Location -Path $InnoSetup
        .\ISCC.exe  $installScript.Path    
        Write-Host "Instaler created with version: $($version)" -ForegroundColor Green

        Write-Host "Cleaning temp files and folders..." -ForegroundColor Green
        Set-Location $currentPath
        Remove-Item -Path ".\publish" -Recurse
        Remove-Item -Path $installScript
        Remove-Item -Path ".\CEF_Dependencies" -Recurse
        Remove-Item -Path ".\Grimoire.Tests\TestResults" -Recurse
        Set-Location ".\Scripts"
        Write-Host "Finished" -ForegroundColor Green

        $RunResponse = Read-Host -Prompt "Run setup? [Y] - yes [N] - no"
        if($RunResponse -eq "Y") {
            Invoke-Expression -Command "..\Release\GrimoireSetup$($nameSufix).exe"
        }
    }
}else {
    Write-Host "Unit tests failed" -ForegroundColor Red
    Pause
}









