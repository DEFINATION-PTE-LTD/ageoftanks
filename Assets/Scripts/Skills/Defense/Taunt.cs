using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 防御技能-嘲讽
/// 是有坦克必须优先攻击具有嘲讽技能的目标
/// </summary>
public class Taunt : MonoBehaviour
{
    //特效说明:正常状态显示保护罩，被攻击时发功防御抵挡一次攻击抵挡时防护罩放大随后恢复到初始大小

    //public float Value = 0;//
    public int Effected = 0;//已触发次数

    //技能预制体
    public GameObject SkillPrefab = null;

    //防御坦克
    public GameObject Tank = null;
    public GameObject SkillEffect = null;

    private void Awake()
    {
        //挂载后的默认状态
        SkillPrefab = CommonHelper.GetPrefabs("skill", "Defense/嘲讽");
        SkillEffect = GameObject.Instantiate(SkillPrefab, Tank.transform.parent, false);
        SkillEffect.SetActive(true);
    }



    /// <summary>
    /// 触发防御
    /// </summary>
    /// <returns></returns>
    public bool Trigger()
    {
        if (SkillEffect != null)
        {
            GameObject child = SkillEffect.transform.Find("HitFX_Dark").gameObject;
            if (child != null)
            {
                child.SetActive(true);
                child.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            }
        }
        Effected++;

        return true;

    }
}

