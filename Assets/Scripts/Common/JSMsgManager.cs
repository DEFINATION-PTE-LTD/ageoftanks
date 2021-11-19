using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// js通信管理器
/// </summary>
public class JSMsgManager:MonoBehaviour
{

    static public JSMsgManager _instacne = null;

    public static JSMsgManager Instance
    {
        get
        {
            if (_instacne == null)
            {
                Debug.LogError("JSMsgManager Awake error");
            }
            return _instacne;
        }
    }

    private void Awake()
    {
        if (_instacne == null)
        {
            DontDestroyOnLoad(gameObject);
            JSMsgManager._instacne = gameObject.GetComponent<JSMsgManager>();
        }
        SendMessage("{\"action\":\"awake\",\"data\":\"success\"}");
    }

    //统一消息格式
    //{action:"",data:"",time:""}

    //接收消息
    public void MessageFromJS(string jsonStr)
    {
        Debug.Log(jsonStr);
        string action = "";
        switch (action)
        {
            case "":
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

