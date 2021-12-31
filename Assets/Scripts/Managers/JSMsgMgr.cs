using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// js通信管理器
/// </summary>
public class JSMsgMgr: MonoBehaviour
{

    static public JSMsgMgr _instacne = null;

    public static JSMsgMgr Instance
    {
        get
        {
            if (_instacne == null)
            {
                Debug.LogError("JSMsgMgr Awake error");
            }
            return _instacne;
        }
    }

    private void Awake()
    {
        if (_instacne == null)
        {
            DontDestroyOnLoad(gameObject);
            JSMsgMgr._instacne = gameObject.GetComponent<JSMsgMgr>();
        }
        SendMessage("{\"action\":\"awake\",\"data\":\"success\"}");

        //注册通知--获取地址(点击进入试玩时)
        EventCenter.AddListener(eEventType.RequestWalletAddress, GetAddressFromJS);
       
    }


    //向JS发送获取地址的通知
    public void GetAddressFromJS()
    {
        SendMessage("{\"action\":\"getAddress\",\"data\":\"\"}");
    }

    private void Start()
    {
       
    }

    /// <summary>
    /// 与js通信统一接收方法
    /// </summary>
    /// <param name="jsonStr">消息体：{"action":"","data":""}</param>
    public void MessageFromJS(string jsonStr)
    {
        Debug.Log(jsonStr);
        JObject jsonObj =(JObject)JsonConvert.DeserializeObject(jsonStr);
        string action = jsonObj["action"].ToString();
        string body = jsonObj["data"].ToString();
        switch (action)
        {
            case "setplatform":
                if (body.ToLower() == "true")
                {
                    ResourceCtrl.Instance.IsPC = true;
                    ResourceCtrl.Instance.QualityVal = 4;
                }
                else
                {
                    ResourceCtrl.Instance.IsPC = false;
                    ResourceCtrl.Instance.QualityVal =1;
                }
                break;
                //接收到js发送的地址
            case "setAddress":
                if (string.IsNullOrEmpty(body) ==false)
                {
                    //广播地址给登录页
                    EventCenter.Broadcast<string>(eEventType.ReciveWalletAddress, jsonObj["data"].ToString());
                }
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// 与js通信统一发送方法
    /// </summary>
    /// <param name="jsonStr">消息体：{"action":"","data":""}</param>
    [Obsolete]
    public void SendMessage(string jsonStr)
    {
        Application.ExternalCall("MessageFromUnity", jsonStr);
    }
}

