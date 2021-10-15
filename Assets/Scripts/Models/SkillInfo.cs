using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 技能基本信息
/// </summary>
public class SkillInfo
{
    /// <summary>
    /// 技能名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 类型 攻击、防御
    /// </summary>
    public string Type { get; set; }
    /// <summary>
    /// 图标 Resource/Texture/skill
    /// </summary>
    public string Icon { get; set; }
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// 技能介绍
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// 标题（英文）
    /// </summary>
    public string Title_en { get; set; }
    /// <summary>
    /// 技能介绍（英文）
    /// </summary>
    public string Description_en { get; set; }
}

