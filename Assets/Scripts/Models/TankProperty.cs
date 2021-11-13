using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 坦克属性类
/// </summary>
public class TankProperty
{
    /// <summary>
    /// 编号
    /// </summary>
    public string Code { get; set; }
    /// <summary>
    /// 是否为组装坦克
    /// </summary>
    public bool IsSetup { get; set; } = false;
    /// <summary>
    /// 坦克类型
    /// </summary>
    public string TankType { get; set; }
    //速度(10~50)、血量(200~1000)、暴击率(5~20)、防御值(10~20)、攻击力(50~200)、承载力(10000~30000)、重量(1000~3000)
    /// <summary>
    /// 速度
    /// </summary>
    public float Speed { get; set; }
    /// <summary>
    /// 血量
    /// </summary>
    public float Blood { get; set; }
    /// <summary>
    /// 攻击力
    /// </summary>
    public float Attack { get; set; }
    /// <summary>
    /// 暴击率
    /// </summary>
    public float CritRate { get; set; }
    ///// <summary>
    ///// 防御值
    ///// </summary>
    //public float Defense { get; set; }
    /// <summary>
    /// 承载力
    /// </summary>
    public float Bearer { get; set; }
    /// <summary>
    /// 重量
    /// </summary>
    public float Weight { get; set; }
    /// <summary>
    /// 射程
    /// </summary>
    public float Range { get; set; }
    ///// <summary>
    ///// 命中率
    ///// </summary>
    //public float HitRate { get; set; }

    public GameObject TankObject { get; set; }
    /// <summary>
    /// 攻击技能
    /// </summary>
    public AOT_SkillInfo AttackSkill { get; set; }
    /// <summary>
    /// 防御技能
    /// </summary>
    public AOT_SkillInfo DefenseSkill { get; set; }

    public TankProperty() {
    
    }
    //随机生成一个坦克属性
    public TankProperty(string code, string tankType,GameObject gameObject) 
    {
        Code = code;
        TankType = tankType;
        Speed = CommonHelper.GetRandom(10,50);
        Blood = CommonHelper.GetRandom(800, 1000);
        Attack = CommonHelper.GetRandom(80, 130);
        CritRate = CommonHelper.GetRandom(5, 20);
        //Defense = CommonHelper.GetRandom(10, 20);
        Bearer = CommonHelper.GetRandom(10000, 30000);
        Weight = CommonHelper.GetRandom(1000, 3000);
        Range = 50;
        //HitRate = 85;
        TankObject = gameObject;
        AttackSkill = null;
        DefenseSkill = null;
    }


}
