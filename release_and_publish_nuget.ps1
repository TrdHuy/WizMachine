
$ISLOCAL=$env:ISLOCAL ?? $true
$psVersion = $PSVersionTable.PSVersion

# In ra phiên bản PowerShell
Write-Host "PowerShell Version: $($psVersion.Major).$($psVersion.Minor).$($psVersion.Build)"

$TOKEN=$env:GITHUB_TOKEN
$OWNER=$env:REPO_OWNER
$REPO=$env:REPO_NAME
$BRANCH=$env:TARGET_RELEASED_BRANCH

$VERSION_UP_ID = $env:VERSION_UP_ID
$PROJECT_NAME = $env:PROJECT_NAME
$PROJECT_PATH = $env:PROJECT_PATH
$PUBLISH_DIR = $env:PUBLISH_DIR
$PUBLISH_NAME_CONTAINER_FILE_PATH = $env:PUBLISH_NAME_CONTAINER_FILE_PATH

$NUGET_PUBLISH_DIR = $env:NUGET_PUBLISH_DIR
$NUSPEC_FILE_PATH = $env:NUSPEC_FILE_PATH
$NUSPEC_FILE_NAME = $env:NUSPEC_FILE_NAME

if ($ISLOCAL -eq $true) {
	Write-Host "Assign local variable"

	$localXmlString = Get-Content -Raw -Path "local.config"
	
	# Tạo đối tượng XmlDocument và load chuỗi XML vào nó
	$localXmlDoc = New-Object System.Xml.XmlDocument
	$localXmlDoc.PreserveWhitespace = $true
	$localXmlDoc.LoadXml($localXmlString)

	$TOKEN=$localXmlDoc.configuration.GITHUB_TOKEN
	$OWNER=$localXmlDoc.configuration.REPO_OWNER
	$REPO=$localXmlDoc.configuration.REPO_NAME
	$BRANCH=$localXmlDoc.configuration.TARGET_RELEASED_BRANCH

	$VERSION_UP_ID = $localXmlDoc.configuration.VERSION_UP_ID
	$PROJECT_PATH = $localXmlDoc.configuration.PROJECT_PATH
	$PUBLISH_DIR = $localXmlDoc.configuration.PUBLISH_DIR
	$NUGET_PUBLISH_DIR = $localXmlDoc.configuration.NUGET_PUBLISH_DIR
	$NUSPEC_FILE_PATH = $localXmlDoc.configuration.NUSPEC_FILE_PATH
	$NUSPEC_FILE_NAME = $localXmlDoc.configuration.NUSPEC_FILE_NAME
	$PROJECT_NAME = $localXmlDoc.configuration.PROJECT_NAME
	$PUBLISH_NAME_CONTAINER_FILE_PATH = $localXmlDoc.configuration.PUBLISH_NAME_CONTAINER_FILE_PATH
}

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

if (-not $VERSION_UP_ID) {
    throw "VERSION_UP_ID must not be null "
}
if (-not $PROJECT_PATH) {
    throw "PROJECT_PATH must not be null "
}
if (-not $PUBLISH_DIR) {
    throw "PUBLISH_DIR must not be null "
}
if (-not $NUSPEC_FILE_PATH) {
    throw "NUSPEC_FILE_PATH must not be null "
}
if (-not $NUGET_PUBLISH_DIR) {
    throw "NUGET_PUBLISH_DIR must not be null "
}
if (-not $NUSPEC_FILE_NAME) {
    throw "NUSPEC_FILE_NAME must not be null "
}
if (-not $PUBLISH_NAME_CONTAINER_FILE_PATH) {
    throw "PUBLISH_NAME_CONTAINER_FILE_PATH must not be null "
}

Write-Host ================================
Write-Host OWNER=$OWNER 
Write-Host REPO=$REPO 
Write-Host BRANCH=$BRANCH 
Write-Host VERSION_UP_ID=$VERSION_UP_ID 
Write-Host PROJECT_PATH=$PROJECT_PATH 
Write-Host PUBLISH_DIR=$PUBLISH_DIR 
Write-Host NUSPEC_FILE_PATH=$NUSPEC_FILE_PATH 
Write-Host NUGET_PUBLISH_DIR=$NUGET_PUBLISH_DIR
Write-Host PROJECT_NAME=$PROJECT_NAME
Write-Host ================================`n`n`n

#Lấy commit cuối cùng của bản latest release
$result = Invoke-RestMethod -Uri "https://api.github.com/repos/$OWNER/$REPO/releases/latest" -Headers @{ Authorization = "token $TOKEN" }
$lastReleasedCommitSha = $result[0].target_commitish
$commitInfoRes = Invoke-RestMethod -Uri "https://api.github.com/repos/$OWNER/$REPO/commits/$lastReleasedCommitSha" -Headers @{ Authorization = "token $TOKEN" }
$lastReleasedCommitMes = $commitInfoRes.commit.message
Write-Host lastReleasedCommitMes=$lastReleasedCommitMes 
#
# Sử dụng GitHub API để lấy thông tin về commit cuối cùng trên nhánh
$lastCommitOnBranchRes = Invoke-RestMethod -Uri "https://api.github.com/repos/$OWNER/$REPO/commits/$BRANCH" -Headers @{ Authorization = "token $TOKEN" }
$lastCommitOnBranchSha = $lastCommitOnBranchRes.sha
$lastCommitOnBranchMes = $lastCommitOnBranchRes.commit.message
Write-Host lastCommitOnBranchMes=$lastCommitOnBranchMes

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

function Create-NewRelease ($TagName, $ReleaseName, $ReleaseBody, $AssetPath, $AssetName) {
	$uri = "https://api.github.com/repos/$OWNER/$REPO/releases"
	$headers = @{
		"Authorization" = "token $TOKEN"
		"Accept" = "application/vnd.github.v3+json"
	}

	$body = @{
		tag_name = $TagName
		name = $ReleaseName
		body = $ReleaseBody
		draft = $false
		prerelease = $false
	} | ConvertTo-Json

	$response = Invoke-RestMethod -Uri $uri -Method Post -Headers $headers -Body $body

	# In ra thông tin release được tạo mới
	Write-Host "Release created. ID: $($response.id), Name: $($response.name)"
	$RELEASE_ID=$response.id


	# Tải lên asset vào release
	# Tải lên asset vào release
	$url = "https://uploads.github.com/repos/$OWNER/$REPO/releases/$RELEASE_ID/assets?name=$AssetName"
	$headers = @{
		Authorization = "token $TOKEN"
		Accept = "application/vnd.github.v3+json"
	}

	Invoke-RestMethod -Uri $url -Method Post -Headers $headers -InFile $AssetPath -ContentType "application/zip"
}

# Trích xuất thông tin từ các chuỗi
$lastReleasedInfo = Extract-InfoFromMessage $lastReleasedCommitMes
$lastCommitOnBranchInfo = Extract-InfoFromMessage $lastCommitOnBranchMes
Write-Host lastReleasedInfo.IssueId=($lastReleasedInfo.IssueId)
Write-Host lastCommitOnBranchInfo=$lastCommitOnBranchInfo

$nupkgFileName=""
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
	
		$xmlDocument.package.metadata.version = $lastCommitOnBranchVersion.ToString()
		$newXmlString = $xmlDocument.OuterXml
		Write-Host "New: $newXmlString"
		$nupkgFileName= ($xmlDocument.package.metadata.id) + "." + $lastCommitOnBranchVersion.ToString() + ".nupkg"
		$scriptRoot = $PSScriptRoot
		$nupkgFilePath = $scriptRoot + "\" + $NUGET_PUBLISH_DIR + "\" + $nupkgFileName
		Write-Host "scriptRoot: $scriptRoot"
		Write-Host "nupkgFileName: $nupkgFileName"
		Write-Host "nupkgFilePath: $nupkgFilePath"
		
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
 		msbuild /t:Restore
		msbuild $PROJECT_PATH /t:Publish /p:Configuration=Release /p:PublishDir=$PUBLISH_DIR /p:DebugType=embedded /p:DebugSymbols=false /p:GenerateDependencyFile=false
		try {
			if ($ISLOCAL -eq $true) {
				$cache = dotnet nuget remove source "github"
				Write-Host cache=$cache
			}
			$cache = dotnet nuget add source "https://nuget.pkg.github.com/TrdHuy/index.json" --name "github" --username "trdtranduchuy@gmail.com" --password $TOKEN
			Write-Host cache=$cache
		} finally{}
		$cache = dotnet pack --configuration Release $PROJECT_PATH -p:NuspecFile=$NUSPEC_FILE_NAME --no-build -o $NUGET_PUBLISH_DIR
		Write-Host cache=$cache
		dotnet nuget push $nupkgFilePath --api-key $TOKEN --source "github"


		#============= Create new release=============
		$assetFilePath = Get-Content -Raw -Path $PUBLISH_NAME_CONTAINER_FILE_PATH
		$assetFilePath=$assetFilePath.Trim()
		$assetName = Split-Path $assetFilePath -Leaf
		$tagName = ($xmlDocument.package.metadata.id) + "." + $lastCommitOnBranchVersion.ToString()
		Write-Host assetFilePath=$assetFilePath
		Write-Host assetName=$assetName
		Write-Host tagName=$tagName

		#TODO: CREATE release note
		$releaseBody =  "Update new version"
		Create-NewRelease $tagName $tagName $releaseBody $assetFilePath $assetName
		#============= Create new release=============
		
		return 1
    } else {
		Write-Host "Latest version has been released!"
		return 2
	}
}
