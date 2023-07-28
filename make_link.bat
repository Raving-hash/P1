@REM 这里改成你的目标目录
set "TARGET_DIR=C:\Users\ATRI\P2"
set "CURRENT_DIR=C:\Users\ATRI\P1"

del %TARGET_DIR%
mkdir %TARGET_DIR%
mklink /D "%TARGET_DIR%\Assets" "%CURRENT_DIR%\Assets"
mklink /D "%TARGET_DIR%\ProjectSettings" "%CURRENT_DIR%\ProjectSettings"
mklink /D "%TARGET_DIR%\Packages" "%CURRENT_DIR%\Packages"

pause