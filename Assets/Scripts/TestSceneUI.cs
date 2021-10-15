using UnityEngine;

//using DG.Tweening;

public class TestSceneUI : MonoBehaviour
{
    public GameObject button;

    public GameObject PlayerA;
    public GameObject PlayerB;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void Awake()
    {
        transform.Find("UI/Panel/btn_lianji").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            ShowAttackSkill("连击");
        });
        transform.Find("UI/Panel/btn_zhongji").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            ShowAttackSkill("重击");
        });
        transform.Find("UI/Panel/btn_chuangjia").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            ShowAttackSkill("穿甲");
        });
        transform.Find("UI/Panel/btn_ranshao").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            ShowAttackSkill("燃烧");
        });
        transform.Find("UI/Panel/btn_sanshe").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            ShowAttackSkill("散射");
        });
        transform.Find("UI/Panel/btn_bisha").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            ShowAttackSkill("必杀");
        });
        transform.Find("UI/Panel/btn_bingdong").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            ShowAttackSkill("冰冻");
        });
        transform.Find("UI/Panel/btn_xixue").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            ShowAttackSkill("吸血");
        });
        transform.Find("UI/Panel/btn_jinmo").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            ShowAttackSkill("禁魔");
        });
        transform.Find("UI/Panel/btn_xietongkh").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            ShowAttackSkill("协同开火");
        });
        transform.Find("UI/Panel/btn_diguozl").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            ShowAttackSkill("帝国指令");
        });
        transform.Find("UI/Panel/btn_juedou").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            ShowAttackSkill("决斗");
        });
        transform.Find("UI/Panel/btn_zhugejp").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            ShowAttackSkill("逐个击破");
        });
        transform.Find("UI/Panel/btn_gongwubk").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            ShowAttackSkill("攻无不克");
        });




        transform.Find("UI/Panel/btn_shengdun").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            ShowDenfenseSkill("圣盾");
        });
        transform.Find("UI/Panel/btn_chaofeng").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            ShowDenfenseSkill("嘲讽");
        });
        transform.Find("UI/Panel/btn_zhongjia").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            ShowDenfenseSkill("重甲");
        });
        transform.Find("UI/Panel/btn_shanbi").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            ShowDenfenseSkill("闪避");
        });
        transform.Find("UI/Panel/btn_fusheng").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            ShowDenfenseSkill("复生");
        });
        transform.Find("UI/Panel/btn_huifu").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            ShowDenfenseSkill("恢复");
        });
        transform.Find("UI/Panel/btn_wangyu").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            ShowDenfenseSkill("亡语");
        });
        transform.Find("UI/Panel/btn_chuansong").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            ShowTransfer();
        });
        transform.Find("UI/Panel/btn_siwang").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            ShowDeath();
        });
    }

    /// <summary>
    /// 连击技能
    /// </summary>
    public void Skill_Lianji()
    {
        GameObject skillprefab = transform.Find("技能整合/攻击技能/连击").gameObject;
        Debug.Log(skillprefab.name);

        //GameObject A4 = transform.Find("Platforms/A4").gameObject;
        //GameObject B4 = transform.Find("Platforms/B4").gameObject;

        GameObject newSkill1 = Instantiate(skillprefab);
        newSkill1.transform.SetParent(PlayerB.transform, false);
        newSkill1.SetActive(true);

        Destroy(newSkill1, 5f);
        //newSkill1.transform.DOMove(B4.transform.position, 0.5f).OnComplete(()=> {
        //    Destroy(newSkill1, 5f);
        //});

    }

    /// <summary>
    /// 重击技能
    /// </summary>
    public void Skill_zhongji()
    {
        GameObject skillprefab = transform.Find("技能整合/攻击技能/重击").gameObject;
        Debug.Log(skillprefab.name);

        //GameObject A4 = transform.Find("Platforms/A4").gameObject;
        //GameObject B4 = transform.Find("Platforms/B4").gameObject;

        GameObject newSkill1 = Instantiate(skillprefab);
        newSkill1.transform.SetParent(PlayerB.transform, false);
        newSkill1.SetActive(true);
        Destroy(newSkill1, 5f);

        //newSkill1.transform.DOMove(B4.transform.position, 0.5f).OnComplete(() => {
        //    Destroy(newSkill1, 5f);
        //});

    }

    /// <summary>
    /// 演示攻击技能
    /// </summary>
    public void ShowAttackSkill(string name)
    {
        GameObject skillprefab = transform.Find("技能整合/攻击技能/" + name).gameObject;
        Debug.Log(skillprefab.name);

        //GameObject A4 = transform.Find("Platforms/A4").gameObject;
        //GameObject B4 = transform.Find("Platforms/B4").gameObject;

        GameObject newSkill1 = Instantiate(skillprefab);
        newSkill1.transform.SetParent(PlayerA.transform, false);
        newSkill1.SetActive(true);
        Destroy(newSkill1, 5f);

        //newSkill1.transform.DOMove(B4.transform.position, 0.5f).OnComplete(() => {
        //    Destroy(newSkill1, 5f);
        //});

    }

    /// <summary>
    /// 演示防御技能
    /// </summary>
    public void ShowDenfenseSkill(string name)
    {
        GameObject skillprefab = transform.Find("技能整合/防御技能/" + name).gameObject;
        Debug.Log(skillprefab.name);

        //GameObject A4 = transform.Find("Platforms/A4").gameObject;
        //GameObject B4 = transform.Find("Platforms/B4").gameObject;

        GameObject newSkill1 = Instantiate(skillprefab);
        newSkill1.transform.SetParent(PlayerA.transform, false);
        newSkill1.SetActive(true);
        Destroy(newSkill1, 5f);

        //newSkill1.transform.DOMove(B4.transform.position, 0.5f).OnComplete(() => {
        //    Destroy(newSkill1, 5f);
        //});

    }

    /// <summary>
    /// 演示传送阵
    /// </summary>
    /// <param name="name"></param>
    public void ShowTransfer()
    {
        GameObject skillprefab = transform.Find("技能整合/传送阵").gameObject;
        Debug.Log(skillprefab.name);

        //GameObject A4 = transform.Find("Platforms/A4").gameObject;
        //GameObject B4 = transform.Find("Platforms/B4").gameObject;

        GameObject newSkill1 = Instantiate(skillprefab);
        newSkill1.transform.SetParent(PlayerA.transform, false);
        newSkill1.SetActive(true);
        Destroy(newSkill1, 5f);

        //newSkill1.transform.DOMove(B4.transform.position, 0.5f).OnComplete(() => {
        //    Destroy(newSkill1, 5f);
        //});

    }

    /// <summary>
    /// 演示死亡效果
    /// </summary>
    /// <param name="name"></param>
    public void ShowDeath()
    {
        GameObject skillprefab = transform.Find("技能整合/死亡").gameObject;
        Debug.Log(skillprefab.name);

        //GameObject A4 = transform.Find("Platforms/A4").gameObject;
        //GameObject B4 = transform.Find("Platforms/B4").gameObject;

        GameObject newSkill1 = Instantiate(skillprefab);
        newSkill1.transform.SetParent(PlayerA.transform, false);
        newSkill1.SetActive(true);
        Destroy(newSkill1, 5f);

        //newSkill1.transform.DOMove(B4.transform.position, 0.5f).OnComplete(() => {
        //    Destroy(newSkill1, 5f);
        //});

    }
}
