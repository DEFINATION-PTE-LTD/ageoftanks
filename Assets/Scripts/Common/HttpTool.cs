using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Text;
using System.IO;

/// <summary>
/// Http Request SDK 
/// </summary>
public class HttpTool : MonoBehaviour
{

    private static HttpTool _instacne = null;
    private string baseUrl = "http://game.ageoftanks.io/serverapi/";
    //private string baseUrl = "http://localhost:16555/";


    public static HttpTool Instance
    {
        get
        {
            if (_instacne == null)
            {
                
                Debug.LogError("Awake error");
            }
            return _instacne;
        }
    }

    void Awake()
    {
        if (_instacne == null)
        {
            DontDestroyOnLoad(gameObject);
            HttpTool._instacne = gameObject.GetComponent<HttpTool>();
        }
        //requestHeader.Add("sKey", sKey);
    }

    public  void Get(string methodName, Action<string> callback)
    {
        StartCoroutine(GetRequest(methodName, callback));
    }
    public IEnumerator GetRequest(string methodName, Action<string> callback)
    {
        string url = baseUrl + methodName;
        Dictionary<string, string> requestHeader = new Dictionary<string, string>();  //  header
        //http header 的内容
        requestHeader.Add("Content-Type", "application/json");
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            //设置header
            foreach (var v in requestHeader)
            {
                webRequest.SetRequestHeader(v.Key, v.Value);
            }
            yield return webRequest.SendWebRequest();

            if (webRequest.isHttpError || webRequest.isNetworkError)
            {
                Debug.LogError(webRequest.error + "\n" + webRequest.downloadHandler.text);
                if (callback != null)
                {
                    callback(null);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(webRequest.downloadHandler.text);
                }
            }
        }
    }

    //jsonString 为json字符串，post提交的数据包为json
    public void Post(string methodName, string jsonString, Action<string> callback)
    {
        StartCoroutine(PostRequest(methodName, jsonString, callback));
    }
    public IEnumerator PostRequest(string methodName, string jsonString, Action<string> callback)
    {
        string url = baseUrl + methodName;
        // Debug.Log(string.Format("url:{0} postData:{1}",url,jsonString));
        Dictionary<string, string> requestHeader = new Dictionary<string, string>();  //  header
        //http header 的内容
        requestHeader.Add("Content-Type", "application/json");
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            foreach (var v in requestHeader)
            {
                webRequest.SetRequestHeader(v.Key, v.Value);
            }
            yield return webRequest.SendWebRequest();

            if (webRequest.isHttpError || webRequest.isNetworkError)
            {
                Debug.LogError(webRequest.error + "\n" + webRequest.downloadHandler.text);
                if (callback != null)
                {
                    callback(null);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(webRequest.downloadHandler.text);
                }
            }
        }
    }


    /// <summary>
    /// 加载服务器图片
    /// </summary>
    /// <param name="url"></param>
    /// <param name="width"></param>
    /// <param name="high"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public IEnumerator LoadRemoteImg(string url, int width, int high, Action<Sprite> callback)
    {

        UnityWebRequest unityWebRequest = new UnityWebRequest(baseUrl+url);
        DownloadHandlerTexture texD1 = new DownloadHandlerTexture(true);
        unityWebRequest.downloadHandler = texD1;
        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.isHttpError || unityWebRequest.isNetworkError)
        {
            Debug.LogError(unityWebRequest.error + "\n" + unityWebRequest.downloadHandler.text);
            if (callback != null)
            {
                callback(null);
            }
        }
        else
        {
            Texture2D texture2D = new Texture2D(width, high);
            texture2D = texD1.texture;
       
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height),
                new Vector2(0.5f, 0.5f));
            if (callback != null)
            {
                callback(sprite);
            }
        }


    }





    /// <summary>
    /// 加载图片并且永久保存到本地
    /// </summary>
    /// <param name="url">要加载的图片链接</param>
    /// <param name="filename">保存的文件名</param>
    /// <param name="width">宽度</param>
    /// <param name="high">高度</param>
    /// <param name="callback">回调</param>
    /// <returns></returns>
    /// float width = Root.transform.Find("MainPanel/LeftPanel/RawImage").GetComponent<RectTransform>().rect.size.x;
    ///float height = Root.transform.Find("MainPanel/LeftPanel/RawImage").GetComponent<RectTransform>().rect.size.y;
    ///StartCoroutine(HttpTool.Instance.LoadSprite("http://game.ageoftanks.io/serverapi/logo.png", "logo", (int) width, (int) height, (Sprite sp) => {
    ///    if (sp != null)
    ///    {
    ///Root.transform.Find("MainPanel/LeftPanel/RawImage").GetComponent<RawImage>().texture = sp.texture;
    ///}
    ///}));
    public IEnumerator LoadSprite(string url,string filename, int width,int high, Action<Sprite> callback)
    {
        if (File.Exists(Application.dataPath + "/download/" + filename + ".png"))
        {
            Sprite sprite = LoadByIo(Application.dataPath + "/download/" + filename + ".png", width, high);
            callback(sprite);
        }
        else
        {

            UnityWebRequest unityWebRequest = new UnityWebRequest(url);
            DownloadHandlerTexture texD1 = new DownloadHandlerTexture(true);
            unityWebRequest.downloadHandler = texD1;
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.isHttpError || unityWebRequest.isNetworkError)
            {
                Debug.LogError(unityWebRequest.error + "\n" + unityWebRequest.downloadHandler.text);
                if (callback != null)
                {
                    callback(null);
                }
            }
            else
            {
                Texture2D texture2D = new Texture2D(width, high);
                texture2D = texD1.texture;
                SaveLocally(texture2D, filename, ".png");
                Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height),
                    new Vector2(0.5f, 0.5f));
                if (callback != null)
                {
                    callback(sprite);
                }
            }
        }

    }


    /// <summary>
    /// 下载的图片永久保存在本地
    /// </summary>
    /// <param name="texture2D"></param>
    /// <param name="texName">自定义图片名称</param>
    /// <param name="spriteType">要保存的图片格式 （。png/。jpg）等</param>
    public void SaveLocally(Texture2D texture2D, string texName, string spriteType)
    {
        if (Directory.Exists(Application.dataPath + "/download") == false) 
        {
            Directory.CreateDirectory(Application.dataPath + "/download");
        }

        Byte[] bytes = texture2D.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/download/" + texName + spriteType, bytes);
    }

    /// <summary>
    /// 以IO方式进行加载图片
    /// </summary>
    /// <param name="url">本地路径</param>
    /// <param name="width">宽度</param>
    /// <param name="height">高度</param>
    /// <returns></returns>
    public Sprite LoadByIo(string url,int width,int height)
    {
        double startTime = (double)Time.time;
        //创建文件读取流
        FileStream fileStream = new FileStream(url, FileMode.Open, FileAccess.Read);
        //创建文件长度缓冲区
        byte[] bytes = new byte[fileStream.Length];
        //读取文件
        fileStream.Read(bytes, 0, (int)fileStream.Length);

        //释放文件读取流
        fileStream.Close();
        //释放本机屏幕资源
        fileStream.Dispose();
        fileStream = null;

        //创建Texture

        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(bytes);

        //创建Sprite
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        startTime = (double)Time.time - startTime;
        Debug.Log("IO加载" + startTime);
        return sprite;
    }



    // 上传视频
    IEnumerator UploadVideo()
    {
        byte[] gifByte = File.ReadAllBytes("E:/Work/ffepgtest/gif/a.gif");
        WWWForm form = new WWWForm();
        //根据自己长传的文件修改格式
        form.AddBinaryData("file", gifByte, "myGif.mp4", "a/gif");

        using (UnityWebRequest www = UnityWebRequest.Post(baseUrl + "/upload/", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string text = www.downloadHandler.text;
                Debug.Log("服务器返回值" + text);//正确打印服务器返回值
            }
        }
    }

}