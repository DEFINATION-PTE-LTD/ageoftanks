using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 攻击技能-连击
/// 连续发起多次攻击（2-6次）
/// </summary>
public class Batter : MonoBehaviour
{
    
    public int Value = 0;//攻击次数
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
        SkillPrefab = CommonHelper.GetPrefabs("skill", "Attack/连击");
        
    }


    /// <summary>
    /// 攻击特效
    /// </summary>
    /// <returns></returns>
    public bool EffectAttack(Action action)
    {
        //连续发起多次攻击（2-6次）次数在获得是就已固定
        int hitcount = Value;

        SkillEffect = GameObject.Instantiate(SkillPrefab, FromTank.transform.parent, false);
        SkillEffect.SetActive(true);
        GameObject child = SkillEffect.transform.Find("fx_magic_lightning_falling_surround_balls_blue").gameObject;
        child.SetActive(true);
        child.GetComponent<ParticleSystem>().Play();
        SkillEffect.transform.DOScale(new Vector3(1,1,1),1f).From(new Vector3(0,0,0));
        SkillEffect.transform.DOMove(TargetTank.transform.parent.position, 1f).OnComplete(()=> {
            StartCoroutine(CommonHelper.DelayToInvokeDo(() =>{ DestroyImmediate(SkillEffect);}, 2f));
        });
      
        Effected++;

        for (int i = 0; i < hitcount-1; i++)
        {
            float delay = (i+1) * 0.5f; //延迟时间
            StartCoroutine(CommonHelper.DelayToInvokeDo(() => {
                action();
                //SimpleAttack(fromItem, targets, true); 
            }, delay));
        }
        return true;
    }
}

