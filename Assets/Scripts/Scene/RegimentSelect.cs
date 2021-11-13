using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;
using System.Linq;

public class RegimentSelect : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    public GameObject Root;


    //可被拖拽的元素
    List<string> drags = new List<string>();
    public GameObject dragObject;
    public Vector3 dragObjectPos;
    public GameObject dragItem;
    public int StartIndex = -1, EndIndex = -1;

    TankProperty[] selects = new TankProperty[7];

    private void Awake()
    {
        InitTankList();
        InitCardPool();
        Root.transform.Find("MainPanel/RightPanel/BtnPlay").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartAction);
        Root.transform.Find("MainPanel/RightPanel/BtnSynthesis").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
            Root.transform.Find("MountPanel").gameObject.SetActive(true);
        });
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
        if (HasEmpty() == true)
        {
            Root.transform.Find("MainPanel/RightPanel/txtTip").gameObject.SetActive(true);
            Root.transform.Find("MainPanel/RightPanel/txtTip").DOShakePosition(1f, new Vector3(4, 0, 0));
        }
        else
        {
            Root.transform.Find("MainPanel/RightPanel/txtTip").gameObject.SetActive(false);
            ResourceCtrl.Instance.SelectList = selects.ToList();
            ResourceCtrl.Instance.SelectListB = ResourceCtrl.Instance.TankList.FindAll(u => selects.Contains(u) == false).GetRange(0, 7);
            // SceneManager.LoadScene("FightScene2");
            SceneManager.LoadScene("Alienworld");
            System.GC.Collect();
        }
    }


    #endregion

    //初始化坦克列表
    void InitTankList()
    {
        //默认选中前6个
        //ResourceCtrl.Instance.SelectList = ResourceCtrl.Instance.TankList.GetRange(0, 6);
        selects[0] = ResourceCtrl.Instance.TankList[0];
        selects[1] = ResourceCtrl.Instance.TankList[1];
        selects[2] = ResourceCtrl.Instance.TankList[2];
        selects[3] = ResourceCtrl.Instance.TankList[3];
        selects[4] = ResourceCtrl.Instance.TankList[4];
        selects[5] = ResourceCtrl.Instance.TankList[5];

        RefreshSelectCard();
    }

    /// <summary>
    /// 卡牌池初始化
    /// </summary>
    void InitCardPool()
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
                newCard.transform.Find("Panel/image").GetComponent<RawImage>().texture = CommonHelper.LoadTankImage(item.Code);
            }

            newCard.transform.Find("Panel/txtNo").GetComponent<Text>().text = "#" + item.Code;

            newCard.transform.Find("Panel/iconAttack/txtVal").GetComponent<Text>().text = item.Attack.ToString();
            newCard.transform.Find("Panel/iconDefense/txtVal").GetComponent<Text>().text = item.Blood.ToString();

            newCard.transform.Find("Panel/iconCrit/txtVal").GetComponent<Text>().text = item.CritRate.ToString()+"%";
            newCard.transform.Find("Panel/iconSpeed/txtVal").GetComponent<Text>().text = item.Speed.ToString();

            newCard.transform.Find("Panel/attackSkill/icon").GetComponent<RawImage>().texture = CommonHelper.LoadSkillImage(item.AttackSkill.SkillName);
            newCard.transform.Find("Panel/defenseSkill/icon").GetComponent<RawImage>().texture = CommonHelper.LoadSkillImage(item.DefenseSkill.SkillName);

         
            newCard.gameObject.AddComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
                AudioManager.Instance.PlayBtnAudio();
                Debug.Log("当前点击了" + newCard.name);
                showDetail(item);
            });
        }
    }

    /// <summary>
    /// 初始化已选择卡牌
    /// </summary>
    void RefreshSelectCard()
    {
        //SelectList
        //坦克卡牌
        GameObject tankCard = ResourceCtrl.Instance.ResourceRoot.transform.Find("UI/SelectCard").gameObject;
        //空牌
        GameObject emptyCard = ResourceCtrl.Instance.ResourceRoot.transform.Find("UI/EmptyCard").gameObject;

        //卡牌池
        GameObject selectPool = Root.transform.Find("MainPanel/RightPanel/SelectPool").gameObject;

        for (int i = 0; i < 7; i++)
        {
            Transform pos = selectPool.transform.Find("Card" + (i + 1));
            TankProperty item = selects[i];
            if (item != null)
            {
                int childCount = pos.childCount;
                string childName = "";
                for (int j = 0; j < childCount; j++)
                {
                    if (pos.GetChild(j).name == "Text")
                    {
                       //pos.GetChild(j).gameObject.SetActive(false);
                    }
                    else
                    {
                        childName = pos.GetChild(j).gameObject.name;
                        //DestroyImmediate(pos.GetChild(j).gameObject);
                    }
                }

                if (childName == "" || childName != "TankCard" + item.Code)
                {
                    if (childName != "")
                    {
                        DestroyImmediate(pos.Find(childName).gameObject);
                    }

                    GameObject newCard = Instantiate(tankCard, pos);
                    newCard.transform.GetComponent<RectTransform>().position = pos.GetComponent<RectTransform>().position;
                    newCard.SetActive(true);
                    newCard.AddComponent<CanvasGroup>();
                    newCard.name = "TankCard" + item.Code;

                    if (item.IsSetup == true)
                    {
                        newCard.transform.Find("Panel/image").GetComponent<RawImage>().texture = ResourceCtrl.Instance.MountTanksSprite[item.Code].texture;
                    }
                    else
                    {
                        newCard.transform.Find("Panel/image").GetComponent<RawImage>().texture = CommonHelper.LoadTankImage(item.Code);
                    }

                    newCard.transform.Find("Panel/txtNo").GetComponent<Text>().text = "#" + item.Code;
                    newCard.transform.Find("Panel/iconAttack/txtVal").GetComponent<Text>().text = item.Attack.ToString();
                    newCard.transform.Find("Panel/iconDefense/txtVal").GetComponent<Text>().text = item.Blood.ToString();
                    newCard.transform.Find("Panel/iconCrit/txtVal").GetComponent<Text>().text = item.CritRate.ToString() + "%";
                    newCard.transform.Find("Panel/iconSpeed/txtVal").GetComponent<Text>().text = item.Speed.ToString();
                    newCard.transform.Find("Panel/attackSkill/icon").GetComponent<RawImage>().texture =CommonHelper.LoadSkillImage(item.AttackSkill.SkillName); 
                    newCard.transform.Find("Panel/defenseSkill/icon").GetComponent<RawImage>().texture = CommonHelper.LoadSkillImage(item.DefenseSkill.SkillName);
                   

                    newCard.gameObject.AddComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                    {
                        AudioManager.Instance.PlayBtnAudio();
                        showDetail(item);
                    });

                    newCard.transform.DOLocalRotate(new Vector3(0,0,0), 0.8f).From(new Vector3(0, 180, 0));
                }
            }
            else
            {
                int childCount = pos.childCount;
                for (int j = 0; j < childCount; j++)
                {
                    if (pos.GetChild(j).name == "Text")
                    {
                        //pos.GetChild(j).gameObject.SetActive(true);
                    }
                    else
                    {
                         DestroyImmediate(pos.GetChild(j).gameObject);
                    }
                }
            }

        }

        drags = new List<string>();
        for (int i = 0; i < selects.Length; i++)
        {
            if (selects[i] != null) 
            {
                drags.Add("TankCard" + selects[i].Code);
            }
            
        }
    }



    //显示详情
    void showDetail(TankProperty item) 
    {
        GameObject infoPanel =   Root.transform.Find("DetailPanel").gameObject;

        //编号
        infoPanel.transform.Find("txtNo").GetComponent<Text>().text = "#" + item.Code;

        //坦克图
        if (item.IsSetup == true)
        {
            infoPanel.transform.Find("image").GetComponent<RawImage>().texture = ResourceCtrl.Instance.MountTanksSprite[item.Code].texture;
        }
        else
        {
            infoPanel.transform.Find("image").GetComponent<RawImage>().texture = CommonHelper.LoadTankImage(item.Code);
        }

        //技能
        GameObject attackSkill = infoPanel.transform.Find("AttackSkill").gameObject;
        GameObject defenseSkill = infoPanel.transform.Find("DefenseSkill").gameObject;
        if (item.AttackSkill!=null)
        {
            //图标、标题、描述
            attackSkill.transform.Find("Icon").GetComponent<RawImage>().texture = CommonHelper.LoadSkillImage(item.AttackSkill.SkillName);
            attackSkill.transform.Find("txtTitle").GetComponent<Text>().text =item.AttackSkill.Title_en;
            attackSkill.transform.Find("txtInfo").GetComponent<Text>().text = item.AttackSkill.Description_en;
            attackSkill.SetActive(true);
        }
        else
        {
            attackSkill.SetActive(false);
        }

        if (item.DefenseSkill!=null)
        {
            //图标、标题、描述
            defenseSkill.transform.Find("Icon").GetComponent<RawImage>().texture = CommonHelper.LoadSkillImage(item.DefenseSkill.SkillName);
            defenseSkill.transform.Find("txtTitle").GetComponent<Text>().text =item.DefenseSkill.Title_en;
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
        attr.transform.Find("暴击率/txtVal").GetComponent<Text>().text = item.CritRate.ToString()+"%";
        //attr.transform.Find("命中率/txtVal").GetComponent<Text>().text = item.HitRate.ToString()+"%";


        //选择按钮
        UnityEngine.UI.Button btnSelect = infoPanel.transform.Find("BtnSelect").GetComponent<UnityEngine.UI.Button>();
        btnSelect.onClick.RemoveAllListeners();
        
        int index =-1;
        for (int i = 0; i < selects.Length; i++)
        {
            if (selects[i] != null)
            {
                if (selects[i].Code == item.Code)
                {
                    index = i;
                }
            }
        }
        if (index > -1)
        {
            btnSelect.transform.Find("Text").GetComponent<Text>().text = "UnSelect";
            infoPanel.transform.Find("BtnSelect").gameObject.SetActive(true);
            btnSelect.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayBtnAudio();
                for (int i = 0; i < selects.Length; i++)
                {
                    if (selects[i] != null)
                    {
                        if (selects[i].Code == item.Code)
                        {
                            selects[i] = null;
                        }
                    }
                }

                RefreshSelectCard();
                infoPanel.transform.DOScale(new Vector3(0, 0, 0), 0.2f).SetEase(Ease.OutExpo).OnComplete(() =>
                {
                    infoPanel.SetActive(false);
                });
            });
        }
        else
        {
            btnSelect.transform.Find("Text").GetComponent<Text>().text = "Select";


            infoPanel.transform.Find("BtnSelect").gameObject.SetActive(HasEmpty() == false ? false : true);

            btnSelect.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayBtnAudio();
                //ResourceCtrl.Instance.SelectList.Add(item);
                for (int i = 0; i < selects.Length; i++)
                {
                    if (selects[i] == null)
                    {
                        selects[i] = item;
                        break;
                    }
                }

                RefreshSelectCard();
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

    //待选中是否有未选项
    public bool HasEmpty()
    {
        bool res = false;
        for (int i = 0; i < selects.Length; i++)
        {
            if (selects[i] == null)
            {
                res = true;
            }
        }
        return res;
    }


    #region 卡牌拖拽事件
    //需要一个中间物体来显示被拖拽体
    public void OnBeginDrag(PointerEventData eventData)
    {
       
        try
        {
           
            if (drags.Contains(eventData.selectedObject.name))
            {
                Debug.Log("开始拖拽" + eventData.selectedObject.name);
                switch (eventData.selectedObject.transform.parent.name)
                {
                    case "Card1":StartIndex = 0;break;
                    case "Card2": StartIndex = 1; break;
                    case "Card3": StartIndex = 2; break;
                    case "Card4": StartIndex = 3; break;
                    case "Card5": StartIndex = 4; break;
                    case "Card6": StartIndex = 5; break;
                    case "Card7": StartIndex = 6; break;
                }

                dragItem = Root.transform.Find("DragCard").gameObject;
                Instantiate(eventData.selectedObject, dragItem.transform,false);
                dragItem.SetActive(true);
                dragItem.GetComponent<CanvasGroup>().blocksRaycasts = false;

                dragObject = eventData.selectedObject;
                dragObjectPos = eventData.selectedObject.GetComponent<RectTransform>().position;
               
     
            }
        }
        catch (Exception)
        {


        }



    }


    public void OnDrag(PointerEventData eventData)
    {
        try
        {

            if (dragItem != null)
            {
                dragItem.SetActive(true);
                Vector3 pos;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(dragItem.GetComponent<RectTransform>(), eventData.position, eventData.enterEventCamera, out pos);
                dragItem.GetComponent<RectTransform>().position = pos;
            }
        }
        catch (Exception)
        {

        }
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        try
        {
            if (dragItem != null) 
            {
                foreach (UnityEngine.UI.Image item in eventData.pointerEnter.GetComponentsInParent<UnityEngine.UI.Image>())
                {
                    switch (item.gameObject.name)
                    {
                        case "Card1":
                            EndIndex = 0;
                            break;
                        case "Card2":
                            EndIndex = 1;
                            break;
                        case "Card3":
                            EndIndex = 2;
                            break;
                        case "Card4":
                            EndIndex = 3;
                            break;
                        case "Card5":
                            EndIndex = 4;
                            break;
                        case "Card6":
                            EndIndex = 5;
                            break;
                        case "Card7":
                            EndIndex = 6;
                            break;
                    }
                }
                if (EndIndex != -1)
                {
                    TankProperty temp = selects[StartIndex];
                    selects[StartIndex] = selects[EndIndex];
                    selects[EndIndex] = temp;
                    RefreshSelectCard();
                }
                dragItem.SetActive(false);
                    DestroyImmediate(dragItem.transform.GetChild(0).gameObject);
                    dragItem = null;
                    StartIndex = -1;
                    EndIndex = -1;
                    Debug.Log("结束拖拽");
                
            }
            
            
           
        }
        catch (Exception)
        {
        }
    }

    #endregion
}
