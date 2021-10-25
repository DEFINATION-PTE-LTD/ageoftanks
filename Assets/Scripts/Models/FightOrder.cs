using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对战顺序
/// </summary>
public class FightOrder 
{
    //所属玩家
    public Player Player { get; set; }
    
    //坦克对象
    public GameObject Tank { get; set; }
    /// <summary>
    /// 编号
    /// </summary>
    public string Code { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// 死亡状态
    /// </summary>
    public bool Death { get; set; }
    /// <summary>
    /// 速度
    /// </summary>
    public float Speed { get; set; }
    /// <summary>
    /// 暴击率
    /// </summary>
    public float CritRate { get; set; }
    /// <summary>
    /// 命中率
    /// </summary>
    //public float HitRate { get; set; }
    /// <summary>
    /// 射程
    /// </summary>
    public float Range { get; set; }
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
   // public float Defense { get; set; }
    /// <summary>
    /// 攻击技能
    /// </summary>
    public SkillInfo AttackSkill { get; set; }
    /// <summary>
    /// 防御技能
    /// </summary>
    public SkillInfo DefenseSkill { get; set; }
    /// <summary>
    /// 正负增益效果
    /// </summary>
    public List<Buff> Buffs { get; set; }
}

