# WizMachine

### For dev on Window run with cmd

#### 👉 Clone with hook
``` cmd
where gh >nul 2>nul && gh --version && git clone "https://github.com/TrdHuy/WizMachine.git" && cd "WizMachine" && powershell -command "$response = gh api repos/TrdHuy/WizMachine/contents/commit-msg?ref=document | ConvertFrom-Json; $decodedContent = [System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String($response.content)); Write-Host $decodedContent" > .git\hooks\commit-msg || echo GitHub CLI (gh) was not installed.
```

### Publish to Nuget github source

#### 👉 Authorize nuget github source
``` cmd
dotnet nuget add source "https://nuget.pkg.github.com/TrdHuy/index.json" --name "github" --username "trdtranduchuy@gmail.com" --password <TOKEN>
```

#### 👉 Pack the project
``` cmd
dotnet pack --configuration Release WizMachine\WizMachine.csproj -p:NuspecFile=WizMachine_RELEASE.nuspec
```

#### 👉 Push to server
``` cmd
dotnet nuget push <pathToNupkg>" --api-key <TOKEN> --source "github"
```