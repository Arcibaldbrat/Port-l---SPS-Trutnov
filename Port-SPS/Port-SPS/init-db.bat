@echo off
REM Skript pro inicializaci databáze SPS Trutnov portálu

echo ===================================
echo SPS Trutnov - Inicializace Databáze
echo ===================================
echo.

echo 1. Kontrola .NET instalace...
dotnet --version
if errorlevel 1 (
    echo Chyba: .NET SDK není nainstalován!
    pause
    exit /b 1
)

echo.
echo 2. Obnovení balíčků NuGet...
dotnet restore
if errorlevel 1 (
    echo Chyba: Selhalo obnovení balíčků!
    pause
    exit /b 1
)

echo.
echo 3. Instalace/Aktualizace Entity Framework CLI...
dotnet tool update --global dotnet-ef

echo.
echo 4. Vytvoření/Aktualizace databáze...
dotnet ef database update
if errorlevel 1 (
    echo Chyba: Selhala aktualizace databáze!
    echo.
    echo Pokud je to první spuštění, budete možná muset vytvořit migraci:
    echo   dotnet ef migrations add InitialCreate
    pause
    exit /b 1
)

echo.
echo ===================================
echo Inicializace hotova!
echo ===================================
echo.
echo Aplikace je připravena k spuštění:
echo   dotnet run
echo.
pause
