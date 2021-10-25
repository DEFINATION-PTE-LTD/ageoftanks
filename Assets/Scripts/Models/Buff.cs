using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Buff
{
    /// <summary>
    /// 坦克编号
    /// </summary>
    public string TankCode { get; set; }
    /// <summary>
    /// 坦克游戏物体
    /// </summary>
    public GameObject TankObject { get; set; }
    /// <summary>
    /// 攻击方坦克编号
    /// </summary>
    public string FromTankCode { get; set; }
    /// <summary>
    /// 攻击方坦克游戏物体
    /// </summary>
    public GameObject FromTankObject { get; set; }
    /// <summary>
    /// Buff游戏物体
    /// </summary>
    public GameObject BuffObject { get; set; }
    /// <summary>
    /// BUFF名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// BUFF类型 buff、debuff
    /// </summary>
    public string BuffType { get; set; }
    /// <summary>
    /// 参数值
    /// </summary>
    public float Value { get; set; }

    /// <summary>
    /// 已生效次数
    /// </summary>
    public int EffectCount { get; set; }
    /// <summary>
    /// 是否失效
    /// </summary>
    public bool Disable { get; set; }
}


