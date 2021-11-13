using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  攻击技能-帝国指令
///  具有该技能的坦克命中对方坦克后，我方其他坦克只攻击敌方同一个坦克，直至对方摧毁
/// </summary>
public class Directive : MonoBehaviour
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
        SkillPrefab = CommonHelper.GetPrefabs("skill", "Attack/帝国指令");

    }
    /// <summary>
    /// 攻击特效
    /// </summary>
    /// <returns></returns>
    public bool EffectAttack()
    {
        GameObject effect = GameObject.Instantiate(SkillPrefab, TargetTank.transform.parent, false);
        effect.SetActive(true);
        GameObject child = effect.transform.Find("41_RFX_Magic_FxSword1").gameObject;
        child.SetActive(true);
        child.transform.GetComponent<ParticleSystem>().Play();

        StartCoroutine(CommonHelper.DelayToInvokeDo(() => { DestroyImmediate(effect); }, 2f));

        Effected++;

        return true;
    }
}
