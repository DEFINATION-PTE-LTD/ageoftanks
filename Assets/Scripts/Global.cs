using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Global : MonoBehaviour
{
    
    public GameObject Root;

   


    private void Awake()
    {

        InitTankList();
        InitCardPool();
        Root.transform.Find("MainPanel/RightPanel/BtnPlay").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
            if (ResourceCtrl.Instance.SelectList.Count < 7)
            {
                Root.transform.Find("MainPanel/RightPanel/txtTip").gameObject.SetActive(true);
                Root.transform.Find("MainPanel/RightPanel/txtTip").DOShakePosition(1f, new Vector3(4, 0, 0));

            }
            else
            {
                Root.transform.Find("MainPanel/RightPanel/txtTip").gameObject.SetActive(false);
                SceneManager.LoadScene("FightScene2");
            }
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

    //初始化坦克列表
    void InitTankList()
    {
        if (ResourceCtrl.Instance.TankList != null)
        {
            ResourceCtrl.Instance.TankList.Clear();
        }
        // TankList.Add();
        for (int i = 0; i < 12; i++)
        {
            TankProperty tank = new TankProperty((12 + i).ToString().PadLeft(5, '0'), "蝎式坦克", null);
            tank.AttackSkill = ResourceCtrl.Instance.SkillList.Find(u => u.Name == "Batter");// ResourceCtrl.Instance.SkillList.FindAll(u=>u.Type=="Attack")[i];
            tank.DefenseSkill = i < 8 ? ResourceCtrl.Instance.SkillList.FindAll(u => u.Type == "Defense")[CommonHelper.GetRandom(0, 8)] : null;
            ResourceCtrl.Instance.TankList.Add(tank);
        }
        //默认选中前6个
        ResourceCtrl.Instance.SelectList = ResourceCtrl.Instance.TankList.GetRange(0, 6);

        InitSelectCard();
    }

    /// <summary>
    /// 卡牌池初始化
    /// </summary>
    void InitCardPool()
    {
        //坦克卡牌
        GameObject tankCard = ResourceCtrl.Instance.ResourceRoot.transform.Find("UI/TankCard").gameObject;

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
            newCard.transform.Find("Panel/image").GetComponent<RawImage>().texture = CommonHelper.LoadTankImage(item.Code);
            newCard.transform.Find("Panel/txtNo").GetComponent<Text>().text = "#" + item.Code;

            newCard.transform.Find("Panel/iconAttack/txtVal").GetComponent<Text>().text = item.Attack.ToString();
            newCard.transform.Find("Panel/iconDefense/txtVal").GetComponent<Text>().text = item.Blood.ToString();

           
            string skill = "";
            if (item.AttackSkill!=null)
            {
                skill="ATK："+item.AttackSkill.Description_en + "\n";
            }

            if (item.DefenseSkill!=null)
            {
                skill += "DEF：" +item.DefenseSkill.Description_en;
            }
            newCard.transform.Find("Panel/txtSkill").GetComponent<Text>().text = skill;

            newCard.gameObject.AddComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
                Debug.Log("当前点击了" + newCard.name);
                showDetail(item);
            });
        }
    }

    /// <summary>
    /// 初始化已选择卡牌
    /// </summary>
    void InitSelectCard()
    {
        //SelectList
        //坦克卡牌
        GameObject tankCard = ResourceCtrl.Instance.ResourceRoot.transform.Find("UI/TankCard").gameObject;
        //空牌
        GameObject emptyCard = ResourceCtrl.Instance.ResourceRoot.transform.Find("UI/EmptyCard").gameObject;

        //卡牌池
        GameObject selectPool = Root.transform.Find("MainPanel/RightPanel/SelectPool/Viewport/Content").gameObject;

        //销毁子物体
        for (int i = selectPool.transform.childCount-1; i >=0; i--)
        {
            Destroy(selectPool.transform.GetChild(i).gameObject);
        } 
        //添加子物体
        foreach (TankProperty item in ResourceCtrl.Instance.SelectList)
        {
          
                GameObject newCard = Instantiate(tankCard, selectPool.transform);
                newCard.SetActive(true);
                newCard.name = "TankCard" + item.Code;
                newCard.transform.Find("Panel/image").GetComponent<RawImage>().texture = CommonHelper.LoadTankImage(item.Code);
                newCard.transform.Find("Panel/txtNo").GetComponent<Text>().text = "#" + item.Code;

                newCard.transform.Find("Panel/iconAttack/txtVal").GetComponent<Text>().text = item.Attack.ToString();
                newCard.transform.Find("Panel/iconDefense/txtVal").GetComponent<Text>().text = item.Blood.ToString();


                string skill = "";
            if (item.AttackSkill!=null)
            {
                skill = "ATK：" + item.AttackSkill.Description_en + "\n";
            }

            if (item.DefenseSkill!=null)
            {
                skill += "DEF：" + item.DefenseSkill.Description_en;
            }
            newCard.transform.Find("Panel/txtSkill").GetComponent<Text>().text = skill;

                newCard.gameObject.AddComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                {
                    Debug.Log("当前点击了" + newCard.name);
                    showDetail(item);
                });
            
        }
        //emptyCard.transform.SetAsLastSibling();
        if (ResourceCtrl.Instance.SelectList.Count < 7)
        {
            Instantiate(emptyCard, selectPool.transform).SetActive(true);
        }
    }



    //显示详情
    void showDetail(TankProperty item) 
    {
        GameObject infoPanel =   Root.transform.Find("DetailPanel").gameObject;

        //编号
        infoPanel.transform.Find("txtNo").GetComponent<Text>().text = "#" + item.Code;
        //坦克图
        infoPanel.transform.Find("image").GetComponent<RawImage>().texture = CommonHelper.LoadTankImage(item.Code);
        //技能
        GameObject attackSkill = infoPanel.transform.Find("AttackSkill").gameObject;
        GameObject defenseSkill = infoPanel.transform.Find("DefenseSkill").gameObject;
        if (item.AttackSkill!=null)
        {
            //图标、标题、描述
            attackSkill.transform.Find("Icon").GetComponent<RawImage>().texture = CommonHelper.LoadSkillImage(item.AttackSkill.Name);
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
            defenseSkill.transform.Find("Icon").GetComponent<RawImage>().texture = CommonHelper.LoadSkillImage(item.DefenseSkill.Name);
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
        if (ResourceCtrl.Instance.SelectList.Contains(item))
        {
            btnSelect.transform.Find("Text").GetComponent<Text>().text = "UnSelect";
            infoPanel.transform.Find("BtnSelect").gameObject.SetActive(true);
            btnSelect.onClick.AddListener(() => {
                ResourceCtrl.Instance.SelectList.Remove(item);
                InitSelectCard();
                infoPanel.transform.DOScale(new Vector3(0, 0, 0), 0.2f).SetEase(Ease.OutExpo).OnComplete(() =>
                {
                    infoPanel.SetActive(false);
                });
            });
        }
        else 
        {
            btnSelect.transform.Find("Text").GetComponent<Text>().text = "Select";

            infoPanel.transform.Find("BtnSelect").gameObject.SetActive(ResourceCtrl.Instance.SelectList.Count>=7?false:true);

            btnSelect.onClick.AddListener(() => {
                ResourceCtrl.Instance.SelectList.Add(item);
                InitSelectCard();
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

            infoPanel.transform.DOScale(new Vector3(0, 0, 0), 0.2f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                infoPanel.SetActive(false);
            });
        });
    }

   
}
