using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 坦克各部位节点信息
/// </summary>
public class LinkNodeInfo : MonoBehaviour
{
    /// <summary>
    /// 连接节点类型
    /// </summary>
    public LinkType LinkType;
    /// <summary>
    /// 连接节点
    /// </summary>
    public GameObject LinkNode;
}

/// <summary>
/// 连接节点类型
/// </summary>
public enum LinkType
{
    /// <summary>
    /// 机头连接点
    /// </summary>
    Head,
    /// <summary>
    /// 机身连接点
    /// </summary>
    Body,
    /// <summary>
    /// 武器主连接节点
    /// </summary>
    Weapon_Main,
    /// <summary>
    /// 武器左连接节点
    /// </summary>
    Weapon_L,
    /// <summary>
    /// 武器右连接节点
    /// </summary>
    Weapon_R,
    /// <summary>
    /// 武器顶部连接节点
    /// </summary>
    Weapon_Top,
    /// <summary>
    /// 背包连接节点
    /// </summary>
    Backpack,
    /// <summary>
    /// 子弹口连接节点
    /// </summary>
    Barrel

}