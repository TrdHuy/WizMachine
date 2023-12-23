
$TOKEN=$env:GITHUB_TOKEN
$OWNER="TrdHuy"
$REPO="WizMachine"
$BRANCH = "dev"
if (-not $TOKEN) {
    # Gán giá trị mặc định nếu $TOKEN rỗng
    $TOKEN = "hello"
}
$Test_prop = $env:Test_prop
if (-not $Test_prop) {
    # Gán giá trị mặc định nếu $TOKEN rỗng
    $Test_prop = "123456"
}
Write-Host Test_prop=$Test_prop

#Lấy commit cuối cùng của bản latest release
$result = Invoke-RestMethod -Uri "https://api.github.com/repos/$OWNER/$REPO/releases/latest" -Headers @{ Authorization = "token $TOKEN" }
$lastReleasedCommitSha = $result[0].target_commitish
$commitInfoRes = Invoke-RestMethod -Uri "https://api.github.com/repos/TrdHuy/WizMachine/commits/$lastReleasedCommitSha" -Headers @{ Authorization = "token $TOKEN" }
$lastReleasedCommitMes = $commitInfoRes.commit.message
$lastReleasedCommitMes 
#
# Sử dụng GitHub API để lấy thông tin về commit cuối cùng trên nhánh
$lastCommitOnBranchRes = Invoke-RestMethod -Uri "https://api.github.com/repos/$OWNER/$REPO/commits/$BRANCH" -Headers @{ Authorization = "token $TOKEN" }
$lastCommitOnBranchSha = $lastCommitOnBranchRes.sha
$lastCommitOnBranchMes = $lastCommitOnBranchRes.commit.message
$lastCommitOnBranchMes
# Hiển thị commit message
