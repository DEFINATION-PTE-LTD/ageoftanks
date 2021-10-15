using System.Collections.Generic;
using UnityEngine;

/// <summary>
///武器对象结构
/// </summary>
public class WeaponObjectClass
{
    /// <summary>
    /// 武器对象
    /// </summary>
    public GameObject WeaponObj { get; set; }
    /// <summary>
    /// 武器枪管
    /// </summary>
    public List<GameObject> Barrels { get; set; }
    /// <summary>
    /// 子弹预制体路径
    /// </summary>
    public string BulletPath { get; set; }

    /// <summary>
    /// 子弹预制体
    /// </summary>
    public GameObject BulletPerfab { get; set; }
    /// <summary>
    /// 子弹火焰路径
    /// </summary>
    public string FirePath { get; set; }
    /// <summary>
    /// 子弹火焰预制体
    /// </summary>
    public GameObject FirePerfab { get; set; }
}
