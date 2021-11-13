using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 攻击技能-穿甲
/// 被技能击中后的目标，再次受到伤害会附加额外百分比的伤害（10%-100%）
/// </summary>
public class Penetrate : MonoBehaviour
{
    public float Value = 0;//附加百分比伤害
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
        SkillPrefab = CommonHelper.GetPrefabs("skill", "Attack/穿甲");

    }
    /// <summary>
    /// 攻击特效
    /// </summary>
    /// <returns></returns>
    public bool EffectAttack()
    {
        
        SkillEffect = GameObject.Instantiate(SkillPrefab, TargetTank.transform.parent, false);
        SkillEffect.SetActive(true);
        GameObject child = SkillEffect.transform.Find("ArrowFX_FireRain").gameObject;
        child.SetActive(true);
        child.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
       
        StartCoroutine(CommonHelper.DelayToInvokeDo(() => { DestroyImmediate(SkillEffect); }, 2f));
        

        Effected++;

        return true;
    }
}

