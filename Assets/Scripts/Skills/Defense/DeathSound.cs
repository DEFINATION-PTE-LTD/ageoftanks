using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 防御技能-亡语
/// 死亡后对我方增加攻击、血量、速度、防御；对敌方造成伤害、降低速度、降低防御等
/// </summary>
public class DeathSound : MonoBehaviour
{

    public int Total = 1;//
    public int Effected = 0;//已触发次数

    //技能预制体
    public GameObject SkillPrefab = null;

    //防御坦克
    public GameObject Tank = null;
    public GameObject SkillEffect = null;

    private void Awake()
    {
        //挂载后的默认状态
        SkillPrefab = CommonHelper.GetPrefabs("skill", "Defense/亡语");
        SkillEffect = GameObject.Instantiate(SkillPrefab, Tank.transform.parent, false);
        SkillEffect.SetActive(true);
    }



    /// <summary>
    /// 触发防御
    /// </summary>
    /// <returns></returns>
    public bool Trigger()
    {
        if (Effected < Total)
        {
            SkillEffect = GameObject.Instantiate(SkillPrefab, Tank.transform.parent, false);
            SkillEffect.SetActive(true);
            if (SkillEffect != null)
            {
                GameObject child = SkillEffect.transform.Find("CircleFX_Dark").gameObject;
                if (child != null)
                {
                    child.SetActive(true);
                    child.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                }
                Effected++;
                if (Effected == Total)
                {
                    StartCoroutine(CommonHelper.DelayToInvokeDo(() => { DestroyImmediate(SkillEffect); }, 1f));
                }
            }
            return true;
        }
        return false;

    }
}

