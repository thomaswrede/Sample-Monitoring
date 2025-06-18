@echo off
REM Starte Sample.Monitoring.API und warte, bis sie bereit ist (Port 5000, max. 30 Sekunden)
start cmd.exe /C "cd src/Monitoring/Sample.Monitoring.API & title Sample.Monitoring.API & dotnet run > ../../../api.log 2>&1"
set "api_timeout=30"
set "api_elapsed=0"
:waitloop_api
timeout /t 1 >nul
set /a api_elapsed+=1
REM Prüfe, ob Port 7127 offen ist
powershell -Command "(Test-NetConnection -ComputerName localhost -Port 7127).TcpTestSucceeded" | findstr /i "True" >nul
if %errorlevel%==0 goto apiready
if %api_elapsed% geq %api_timeout% (
    echo Timeout: Sample.Monitoring.API nicht bereit.
    goto end
)
goto waitloop_api

:apiready
echo Sample.Monitoring.API ist bereit.

REM Starte Sample-Client-App und warte, bis sie bereit ist (Port 7203, max. 30 Sekunden)
start cmd.exe /C "cd src/Sample-Client-App & title Sample-Client-App & dotnet run > ../../client.log 2>&1"
set "timeout=30"
set "elapsed=0"
:waitloop_client
timeout /t 1 >nul
set /a elapsed+=1
REM Prüfe, ob Port 7203 offen ist
powershell -Command "(Test-NetConnection -ComputerName localhost -Port 7203).TcpTestSucceeded" | findstr /i "True" >nul
if %errorlevel%==0 goto clientready
if %elapsed% geq %timeout% (
    echo Timeout: Client-App nicht bereit.
    goto end
)
goto waitloop_client

:clientready
echo Client-App ist bereit.
REM Öffne Browser
start "" "https://localhost:7203"
goto end

:end
