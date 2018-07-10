@echo off
MODE con: COLS=82 LINES=25

set 文件夹=Recording

set 当前时间=%date:~5,2%%date:~8,2%0%time:~1,1%%time:~3,2%%time:~6,2%

@set frame=30
call "%cd%\Assets\ScreenRecorder\ffmpeg\data.bat"


"%cd%\Assets\ScreenRecorder\ffmpeg\ffmpeg.exe" -framerate %frame% -i "%cd%\%文件夹%\%%5d.png" -c:v libx264 -crf 21 -pix_fmt yuv420p "%cd%\RecordVideo\RecordVideo%当前时间%.mp4"

rd  /s /q "%文件夹%"
start  "" "RecordVideo\" 
::start  "" "RecordVideo\RecordVideo%当前时间%.mp4" 
"%cd%\Assets\ScreenRecorder\ffmpeg\ffplay.exe"  -window_title RecordVideo%当前时间%.mp4 -loop 0 "%cd%\RecordVideo\RecordVideo%当前时间%.mp4"
::pause
