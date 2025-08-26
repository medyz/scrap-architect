# Скрипт проверки установки Unity 2022.3 LTS
Write-Host "Проверка установки Unity 2022.3 LTS..." -ForegroundColor Cyan
Write-Host ""

# Проверка Unity Hub
Write-Host "1. Проверка Unity Hub..." -ForegroundColor Yellow
$unityHubPaths = @(
    "${env:ProgramFiles}\Unity\Hub\Editor\Unity Hub.exe",
    "${env:ProgramFiles(x86)}\Unity\Hub\Editor\Unity Hub.exe",
    "${env:LOCALAPPDATA}\Programs\Unity Hub\Unity Hub.exe"
)

$unityHubFound = $false
foreach ($path in $unityHubPaths) {
    if (Test-Path $path) {
        Write-Host "   Unity Hub найден: $path" -ForegroundColor Green
        $unityHubFound = $true
        break
    }
}

if (-not $unityHubFound) {
    Write-Host "   Unity Hub не найден" -ForegroundColor Red
    Write-Host "   Скачайте с: https://unity.com/download" -ForegroundColor Blue
}

# Проверка Unity Editor
Write-Host ""
Write-Host "2. Проверка Unity Editor..." -ForegroundColor Yellow
$unityPaths = @(
    "${env:ProgramFiles}\Unity\Hub\Editor\2022.3.15f1\Editor\Unity.exe",
    "${env:ProgramFiles}\Unity\Hub\Editor\2022.3.20f1\Editor\Unity.exe"
)

$unityFound = $false
foreach ($path in $unityPaths) {
    if (Test-Path $path) {
        Write-Host "   Unity Editor найден: $path" -ForegroundColor Green
        $unityFound = $true
        break
    }
}

if (-not $unityFound) {
    Write-Host "   Unity Editor не найден" -ForegroundColor Red
    Write-Host "   Установите через Unity Hub" -ForegroundColor Blue
}

# Проверка Visual Studio
Write-Host ""
Write-Host "3. Проверка Visual Studio..." -ForegroundColor Yellow
$vsPaths = @(
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe",
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Professional\Common7\IDE\devenv.exe"
)

$vsFound = $false
foreach ($path in $vsPaths) {
    if (Test-Path $path) {
        Write-Host "   Visual Studio найден: $path" -ForegroundColor Green
        $vsFound = $true
        break
    }
}

if (-not $vsFound) {
    Write-Host "   Visual Studio не найден" -ForegroundColor Red
    Write-Host "   Скачайте с: https://visualstudio.microsoft.com/" -ForegroundColor Blue
}

# Проверка Git
Write-Host ""
Write-Host "4. Проверка Git..." -ForegroundColor Yellow
$gitVersion = git --version 2>$null
if ($gitVersion) {
    Write-Host "   Git найден: $gitVersion" -ForegroundColor Green
} else {
    Write-Host "   Git не найден" -ForegroundColor Red
    Write-Host "   Скачайте с: https://git-scm.com/" -ForegroundColor Blue
}

# Проверка .NET SDK
Write-Host ""
Write-Host "5. Проверка .NET SDK..." -ForegroundColor Yellow
$dotnetVersion = dotnet --version 2>$null
if ($dotnetVersion) {
    Write-Host "   .NET SDK найден: $dotnetVersion" -ForegroundColor Green
} else {
    Write-Host "   .NET SDK не найден" -ForegroundColor Red
    Write-Host "   Скачайте с: https://dotnet.microsoft.com/download" -ForegroundColor Blue
}

# Рекомендации
Write-Host ""
Write-Host "Рекомендации:" -ForegroundColor Magenta

if ($unityHubFound -and $unityFound -and $vsFound) {
    Write-Host "   Все основные компоненты установлены!" -ForegroundColor Green
    Write-Host "   Можете создавать Unity проект" -ForegroundColor Green
} else {
    Write-Host "   Не все компоненты установлены" -ForegroundColor Red
    Write-Host "   Следуйте инструкциям в UNITY_SETUP.md" -ForegroundColor Blue
}

Write-Host ""
Write-Host "Полезные ссылки:" -ForegroundColor Cyan
Write-Host "   Unity Hub: https://unity.com/download" -ForegroundColor Blue
Write-Host "   Visual Studio: https://visualstudio.microsoft.com/" -ForegroundColor Blue
Write-Host "   Git: https://git-scm.com/" -ForegroundColor Blue
Write-Host "   .NET SDK: https://dotnet.microsoft.com/download" -ForegroundColor Blue
