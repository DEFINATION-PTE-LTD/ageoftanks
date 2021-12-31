using UnityEditor;
using System.IO;
using UnityEngine;
public class AutoSetLinkNodeInfos
{

	/// <summary>
	/// 自动检索并设置连接点信息；调用该方法的前提，需给游戏物体添加 Nodes组件，并指定NodeType后调用才有效
	/// </summary>
	[MenuItem("MyTools/AutoSetLinkNodeInfos")]
	static void AutoSet()
	{

		UnityEngine.GameObject [] gameObjects = Selection.gameObjects;
		if (gameObjects.Length <= 0)
		{
			return;
		}

		foreach (GameObject item in gameObjects)
		{
			Nodes itemNode = item.GetComponent<Nodes>();

			if (itemNode != null)
			{
				if (itemNode.NodeType == NodeType.Engine)
				{
					if (itemNode.ChildNodes == null)
					{
						itemNode.ChildNodes = new System.Collections.Generic.List<LinkNodeInfo>();
					}
					else
					{
						itemNode.ChildNodes.Clear();
					}

					//在引擎中寻找mount_hull
					foreach (Transform transform in item.transform.GetComponentsInChildren<Transform>())
					{
						if (transform.name.ToLower() == "mount_hull")
						{					
							LinkNodeInfo info = transform.GetComponent<LinkNodeInfo>();
							if (info == null)
							{
								info = transform.gameObject.AddComponent<LinkNodeInfo>();
							}
							info.LinkType = LinkType.Body;
							info.LinkNode = transform.gameObject;
							itemNode.ChildNodes.Add(info);
						}

					}

				}
				else if (itemNode.NodeType == NodeType.Body)
				{

					if (itemNode.ChildNodes == null)
					{
						itemNode.ChildNodes = new System.Collections.Generic.List<LinkNodeInfo>();
					}
					else
					{
						itemNode.ChildNodes.Clear();
					}
					//在机身中寻找mount_hatch、mount_gun、barrelport

					foreach (Transform transform in item.transform.GetComponentsInChildren<Transform>())
					{
						if (transform.name.ToLower() == "mount_hatch")
						{
							LinkNodeInfo info = transform.GetComponent<LinkNodeInfo>();
							if (info == null)
							{
								info = transform.gameObject.AddComponent<LinkNodeInfo>();
							}
							info.LinkType = LinkType.Head;
							info.LinkNode = transform.gameObject;
							itemNode.ChildNodes.Add(info);
						}
						else if (transform.name.ToLower() == "mount_gun_l")
						{
							LinkNodeInfo info = transform.GetComponent<LinkNodeInfo>();
							if (info == null)
							{
								info = transform.gameObject.AddComponent<LinkNodeInfo>();
							}
							info.LinkType = LinkType.Weapon_L;
							info.LinkNode = transform.gameObject;
							itemNode.ChildNodes.Add(info);
						}
						else if (transform.name.ToLower() == "mount_gun_r")
						{
							LinkNodeInfo info = transform.GetComponent<LinkNodeInfo>();
							if (info == null)
							{
								info = transform.gameObject.AddComponent<LinkNodeInfo>();
							}
							info.LinkType = LinkType.Weapon_R;
							info.LinkNode = transform.gameObject;
							itemNode.ChildNodes.Add(info);
						}
						else if (transform.name.ToLower().IndexOf("barrelport")==0)
						{
							LinkNodeInfo info = transform.GetComponent<LinkNodeInfo>();
							if (info == null)
							{
								info = transform.gameObject.AddComponent<LinkNodeInfo>();
							}
							info.LinkType = LinkType.Barrel;
							info.LinkNode = transform.gameObject;
							itemNode.ChildNodes.Add(info);
						}
					}



				}
				else if (itemNode.NodeType == NodeType.Head)
				{
					//头部没有其他连接点，不做处理
				}
				else if (itemNode.NodeType == NodeType.Weapon)
				{
					if (itemNode.ChildNodes == null)
					{
						itemNode.ChildNodes = new System.Collections.Generic.List<LinkNodeInfo>();
					}
					else
					{
						itemNode.ChildNodes.Clear();
					}
					//在武器中寻找barrelport
					foreach (Transform transform in item.transform.GetComponentsInChildren<Transform>())
					{
						if (transform.name.ToLower().IndexOf("barrelport") == 0)
						{
							LinkNodeInfo info = transform.GetComponent<LinkNodeInfo>();
							if (info == null)
							{
								info = transform.gameObject.AddComponent<LinkNodeInfo>();
							}
							info.LinkType = LinkType.Barrel;
							info.LinkNode = transform.gameObject;
							itemNode.ChildNodes.Add(info);
						}
					}
				}

			}
		}
	}
}
