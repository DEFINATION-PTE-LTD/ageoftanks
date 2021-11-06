using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BaseBuff
{

   // ID、图标、名称、释放玩家、目标玩家、释放坦克、目标坦克、释放时间、当前状态（生效、失效）、生效时间、


    /// <summary>
    /// buff编号
    /// </summary>
    public string ID { get; set; }
    /// <summary>
    /// buff图标
    /// </summary>
    public string Icon { get; set; }
    /// <summary>
    /// buff名称
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// 释放者玩家编号
    /// </summary>
    public string EffectPlayer { get; set; }
    /// <summary>
    /// 目标玩家编号
    /// </summary>
    public string TargetPlayer { get; set; }
    /// <summary>
    /// 释放者坦克编号
    /// </summary>
    public string EffectTank { get; set; }
    /// <summary>
    /// 目标坦克编号
    /// </summary>
    public string TargetTank { get; set; }
    /// <summary>
    /// 释放时间
    /// </summary>
    public DateTime EffectTime { get; set; }
    /// <summary>
    /// 生效状态
    /// </summary>
    public bool Enable { get; set; }

}

