@echo off
color 2E
MODE con: COLS=74 LINES=18

cls
echo.
echo ��⵽֮ǰ��δ��ɵ���Ƶ��ɾ��δ��ɵ���Ƶ�����Ǽ���ѹ��δ��ɵ���Ƶ��
echo.
echo ���� 1 ɾ��δ��ɵ���Ƶ
echo.
echo ���� 2 ����ѹ��δ��ɵ���Ƶ
echo.
echo      ��ѡ�� 1 �� 2 �󰴻س�
echo.    
set /p p=����ѡ��:��
if %p%==1 goto 1
if %p%==2 goto 2


:1
cls
@set �ļ���=Recording
@rd  /s /q %�ļ���%
@echo ɾ����ϣ����ֶ��ָ�Unity..
@echo 3��󱾴����Զ��ر�..
@ping -n 3 127.0>nul
exit
::pause>nul



:2
cls


@set �ļ���=Recording

@set ��ǰʱ��=%date:~5,2%%date:~8,2%0%time:~1,1%%time:~3,2%%time:~6,2%

@set frame=30

call "%cd%\Assets\ScreenRecorder\ffmpeg\data.bat"

"%cd%\Assets\ScreenRecorder\ffmpeg\ffmpeg.exe" -framerate %frame% -i "%cd%\%�ļ���%\%%5d.png" -c:v libx264 -crf 21 -pix_fmt yuv420p "%cd%\RecordVideo\RecordVideo%��ǰʱ��%.mp4"

::"%cd%\Assets\ScreenRecorder\ffmpeg\ffmpeg.exe" -f image2  -i "%cd%\%�ļ���%\%%5d.png" -s 320x240 -r 8 "%cd%\RecordVideo\RecordVideo%��ǰʱ��%.gif"

@rd  /s /q "%�ļ���%"
@start  "" "RecordVideo\" 
::@start  "" "RecordVideo\RecordVideo%��ǰʱ��%.mp4" 
"%cd%\Assets\ScreenRecorder\ffmpeg\ffplay.exe"  -window_title RecordVideo%��ǰʱ��%.mp4 -loop 0 "%cd%\RecordVideo\RecordVideo%��ǰʱ��%.mp4"
exit





