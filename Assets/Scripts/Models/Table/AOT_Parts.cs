using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;

//AOT_Parts
public class AOT_Parts
{

	/// <summary>
	/// KeyId
	/// </summary>		

	public int KeyId { get; set; }
	/// <summary>
	/// UUID
	/// </summary>		

	public string UUID { get; set; }
	/// <summary>
	/// Code
	/// </summary>		

	public string Code { get; set; }
	/// <summary>
	/// 部位
	/// </summary>		

	public string Part { get; set; }
	/// <summary>
	/// 等级
	/// </summary>		

	public int Level { get; set; }
	/// <summary>
	/// 重量
	/// </summary>		

	public decimal Weight { get; set; }
	/// <summary>
	/// 承载力
	/// </summary>		

	public decimal Bearing { get; set; }
	/// <summary>
	/// 攻击
	/// </summary>		

	public decimal Attack { get; set; }
	/// <summary>
	/// 血量
	/// </summary>		

	public decimal Blood { get; set; }
	/// <summary>
	/// 速度
	/// </summary>		

	public decimal Speed { get; set; }
	/// <summary>
	/// 攻击范围
	/// </summary>		

	public decimal Range { get; set; }
	/// <summary>
	/// 暴击
	/// </summary>		

	public decimal Crit { get; set; }
	/// <summary>
	/// 攻击技能
	/// </summary>		

	public string AttackSkillCode { get; set; }
	/// <summary>
	/// 防御技能
	/// </summary>		

	public string DefenseSkillCode { get; set; }
	/// <summary>
	/// 模型编号
	/// </summary>		

	public string ModelCode { get; set; }
	/// <summary>
	/// 皮肤编号
	/// </summary>		

	public string SkinCode { get; set; }
	/// <summary>
	/// 封面
	/// </summary>		

	public string Cover { get; set; }
	/// <summary>
	/// 状态
	/// </summary>		

	public int Status { get; set; }

	/// <summary>
	/// Sortnum
	/// </summary>		

	public int Sortnum { get; set; }
	
	/// <summary>
	/// Deletemark
	/// </summary>		

	public bool Deletemark { get; set; }
	
	/// <summary>
	/// Remark
	/// </summary>		

	public string Remark { get; set; }

}
