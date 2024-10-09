@echo off
set P=%~dp0
for /f "delims=""" %%a in ('dir CSharpLoader\Mods /ad/b') do (
echo "%%a"
del %%a.7z
7z a %%a.7z CSharpLoader\Mods\%%a\*
)