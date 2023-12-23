@echo off

dotnet pack --configuration Release WizMachine\WizMachine.csproj -p:NuspecFile=WizMachine_RELEASE.nuspec

cmd /k