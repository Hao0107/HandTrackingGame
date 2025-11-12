@echo off
setlocal enableDelayedExpansion

REM --- Tên file EXE của Unity (Thay đổi nếu cần) ---
set UNITY_EXE=MediapipeHandTracking.exe

REM --- Khởi động ứng dụng Python ở nền ---
echo Starting Python Tracker...
start /B HandTracking\HandTracking.exe

REM --- Đợi 3 giây để kênh UDP khởi động ---
timeout /t 3 /nobreak >nul

REM --- Khởi động ứng dụng Unity ---
echo Starting Unity Game...
start "" "%UNITY_EXE%"

exit