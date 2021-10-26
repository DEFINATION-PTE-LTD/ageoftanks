using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻击技能-散射
/// 具有此技能的坦克优先攻击对方攻击力最弱的坦克（无视嘲讽）
/// </summary>
public class Destroyweak : MonoBehaviour
{
    
    public int Effected = 0;//已触发次数

    //技能预制体
    public GameObject SkillPrefab = null;
    //实例化体
    public GameObject SkillEffect = null;

    //攻击方
    public GameObject FromTank = null;
    //被攻击方
    public GameObject TargetTank = null;


    private void Awake()
    {
        //挂载后的默认状态
        SkillPrefab = CommonHelper.GetPrefabs("skill", "Attack/逐个击破");

    }
    /// <summary>
    /// 攻击特效
    /// </summary>
    /// <returns></returns>
    public bool EffectAttack()
    {

        SkillEffect = GameObject.Instantiate(SkillPrefab, TargetTank.transform.parent, false);
        SkillEffect.SetActive(true);
        GameObject child = SkillEffect.transform.Find("fx_Summoner_o").gameObject;
        child.SetActive(true);
        child.transform.GetComponent<ParticleSystem>().Play();
        StartCoroutine(CommonHelper.DelayToInvokeDo(() => { DestroyImmediate(SkillEffect); }, 2f));
        Effected++;

        return true;
    }
}

