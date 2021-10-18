using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class CommonHelper
{
    /// <summary>
    /// 获取随机数
    /// </summary>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <returns></returns>
    public static int GetRandom(int min, int max)
    {
        return new System.Random(Guid.NewGuid().GetHashCode()).Next(min, max);
    }

    /// <summary>
    /// 延迟执行
    /// </summary>
    /// <param name="action"></param>
    /// <param name="delaySeconds"></param>
    /// <returns></returns>
    public static IEnumerator DelayToInvokeDo(Action action, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        action();
    }

    /// <summary>
    /// 倒计时显示器
    /// </summary>
    /// <param name="time">时间</param>
    /// <param name="onCountdown">每秒倒计时回调，返回当前秒数</param>
    /// <param name="onComplete">倒计时结束后回调</param>
    public static void CountDown(float time, Action<float> onCountdown,  Action onComplete)
    {
        //每一个数字显示1秒时长，动画结束后递归调用并将时间减1秒，从而达到倒计时效果
        vp_Timer.In(1f, () =>
        {
            time -= 1;
            onCountdown(time);
            if (time > 0)
            {
                CountDown(time, onCountdown, onComplete);
            }
            else
            {
                onComplete();
            }
        });
    }

    /// <summary>
    /// 加载坦克图
    /// </summary>
    /// <param name="code">坦克编号</param>
    /// <returns></returns>
    public static Texture LoadTankImage(string code)
    {
        Texture texture = Resources.Load<Texture>("Texture/tank/" + code);
        return texture;
    }
    /// <summary>
    /// 加载技能图
    /// </summary>
    /// <param name="skill">技能名称</param>
    /// <returns></returns>
    public static Texture LoadSkillImage(string skill)
    {
        Texture texture = Resources.Load<Texture>("Texture/skill/" + skill);
        return texture;
    }


    /// <summary>
    /// 材质球替换（换肤功能）
    /// </summary>
    /// <param name="gameObject"></param>
    public static void ReplaceMaterial(GameObject gameObject)
    {
        //把材质球放到Resources文件夹下
        List<Material> mats = new List<Material>();
        mats.Add(Resources.Load<Material>("Materials/A_Spiders_Mat(red)"));
        mats.Add(Resources.Load<Material>("Materials/A_Spiders_Mat(blue)"));
        mats.Add(Resources.Load<Material>("Materials/A_Spiders_Mat(black)"));
        mats.Add(Resources.Load<Material>("Materials/A_Spiders_Mat(yellow)"));
        mats.Add(Resources.Load<Material>("Materials/A_Spiders_Mat(green)"));
        mats.Add(Resources.Load<Material>("Materials/A_Spiders_Mat(rusty)"));

        if (mats == null)
        {
            return;
        }
        List<MeshRenderer> rend = new List<MeshRenderer>();
        Material mat = mats[GetRandom(0, 6)];
        //不替换的
        List<string> noReplace = new List<string>() {
        "Tracks_R_Geom",
        "Tracks_L_Geom",
        "Tracks_FL_Geom",
        "Tracks_RL_Geom",
        "Tracks_FR_Geom",
        "Tracks_RR_Geom",
        "FX_Laser_Ray_Geom",
        "RL_Track_Geom",
        "RR_Track_Geom",
           "FL_Track_Geom",
        "FR_Track_Geom","FL_Wheel_Geom","FR_Wheel_Geom","RL_Wheel_Geom","RR_Wheel_Geom"
        };
        foreach (MeshRenderer item in gameObject.transform.GetComponentsInChildren<MeshRenderer>())
        {
            
            if (noReplace.Contains(item.gameObject.name)==false)
            {
                // rend.Add(item);
                item.material = mat;
            }
        }
        

        List <SkinnedMeshRenderer> skinrend = new List<SkinnedMeshRenderer>(); 
        foreach (SkinnedMeshRenderer item in gameObject.transform.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            if (noReplace.Contains(item.gameObject.name) == false)
            {
                // skinrend.Add(item);
                item.material = mat;
            }
        }
        //rend.enabled = true;
        //rend[0].materials[0] = mat;

        //rend.sharedMaterial = meshRender;//代表这个对象的共享材质资源（这个是替换材质球）
        //                                 //MeshRenderer继承自Renderer所以上面定义成MeshRenderer也可以,
        //                                 //GetComponent<MeshRenderer>().materials[0] = meshRender;//这个表示找到对应的材质但是不能替换材质球，
        //                                 //GetComponent<MeshRenderer>().material.mainTexture = texture;//和上面的一样，可以替换材质的texture

        //Debug.Log(GetComponent<Renderer>().material);
        //Debug.Log(GetComponent<MeshRenderer>().material.mainTexture);
    }


    /// <summary>
    /// 给物体添加Mesh特效
    /// </summary>
    /// <param name="effectName">特效名称</param>
    /// <param name="MeshObject">目标物体</param>
    public static GameObject AddEffect(string effectName,GameObject MeshObject)
    {
        GameObject Effect = Resources.Load<GameObject>("Effects/" + effectName);
        GameObject currentInstance = GameObject.Instantiate(Effect, MeshObject.transform,false);
        PSMeshRendererUpdater psUpdater = currentInstance.GetComponent<PSMeshRendererUpdater>();
        psUpdater.UpdateMeshEffect(MeshObject);
        
        return currentInstance;
    }

    /// <summary>
    /// 获取子弹预制体
    /// </summary>
    /// <param name="type">武器类型：DoubleGun、GLauncher、Shocker、Shocker_Rifle、Sniper</param>
    /// <param name="bulletPrefab"></param>
    /// <param name="firePrefab"></param>
    public static void GetBulletPrefab(string type, out GameObject bulletPrefab, out GameObject firePrefab)
    {
        switch (type.ToLower())
        {
            case "doublegun":
                bulletPrefab = Resources.Load<GameObject>("Bullets/DoubleGun/Bullet_BlazingRed_Big_Projectile").gameObject;
                firePrefab = Resources.Load<GameObject>("Bullets/DoubleGun/Bullet_BlazingRed_Big_MuzzleFlare").gameObject;
                break;
            case "glauncher":
                bulletPrefab = Resources.Load<GameObject>("Bullets/GLauncher/Plasma_LightBlue_Medium_Projectile").gameObject;
                firePrefab = Resources.Load<GameObject>("Bullets/GLauncher/Plasma_LightBlue_Medium_MuzzleFlare").gameObject;
                break;
            case "shocker":
                bulletPrefab = Resources.Load<GameObject>("Bullets/Shocker/Plasma_RagingRed_Big_Projectile").gameObject;
                firePrefab = Resources.Load<GameObject>("Bullets/Shocker/Plasma_RagingRed_Big_MuzzleFlare").gameObject;
                break;
            case "shocker_rifle":
                bulletPrefab = Resources.Load<GameObject>("Bullets/Shocker_Rifle/Laser_Red_Medium_Projectile").gameObject;
                firePrefab = Resources.Load<GameObject>("Bullets/Shocker_Rifle/Laser_Red_Medium_MuzzleFlare").gameObject;
                break;
            case "sniper":
                bulletPrefab = Resources.Load<GameObject>("Bullets/Sniper/Bullet_GoldFire_Big_Projectile").gameObject;
                firePrefab = Resources.Load<GameObject>("Bullets/Sniper/Bullet_GoldFire_Big_MuzzleFlare").gameObject;
                break;
            default:
                bulletPrefab = Resources.Load<GameObject>("Bullets/Sniper/Bullet_GoldFire_Big_Projectile").gameObject;
                firePrefab = Resources.Load<GameObject>("Bullets/Sniper/Bullet_GoldFire_Big_MuzzleFlare").gameObject;
                break;
        }
    }


    /// <summary>
    /// 获取预制体
    /// </summary>
    /// <param name="type">分类：取值包含 backpack、head、body、weapon、leg、tank、bullet、skill</param>
    /// <param name="path">路径</param>
    /// <returns></returns>
    public static GameObject GetPrefabs(string type, string path)
    {
        string root = "";
        switch (type.ToLower())
        {
            case "backpack":
                root = "Backpacks";
                break;
            case "head":
                root = "Heads";
                break;
            case "body":
                root = "Bodys";
                break;
            case "weapon":
                root = "Weapons";
                break;
            case "leg":
                root = "Legs";
                break;
            case "tank":
                root = "Tanks";
                break;
            case "bullet":
                root = "Bullets";
                break;
            case "skill":
                root = "Skills";
                break;

        }
        GameObject tf = Resources.Load<GameObject>($"{root}/{path}");
        if (tf != null)
        {
            return tf;
        }
        else
        {
            return null;
        }
    }


    /// <summary>
    /// 概率判定
    /// </summary>
    /// <param name="rate">概率值1~100</param>
    /// <returns></returns>
    public static bool IsHit(float rate)
    {
        int random = CommonHelper.GetRandom(1, 100);
        return random <= rate;
    }



   
}
