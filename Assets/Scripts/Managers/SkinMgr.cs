using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 皮肤系统模块
/// </summary>
public class SkinMgr : MonoBehaviour
{
    static public SkinMgr _instacne = null;


    public static SkinMgr Instance
    {
        get
        {
            if (_instacne == null)
            {
                Debug.LogError("SkinManager Awake error");
            }
            return _instacne;
        }
    }

    private void Awake()
    {
        if (_instacne == null)
        {
            DontDestroyOnLoad(gameObject);
            SkinMgr._instacne = gameObject.GetComponent<SkinMgr>();
        }
    }

    //皮肤加载

    //皮肤更换

    /// <summary>
    /// 材质球替换（换肤功能）
    /// </summary>
    /// <param name="gameObject"></param>
    public static void ReplaceMaterial(GameObject gameObject, string skinname)
    {
        Material mat;// = mats[GetRandom(0, 6)];
        switch (skinname)
        {
            case "red":
                mat = Resources.Load<Material>("Materials/A_Spiders_Mat(red)");
                break;
            case "blue":
                mat = Resources.Load<Material>("Materials/A_Spiders_Mat(blue)");
                break;
            case "black":
                mat = Resources.Load<Material>("Materials/A_Spiders_Mat(black)");
                break;
            case "yellow":
                mat = Resources.Load<Material>("Materials/A_Spiders_Mat(yellow)");
                break;
            case "green":
                mat = Resources.Load<Material>("Materials/A_Spiders_Mat(green)");
                break;
            default:
                mat = Resources.Load<Material>("Materials/A_Spiders_Mat(rusty)");
                break;
        }


        //不替换的
        List<string> noReplace = new List<string>() {
            "Tracks_R_Geom","Tracks_L_Geom",
            "Tracks_FL_Geom","Tracks_RL_Geom","Tracks_FR_Geom","Tracks_RR_Geom",
            "FX_Laser_Ray_Geom",
            "RL_Track_Geom","RR_Track_Geom",
            "FL_Track_Geom","FR_Track_Geom",
            "FL_Wheel_Geom","FR_Wheel_Geom","RL_Wheel_Geom","RR_Wheel_Geom"
        };
        foreach (MeshRenderer item in gameObject.transform.GetComponentsInChildren<MeshRenderer>())
        {

            if (noReplace.Contains(item.gameObject.name) == false)
            {
                // rend.Add(item);
                item.material = mat;
            }
        }


        List<SkinnedMeshRenderer> skinrend = new List<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer item in gameObject.transform.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            if (noReplace.Contains(item.gameObject.name) == false)
            {
                // skinrend.Add(item);
                item.material = mat;
            }
        }

    }

    /// <summary>
    /// 材质球替换（换肤功能）
    /// </summary>
    /// <param name="gameObject"></param>
    public static void ReplaceMaterialByPath(GameObject gameObject, string path)
    {
        Material mat = Resources.Load<Material>(path);

        //不替换的
        List<string> noReplace = new List<string>() {
            "Tracks_R_Geom","Tracks_L_Geom",
            "Tracks_FL_Geom","Tracks_RL_Geom","Tracks_FR_Geom","Tracks_RR_Geom",
            "FX_Laser_Ray_Geom",
            "RL_Track_Geom","RR_Track_Geom",
            "FL_Track_Geom","FR_Track_Geom",
            "FL_Wheel_Geom","FR_Wheel_Geom","RL_Wheel_Geom","RR_Wheel_Geom"
        };
        foreach (MeshRenderer item in gameObject.transform.GetComponentsInChildren<MeshRenderer>())
        {

            if (noReplace.Contains(item.gameObject.name) == false)
            {
                // rend.Add(item);
                item.material = mat;
            }
        }


        List<SkinnedMeshRenderer> skinrend = new List<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer item in gameObject.transform.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            if (noReplace.Contains(item.gameObject.name) == false)
            {
                // skinrend.Add(item);
                item.material = mat;
            }
        }

    }


}
