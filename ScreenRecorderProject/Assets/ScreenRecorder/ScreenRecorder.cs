using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using System;
using UnityEngine.UI;


public static class DebugUtil
{

    public static void DumpRenderTexture(RenderTexture rt, string pngOutPath)
    {
        var oldRT = RenderTexture.active;

        var tex = new Texture2D(rt.width, rt.height);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        File.WriteAllBytes(pngOutPath, tex.EncodeToPNG());
        RenderTexture.active = oldRT;
    }

}

public class ScreenRecorder : MonoBehaviour
{
    private string RecordName = "Recording";
    public int frameRate = 30;
    private string frameRatedata;
    public float StartRecordTime = 0;
    public float StopRecordTime = 0;
    public bool AutoToRecord;
    private int StartRecordC;
    private int StopRecordC;
    private int once1;
    private int once2;
    private int once3;
    private int once4;
    private int CurrentFrame = -1;
    private bool ExportGIF = false;
    public bool Recording = false;
    private int Recordstate = 0;
    private int defaultfps;
    private float size1;
    private float size2;

    public Camera m_camera;
    private Canvas m_canvas;

    private const bool outputPNG = true;

    void Start()
    {
        print("主键盘  9  开始录制   0   停止录制");
        StartRecordC = 0;
        StopRecordC = 0;
        once1 = 0;
        once2 = 1;
        once4 = 0;
        once3 = 0;
        Recording = false;
        Recordstate = 0;
        //检测是否存在
        string pname = "\\00001.png";
        //print ("Recording"+pname);
        if (System.IO.File.Exists("Recording" + pname))
        {

            if (outputPNG)
            {
                print("Recording文件夹存在,先删除再创建");
                string ss = Application.dataPath + "/../" + "Recording";
                print(ss);
                Directory.Delete(Application.dataPath+"/../"+"Recording", true);
                Directory.CreateDirectory(Application.dataPath + "/../" + "Recording");
            }
            else
            {
                Process.Start("Assets\\ScreenRecorder\\ffmpeg\\Inputvideo_choose.bat");
                print("Untiy已经暂停，请手动恢复运行");
                EditorApplication.isPaused = true;
                //print ("文件夹存在,创建");
            }

        }

        defaultfps = Time.captureFramerate;
        frameRatedata = "set frame=" + frameRate;
        fnDeleteFile("Assets\\ScreenRecorder\\ffmpeg", "data.bat");
        fnCreateFile("Assets\\ScreenRecorder\\ffmpeg", "data.bat", frameRatedata);
        size1 = Screen.width;
        size2 = Screen.height;
        if (size1 % 2 != 0)
        {   //能否被整除

            print("Game窗口的分辨率为 " + size1 + "x" + size2 + " 宽度(" + size1 + ")不能被2整除，最后将不能录制生成视频，请重新设置Game窗口分辨率");
            EditorApplication.isPaused = true;
        }
        if (size2 % 2 != 0)
        {
            print("Game窗口的分辨率为 " + size1 + "x" + size2 + " 高度(" + size2 + ")不能被2整除，最后将不能录制生成视频，请重新设置Game窗口分辨率");
            EditorApplication.isPaused = true;
        }

        if (m_camera == null)
        {
            m_camera = Camera.main;
            m_camera.clearFlags = CameraClearFlags.SolidColor;
            m_camera.backgroundColor = new Color(0, 0, 0, 0);
        }

        m_camera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.BGRA32, RenderTextureReadWrite.Default);


        GameObject g_camera = new GameObject("MyCamera");
        Camera temp_camera = g_camera.AddComponent<Camera>();
        temp_camera.clearFlags = CameraClearFlags.SolidColor;
        temp_camera.backgroundColor = new Color(0, 0, 0, 0);


        GameObject g = new GameObject("MyCanvas");
        m_canvas = g.AddComponent<Canvas>();
        m_canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler cs = g.AddComponent<CanvasScaler>();
        cs.scaleFactor = 10.0f;
        cs.dynamicPixelsPerUnit = 10f;

        GraphicRaycaster gr = g.AddComponent<GraphicRaycaster>();
        g.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 3.0f);
        g.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 3.0f);


        GameObject g_image = new GameObject("MyImage");
        g_image.transform.parent = g.transform;
        RawImage rawimage = g_image.AddComponent<RawImage>();

        rawimage.texture = m_camera.targetTexture;

        rawimage.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
        rawimage.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 0);
        rawimage.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
        rawimage.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);


        rawimage.rectTransform.anchorMin = new Vector2(0, 0);
        rawimage.rectTransform.anchorMax = new Vector2(1, 1);



    }
    void Update()
    {

        if (Input.GetKeyDown("9"))
        {
            if (once2 == 1)
            {
                StartRecordC = 1;
                AutoToRecord = false;
                Recording = true;
                Recordstate = 1;
            }
        }


        if (Input.GetKeyDown("0"))
        {
            StopRecordC = 1;
            Recording = false;
            Recordstate = 0;
        }

        //otherstart



        //otherstart

        if (AutoToRecord)
        {

            if (once1 == 0)
            {
                if (Time.fixedTime > StartRecordTime)
                {
                    if (Time.fixedTime < StartRecordTime + 1)
                    {
                        //AutoToRecord = false;			   
                        StartRecordC = 1;
                        once1 = 1;
                        Recording = true;
                        Recordstate = 1;
                    }
                }
            }



            if (once4 == 0)
            {
                if (StopRecordTime != 0)
                {
                    if (Time.fixedTime > StopRecordTime)
                        if (Time.fixedTime < StopRecordTime + 1)
                        {
                            StopRecordC = 1;
                            Recording = false;
                            Recordstate = 0;
                        }
                }
            }
        }



        if (StartRecordC == 1)
        {
            if (once3 == 0)
            {

                if (outputPNG)
                {
                    Directory.Delete(Application.dataPath + "/../" + "Recording", true);
                    Directory.CreateDirectory(Application.dataPath + "/../" + "Recording");
                }
                else
                {
                    System.IO.Directory.CreateDirectory(RecordName);
                }

                System.IO.Directory.CreateDirectory("RecordVideo");
                once3 = 1;
                print("Untiy正在录制...");

            }
            once2 = 0;
            Time.captureFramerate = frameRate;
            if (StopRecordC != 1)
            {
                if (CurrentFrame == -1)
                    CurrentFrame = Time.frameCount;
                string name = string.Format("{0}/{1:D05}.png", RecordName, Time.frameCount - CurrentFrame + 1);
                //Application.CaptureScreenshot (name);

                DebugUtil.DumpRenderTexture(m_camera.targetTexture, name);

            }
            else
            {
                //exit
                once4 = 1;
                CurrentFrame = -1;
                StopRecordC = 0;
                StartRecordC = 0;
                once2 = 1;
                once3 = 0;

                if (outputPNG)
                {

                }
                else
                {
                    if (ExportGIF)
                    {
                        //Process.Start ("Assets\\ScreenRecorder\\ffmpeg\\Inputvideo.bat");  
                    }
                    else
                    {
                        //Process.Start ("Assets\\ScreenRecorder\\ffmpeg\\Inputvideo_nogif.bat");  
                    }
                }


                
                Time.captureFramerate = defaultfps;

                if (outputPNG)
                {
                    print("录制视频压制到: 工程根目录\\"+ RecordName); 
                }
                else
                {
                    print("录制视频压制到: 工程根目录\\Recordvideo");
                }


            }
        }
        
        //state
        if (once2 == 1)
        {
            if (Recordstate == 0)
            {
                if (Recording == true)
                {
                    StartRecordC = 1;
                    AutoToRecord = false;
                    Recordstate = 1;
                }

            }
        }

        if (once2 == 0)
        {
            if (Recordstate == 1)
            {
                if (Recording == false)
                {
                    Recordstate = 0;
                    StopRecordC = 1;
                }
            }
        }


    }

    //文字操作

    void fnCreateFile(string sPath, string sName, string nDate)
    {
        StreamWriter t_sStreamWriter; // 文件流信息
        FileInfo t_fFileInfo = new FileInfo(sPath + "//" + sName);
        if (!t_fFileInfo.Exists)
        {
            t_sStreamWriter = t_fFileInfo.CreateText();  // 如果此文件不存在则创建
        }
        else
        {
            t_sStreamWriter = t_fFileInfo.AppendText(); // 如果此文件存在则打开
        }
        t_sStreamWriter.WriteLine(nDate); // 以行的形式写入信息 
        t_sStreamWriter.Close(); //关闭流
        t_sStreamWriter.Dispose(); // 销毁流
    }
    /*
     * path：读取文件的路径
     * name：读取文件的名称
     */
    ArrayList fnLoadFile(string sPath, string sName)
    {
        StreamReader t_sStreamReader = null; // 使用流的形式读取
                                             //try
                                             //{
        t_sStreamReader = File.OpenText(sPath + "//" + sName);
        //}
        //catch (Exception ex)
        //{
        //    return null;
        //}
        string t_sLine; // 每行的内容
        ArrayList t_aArrayList = new ArrayList(); // 容器
        while ((t_sLine = t_sStreamReader.ReadLine()) != null)
        {
            t_aArrayList.Add(t_sLine); // 将每一行的内容存入数组链表容器中
        }
        t_sStreamReader.Close(); // 关闭流

        t_sStreamReader.Dispose(); // 销毁流

        return t_aArrayList; // 将数组链表容器返回
    }
    /*
     * sPath：删除文件的路径
     * sName：删除文件的名称
     */
    void fnDeleteFile(string sPath, string sName)
    {
        File.Delete(sPath + "//" + sName);
    }
}