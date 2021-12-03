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
    }

    //统一消息格式
    //{action:"",data:"",time:""}

    //接收消息
    public void MessageFromJS(string jsonStr)
    {
        Debug.Log(jsonStr);
        JObject jsonObj =(JObject)JsonConvert.DeserializeObject(jsonStr);
        string action = jsonObj["action"].ToString();
        switch (action)
        {
            case "setplatform":
                if (jsonObj["data"].ToString().ToLower() == "true")
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
            default:
                break;
        }
    }


    //发送消息
    [Obsolete]
    public void SendMessage(string jsonStr)
    {
        Application.ExternalCall("MessageFromUnity", jsonStr);
    }
}

