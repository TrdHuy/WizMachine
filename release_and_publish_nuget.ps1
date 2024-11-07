param (
)
$psVersion = $PSVersionTable.PSVersion
$scriptRoot = (Get-Location).Path
# In ra phiên bản PowerShell
Write-Host "PowerShell Version: $($psVersion.Major).$($psVersion.Minor).$($psVersion.Build)"
Write-Host "ScriptRoot: $scriptRoot"
$ISDEBUG = $false
$ISLOCAL = $env:ISLOCAL
if (-not $ISLOCAL) {
	$ISLOCAL = $true
}

$TOKEN = $env:GITHUB_TOKEN
$SIGINING_TOKEN = $env:SIGINING_TOKEN
$OWNER = $env:REPO_OWNER
$REPO = $env:REPO_NAME
$BRANCH = $env:TARGET_RELEASED_BRANCH

$WORK_FLOW_ISSUE_ID = $env:WORK_FLOW_ISSUE_ID
$VERSION_UP_ID = $env:VERSION_UP_ID
$PROJECT_NAME = $env:PROJECT_NAME
$PROJECT_PATH = $env:PROJECT_PATH
$PUBLISH_DIR = $env:PUBLISH_DIR
$PUBLISH_NAME_CONTAINER_FILE_PATH = $env:PUBLISH_NAME_CONTAINER_FILE_PATH
$NUGET_PUBLISH_DESCRIPTION_TITLE = $env:NUGET_PUBLISH_DESCRIPTION_TITLE
$NUGET_PUBLISH_DIR = $env:NUGET_PUBLISH_DIR
$RAW_NUSPEC_FILE_PATH = $env:RAW_NUSPEC_FILE_PATH
$RAW_NUSPEC_FILE_NAME = $env:RAW_NUSPEC_FILE_NAME

if ($ISLOCAL -eq $true) {
	Write-Host "Assign local variable"

	$localXmlString = Get-Content -Raw -Path "local.config"
	
	# Tạo đối tượng XmlDocument và load chuỗi XML vào nó
	$localXmlDoc = New-Object System.Xml.XmlDocument
	$localXmlDoc.PreserveWhitespace = $true
	$localXmlDoc.LoadXml($localXmlString)

	$TOKEN = $localXmlDoc.configuration.GITHUB_TOKEN
	$SIGINING_TOKEN = $localXmlDoc.configuration.SIGINING_TOKEN
	$OWNER = $localXmlDoc.configuration.REPO_OWNER
	$REPO = $localXmlDoc.configuration.REPO_NAME
	$BRANCH = $localXmlDoc.configuration.TARGET_RELEASED_BRANCH

	$WORK_FLOW_ISSUE_ID = $localXmlDoc.configuration.WORK_FLOW_ISSUE_ID
	$VERSION_UP_ID = $localXmlDoc.configuration.VERSION_UP_ID
	$PROJECT_PATH = $localXmlDoc.configuration.PROJECT_PATH
	$PUBLISH_DIR = $localXmlDoc.configuration.PUBLISH_DIR
	$NUGET_PUBLISH_DESCRIPTION_TITLE = $localXmlDoc.configuration.NUGET_PUBLISH_DESCRIPTION_TITLE
	$NUGET_PUBLISH_DIR = $localXmlDoc.configuration.NUGET_PUBLISH_DIR
	$RAW_NUSPEC_FILE_PATH = $localXmlDoc.configuration.RAW_NUSPEC_FILE_PATH
	$RAW_NUSPEC_FILE_NAME = $localXmlDoc.configuration.RAW_NUSPEC_FILE_NAME
	$PROJECT_NAME = $localXmlDoc.configuration.PROJECT_NAME
	$PUBLISH_NAME_CONTAINER_FILE_PATH = $localXmlDoc.configuration.PUBLISH_NAME_CONTAINER_FILE_PATH
}


if (-not $PROJECT_NAME) {
	throw "PROJECT_NAME must not be null "
}
if (-not $SIGINING_TOKEN) {
	throw "SIGINING_TOKEN must not be null "
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
if (-not $RAW_NUSPEC_FILE_PATH) {
	throw "NUSPEC_FILE_PATH must not be null "
}
if (-not $NUGET_PUBLISH_DIR) {
	throw "NUGET_PUBLISH_DIR must not be null "
}
if (-not $RAW_NUSPEC_FILE_NAME) {
	throw "RAW_NUSPEC_FILE_NAME must not be null "
}
if (-not $PUBLISH_NAME_CONTAINER_FILE_PATH) {
	throw "PUBLISH_NAME_CONTAINER_FILE_PATH must not be null "
}
if (-not $NUGET_PUBLISH_DESCRIPTION_TITLE) {
	$NUGET_PUBLISH_DESCRIPTION_TITLE = "Update and fix minor bugs:"	
}

$IS_FIRST_RELEASE = $($(Invoke-RestMethod -Uri "https://api.github.com/repos/$OWNER/$REPO/releases" -Method Get -ErrorAction Stop -Headers @{ Authorization = "token $TOKEN" }).Count -eq 0)

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
Write-Host PUBLISH_NAME_CONTAINER_FILE_PATH=$PUBLISH_NAME_CONTAINER_FILE_PATH
Write-Host NUGET_PUBLISH_DESCRIPTION_TITLE=$NUGET_PUBLISH_DESCRIPTION_TITLE
Write-Host IS_FIRST_RELEASE=$IS_FIRST_RELEASE
Write-Host ================================`n`n`n

function Convert-NupkgVersion {
	param (
		[string]$version
	)
 
	# Split the version string by '.'
	$versionParts = $version -split '\.'
 
	# Remove trailing zeros
	if ($versionParts[-1] -eq '0') {
		$versionParts = $versionParts[0..($versionParts.Length - 2)]
	}
 
	# Join the version parts back into a string
	$newVersion = $versionParts -join '.'
 
	return $newVersion
}

function Get-TitleAndIssueIdFromMessage($commitMessage) {
	
	Write-Host commitMessage=$commitMessage
	# Sử dụng biểu thức chính quy để trích xuất Issue ID và Commit Title

	if ($commitMessage -match '\[\#(\d+)\]\s*(.*)') {
		$issueId = $matches[1]
		$commitTitle = $matches[2]

		# In kết quả
		Write-Output "Issue ID: $issueId"
		Write-Output "Commit Title: $commitTitle"
		return @{
			IssueId     = $issueId
			CommitTitle = $commitTitle
		}
	}
 else {
		Write-Output "Không tìm thấy thông tin commit trong chuỗi."
	}
}

function Create-ReleaseNote ($baseSha, $headSha) {
	# Tạo URL để so sánh
	$compareUrl = "https://api.github.com/repos/$OWNER/$REPO/compare/$baseSha...$headSha"

	# Gửi yêu cầu API
	$headers = @{
		Authorization = "token $TOKEN"
		Accept        = "application/vnd.github.v3+json"
	}

	$response = Invoke-RestMethod -Uri $compareUrl -Method Get -Headers $headers

	# In thông tin so sánh
	Write-Host "Comparison between $baseSha and $headSha :"
	Write-Host "URL: $($response.html_url)"
	Write-Host "Status: $($response.status)"
	Write-Host "Ahead By: $($response.ahead_by)"
	Write-Host "Behind By: $($response.behind_by)"
	Write-Host "Commits: $($response.total_commits)"

	$commits = $response.commits | ForEach-Object { 
		$temp = Get-TitleAndIssueIdFromMessage $_.commit.message
		if ($temp -and $temp.IssueId -ne $VERSION_UP_ID -and $temp.IssueId -ne $WORK_FLOW_ISSUE_ID) {
			#TODO: dynamic the icon this
			"### :palm_tree: [#" + $temp.IssueId + "] " + $temp.CommitTitle + "`n"
		} 
	} 
	return ($commits -join "`n") + "`n`n" +
	"**Full Changelog**: https://github.com/$OWNER/$REPO/compare/$baseSha...$headSha" 
}


function Extract-InfoFromMessage ($message) {
	$regex = '\[#(\d+)\] Version up: (\d+\.\d+\.\d+\.\d+)'
	$match = $message -match $regex

	if ($match) {
		$issueId = $matches[1]
		$version = $matches[2]
		if ($issueId -ne $VERSION_UP_ID) {
			throw "Need to create version up CL first!" 
		}
		return @{
			IssueId = $issueId
			Version = $version
		}
	}
	else {
		Write-Host "Không thể trích xuất thông tin từ chuỗi thông điệp."
		throw "Need to create version up CL first!" 
		return $null
	}
}

function Create-NewRelease ($TagName, $ReleaseName, $ReleaseBody, $AssetFiles) {
	$uri = "https://api.github.com/repos/$OWNER/$REPO/releases"
	$headers = @{
		"Authorization" = "token $TOKEN"
		"Accept"        = "application/vnd.github.v3+json"
	}

	Write-Host ReleaseBody= $ReleaseBody
	$body = @{
		tag_name         = $TagName
		target_commitish = $BRANCH
		name             = $ReleaseName
		body             = $ReleaseBody
		draft            = $false
		prerelease       = $false
	} | ConvertTo-Json -EscapeHandling EscapeNonAscii
	
	Write-Host body= $body

	$response = Invoke-RestMethod -Uri $uri -Method Post -Headers $headers -Body $body

	# In ra thông tin release được tạo mới
	Write-Host "Release created. ID: $($response.id), Name: $($response.name)"
	$RELEASE_ID = $response.id

	# Tải lên asset vào release
	foreach ($asset in $AssetFiles) {
		$assetName = Split-Path -Leaf $asset
		$url = "https://uploads.github.com/repos/$OWNER/$REPO/releases/$RELEASE_ID/assets?name=$assetName"
		$headers = @{
			Authorization = "token $TOKEN"
			Accept        = "application/vnd.github.v3+json"
		}
		Invoke-RestMethod -Uri $url -Method Post -Headers $headers -InFile $asset -ContentType "application/zip"
	}
}

function Publish-Nuget ($publishDir,
	$releaseNote, 
	$nuspecXMlString, 
	$buildConfig, 
	$projectName,
	$platform,
	$publishVersion) {
	$filesToInclude = @(@{
			extension = "dll"
			target    = "lib\net6.0-windows7.0"
		}, 
		@{
			extension = "md"
			target    = "docs"
		})

	if ($buildConfig -eq "Debug") {
		$filesToInclude += @(
			@{
				extension = "lib"
				target    = "lib\net6.0-windows7.0"
			},
			@{
				extension = "pdb"
				target    = "lib\net6.0-windows7.0"
			},
			@{
				extension = "json"
				target    = "lib\net6.0-windows7.0"
			},
			@{
				extension = "exp"
				target    = "lib\net6.0-windows7.0"
			}
		)
	}
	# Tạo đối tượng XmlDocument và load chuỗi XML vào nó
	$xmlDocument = New-Object System.Xml.XmlDocument
	$xmlDocument.PreserveWhitespace = $true
	$xmlDocument.LoadXml($nuspecXMlString)
	
	if ($buildConfig -eq "Debug") {
		$xmlDocument.package.metadata.id = "$projectName$platform-Debug"
	}
	else {
		$xmlDocument.package.metadata.id = "$projectName$platform"
	}
	$xmlDocument.package.metadata.version = $publishVersion.ToString()
	$xmlDocument.package.metadata.releaseNotes = $releaseNote
	$xmlDocument.package.metadata.description = $NUGET_PUBLISH_DESCRIPTION_TITLE + "`n" + $releaseNote
	if ($xmlDocument.package.files.HasChildNodes -eq $true) {
		$needRemoveNode = $xmlDocument.package.files.FirstChild.ParentNode
		$xmlDocument.package.RemoveChild($needRemoveNode)
		$newFilesElement = $xmlDocument.CreateElement("files", $null)
		$xmlDocument.package.AppendChild($newFilesElement)
	}
	else {
		$newFilesElement = $xmlDocument.package.files
	}
	foreach ($item in $filesToInclude) {
		$newFileElement = $xmlDocument.CreateElement("file")
		$newFileElement.SetAttribute("src", $publishDir + "\*." + $item.extension)
		$newFileElement.SetAttribute("target", $item.target)
		$newFilesElement.AppendChild($newFileElement)
	}

	$nupkgFileName = ($xmlDocument.package.metadata.id) + "." + (Convert-NupkgVersion -version $publishVersion.ToString()) + ".nupkg"
	$nuspecFileName = ($xmlDocument.package.metadata.id) + "." + $publishVersion.ToString() + "." + $platform + ".nuspec"
	$absoluteRawNuspecPath = Resolve-Path $RAW_NUSPEC_FILE_PATH
	$absoluteRawNuspecParentPath = Split-Path $absoluteRawNuspecPath
	$nupkgFilePath = $scriptRoot + "\" + $NUGET_PUBLISH_DIR + "\" + $nupkgFileName
	$nuspecFilePath = $absoluteRawNuspecParentPath + "\" + $nuspecFileName
		
	Write-Host "scriptRoot: $scriptRoot"
	Write-Host "nupkgFileName: $nupkgFileName"
	Write-Host "nupkgFilePath: $nupkgFilePath"
		
	Write-Host "==================New .nuspec file content: ================="
	Write-Host ($xmlDocument.OuterXml)
	Write-Host "=============================================================`n`n`n"

	$settings = New-Object System.Xml.XmlWriterSettings
	$settings.Encoding = [System.Text.Encoding]::UTF8
	$settings.Indent = $true
	
	# Thử ghi XML vào tệp tin với StreamWriter và mã hóa UTF-8
	try {
		New-Item -Path $nuspecFilePath -ItemType File -Force
		$stream = New-Object System.IO.StreamWriter($nuspecFilePath, $false, [System.Text.Encoding]::UTF8)
		$xmlWriter = [System.Xml.XmlWriter]::Create($stream, $settings)
		$xmlDocument.Save($xmlWriter)
	}
	finally {
		if ($xmlWriter) {
			$xmlWriter.Close()
		}
		if ($stream) {
			$stream.Close()
		}
	}
			
	Write-Host "`n`n`n"
	Write-Host "==================Start publish project: ================="
	msbuild /t:Restore

	if ($buildConfig -eq "Debug") {
		msbuild $PROJECT_PATH /t:Publish /p:IsFromDotnet=true /p:GitToken=$SIGINING_TOKEN `
			/p:Configuration=$buildConfig /p:Platform=$platform /p:PublishDir=$publishDir
	}
	else {
		msbuild $PROJECT_PATH /t:Publish /p:IsFromDotnet=true /p:GitToken=$SIGINING_TOKEN `
			/p:Configuration=$buildConfig /p:Platform=$platform /p:PublishDir=$publishDir `
			/p:DebugType=embedded /p:DebugSymbols=false /p:GenerateDependencyFile=false  
	}
			
	Write-Host "==========================================================`n`n`n"
			
	Write-Host "==================Start packing project: ================="
	try {
		if ($ISLOCAL -eq $true) {
			$cache = dotnet nuget remove source "github"
			Write-Host $cache
		}
		$cache = dotnet nuget add source "https://nuget.pkg.github.com/TrdHuy/index.json" --name "github" --username "trdtranduchuy@gmail.com" --password $TOKEN
		Write-Host $cache
	}
	catch {
		Write-Host "An error occurred: $_.Exception.Message"
	} 
	finally {}
	$cache = dotnet pack --configuration $buildConfig $PROJECT_PATH -p:NuspecFile=$nuspecFilePath --no-build -o $NUGET_PUBLISH_DIR
	Write-Host $cache
	dotnet nuget push $nupkgFilePath --api-key $TOKEN --source "github"
	Write-Host "==========================================================`n`n`n"
		
}

# Sử dụng GitHub API để lấy thông tin về commit cuối cùng trên nhánh
$lastCommitOnBranchRes = Invoke-RestMethod -Uri "https://api.github.com/repos/$OWNER/$REPO/commits/$BRANCH" -Headers @{ Authorization = "token $TOKEN" }
$lastCommitOnBranchSha = $lastCommitOnBranchRes.sha
$lastCommitOnBranchMes = $lastCommitOnBranchRes.commit.message
Write-Host lastCommitOnBranchMes=$lastCommitOnBranchMes
Write-Host lastCommitOnBranchSha=$lastCommitOnBranchSha 

#TODO: Implement first release
if ($IS_FIRST_RELEASE -eq $true) {
	
}
else {
	if (-not $ISDEBUG) {
		#Lấy commit cuối cùng của bản latest release
		$result = Invoke-RestMethod -Uri "https://api.github.com/repos/$OWNER/$REPO/releases/latest" -Headers @{ Authorization = "token $TOKEN" }
		$latestReleaseTagName = $result[0].tag_name

		$tags = Invoke-RestMethod -Uri "https://api.github.com/repos/$OWNER/$REPO/tags" -Method Get -Headers @{ "Authorization" = "token $TOKEN" }
		# Lấy tag name của latest release từ danh sách các tags
		$latestReleaseTagInfo = ($tags | Where-Object { $_.name -eq $latestReleaseTagName })

		$lastReleasedCommitSha = $latestReleaseTagInfo.commit.sha
		$commitInfoRes = Invoke-RestMethod -Uri "https://api.github.com/repos/$OWNER/$REPO/commits/$lastReleasedCommitSha" -Headers @{ Authorization = "token $TOKEN" }
		$lastReleasedCommitMes = $commitInfoRes.commit.message
		Write-Host lastReleasedCommitMes=$lastReleasedCommitMes 
		Write-Host lastReleasedCommitSha=$lastReleasedCommitSha 

		# Trích xuất thông tin từ các chuỗi
		$lastReleasedInfo = Extract-InfoFromMessage $lastReleasedCommitMes
		$lastCommitOnBranchInfo = Extract-InfoFromMessage $lastCommitOnBranchMes
		Write-Host lastReleasedInfo.IssueId=($lastReleasedInfo.IssueId)
		Write-Host lastCommitOnBranchInfo=$lastCommitOnBranchInfo

		$nupkgFileName = ""
	}
	
	# So sánh các phiên bản và hiển thị kết quả
	if ($ISDEBUG -eq $true -or ($lastReleasedInfo -and $lastCommitOnBranchInfo)) {
		$lastReleasedVersion = [version]$lastReleasedInfo.Version
		$lastCommitOnBranchVersion = [version]$lastCommitOnBranchInfo.Version
		if ($ISDEBUG -eq $true) {
			$lastCommitOnBranchVersion = "100.100.100.100"
		}

		if ($ISDEBUG -eq $true -or ($lastCommitOnBranchVersion -gt $lastReleasedVersion)) {
			#Clean NugetPublish folder
			$nugetPublishFolderPath = $scriptRoot + "\" + $NUGET_PUBLISH_DIR
			if (Test-Path -Path $nugetPublishFolderPath) {
				Remove-Item -Path $nugetPublishFolderPath -Recurse -Force
				Write-Host "Directory '$nugetPublishFolderPath' has been deleted."
			}
			else {
				Write-Host "Directory '$nugetPublishFolderPath' does not exist. No deletion needed."
			}
			$assetFiles = @()
			$platform = "x64"
			$buildConfig = "Release"
			$publishDir = $nugetPublishFolderPath + "\" + $PUBLISH_DIR + $platform + $buildConfig
			$releaseNote = Create-ReleaseNote $lastReleasedCommitSha $lastCommitOnBranchSha 
			$xmlString = Get-Content -Raw -Path $RAW_NUSPEC_FILE_PATH

			Publish-Nuget -publishDir $publishDir `
				-releaseNote $releaseNote `
				-nuspecXMlString $xmlString `
				-buildConfig $buildConfig `
				-projectName $PROJECT_NAME `
				-platform $platform `
				-publishVersion $lastCommitOnBranchVersion
			$zipFileName = "$PROJECT_NAME.$platform.$buildConfig" + ".zip"
			$zipFilePath = Join-Path $nugetPublishFolderPath $zipFileName
			Compress-Archive -Path $publishDir -DestinationPath $zipFilePath -Force
			$assetFiles += $zipFilePath

			$platform = "x86"
			$buildConfig = "Release"
			$publishDir = $scriptRoot + "\" + $NUGET_PUBLISH_DIR + "\" + $PUBLISH_DIR + $platform + $buildConfig
			Publish-Nuget -publishDir $publishDir `
				-releaseNote $releaseNote `
				-nuspecXMlString $xmlString `
				-buildConfig $buildConfig `
				-projectName $PROJECT_NAME `
				-platform $platform `
				-publishVersion $lastCommitOnBranchVersion
			$zipFileName = "$PROJECT_NAME.$platform.$buildConfig" + ".zip"
			$zipFilePath = Join-Path $nugetPublishFolderPath $zipFileName
			Compress-Archive -Path $publishDir -DestinationPath $zipFilePath -Force
			$assetFiles += $zipFilePath

			$platform = "x64"
			$buildConfig = "Debug"
			$publishDir = $scriptRoot + "\" + $NUGET_PUBLISH_DIR + "\" + $PUBLISH_DIR + $platform + $buildConfig
			Publish-Nuget -publishDir $publishDir `
				-releaseNote $releaseNote `
				-nuspecXMlString $xmlString `
				-buildConfig $buildConfig `
				-projectName $PROJECT_NAME `
				-platform $platform `
				-publishVersion $lastCommitOnBranchVersion
			$zipFileName = "$PROJECT_NAME.$platform.$buildConfig" + ".zip"
			$zipFilePath = Join-Path $nugetPublishFolderPath $zipFileName
			Compress-Archive -Path $publishDir -DestinationPath $zipFilePath -Force
			$assetFiles += $zipFilePath

			$platform = "x86"
			$buildConfig = "Debug"
			$publishDir = $scriptRoot + "\" + $NUGET_PUBLISH_DIR + "\" + $PUBLISH_DIR + $platform + $buildConfig
			Publish-Nuget -publishDir $publishDir `
				-releaseNote $releaseNote `
				-nuspecXMlString $xmlString `
				-buildConfig $buildConfig `
				-projectName $PROJECT_NAME `
				-platform $platform `
				-publishVersion $lastCommitOnBranchVersion
			$zipFileName = "$PROJECT_NAME.$platform.$buildConfig" + ".zip"
			$zipFilePath = Join-Path $nugetPublishFolderPath $zipFileName
			Compress-Archive -Path $publishDir -DestinationPath $zipFilePath -Force
			$assetFiles += $zipFilePath

			Write-Host "==================Start create new release : ================="
			$tagName = $PROJECT_NAME + "." + $lastCommitOnBranchVersion.ToString()
			Write-Host tagName=$tagName
			$releaseBody = "# " + $NUGET_PUBLISH_DESCRIPTION_TITLE + "`n" + $releaseNote
			Create-NewRelease $tagName $tagName $releaseBody $assetFiles
			Write-Host "==========================================================`n`n`n"

			return "SUCCESS"
		}
		else {
			Write-Host "Latest version has been released!"
			return "HAS_BEEN_RELEASED"
		}
	}
}
