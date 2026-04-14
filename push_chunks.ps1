Remove-Item -Recurse -Force .git -ErrorAction Ignore
git init
git remote add origin https://github.com/alirizamistik17/MergeCoin_Game.git

Write-Host "Commit 1: Settings and base files"
git add .gitignore *.sln *.csproj .vscode/ Packages/ ProjectSettings/ UserSettings/
git commit -m "Add base project files"
git branch -M main
git push -u origin main

$dirs = @("2D Casual UI", "GamedevDreamer", "Gems and gold", "JMO Assets", "Scenes", "Settings", "TextMesh Pro", "Universal Stylized UI", "_Project")
foreach ($d in $dirs) {
    Write-Host "Committing $d..."
    git add "Assets/$d/"
    git add "Assets/$d.meta"
    git commit -m "Add $d"
    git push origin main
}

Write-Host "Committing remaining files..."
git add .
git commit -m "Add remaining assets"
git push origin main
