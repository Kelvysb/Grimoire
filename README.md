# Grimoire - Script Manager

### About

Organize and execute scripts, retrieve status, execute actions, automatic executions, etc.

[Tutorial](https://kelvysb.github.io/Grimoire)

[Releases](https://github.com/Kelvysb/Grimoire/releases)

### Create installer

Run .\Scripts\CreateInstaller.ps1

* The Script will be created based on the version contained on .\Grimoire\Grimoire.csproj
* Dotnet Core 3.1 required.
* InnoSetup 6 Required (if not installed on default path the script will ask for the executable path).
* The setup file will be created only if the unit tests pass.
* The setup file will be generated on .\Release\GrimoireSetup_****.exe

### Run unit tests

Run .\Scripts\RunTests.ps1

### Packages Reference

* [Blazor](https://github.com/dotnet/blazor)
* [BlazorInputFile](https://github.com/SteveSandersonMS/BlazorInputFile)
* [Chromely](https://github.com/chromelyapps/Chromely)
* [PowerShell SDK](https://github.com/PowerShell/PowerShell)
* [Terminal.Gui - Terminal GUI toolkit for .NET](https://github.com/migueldeicaza/gui.cs)

### History

* Version - 1.2.0

    - Added Input interpolation.
    - [Bug fix] Typo corrections.
    - Updated documentation.

* v1.0.6-beta.6
    - [Bug fix] Error on timeout - handled correctly on result errors list.
    - [Bug fix] Multiple interpolation keys on the same line now don't break.
    - [Bug fix] Script keys - Now keys are verified only on script save, avoiding file access error.
    - Added new unit tests for Vault feature.
    - Added unit tests check on the installer generator script.
    - Added 'run setup' option at end of installer generator script.

* v1.0.5-beta.5
    - Added Secret Vault Feature.
    - Added Pin to secure Vault.
    - Added Script interpolation: {{Key}} replaced by value stored on Vault.
    - Added Group Visualization on Script List.
    - Added Configuration to toggle Group Visualization on Script List.
    - Updated choose file buttons.

* v1.0.3-beta.4
    - Added timeout on script.
    - Added double click on the list to run script.

* v1.0.2-beta.3
    - Added error handling.
    - fix warning - error shift bug.
    - fix log bug.
    - fixed installer CEF dependencies bug.

* v1.0.1-beta.2
    - New Blazor + Chromely interface.
    - New script to generate installer.
    - New Light theme.

### Contact

contact@kelvysb.com
