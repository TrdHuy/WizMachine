# WizMachine

### For dev on Window run with cmd

#### 👉 Clone with hook
``` cmd
where gh >nul 2>nul && gh --version && git clone "https://github.com/TrdHuy/WizMachine.git" && cd "WizMachine" && powershell -command "$response = gh api repos/TrdHuy/WizMachine/contents/commit-msg?ref=document | ConvertFrom-Json; $decodedContent = [System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String($response.content)); Write-Host $decodedContent" > .git\hooks\commit-msg || echo GitHub CLI (gh) was not installed.
```

#### 👉 Publish new release
``` cmd
git tag wengine/v<new version here>
git push origin wengine/v<new version here>
```
