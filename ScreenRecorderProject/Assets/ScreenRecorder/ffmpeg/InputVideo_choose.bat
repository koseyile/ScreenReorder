@echo off
color 2E
MODE con: COLS=74 LINES=18

cls
echo.
echo 检测到之前有未完成的视频。删除未完成的视频，还是继续压制未完成的视频？
echo.
echo 　　 1 删除未完成的视频
echo.
echo 　　 2 继续压制未完成的视频
echo.
echo      请选择 1 或 2 后按回车
echo.    
set /p p=　请选择:　
if %p%==1 goto 1
if %p%==2 goto 2


:1
cls
@set 文件夹=Recording
@rd  /s /q %文件夹%
@echo 删除完毕，请手动恢复Unity..
@echo 3秒后本窗口自动关闭..
@ping -n 3 127.0>nul
exit
::pause>nul



:2
cls


@set 文件夹=Recording

@set 当前时间=%date:~5,2%%date:~8,2%0%time:~1,1%%time:~3,2%%time:~6,2%

@set frame=30

call "%cd%\Assets\ScreenRecorder\ffmpeg\data.bat"

"%cd%\Assets\ScreenRecorder\ffmpeg\ffmpeg.exe" -framerate %frame% -i "%cd%\%文件夹%\%%5d.png" -c:v libx264 -crf 21 -pix_fmt yuv420p "%cd%\RecordVideo\RecordVideo%当前时间%.mp4"

::"%cd%\Assets\ScreenRecorder\ffmpeg\ffmpeg.exe" -f image2  -i "%cd%\%文件夹%\%%5d.png" -s 320x240 -r 8 "%cd%\RecordVideo\RecordVideo%当前时间%.gif"

@rd  /s /q "%文件夹%"
@start  "" "RecordVideo\" 
::@start  "" "RecordVideo\RecordVideo%当前时间%.mp4" 
"%cd%\Assets\ScreenRecorder\ffmpeg\ffplay.exe"  -window_title RecordVideo%当前时间%.mp4 -loop 0 "%cd%\RecordVideo\RecordVideo%当前时间%.mp4"
exit





