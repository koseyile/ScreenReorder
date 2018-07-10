@echo off
MODE con: COLS=82 LINES=25
@set 文件夹=Recording

@set 当前时间=%date:~5,2%%date:~8,2%0%time:~1,1%%time:~3,2%%time:~6,2%

@set frame=30
call "data.bat"



"%cd%\ffmpeg.exe" -framerate %frame% -i  "%cd%\..\..\..\%文件夹%\%%5d.png" -c:v libx264 -crf 21 -pix_fmt yuv420p "%cd%\..\..\..\RecordVideo\RecordVideo%当前时间%.mp4"

::"%cd%\ffmpeg.exe" -i "%cd%\..\..\..\RecordVideo\RecordVideo%当前时间%.mp4" -t 10 -s 320x240 -f gif -r 8 "%cd%\..\..\..\RecordVideo\RecordVideo%当前时间%.gif"

::"%cd%\ffmpeg.exe" -f image2  -i "%cd%\..\..\..\%文件夹%\%%5d.png" -s 320x240 -t 8 "%cd%\..\..\..\RecordVideo\RecordVideo%当前时间%.gif"

::@rd  /s /q "..\..\..\%文件夹%"
@start  "" "..\..\..\RecordVideo\" 
::@start  "" "..\..\..\RecordVideo\RecordVideo%当前时间%.mp4" 
"%cd%\ffplay.exe"  -window_title RecordVideo%当前时间%.mp4 -loop 0 "%cd%\..\..\..\RecordVideo\RecordVideo%当前时间%.mp4"
@pause
