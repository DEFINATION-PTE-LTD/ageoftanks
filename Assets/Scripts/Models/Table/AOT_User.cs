using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class AOT_User
{
	/// <summary>
	/// 自增长主键
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
	/// UserName
	/// </summary>		

	public string UserName { get; set; }
	/// <summary>
	/// Photo
	/// </summary>		

	public string Photo { get; set; }
	/// <summary>
	/// Account
	/// </summary>		

	public string Account { get; set; }
	/// <summary>
	/// Password
	/// </summary>		

	public string Password { get; set; }
	/// <summary>
	/// 注册时填写的邀请码
	/// </summary>		

	public string RegCode { get; set; }
	/// <summary>
	/// 用户的邀请码
	/// </summary>		

	public string InviteCode { get; set; }
	/// <summary>
	/// 0:用户、1：1星、2：2星、3：3星、4：4星、5：5星、6：合伙人
	/// </summary>		

	public int Level { get; set; }
	/// <summary>
	/// Degree
	/// </summary>		

	public int Degree { get; set; }
	/// <summary>
	/// InviteNum
	/// </summary>		

	public int InviteNum { get; set; }
	/// <summary>
	/// TeamNum
	/// </summary>		

	public int TeamNum { get; set; }
	/// <summary>
	/// 每日0点刷新
	/// </summary>		

	public int TodayCount { get; set; }
	/// <summary>
	/// 删除标记 0：未删除 1：删除
	/// </summary>		

	public bool Deletemark { get; set; }
	/// <summary>
	/// 创建时间
	/// </summary>		

	public DateTime? Createtime { get; set; }
	/// <summary>
	/// 排序
	/// </summary>		

	public int Sortnum { get; set; }
	/// <summary>
	/// 创建人KeyId
	/// </summary>		

	public string Creator { get; set; }
	/// <summary>
	/// Remark
	/// </summary>		

	public string Remark { get; set; }
}

