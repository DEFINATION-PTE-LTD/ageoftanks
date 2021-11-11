using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 连接节点
/// </summary>
public class Nodes:MonoBehaviour
{
    public NodeType NodeType;
    public List<LinkNodeInfo> ChildNodes;
}
/// <summary>
/// 节点类型
/// </summary>
public enum NodeType
{
    /// <summary>
    /// 机头
    /// </summary>
    Head,
    /// <summary>
    /// 机身
    /// </summary>
    Body,
    /// <summary>
    /// 武器
    /// </summary>
    Weapon,
    /// <summary>
    /// 引擎
    /// </summary>
    Engine,
    /// <summary>
    /// 背包
    /// </summary>
    Backpack

}
