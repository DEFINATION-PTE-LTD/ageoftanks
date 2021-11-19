using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;
using System.Linq;

public class SoloSelect : MonoBehaviour
{

    public GameObject Root;

    private void Awake()
    {

        InitCardPool();
        Root.transform.Find("MainPanel/RightPanel/BtnPlay").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartAction);
        Root.transform.Find("MainPanel/RightPanel/BtnSynthesis").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(()=> {
            Root.transform.Find("MountPanel").gameObject.SetActive(true);
        });

        

        ResourceCtrl.Instance.SelectList.Clear();
        ResourceCtrl.Instance.SelectListB.Clear();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }


    #region 按钮点击回调
    //开始试玩按钮
    private void StartAction()
    {
        AudioManager.Instance.PlayBtnAudio();
        if (ResourceCtrl.Instance.SelectList.Count == 0)
        {
            Root.transform.Find("MainPanel/RightPanel/txtTip").gameObject.SetActive(true);
            Root.transform.Find("MainPanel/RightPanel/txtTip").DOShakePosition(1f, new Vector3(4, 0, 0));
        }
        else
        {
            Root.transform.Find("MainPanel/RightPanel/txtTip").gameObject.SetActive(false);
            //默认选中1个
            ResourceCtrl.Instance.SelectListB.Clear();
            int count = ResourceCtrl.Instance.TankListB.Count;
            ResourceCtrl.Instance.SelectListB.Add( ResourceCtrl.Instance.TankListB[CommonHelper.GetRandom(0, count)]);
            SceneManager.LoadScene("AlienworldSolo");
            System.GC.Collect();
        }
    }
    #endregion



    /// <summary>
    /// 卡牌池初始化
    /// </summary>
    public void InitCardPool()
    {
        //坦克卡牌
        GameObject tankCard = ResourceCtrl.Instance.ResourceRoot.transform.Find("UI/UnSelectCard").gameObject;

        //卡牌池
        GameObject cardPool = Root.transform.Find("MainPanel/RightPanel/CardPool/Viewport/Content").gameObject;

        //销毁子物体
        for (int i = cardPool.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(cardPool.transform.GetChild(i).gameObject);
        }

        foreach (TankProperty item in ResourceCtrl.Instance.TankList)
        {
            GameObject newCard = Instantiate(tankCard, cardPool.transform);
            newCard.SetActive(true);
            newCard.name = "TankCard" + item.Code;
            if (item.IsSetup == true)
            {
                newCard.transform.Find("Panel/image").GetComponent<RawImage>().texture = ResourceCtrl.Instance.MountTanksSprite[item.Code].texture;
            }
            else
            {
                // newCard.transform.Find("Panel/image").GetComponent<RawImage>().texture = CommonHelper.LoadTankImage(item.Code);
                newCard.transform.Find("Panel/image").GetComponent<RawImage>().texture = ResourceCtrl.Instance.TanksSprite[item.Code].texture;
            }
            newCard.transform.Find("Panel/txtNo").GetComponent<Text>().text = "#" + item.Code;

            newCard.transform.Find("Panel/iconAttack/txtVal").GetComponent<Text>().text = item.Attack.ToString();
            newCard.transform.Find("Panel/iconDefense/txtVal").GetComponent<Text>().text = item.Blood.ToString();
            newCard.transform.Find("Panel/iconCrit/txtVal").GetComponent<Text>().text = item.CritRate.ToString() + "%";
            newCard.transform.Find("Panel/iconSpeed/txtVal").GetComponent<Text>().text = item.Speed.ToString();

            if (item.AttackSkill != null)
            {
                newCard.transform.Find("Panel/attackSkill/icon").GetComponent<RawImage>().texture = CommonHelper.LoadSkillImage(item.AttackSkill.SkillName);
            }
            else 
            {
                newCard.transform.Find("Panel/attackSkill/icon").gameObject.SetActive(false);
            }
            if (item.DefenseSkill != null)
            {
                newCard.transform.Find("Panel/defenseSkill/icon").GetComponent<RawImage>().texture = CommonHelper.LoadSkillImage(item.DefenseSkill.SkillName);
            }
            else
            {
                newCard.transform.Find("Panel/defenseSkill/icon").gameObject.SetActive(false);
            }

          
            newCard.gameObject.AddComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
                AudioManager.Instance.PlayBtnAudio();
                Debug.Log("当前点击了" + newCard.name);
                showDetail(item);
            });
        }
    }



    //显示详情
    public void showDetail(TankProperty item)
    {
        GameObject infoPanel = Root.transform.Find("DetailPanel").gameObject;

        //编号
        infoPanel.transform.Find("txtNo").GetComponent<Text>().text = "#" + item.Code;
        //坦克图
        if (item.IsSetup == true)
        {
            infoPanel.transform.Find("image").GetComponent<RawImage>().texture = ResourceCtrl.Instance.MountTanksSprite[item.Code].texture;
        }
        else
        {
            //infoPanel.transform.Find("image").GetComponent<RawImage>().texture = CommonHelper.LoadTankImage(item.Code);
            infoPanel.transform.Find("image").GetComponent<RawImage>().texture = ResourceCtrl.Instance.TanksSprite[item.Code].texture;

        }

        
        //技能
        GameObject attackSkill = infoPanel.transform.Find("AttackSkill").gameObject;
        GameObject defenseSkill = infoPanel.transform.Find("DefenseSkill").gameObject;
        if (item.AttackSkill != null)
        {
            //图标、标题、描述
            attackSkill.transform.Find("Icon").GetComponent<RawImage>().texture = CommonHelper.LoadSkillImage(item.AttackSkill.SkillName);
            attackSkill.transform.Find("txtTitle").GetComponent<Text>().text = item.AttackSkill.Title_en;
            attackSkill.transform.Find("txtInfo").GetComponent<Text>().text = item.AttackSkill.Description_en;
            attackSkill.SetActive(true);
        }
        else
        {
            attackSkill.SetActive(false);
        }

        if (item.DefenseSkill != null)
        {
            //图标、标题、描述
            defenseSkill.transform.Find("Icon").GetComponent<RawImage>().texture = CommonHelper.LoadSkillImage(item.DefenseSkill.SkillName);
            defenseSkill.transform.Find("txtTitle").GetComponent<Text>().text = item.DefenseSkill.Title_en;
            defenseSkill.transform.Find("txtInfo").GetComponent<Text>().text = item.DefenseSkill.Description_en;
            defenseSkill.SetActive(true);
        }
        else
        {
            defenseSkill.SetActive(false);
        }
        infoPanel.SetActive(true);
        infoPanel.transform.DOScale(new Vector3(1, 1, 1), 0.2f).From(new Vector3(0, 0, 0));

        //属性
        GameObject attr = infoPanel.transform.Find("Attributes").gameObject;
        attr.transform.Find("血量/txtVal").GetComponent<Text>().text = item.Blood.ToString();
        attr.transform.Find("攻击力/txtVal").GetComponent<Text>().text = item.Attack.ToString();
        //attr.transform.Find("防御值/txtVal").GetComponent<Text>().text = item.Defense.ToString();
        attr.transform.Find("射程/txtVal").GetComponent<Text>().text = item.Range.ToString();
        attr.transform.Find("速度/txtVal").GetComponent<Text>().text = item.Speed.ToString();
        attr.transform.Find("暴击率/txtVal").GetComponent<Text>().text = item.CritRate.ToString() + "%";
        //attr.transform.Find("命中率/txtVal").GetComponent<Text>().text = item.HitRate.ToString()+"%";


        //选择按钮
        UnityEngine.UI.Button btnSelect = infoPanel.transform.Find("BtnSelect").GetComponent<UnityEngine.UI.Button>();
        btnSelect.onClick.RemoveAllListeners();


        if (ResourceCtrl.Instance.SelectList.FindIndex(u => u.Code == item.Code) > -1)
        {
            btnSelect.transform.Find("Text").GetComponent<Text>().text = "UnSelect";
            infoPanel.transform.Find("BtnSelect").gameObject.SetActive(true);
            btnSelect.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayBtnAudio();
                //标记状态切换

                Root.transform.Find("MainPanel/RightPanel/CardPool/Viewport/Content/TankCard" + item.Code + "/Panel/CheckStatus").gameObject.SetActive(false);
                ResourceCtrl.Instance.SelectList.Clear();
                infoPanel.transform.DOScale(new Vector3(0, 0, 0), 0.2f).SetEase(Ease.OutExpo).OnComplete(() =>
                {
                    infoPanel.SetActive(false);
                });
            });
        }
        else
        {
            btnSelect.transform.Find("Text").GetComponent<Text>().text = "Select";


            // infoPanel.transform.Find("BtnSelect").gameObject.SetActive();

            btnSelect.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayBtnAudio();
                if (ResourceCtrl.Instance.SelectList.Count > 0)
                {
                    Root.transform.Find("MainPanel/RightPanel/CardPool/Viewport/Content/TankCard" + ResourceCtrl.Instance.SelectList[0].Code + "/Panel/CheckStatus").gameObject.SetActive(false);
                    ResourceCtrl.Instance.SelectList.Clear();
                }

                ResourceCtrl.Instance.SelectList.Add(item);
                Root.transform.Find("MainPanel/RightPanel/CardPool/Viewport/Content/TankCard" + item.Code + "/Panel/CheckStatus").gameObject.SetActive(true);
                infoPanel.transform.DOScale(new Vector3(0, 0, 0), 0.2f).SetEase(Ease.OutExpo).OnComplete(() =>
                {
                    infoPanel.SetActive(false);
                });
            });

        }

        //关闭按钮
        UnityEngine.UI.Button btnClose = infoPanel.transform.Find("BtnClose").GetComponent<UnityEngine.UI.Button>();
        btnClose.onClick.RemoveAllListeners();
        btnClose.onClick.AddListener(() => {
            AudioManager.Instance.PlayBtnAudio();
            infoPanel.transform.DOScale(new Vector3(0, 0, 0), 0.2f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                infoPanel.SetActive(false);
            });
        });
    }

}
