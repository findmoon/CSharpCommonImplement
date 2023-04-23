@ECHO off
Echo Please run as administrator
setlocal
cd /d %~dp0
set csDir=%cd%\Program.cs
echo %csDir%
powershell
set-executionpolicy remotesigned;
$source = Get-Content -Raw -Path %csDir%;
Add-Type -TypeDefinition "$source";
[Program]::Main()