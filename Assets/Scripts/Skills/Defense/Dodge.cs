using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 防御技能-闪避
/// 几率闪避一次攻击（10%-80%）
/// </summary>
public class Dodge : MonoBehaviour
{
    //特效说明:正常状态显示保护罩，被攻击时发动防御技能，显示另外一个特效

    public float Value = 0.1f;//闪避几率百分比
    public int Effected = 0;//已触发次数

    //技能预制体
    public GameObject SkillPrefab = null;

    //防御坦克
    public GameObject Tank = null;
    public GameObject SkillEffect = null;

    private void Awake()
    {
        //挂载后的默认状态
        SkillPrefab = CommonHelper.GetPrefabs("skill", "Defense/闪避");
        SkillEffect = GameObject.Instantiate(SkillPrefab, Tank.transform.parent, false);
        SkillEffect.SetActive(true);
    }



    /// <summary>
    /// 触发防御
    /// </summary>
    /// <returns></returns>
    public bool Trigger()
    {
        if (CommonHelper.IsHit(Value * 100))
        {
            GameObject child = SkillEffect.transform.Find("fx_Summoner_e").gameObject;
            child.SetActive(true);
            child.GetComponent<ParticleSystem>().Play();

            Effected++;


            return true;
        }
        else
        {
            return false;
        }
    }
}

