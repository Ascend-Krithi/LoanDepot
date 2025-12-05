@echo off

set scriptpath=%~dp0
set filename=APConfig.xml

setlocal EnableDelayedExpansion
echo MSP
echo:
echo Select the execution browser:
echo:
echo Enter 1 for Chrome
echo Enter 2 for Edge
echo Enter 3 for Firefox
echo: 
set /p input1=Type the input: 

set browser=0

if %input1%==1 set browser=Chrome
if %input1%==2 set browser=Edge
if %input1%==3 set browser=Firefox

set tempfilename1=second1.xml
set temppath1=%scriptpath%%tempfilename1%

(for /F "delims=" %%a in (%temppath%) do (
   set "line1=%%a"
   set "newLine1=!line1:Browser>=!"
   if "!newLine1!" neq "!line1!" (
      set "newLine1=<Browser>%browser%</Browser>"
   )   
   echo !newLine1!
)) > %temppath1%

del %temppath%

set str=%scriptpath%
set newpath=\bin\Debug\Config\
call set str=%%str:\Config\=%newpath%%%
set destinationpath=%str%%filename%

move /y "%temppath1%" "%destinationpath%"