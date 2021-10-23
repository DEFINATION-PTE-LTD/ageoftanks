using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 防御技能-还击
/// 受到敌方伤害后，会攻击伤害来源的对方坦克
/// </summary>
public class Revenge : MonoBehaviour
{
   // public int Total = 1;//
    public int Effected = 0;//已触发次数

    //技能预制体
    public GameObject SkillPrefab = null;

    //防御坦克
    public GameObject Tank = null;
    public GameObject SkillEffect = null;

    private void Awake()
    {
        //挂载后的默认状态
        SkillPrefab = CommonHelper.GetPrefabs("skill", "Defense/还击");
        SkillEffect = GameObject.Instantiate(SkillPrefab, Tank.transform.parent, false);
        SkillEffect.SetActive(true);
    }



    /// <summary>
    /// 触发防御
    /// </summary>
    /// <returns></returns>
    public bool Trigger()
    {

        GameObject child = SkillEffect.transform.Find("Magic shield 13").gameObject;
        child.SetActive(true);
        child.GetComponent<ParticleSystem>().Play();
        StartCoroutine(CommonHelper.DelayToInvokeDo(() =>
        {
            child.SetActive(false);
        }, 2f));

        //Vector3 oldScale = SkillPrefab.transform.localScale;
        //Vector3 newScale = new Vector3(1.5f, 1.5f, 1.5f);
        //SkillEffect.transform.DOScale(newScale, 0.5f).SetEase(Ease.InOutExpo);
        //StartCoroutine(CommonHelper.DelayToInvokeDo(() =>
        //{
        //    SkillEffect.transform.DOScale(oldScale, 0.2f).SetEase(Ease.InOutExpo);
        //}, 1f));

        Effected++;
            
        return true;
     
    }
}

