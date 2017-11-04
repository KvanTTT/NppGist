@setlocal enableextensions
@cd /d "%~dp0"

set programfilesx86=%programfiles(x86)%\Notepad++\plugins

if exist "%programfilesx86%" (
    copy "NppGist\bin\x64\Debug\NppGist.dll" "%programfiles%\Notepad++\plugins\NppGist.dll"
    copy "NppGist\bin\x86\Debug\NppGist.dll" "%programfilesx86%\NppGist.dll"
) else (
    copy "NppGist\bin\x86\Debug\NppGist.dll" "%programfiles%\Notepad++\plugins\NppGist.dll"
)