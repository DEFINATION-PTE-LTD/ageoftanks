using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 防御技能-恢复
/// 每回合回复自身血量（10%-80%）
/// </summary>
public class Recover:MonoBehaviour
{
    //特效说明:正常状态显示保护罩，被攻击时发功防御抵挡一次攻击抵挡时防护罩放大随后恢复到初始大小

    public float Value = 0;//
    public int Effected = 0;//已触发次数

    //技能预制体
    public GameObject SkillPrefab = null;

    //防御坦克
    public GameObject Tank = null;
    public GameObject SkillEffect = null;

    private void Awake()
    {
        //挂载后的默认状态
        SkillPrefab = CommonHelper.GetPrefabs("skill", "Defense/恢复");
        SkillEffect = GameObject.Instantiate(SkillPrefab, Tank.transform.parent, false);
        SkillEffect.SetActive(true);
    }



    /// <summary>
    /// 触发防御
    /// </summary>
    /// <returns></returns>
    public bool Trigger()
    {

        GameObject child = SkillEffect.transform.Find("15_RFX_Magic_Buff2").gameObject;
        child.SetActive(true);
        child.GetComponent<ParticleSystem>().Play();

        Effected++;

        return true;

    }
}

