using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Text;

/// <summary>
/// Http Request SDK 
/// </summary>
public  class HttpTool : MonoBehaviour
{

    private static HttpTool _instacne = null;
    private string baseUrl = "http://game.ageoftanks.io/serverapi/";


    Dictionary<string, string> requestHeader = new Dictionary<string, string>();  //  header
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
        DontDestroyOnLoad(gameObject);
        HttpTool._instacne = gameObject.GetComponent<HttpTool>();

        //http header 的内容
        requestHeader.Add("Content-Type", "application/json");
        //requestHeader.Add("sKey", sKey);

    }

    public  void Get(string methodName, Action<string> callback)
    {
        StartCoroutine(GetRequest(methodName, callback));
    }
    public IEnumerator GetRequest(string methodName, Action<string> callback)
    {
        string url = baseUrl + methodName;
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
}