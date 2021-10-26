using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 攻击技能-冰冻
/// 目标一个回合内无法行动
/// </summary>
public class Frozen : MonoBehaviour
{
    public int Value = 0;//冰冻回合数


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
        SkillPrefab = CommonHelper.GetPrefabs("skill", "Attack/冰冻");

    }
    /// <summary>
    /// 攻击特效
    /// </summary>
    /// <returns></returns>
    public bool EffectAttack()
    {

        SkillEffect = GameObject.Instantiate(SkillPrefab, TargetTank.transform.parent, false);
        SkillEffect.SetActive(true);
        GameObject child = SkillEffect.transform.Find("ice_eff_08").gameObject;
        child.SetActive(true);
        child.transform.GetComponent<ParticleSystem>().Play();
        StartCoroutine(CommonHelper.DelayToInvokeDo(() => { DestroyImmediate(SkillEffect); }, 2f));
        Effected++;

        return true;
    }
}
