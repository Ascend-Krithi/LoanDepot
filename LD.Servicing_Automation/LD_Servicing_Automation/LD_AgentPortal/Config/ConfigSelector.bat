@echo off

set scriptpath=%~dp0
set filename=APConfig.xml

setlocal EnableDelayedExpansion
echo AGENT PORTAL
echo:
echo Select the execution environment:
echo:
echo Enter 1 for QA
echo Enter 2 for SG
echo: 
set /p input=Type the input: 

set envValue=0
if %input%==1 set envValue=QA
if %input%==2 set envValue=SG
if %input%==3 set envValue=PROD

set configpath=%scriptpath%%filename%
set tempfilename=second.xml
set temppath=%scriptpath%%tempfilename%

(for /F "delims=" %%a in (%configpath%) do (
   set "line=%%a"
   set "newLine=!line:Environment>=!"
   if "!newLine!" neq "!line!" (
      set "newLine=<Environment>%envValue%</Environment>"
   )   
   echo !newLine!
)) > %temppath%

echo:
echo Select the execution browser:
echo:
echo Enter 1 for Chrome
echo Enter 2 for Edge
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