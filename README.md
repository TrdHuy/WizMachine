# 💩 For dev on Window run with cmd

#### 👉 Clone with hook
``` cmd
where gh >nul 2>nul && gh --version && git clone "https://github.com/TrdHuy/WizMachine.git" && cd "WizMachine" && powershell -command "$response = gh api repos/TrdHuy/WizMachine/contents/commit-msg?ref=document | ConvertFrom-Json; $decodedContent = [System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String($response.content)); Write-Host $decodedContent" > .git\hooks\commit-msg || echo GitHub CLI (gh) was not installed.
```
---
</br>
</br>
</br>

# 💩 To create a new version up automatically
``` cmd
gh workflow run AutoVersionBump -F branch="dev" -F type="build"
```

```
In there:
type is Version up type
type="major"
type="minor"
type="build"
type="patch"

force indicates that, you will version up even if previous commit is version up commit
force="true"
force="false"
```
---
</br>
</br>
</br>

# 💩 Publish to Nuget github source
<details open>
<summary>
 🌵🌵🌵🌵🌵🌵
</summary>
</br>

> ## 👉 Authorize nuget github source
> ``` cmd
> dotnet nuget add source "https://nuget.pkg.github.com/TrdHuy/index.json" --name "github" --username "trdtranduchuy@gmail.com" --password <TOKEN>
> ```
>
> ## 👉 Pack the project
> ``` cmd
> dotnet pack --configuration Release WizMachine\WizMachine.csproj -p:NuspecFile=WizMachine_RELEASE.nuspec
> ```
> 
> ## 👉 Push to server
> ``` cmd
> dotnet nuget push <pathToNupkg>" --api-key <TOKEN> --source "github"
> ```
> ## 👉 Run auto publish WizMachine nuget package via GITHUB ACTION
> * Make sure to install gh, then run this command in solution folder
> ```cmd
> gh workflow run ReleaseAndPublishNuget
> ```
> * OR you can use github api via powershell
> ```ps1
> $payload = @{
>     ref = "<YOUR TARGET BRANCH>"
> } | ConvertTo-Json
> Invoke-RestMethod -Uri "https://api.github.com/repos/TrdHuy/WizMachine/actions/workflows/80327278/dispatches" `
>     -Headers @{ Authorization = "token <YOUR GITHUB TOKEN>" } `
>     -Method Post `
>     -Body $payload `
>     -ContentType "application/json"
> ```
> ## 👉 Run auto publish WizMachine nuget package on LOCAL
>
> * Create "local.config" file in Solution DIR
> ``` xml
> <?xml version="1.0" encoding="utf-8"?>
> <configuration>
> 	<GITHUB_TOKEN>[Your github token PAT]</GITHUB_TOKEN>
> 	<REPO_OWNER>TrdHuy</REPO_OWNER>
> 	<REPO_NAME>WizMachine</REPO_NAME>
> 	<TARGET_RELEASED_BRANCH>dev</TARGET_RELEASED_BRANCH>
> 	<VERSION_UP_ID>2</VERSION_UP_ID>
> 	<WORK_FLOW_ISSUE_ID>1</WORK_FLOW_ISSUE_ID>
> 	<PROJECT_PATH>WizMachine/WizMachine.csproj</PROJECT_PATH>
> 	<PUBLISH_DIR>light_publish</PUBLISH_DIR>
> 	<NUGET_PUBLISH_DIR>nuget_publish</NUGET_PUBLISH_DIR>
> 	<NUSPEC_FILE_PATH>WizMachine/WizMachine_RELEASE.nuspec</NUSPEC_FILE_PATH>
> 	<NUSPEC_FILE_NAME>WizMachine_RELEASE.nuspec</NUSPEC_FILE_NAME>
> 	<PROJECT_NAME>WizMachine</PROJECT_NAME>
> 	<PUBLISH_NAME_CONTAINER_FILE_PATH>bin/publishedZipPath</PUBLISH_NAME_CONTAINER_FILE_PATH>
> </configuration>
> ```
> * Create "nuget.config" file in Solution DIR
> ``` xml
> <?xml version="1.0" encoding="utf-8"?>
> <configuration>
>     <packageSources>
>         <clear />
>         <add key="github" value="https://nuget.pkg.github.com/TrdHuy/index.json" />
>     </packageSources>
>     <packageSourceCredentials>
>         <github>
>             <add key="Username" value="TrdHuy" />
>             <add key="ClearTextPassword" value="[Your github token PAT]" />
>         </github>
>     </packageSourceCredentials>
> </configuration>
> ```
> * Run release_and_publish_nuget.ps1
> > * powershell v7.0
> > ```cmd
> > pwsh -File release_and_publish_nuget.ps1
> > ```
> > * powershell v5.0
> > ```cmd
> > powershell -File release_and_publish_nuget.ps1
> > ```
</details>

---
