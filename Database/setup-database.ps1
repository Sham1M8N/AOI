# Creates the AOI LocalDB database file and applies schema.sql + seed data.
# Run this once after cloning the repo, before opening AOI.sln.

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$appDataDir = Join-Path $repoRoot "AOI\App_Data"
$mdfPath = Join-Path $appDataDir "AOIdb.mdf"
$ldfPath = Join-Path $appDataDir "AOIdb_log.ldf"
$schemaSql = Join-Path $repoRoot "Database\schema.sql"

if (Test-Path $mdfPath) {
    Write-Host "AOI/App_Data/AOIdb.mdf already exists - delete it first if you want to recreate the database." -ForegroundColor Yellow
    exit 1
}

New-Item -ItemType Directory -Force -Path $appDataDir | Out-Null

$sqlLocalDb = "C:\Program Files\Microsoft SQL Server\150\Tools\Binn\SqlLocalDB.exe"
$sqlcmd = Get-Command sqlcmd -ErrorAction SilentlyContinue
if (-not $sqlcmd) {
    $sqlcmdPath = "C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\170\Tools\Binn\SQLCMD.EXE"
} else {
    $sqlcmdPath = $sqlcmd.Source
}

if (Test-Path $sqlLocalDb) {
    & $sqlLocalDb start MSSQLLocalDB | Out-Null
}

Write-Host "Creating database at $mdfPath ..."
$createDb = "CREATE DATABASE AOIdb ON PRIMARY (NAME='AOIdb', FILENAME='$mdfPath') LOG ON (NAME='AOIdb_log', FILENAME='$ldfPath');"
& $sqlcmdPath -S "(localdb)\MSSQLLocalDB" -Q $createDb

Write-Host "Applying schema.sql ..."
& $sqlcmdPath -S "(localdb)\MSSQLLocalDB" -d "AOIdb" -i $schemaSql

Write-Host "Detaching so the app's own connection (AttachDbFilename) can use it ..."
& $sqlcmdPath -S "(localdb)\MSSQLLocalDB" -Q "ALTER DATABASE AOIdb SET SINGLE_USER WITH ROLLBACK IMMEDIATE; EXEC sp_detach_db 'AOIdb';"

Write-Host "Done. Default admin login: admin / Admin123! (change this after first login)." -ForegroundColor Green
