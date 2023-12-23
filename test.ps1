
$psVersion = $PSVersionTable.PSVersion

# In ra phiên bản PowerShell
Write-Host "PowerShell Version: $($psVersion.Major).$($psVersion.Minor).$($psVersion.Build)"


$TOKEN=$env:GITHUB_TOKEN
$OWNER=$env:REPO_OWNER
$REPO=$env:REPO_NAME
$BRANCH=$env:TARGET_RELEASED_BRANCH

$VERSION_UP_ID = $env:VERSION_UP_ID ?? { throw "VERSION_UP_ID is null or not defined." }
$PROJECT_PATH = $env:PROJECT_PATH ?? { throw "PROJECT_PATH is null or not defined." }
$PUBLISH_DIR = $env:PUBLISH_DIR ?? { throw "PUBLISH_DIR is null or not defined." }
$NUSPEC_FILE_PATH = $env:NUSPEC_FILE_PATH ?? { throw "NUSPEC_FILE_PATH is null or not defined." }
Write-Host NUSPEC_FILE_PATH=$NUSPEC_FILE_PATH
if (-not $TOKEN) {
    throw "GITHUB_TOKEN must not be null "
}
if (-not $OWNER) {
    throw "REPO_OWNER must not be null "
}
if (-not $REPO) {
    throw "REPO_NAME must not be null "
}
if (-not $BRANCH) {
    throw "TARGET_RELEASED_BRANCH must not be null "
}

#Lấy commit cuối cùng của bản latest release
$result = Invoke-RestMethod -Uri "https://api.github.com/repos/$OWNER/$REPO/releases/latest" -Headers @{ Authorization = "token $TOKEN" }
$lastReleasedCommitSha = $result[0].target_commitish
$commitInfoRes = Invoke-RestMethod -Uri "https://api.github.com/repos/TrdHuy/WizMachine/commits/$lastReleasedCommitSha" -Headers @{ Authorization = "token $TOKEN" }
$lastReleasedCommitMes = $commitInfoRes.commit.message
Write-Host lastReleasedCommitMes=$lastReleasedCommitMes 
#
# Sử dụng GitHub API để lấy thông tin về commit cuối cùng trên nhánh
$lastCommitOnBranchRes = Invoke-RestMethod -Uri "https://api.github.com/repos/$OWNER/$REPO/commits/$BRANCH" -Headers @{ Authorization = "token $TOKEN" }
$lastCommitOnBranchSha = $lastCommitOnBranchRes.sha
$lastCommitOnBranchMes = $lastCommitOnBranchRes.commit.message
Write-Host lastCommitOnBranchMes=$$lastCommitOnBranchMes

# Hiển thị commit message



$VERSION_UP_ID = 2
$PROJECT_PATH = "WizMachine/WizMachine.csproj"
$PUBLISH_DIR = "light_publish"
$NUSPEC_FILE_PATH = "WizMachine\WizMachine_RELEASE.nuspec"

# Hàm để trích xuất thông tin từ chuỗi thông điệp
function Extract-InfoFromMessage ($message) {
    $regex = '\[#(\d+)\] Version up: (\d+\.\d+\.\d+\.\d+)'
    $match = $message -match $regex

    if ($match) {
        $issueId = $matches[1]
        $version = $matches[2]
		if($issueId -ne $VERSION_UP_ID) {
			throw "Need to create version up CL first!" 
		}
		return @{
			IssueId = $issueId
			Version = $version
		}
    } else {
        Write-Host "Không thể trích xuất thông tin từ chuỗi thông điệp."
		throw "Need to create version up CL first!" 
        return $null
    }
}

# Trích xuất thông tin từ các chuỗi
$lastReleasedInfo = Extract-InfoFromMessage $lastReleasedCommitMes
$lastCommitOnBranchInfo = Extract-InfoFromMessage $lastCommitOnBranchMes
Write-Host lastReleasedInfo.IssueId=($lastReleasedInfo.IssueId)
Write-Host lastCommitOnBranchInfo=$lastCommitOnBranchInfo

# So sánh các phiên bản và hiển thị kết quả
if ($lastReleasedInfo -and $lastCommitOnBranchInfo) {
    $lastReleasedVersion = [version]$lastReleasedInfo.Version
    $lastCommitOnBranchVersion = [version]$lastCommitOnBranchInfo.Version
    if ($lastCommitOnBranchVersion -gt $lastReleasedVersion) {
		$xmlString = Get-Content -Raw -Path $NUSPEC_FILE_PATH
	
		# Tạo đối tượng XmlDocument và load chuỗi XML vào nó
		$xmlDocument = New-Object System.Xml.XmlDocument
		$xmlDocument.PreserveWhitespace = $true
		$xmlDocument.LoadXml($xmlString)
	
		$element1 = $xmlDocument.package.metadata.version = $lastCommitOnBranchVersion.ToString()
		Write-Host "Value of element1: $element1"
		$newXmlString = $xmlDocument.OuterXml
		Write-Host "New: $newXmlString"
	
		$settings = New-Object System.Xml.XmlWriterSettings
		$settings.Encoding = [System.Text.Encoding]::UTF8
		$settings.Indent = $true
	
		# Thử ghi XML vào tệp tin với StreamWriter và mã hóa UTF-8
		try {
			$stream = New-Object System.IO.StreamWriter($NUSPEC_FILE_PATH, $false, [System.Text.Encoding]::UTF8)
			$xmlWriter = [System.Xml.XmlWriter]::Create($stream, $settings)
			$xmlDocument.Save($xmlWriter)
		} finally {
			if ($xmlWriter) {
			$xmlWriter.Close()
			}
			if ($stream) {
			$stream.Close()
			}
		}
		# return 1 if successfully create new publish
 		#msbuild /t:Restore
		#msbuild $PROJECT_PATH /t:Publish /p:Configuration=Release /p:PublishDir=$PUBLISH_DIR /p:DebugType=embedded /p:DebugSymbols=false /p:GenerateDependencyFile=false
		
		return 1
    } else {
		Write-Host "Latest version has been released!"
		return 2
	}
}

