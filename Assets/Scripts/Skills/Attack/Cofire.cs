using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  攻击技能-协同开火
///  具有该技能的坦克攻击后，一个或者多个我方坦克也进行攻击一次（1-5）
/// </summary>
public class Cofire : MonoBehaviour
{
    public int Value = 0;//同时攻击的坦克数
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
        SkillPrefab = CommonHelper.GetPrefabs("skill", "Attack/协同开火");

    }
    /// <summary>
    /// 攻击特效
    /// </summary>
    /// <returns></returns>
    public bool EffectAttack(List<FightItem> targets)
    {

        foreach (FightItem item in targets)
        {
            GameObject effect = GameObject.Instantiate(SkillPrefab, item.Tank.transform.parent, false);
            effect.SetActive(true);
            GameObject child = effect.transform.Find("fx_magic_lightning_summon_blue").gameObject;
            child.SetActive(true);
            child.transform.GetComponent<ParticleSystem>().Play();

            StartCoroutine(CommonHelper.DelayToInvokeDo(() => { DestroyImmediate(effect); }, 2f));
        }
       

        Effected++;

        return true;
    }
}
