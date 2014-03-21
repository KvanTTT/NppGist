@setlocal enableextensions
@cd /d "%~dp0"

copy "NppGist\bin\Debug\NppGist.dll" "%programfiles%\Notepad++\plugins\NppGist.dll"
copy "NppGist\bin\Debug\NppGist.dll" "%programfiles(x86)%\Notepad++\plugins\NppGist.dll"