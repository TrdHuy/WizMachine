
$TOKEN=$env:GITHUB_TOKEN
$OWNER=$env:REPO_OWNER
$REPO=$env:REPO_NAME
$BRANCH=$env:TARGET_RELEASED_BRANCH
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
$lastReleasedCommitMes 
#
# Sử dụng GitHub API để lấy thông tin về commit cuối cùng trên nhánh
$lastCommitOnBranchRes = Invoke-RestMethod -Uri "https://api.github.com/repos/$OWNER/$REPO/commits/$BRANCH" -Headers @{ Authorization = "token $TOKEN" }
$lastCommitOnBranchSha = $lastCommitOnBranchRes.sha
$lastCommitOnBranchMes = $lastCommitOnBranchRes.commit.message
$lastCommitOnBranchMes
# Hiển thị commit message
