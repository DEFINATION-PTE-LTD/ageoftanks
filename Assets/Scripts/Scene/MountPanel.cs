using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MountPanel : MonoBehaviour
{
    public GameObject mountArea;
    /// <summary>
    /// 面板root
    /// </summary>
    public GameObject mountPanel;
    /// <summary>
    /// 完成面板
    /// </summary>
    public GameObject completePanel;
    /// <summary>
    /// 部件属性面板
    /// </summary>
    public GameObject partCard;
    /// <summary>
    /// 部件列表
    /// </summary>
    public GameObject listView;
    /// <summary>
    /// 选中项
    /// </summary>
    public string selectedTab = "btnEngine";

    public AOT_Parts Head;
    public AOT_Parts Body;
    public AOT_Parts Weapon;
    public AOT_Parts Engine;

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
        InitTabs();

        SwitchTab("btnEngine");
        SwitchTabList("Engine");

        mountPanel.transform.Find("btnMount").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
            MountTank mountTank = mountArea.GetComponent<MountTank>();
            mountArea.SetActive(true);
            mountArea.GetComponent<ObjectRot>().AutoRotate = false;
            mountArea.transform.Find("NewTank").localRotation = new Quaternion(0, 0, 0,0);
            if (Engine != null && Body != null && Head != null && Weapon != null)
            {
                HideMessage();
                //承载力判断
                if (Engine.Bearing < Body.Weight + Head.Weight + Weapon.Weight)
                {
                    ShowMessage("Insufficient carrying capacity");
                    return;
                }


                mountTank.BeginMount(Engine, Body, Head, Weapon);
                AOT_SetupRecord record = ResourceCtrl.Instance.GetTankSetupRecord(Engine, Body, Head, Weapon);//获取组装记录
                ResourceCtrl.Instance.MountTanks.Add(record);
               
                StartCoroutine(CommonHelper.DelayToInvokeDo(() =>
                {
                    GetTankTexure(record.Code);
                    //加入坦克列表
                    ResourceCtrl.Instance.InsertToTankList(record);
                    transform.parent.SendMessage("InitCardPool");
                    mountArea.GetComponent<ObjectRot>().AutoRotate = true;
                }, 1f));//获取截图
                ShowTankInfo(record);
                //组装成功后更改状态
                ResourceCtrl.Instance.PartsList.Find(u => u.Code == Engine.Code).Status = 2;
                ResourceCtrl.Instance.PartsList.Find(u => u.Code == Body.Code).Status = 2;
                ResourceCtrl.Instance.PartsList.Find(u => u.Code == Head.Code).Status = 2;
                ResourceCtrl.Instance.PartsList.Find(u => u.Code == Weapon.Code).Status = 2;
                Engine = null;
                Body = null;
                Head = null;
                Weapon = null;
                RefreshSelected();
                mountPanel.SetActive(false);
                completePanel.SetActive(true);

            }
            else 
            {
                ShowMessage("Requires complete four parts to be assembled");
            }
        });
        mountPanel.transform.Find("btnClose").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
            transform.gameObject.SetActive(false);
            mountArea.SetActive(false);
        });

        completePanel.transform.Find("btnContinue").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
            mountArea.SetActive(false);
            mountPanel.SetActive(true);
            completePanel.SetActive(false);
            SwitchTab("btnEngine");
            SwitchTabList("Engine");
        });
        completePanel.transform.Find("btnClose").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
            transform.gameObject.SetActive(false);
            mountPanel.SetActive(true);
            completePanel.SetActive(false);
        });

    }


    /// <summary>
    /// 初始化tab选项
    /// </summary>
    public void InitTabs()
    {
        Transform btnContainer = mountPanel.transform.Find("btnContainer");
        UnityEngine.UI.Button btnEngine = btnContainer.Find("btnEngine").gameObject.AddComponent<UnityEngine.UI.Button>();
        UnityEngine.UI.Button btnBody = btnContainer.Find("btnBody").gameObject.AddComponent<UnityEngine.UI.Button>();
        UnityEngine.UI.Button btnNose = btnContainer.Find("btnNose").gameObject.AddComponent<UnityEngine.UI.Button>();
        UnityEngine.UI.Button btnArms = btnContainer.Find("btnArms").gameObject.AddComponent<UnityEngine.UI.Button>();


        btnEngine.onClick.AddListener(() => {
            SwitchTab(btnEngine.name);
            SwitchTabList("Engine");
        });
        btnBody.onClick.AddListener(() => {
            SwitchTab(btnBody.name);
            SwitchTabList("Body");
        });
        btnNose.onClick.AddListener(() => {
            SwitchTab(btnNose.name);
            SwitchTabList("Head");
        });
        btnArms.onClick.AddListener(() => {
            SwitchTab(btnArms.name);
            SwitchTabList("Weapon");
        });

    }
    
    /// <summary>
    /// tab切换
    /// </summary>
    /// <param name="tabName"></param>
    public void SwitchTab(string tabName)
    {
        Transform btnContainer = mountPanel.transform.Find("btnContainer");
        int childCount = btnContainer.childCount;
        selectedTab = tabName;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = btnContainer.GetChild(i);
            if (child != null)
            {
                child.Find("selected").gameObject.SetActive(child.name == tabName);
            }
           
        }
    }
    /// <summary>
    /// 切换数据
    /// </summary>
    /// <param name="part"></param>
    public void SwitchTabList(string part)
    {
        List<AOT_Parts> list = ResourceCtrl.Instance.PartsList.FindAll(u => u.Part == part && u.Status==1);
        Transform content = listView.transform.Find("Viewport/Content");
        int childCount= content.childCount;
        for (int i = childCount-1; i >=0; i--)
        {
            Transform child = content.GetChild(i);
            if (child != null)
            {
                DestroyImmediate(child.gameObject);
            }

        }
        
        foreach (AOT_Parts item in list)
        {
            GameObject newCard = BindItem(part, item);
            newCard.transform.SetParent(content.transform, false);
        }
    }

    /// <summary>
    /// 部件项数据绑定
    /// </summary>
    /// <param name="part"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public GameObject BindItem(string part, AOT_Parts item)
    {
        GameObject newCard = Instantiate(partCard);
        newCard.name = item.Code;
        newCard.SetActive(true);
        
        newCard.transform.Find("partCover/image").GetComponent<RawImage>().texture = ResourceCtrl.Instance.PartsSprite[item.Code].texture ;
        newCard.transform.Find("partCover/iconLvl" + item.Level).gameObject.SetActive(true);
        GameObject iconSkill = newCard.transform.Find("partCover/iconSkill").gameObject;
        GameObject checkStatus = newCard.transform.Find("checkStatus").gameObject;
        checkStatus.GetComponent<Toggle>().onValueChanged.AddListener((bool isOn) => {
            SelectItem(part, item, isOn);
        });
        switch (part.ToLower())
        {
            case "engine":
                Transform engineProps = newCard.transform.Find("engineProps");
                engineProps.gameObject.SetActive(true);
                engineProps.Find("speed/txtVal").GetComponent<Text>().text = Convert.ToInt32(item.Speed).ToString();
                engineProps.Find("bearing/txtVal").GetComponent<Text>().text = Convert.ToInt32(item.Bearing).ToString();
                engineProps.Find("weight/txtVal").GetComponent<Text>().text = Convert.ToInt32(item.Weight).ToString();
                if (Engine != null)
                {
                    if (item.Code == Engine.Code)
                    {
                        checkStatus.GetComponent<Toggle>().isOn = true;
                    }
                }
                break;
            case "head":
                Transform noseProps = newCard.transform.Find("noseProps");
                noseProps.gameObject.SetActive(true);
                noseProps.Find("range/txtVal").GetComponent<Text>().text = Convert.ToInt32(item.Range).ToString();
                noseProps.Find("crit/txtVal").GetComponent<Text>().text = Convert.ToInt32(item.Crit).ToString() + "%";
                noseProps.Find("weight/txtVal").GetComponent<Text>().text = Convert.ToInt32(item.Weight).ToString();
                if (Head != null)
                {
                    if (item.Code == Head.Code)
                    {
                        checkStatus.GetComponent<Toggle>().isOn = true;
                    }
                }
                break;
            case "body":
                Transform bodyProps = newCard.transform.Find("bodyProps");
                bodyProps.gameObject.SetActive(true);
                bodyProps.Find("hp/txtVal").GetComponent<Text>().text = Convert.ToInt32(item.Blood).ToString();
                bodyProps.Find("weight/txtVal").GetComponent<Text>().text = Convert.ToInt32(item.Weight).ToString();
                if (!string.IsNullOrEmpty(item.DefenseSkillCode))
                {
                    iconSkill.gameObject.SetActive(true);
                    iconSkill.GetComponent<RawImage>().texture =  CommonHelper.LoadSkillImage(ResourceCtrl.Instance.SkillList.Find(u=>u.Code==item.DefenseSkillCode).SkillName) ;
                }
                if (Body != null)
                {
                    if (item.Code == Body.Code)
                    {
                        checkStatus.GetComponent<Toggle>().isOn = true;
                    }
                }
                break;
            case "weapon":
                Transform weaponProps = newCard.transform.Find("weaponProps");
                weaponProps.gameObject.SetActive(true);
                weaponProps.Find("atk/txtVal").GetComponent<Text>().text = Convert.ToInt32(item.Attack).ToString();
                weaponProps.Find("weight/txtVal").GetComponent<Text>().text = Convert.ToInt32(item.Weight).ToString();
                if (!string.IsNullOrEmpty(item.AttackSkillCode))
                {
                    iconSkill.gameObject.SetActive(true);
                    iconSkill.GetComponent<RawImage>().texture = CommonHelper.LoadSkillImage(ResourceCtrl.Instance.SkillList.Find(u => u.Code == item.AttackSkillCode).SkillName);
                }
                if (Weapon != null)
                {
                    if (item.Code == Weapon.Code)
                    {
                        checkStatus.GetComponent<Toggle>().isOn = true;
                    }
                }
                break;
        }
        return newCard;
    }

    /// <summary>
    /// 选中数据
    /// </summary>
    /// <param name="part"></param>
    /// <param name="item"></param>
    /// <param name="isOn"></param>
    public void SelectItem(string part, AOT_Parts item,bool isOn)
    {
        switch (part.ToLower())
        {
            case "engine":
                if (Engine != null) 
                {
                    if (listView.transform.Find("Viewport/Content/" + Engine.Code) != null)
                    {
                        listView.transform.Find("Viewport/Content/" + Engine.Code + "/checkStatus").GetComponent<Toggle>().isOn = false;
                    }
                }
                Engine = (isOn==true?item:null);
                break;
            case "head":
                if (Head != null)
                {
                    if (listView.transform.Find("Viewport/Content/" + Head.Code) != null)
                    {
                        listView.transform.Find("Viewport/Content/" + Head.Code + "/checkStatus").GetComponent<Toggle>().isOn = false;
                    }
                }
                Head = (isOn == true ? item : null);
                break;
            case "body":
                if (Body != null)
                {
                    if (listView.transform.Find("Viewport/Content/" + Body.Code) != null)
                    {
                        listView.transform.Find("Viewport/Content/" + Body.Code + "/checkStatus").GetComponent<Toggle>().isOn = false;
                    }
                }
                Body = (isOn == true ? item : null);
                break;
            case "weapon":
                if (Weapon != null)
                {
                    if (listView.transform.Find("Viewport/Content/" + Weapon.Code) != null)
                    {
                        listView.transform.Find("Viewport/Content/" + Weapon.Code + "/checkStatus").GetComponent<Toggle>().isOn = false;
                    }
                }
                Weapon = (isOn == true ? item : null);
                break;
        }

        RefreshSelected();
    }

    /// <summary>
    /// 刷新显示已选项
    /// </summary>
    public void RefreshSelected()
    {
        if (Head != null)
        {
            mountPanel.transform.Find("mountSelected/imgHead").GetComponent<RawImage>().texture = ResourceCtrl.Instance.PartsSprite[Head.Code].texture ;
            mountPanel.transform.Find("mountSelected/imgHead").gameObject.SetActive(true);
        }
        else 
        {
            mountPanel.transform.Find("mountSelected/imgHead").gameObject.SetActive(false);
        }

        if (Body != null)
        {
            mountPanel.transform.Find("mountSelected/imgBody").GetComponent<RawImage>().texture = ResourceCtrl.Instance.PartsSprite[Body.Code].texture;
            mountPanel.transform.Find("mountSelected/imgBody").gameObject.SetActive(true);
        }
        else
        {
            mountPanel.transform.Find("mountSelected/imgBody").gameObject.SetActive(false);
        }

        if (Weapon != null)
        {
            mountPanel.transform.Find("mountSelected/imgWeapon").GetComponent<RawImage>().texture = ResourceCtrl.Instance.PartsSprite[Weapon.Code].texture;
            mountPanel.transform.Find("mountSelected/imgWeapon").gameObject.SetActive(true);
        }
        else
        {
            mountPanel.transform.Find("mountSelected/imgWeapon").gameObject.SetActive(false);
        }

        if (Engine != null)
        {
            mountPanel.transform.Find("mountSelected/imgEngine").GetComponent<RawImage>().texture = ResourceCtrl.Instance.PartsSprite[Engine.Code].texture;
            mountPanel.transform.Find("mountSelected/imgEngine").gameObject.SetActive(true);
        }
        else
        {
            mountPanel.transform.Find("mountSelected/imgEngine").gameObject.SetActive(false);
        }
    }

    //获得坦克截图
    public void GetTankTexure(string Code)
    {
        Texture2D texture = CommonHelper.TextureToTexture2D(completePanel.transform.Find("tankImg").GetComponent<RawImage>().texture);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
               new Vector2(0.5f, 0.5f));
        if (ResourceCtrl.Instance.MountTanksSprite.ContainsKey(Code))
        {
            ResourceCtrl.Instance.MountTanksSprite[Code] = sprite;
        }
        else
        {
            ResourceCtrl.Instance.MountTanksSprite.Add(Code, sprite);
        }
        Debug.Log(Code+"图片已成功");
    }

    /// <summary>
    /// 显示坦克基本信息
    /// </summary>
    /// <param name="record"></param>
    public void ShowTankInfo(AOT_SetupRecord item)
    {
        //编号
        completePanel.transform.Find("txtNo").GetComponent<Text>().text = "#" + item.Code;
        completePanel.transform.Find("txtLevel").GetComponent<Text>().text = ResourceCtrl.Instance.Levels.Find(u => u.Level == item.Level).Title_en;
        //坦克图
        //completePanel.transform.Find("tankImg").GetComponent<RawImage>().texture = CommonHelper.LoadTankImage(item.Code);


        //技能
        GameObject attackSkill = completePanel.transform.Find("AttackSkill").gameObject;
        GameObject defenseSkill = completePanel.transform.Find("DefenseSkill").gameObject;

        AOT_SkillInfo AttackSkillInfo = null, DefenseSkillInfo = null;
        if (!string.IsNullOrEmpty(item.AttackSkillCode))
        {
            AttackSkillInfo = ResourceCtrl.Instance.SkillList.Find(u => u.Code == item.AttackSkillCode);
        }
        else {
            attackSkill.SetActive(false);
        }

        if (!string.IsNullOrEmpty(item.DefenseSkillCode))
        {
            DefenseSkillInfo = ResourceCtrl.Instance.SkillList.Find(u => u.Code == item.DefenseSkillCode);
        }
        else
        {
            defenseSkill.SetActive(false);
        }


        if (AttackSkillInfo != null)
        {
            //图标、标题、描述
            attackSkill.transform.Find("Icon").GetComponent<RawImage>().texture = CommonHelper.LoadSkillImage(AttackSkillInfo.SkillName);
            attackSkill.transform.Find("txtTitle").GetComponent<Text>().text = AttackSkillInfo.Title_en;
            attackSkill.transform.Find("txtInfo").GetComponent<Text>().text = AttackSkillInfo.Description_en;
            attackSkill.SetActive(true);
        }
        else
        {
            attackSkill.SetActive(false);
        }

        if (DefenseSkillInfo != null)
        {
            //图标、标题、描述
            defenseSkill.transform.Find("Icon").GetComponent<RawImage>().texture = CommonHelper.LoadSkillImage(DefenseSkillInfo.SkillName);
            defenseSkill.transform.Find("txtTitle").GetComponent<Text>().text = DefenseSkillInfo.Title_en;
            defenseSkill.transform.Find("txtInfo").GetComponent<Text>().text = DefenseSkillInfo.Description_en;
            defenseSkill.SetActive(true);
        }
        else
        {
            defenseSkill.SetActive(false);
        }
        completePanel.SetActive(true);
        completePanel.transform.DOScale(new Vector3(1, 1, 1), 0.2f).From(new Vector3(0, 0, 0));

        //属性
        GameObject attr = completePanel.transform.Find("Attributes").gameObject;

        attr.transform.Find("iconLvl1").gameObject.SetActive(false);
        attr.transform.Find("iconLvl2").gameObject.SetActive(false);
        attr.transform.Find("iconLvl3").gameObject.SetActive(false);
        attr.transform.Find("iconLvl4").gameObject.SetActive(false);
        attr.transform.Find("iconLvl5").gameObject.SetActive(false);

        attr.transform.Find("iconLvl" + item.Level).gameObject.SetActive(true);
        attr.transform.Find("血量/txtVal").GetComponent<Text>().text = Convert.ToInt32(item.Blood).ToString();
        attr.transform.Find("攻击力/txtVal").GetComponent<Text>().text = Convert.ToInt32(item.Attack).ToString();
        //attr.transform.Find("防御值/txtVal").GetComponent<Text>().text = item.Defense.ToString();
        attr.transform.Find("射程/txtVal").GetComponent<Text>().text = Convert.ToInt32(item.Range).ToString();
        attr.transform.Find("速度/txtVal").GetComponent<Text>().text = Convert.ToInt32(item.Speed).ToString();
        attr.transform.Find("暴击率/txtVal").GetComponent<Text>().text = Convert.ToInt32(item.Crit).ToString() + "%";
        //attr.transform.Find("命中率/txtVal").GetComponent<Text>().text = item.HitRate.ToString()+"%";
    }

    /// <summary>
    /// 显示提示信息
    /// </summary>
    /// <param name="msg"></param>
    public void ShowMessage(string message)
    {
        Text msg = mountPanel.transform.Find("txtMsg").GetComponent<Text>();
        msg.gameObject.SetActive(true);
        msg.transform.DOShakePosition(1f);
        msg.text = message;
    }
    public void HideMessage()
    {
        mountPanel.transform.Find("txtMsg").gameObject.SetActive(false);
    }
}
