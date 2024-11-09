param (
    [string]$ProjectDir,
    [string]$VersionMajor,
    [string]$VersionMinor,
    [string]$VersionPatch,
    [string]$VersionBuild
)

# Đường dẫn đầy đủ đến tệp version.rc
$VersionRcPath = "$ProjectDir\version.rc"

# Nội dung của file version.rc
$Content = @"
#include "windows.h"

VS_VERSION_INFO VERSIONINFO
 FILEVERSION $VersionMajor,$VersionMinor,$VersionPatch,$VersionBuild
 PRODUCTVERSION $VersionMajor,$VersionMinor,$VersionPatch,$VersionBuild
 FILEFLAGSMASK 0x3fL
#ifdef _DEBUG
 FILEFLAGS 0x1L
#else
 FILEFLAGS 0x0L
#endif
 FILEOS 0x4L
 FILETYPE 0x2L
 FILESUBTYPE 0x0L
BEGIN
    BLOCK "StringFileInfo"
    BEGIN
        BLOCK "040904b0"
        BEGIN
            VALUE "CompanyName", "DEZONE99\\0"
            VALUE "FileDescription", "Dynamic DLL Versioning\\0"
            VALUE "FileVersion", "$VersionMajor.$VersionMinor.$VersionPatch.$VersionBuild\\0"
            VALUE "InternalName", "engine.dll\\0"
            VALUE "OriginalFilename", "engine.dll\\0"
            VALUE "ProductName", "MAIN ENGINE\\0"
            VALUE "ProductVersion", "$VersionMajor.$VersionMinor.$VersionPatch.$VersionBuild\\0"
        END
    END
    BLOCK "VarFileInfo"
    BEGIN
        VALUE "Translation", 0x409, 1200
    END
END
"@

# Ghi nội dung ra file version.rc
Write-Host "huytd1 ProjectDir $ProjectDir"

Write-Host "huytd1 VersionMajor $VersionMajor"
Write-Host "huytd1 Content $Content"
Write-Host "Generating version.rc at $VersionRcPath"
Set-Content -Path $VersionRcPath -Value $Content
