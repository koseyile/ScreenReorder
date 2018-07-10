制作说明
版本ScreenRecorder1.0
制作日期:2017.6.30

支持unity4,unity5

使用方法：

脚本随意挂在哪里都可以,或者 GameObject - Creat ScreenRecorder 创建.



注意： 警告:Game窗口的分辨率不能为奇数，否则不能生成视频!!!! (我自己也经常犯这个问题,以后可能会改进这个问题)
       如果要改分辨率，可以在game窗口去改数值，
       Unity5.5:如果Game窗口设置了分辨率，那么生成的视频就是这个分辨率。窗口实际显示大小就不影响录制大小
       Unity4:要多大分辨率，就放大Game窗口到多大。如果设置了分辨率，至少得把Game窗口放大到留出有灰色部分为止
       如果要删除本脚本,直接删除 Assets\ScreenRecorder 即可
       录制完成播放视频的播放器没有色差。是最终效果


录制方法三种
1.快捷键 9 启动录制 0 结束录制
2.运行时点击面板上Recording开启/结束录制
3.设定起始时间和结束时间后，打开自动录制功能就会自动录制
  自动录制（AutoToRecord）的时候,如果结束时间(StopRecordTime)等于0，
  表示没有结束时间，需要手动停止录制。

支持生成gif动图  320x240   8fps 
如果需要改变gif图的分辨率，可以去 "Assets\ScreenRecorder\ffmpeg\InputVideo.bat" 里改变它的分辨率

如果录制过程中意外关闭场景，下一次开启场景时，会继续上次未完成的视频操作，
您可以选择删除上一次的未完成的残留视频，或者继续压制上一次的视频。此期间，
Unity会自动暂停，选择完成后，请手动恢复启动您的Unity

翻译：
Frame Rate         录制的帧速率
Start Record Time  开始录制时间（需要开启Auto To Record生效）  
Stop Record Time   结束录制时间（需要开启Auto To Record生效）  
Auto To Record     自动录制
Export Gif         同时输出gif动图 （320x240 8fps）
Recording          录制状态 （录制时，这个会自动勾上，也可以手动点这个控制录制/停止录制）
