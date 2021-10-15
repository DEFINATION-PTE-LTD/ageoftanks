using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 英雄属性类
/// </summary>
public class HeroProperty 
{

    /// <summary>
    /// 英雄编号
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 英雄名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 速度
    /// </summary>
    public float Speed { get; set; }
    /// <summary>
    /// 暴击率
    /// </summary>
    public float CritRate { get; set; }
    /// <summary>
    /// 攻击力
    /// </summary>
    public float Attack { get; set; }
    /// <summary>
    /// 血量
    /// </summary>
    public float Blood { get; set; }
    /// <summary>
    /// 防御
    /// </summary>
    public float Defense { get; set; }
}
