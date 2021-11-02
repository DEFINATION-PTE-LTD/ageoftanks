using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIResult
{

    /// <summary>
    /// 执行状态 true：成功 false：失败
    /// </summary>
    public bool success { get; set; } = true;
    /// <summary>
    /// 结果数据
    /// </summary>
    public object data { get; set; } = "";
    /// <summary>
    /// 错误消息
    /// </summary>
    public string message { get; set; } = "";
    /// <summary>
    /// 错误消息(英文)
    /// </summary>
    public string message_en { get; set; } = "";
    /// <summary>
    /// 错误消息(繁体)
    /// </summary>
    public string message_ft { get; set; } = "";
}