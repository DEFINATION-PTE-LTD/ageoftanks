using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家属性类
/// </summary>
public class Player 
{

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 头像
    /// </summary>
    public string Photo { get; set; }
    /// <summary>
    /// 坦克属性列表
    /// </summary>
    public List<TankProperty> TankList{ get; set; }
    /// <summary>
    /// 英雄属性
    /// </summary>
    public HeroProperty Hero { get; set; }

}
