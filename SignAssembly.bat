@ECHO OFF
SETLOCAL EnableDelayedExpansion

set TargetPath="D:\Projects\ImageViewer\ImageView\bin\Release\net6.0-windows7.0\ImageViewer.exe"
set TargetPath2="D:\Projects\ImageViewer\ImageView\bin\Release\net6.0-windows7.0\ImageViewer.dll"
rem set TargetPath3="D:\Projects\ImageViewer\ImageView\bin\Release\net6.0-windows7.0\GeneralToolkitLib.dll"
set CertPath="D:\Projects\cert\signingCert.pfx"

echo Begining signing of: %TargetPath:"=%
SignTool sign /fd SHA256 /a /f %CertPath% /td SHA256 /tr http://timestamp.digicert.com %TargetPath%
SignTool sign /fd SHA256 /a /f %CertPath% /td SHA256 /tr http://timestamp.digicert.com %TargetPath2%
rem SignTool sign /fd SHA256 /a /f %CertPath% /td SHA256 /tr http://timestamp.digicert.com %TargetPath3%

ECHO ON