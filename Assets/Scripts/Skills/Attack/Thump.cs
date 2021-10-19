using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 攻击技能-重击
/// 对敌方造成多倍攻击伤害（200%-600%）
/// </summary>
public class Thump : MonoBehaviour
{
    public float Value = 1;//攻击倍数
    public int Effected = 0;//已触发次数

    //技能预制体
    public GameObject SkillPrefab = null;
    //实例化体
    public GameObject SkillEffect = null;

    //攻击方
    public GameObject FromTank = null;
    //被攻击放
    public GameObject TargetTank = null;


    private void Awake()
    {
        //挂载后的默认状态
        SkillPrefab = CommonHelper.GetPrefabs("skill", "Attack/重击");

    }


    /// <summary>
    /// 攻击特效
    /// </summary>
    /// <returns></returns>
    public bool EffectAttack()
    {
        SkillEffect = GameObject.Instantiate(SkillPrefab, TargetTank.transform.parent, false);
        SkillEffect.SetActive(true);
        GameObject child = SkillEffect.transform.Find("9_RFX_HitSplat3").gameObject;
        child.SetActive(true);
        child.GetComponent<ParticleSystem>().Play();
        StartCoroutine(CommonHelper.DelayToInvokeDo(() => { DestroyImmediate(SkillEffect); }, 2f));
        Effected++;

        return true;
    }
}

