
# FOR CSPROJ
# <Target Name="ExecuteScript" AfterTargets="DownloadScript">
#     <Exec Command="powershell -File '$(ProjectDir)\SignToolUtilForClient.ps1' -filesToSign '$(TargetDir)ArtWiz.exe,$(TargetDir)ArtWiz.dll'"/>
# </Target>

# FOR PSW
# $filesToSign = "ArtWiz.exe,ArtWiz.dll"
# .\SignToolUtilForClient.ps1 -filesToSign $filesToSign
param (
     [string]$filesToSign,
     [string]$localConfigPath,
     [string]$prvtoken
)
$_TOKEN = $prvtoken
$ISLOCAL = $env:ISLOCAL
if (-not $ISLOCAL) {
     $ISLOCAL = $true
}
 
Write-Host filesToSign: $filesToSign
Write-Host localConfigPath: $localConfigPath
Write-Host isLocal: $ISLOCAL

if (-not $_TOKEN) {
     Write-Host "Assign local variable"

     $localXmlString = Get-Content -Raw -Path $localConfigPath
	
     # Tạo đối tượng XmlDocument và load chuỗi XML vào nó
     $localXmlDoc = New-Object System.Xml.XmlDocument
     $localXmlDoc.PreserveWhitespace = $true
     $localXmlDoc.LoadXml($localXmlString)
     $_TOKEN = $localXmlDoc.configuration.GITHUB_TOKEN

}

if (-not $_TOKEN) {
     throw "======= FATAL =====> GITHUB_TOKEN must not be null."
}

#if (-Not (Get-Command signtool -ErrorAction SilentlyContinue)) {
#    throw "======= FATAL =====> signtool is not available.`
#Please make sure it is installed and added to your system's PATH.`
#The location of the usual signtool is: 'C:\Program Files (x86)\Windows Kits\10\bin\'"
#}

$headers = @{
     Authorization = "token $_TOKEN"
}
$url = "https://raw.githubusercontent.com/TrdHuy/_TrdBuildPlugin/master/SignToolUtil.ps1"
$script = Invoke-RestMethod -Uri $url -Headers $headers

$modifiedScriptContent = "`$filesToSign = `"$filesToSign`"`n" `
     + "`$TOKEN = `"$_TOKEN`"`n" `
     + $script
Write-Host Start signing internal
Invoke-Expression $modifiedScriptContent