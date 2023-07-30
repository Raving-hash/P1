@REM ����ĳ����Ŀ��Ŀ¼
set "TARGET_DIR=C:\Users\Administrator\Documents\P2"
set "CURRENT_DIR=C:\Users\Administrator\Documents\P1"

del %TARGET_DIR%
mkdir %TARGET_DIR%
mklink /D "%TARGET_DIR%\Assets" "%CURRENT_DIR%\Assets"
mklink /D "%TARGET_DIR%\ProjectSettings" "%CURRENT_DIR%\ProjectSettings"
mklink /D "%TARGET_DIR%\Packages" "%CURRENT_DIR%\Packages"

pause