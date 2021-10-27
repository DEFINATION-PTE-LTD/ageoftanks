using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 防御技能-复生
/// 死亡后可复活一次
/// </summary>

public class Revive : MonoBehaviour
{
   
    public int Total = 1;//可抵挡攻击次数
    public int Effected = 0;//已触发次数

    //技能预制体
    public GameObject SkillPrefab = null;

    //防御坦克
    public GameObject Tank = null;
    public GameObject SkillEffect = null;

    private void Awake()
    {
        //挂载后的默认状态
        SkillPrefab = CommonHelper.GetPrefabs("skill", "Defense/复生");
        SkillEffect = GameObject.Instantiate(SkillPrefab, Tank.transform.parent, false);
        SkillEffect.SetActive(true);
    }



    /// <summary>
    /// 触发防御,在有效防御内触发，达到次数后自动销毁
    /// </summary>
    /// <returns></returns>
    public bool Trigger()
    {
        if (Effected < Total)
        {
            if (SkillEffect != null)
            {
                GameObject child = SkillEffect.transform.Find("16_RFX_Magic_Buff3").gameObject;
                if (child != null)
                {
                    child.SetActive(true);
                    child.GetComponent<ParticleSystem>().Play();
                }
            }
            Effected++;
                if (Effected == Total)
                {
                    StartCoroutine(CommonHelper.DelayToInvokeDo(() => { DestroyImmediate(SkillEffect); }, 3f));
                }
            
            return true;
        }
        return false;
    }
}

