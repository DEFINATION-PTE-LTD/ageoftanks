using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 资源场景控制
/// </summary>
public class ResourceCtrl : MonoBehaviour
{

    public GameObject Tank;

    public AOT_User UserInfo;
    public GameObject ResourceRoot;
    public List<TankProperty> TankList = new List<TankProperty>();
    public List<TankProperty> SelectList = new List<TankProperty>();
    public List<TankProperty> SelectListB = new List<TankProperty>();

    public List<AOT_SkillInfo> SkillList = new List<AOT_SkillInfo>();//技能信息
    public List<AOT_SkinInfo> SkinList = new List<AOT_SkinInfo>();//皮肤信息
    public List<AOT_Models> ModelList = new List<AOT_Models>();//模型信息
    public List<AOT_Parts> PartsList = new List<AOT_Parts>(); //坦克部件信息
    public List<AOT_Tanks> BaseTanks = new List<AOT_Tanks>(); //坦克信息
    public List<AOT_SetupRecord> MountTanks = new List<AOT_SetupRecord>();//组装坦克信息

    public Dictionary<string, Sprite> PartsSprite = new Dictionary<string, Sprite>();//部件图片
    public Dictionary<string, Sprite> MountTanksSprite = new Dictionary<string, Sprite>();//组装坦克图片

    public List<LevelInfo> Levels = new List<LevelInfo>();//等级名称


    private static ResourceCtrl instance = null;
    public static ResourceCtrl Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Tank.GetComponent<Animator>().Play("Walk");

        ResourceRoot = gameObject;
        GetUserInfo();
        GetLevelInfo();
        

        System.GC.Collect();
        StartCoroutine(CommonHelper.DelayToInvokeDo(() => {
           SceneManager.LoadScene("BattleMode");
        }, 5f));
        
    }


    /// <summary>
    /// 获取用户信息
    /// </summary>
    void GetUserInfo()
    {
        if (PlayerPrefs.HasKey("userinfo"))
        {
            string str = PlayerPrefs.GetString("userinfo");
            UserInfo = JSONhelper.ConvertToObject<AOT_User>(str);

            GetSkillInfo();
            GetSkinInfo();
            GetModelInfo();
            GetParts();
            GetTanks();
        }
        else
        {
            Debug.Log("用户信息获取失败");
        }
    }
    /// <summary>
    /// 技能基础信息初始化
    /// </summary>
    void GetSkillInfo()
    {
        List<SkillInfo> list = new List<SkillInfo>();

        #region 14个攻击技能
        //list.Add(new SkillInfo
        //{
        //    Name = "Batter",
        //    Type = "Attack",
        //    Icon = "Resource/Texture/skill/Batter",
        //    Title = "连击",
        //    Description = "连续发起3次攻击",
        //    Title_en = "Batter",
        //    Description_en = "Launch 3 consecutive attacks"
        //});
        //list.Add(new SkillInfo
        //{
        //    Name = "Thump",
        //    Type = "Attack",
        //    Icon = "Resource/Texture/skill/Thump",
        //    Title = "重击",
        //    Description = "对敌方造成2倍攻击伤害",
        //    Title_en = "Thump",
        //    Description_en = "Deal 2 times attack damage to the enemy"
        //});
        //list.Add(new SkillInfo
        //{
        //    Name = "Piercing",
        //    Type = "Attack",
        //    Icon = "Resource/Texture/skill/Piercing",
        //    Title = "穿甲",
        //    Description = "被技能击中后的目标，再次受到伤害会附加额外50%的伤害",
        //    Title_en = "Piercing",
        //    Description_en = "After being hit by the skill, if it is damaged again, an additional 50% damage will be added"
        //});
        //list.Add(new SkillInfo
        //{
        //    Name = "Combustion",
        //    Type = "Attack",
        //    Icon = "Resource/Texture/skill/Combustion",
        //    Title = "燃烧",
        //    Description = "每回合造成最大生命值10%的伤害",
        //    Title_en = "Combustion",
        //    Description_en = "Deal 10% damage of maximum health per round"
        //});
        //list.Add(new SkillInfo
        //{
        //    Name = "Scattering",
        //    Type = "Attack",
        //    Icon = "Resource/Texture/skill/Scattering",
        //    Title = "散射",
        //    Description = "同时对3个目标造成普通攻击伤害",
        //    Title_en = "Scattering",
        //    Description_en = "Deal normal attack damage to 3 targets at the same time"
        //});
        //list.Add(new SkillInfo
        //{
        //    Name = "Kill",
        //    Type = "Attack",
        //    Icon = "Resource/Texture/skill/Kill",
        //    Title = "必杀",
        //    Description = "25%概率一击必杀目标",
        //    Title_en = "Kill",
        //    Description_en = "25% chance to kill the target with one hit"
        //});
        //list.Add(new SkillInfo
        //{
        //    Name = "Frozen",
        //    Type = "Attack",
        //    Icon = "Resource/Texture/skill/Frozen",
        //    Title = "冰冻",
        //    Description = "目标一个回合内无法行动",
        //    Title_en = "Frozen",
        //    Description_en = "The target cannot act within a round"
        //});
        //list.Add(new SkillInfo
        //{
        //    Name = "SuckBlood",
        //    Type = "Attack",
        //    Icon = "Resource/Texture/skill/SuckBlood",
        //    Title = "吸血",
        //    Description = "对目标造成伤害并回复自身40%的血量",
        //    Title_en = "Suck blood",
        //    Description_en = "Inflict damage to the target and restore 40% of its own HP"
        //});
        //list.Add(new SkillInfo
        //{
        //    Name = "Forbidden",
        //    Type = "Attack",
        //    Icon = "Resource/Texture/skill/Forbidden",
        //    Title = "禁魔",
        //    Description = "被命中并造成伤害后的目标无法使用攻击及防御特效",
        //    Title_en = "Forbidden",
        //    Description_en = "After being hit and causing damage, the target cannot use attack and defense special effects"
        //});
        //list.Add(new SkillInfo
        //{
        //    Name = "Cofire",
        //    Type = "Attack",
        //    Icon = "Resource/Texture/skill/Cofire",
        //    Title = "协同开火",
        //    Description = "攻击后，3个我方坦克也进行攻击一次",
        //    Title_en = "Co-fire",
        //    Description_en = "After the attack, 3 of our tanks also attacked once"
        //});
        //list.Add(new SkillInfo
        //{
        //    Name = "EmpireDirective",
        //    Type = "Attack",
        //    Icon = "Resource/Texture/skill/EmpireDirective",
        //    Title = "帝国指令",
        //    Description = "命中对方坦克后，我方其他坦克只攻击敌方同一个坦克，直至对方摧毁",
        //    Title_en = "Empire directive",
        //    Description_en = "After hitting the opponent’s tank, our other tanks will only attack the same enemy tank until the opponent destroys"
        //});
        //list.Add(new SkillInfo
        //{
        //    Name = "Duel",
        //    Type = "Attack",
        //    Icon = "Resource/Texture/skill/Duel",
        //    Title = "决斗",
        //    Description = "被攻击的坦克只能攻击我方坦克，直到一方被摧毁",
        //    Title_en = "Duel",
        //    Description_en = "The attacked tank can only attack our tank until one is destroyed"
        //});
        //list.Add(new SkillInfo
        //{
        //    Name = "Destroyweak",
        //    Type = "Attack",
        //    Icon = "Resource/Texture/skill/Destroyweak",
        //    Title = "逐个击破",
        //    Description = "优先攻击对方攻击力最弱的坦克（无视嘲讽）",
        //    Title_en = "Destroy one by one",
        //    Description_en = "Prioritize the attack on the tank with the weakest attack power (ignore the taunt)"
        //});
        //list.Add(new SkillInfo
        //{
        //    Name = "Invincible",
        //    Type = "Attack",
        //    Icon = "Resource/Texture/skill/Invincible",
        //    Title = "攻无不克",
        //    Description = "优先攻击对方攻击力最强的坦克（无视嘲讽）",
        //    Title_en = "Invincible",
        //    Description_en = "Give priority to attack the opponent's most powerful tank (ignore the taunt)"
        //});
        #endregion

        #region 9个防御技能
        //list.Add(new SkillInfo
        //{
        //    Name = "HolyShield",
        //    Type = "Defense",
        //    Icon = "Resource/Texture/skill/HolyShield",
        //    Title = "圣盾",
        //    Description = "额外抵挡4次攻击",
        //    Title_en = "Holy shield",
        //    Description_en = "Resist 4 additional attacks"
        //});
        //list.Add(new SkillInfo
        //{
        //    Name = "Taunt",
        //    Type = "Defense",
        //    Icon = "Resource/Texture/skill/Taunt",
        //    Title = "嘲讽",
        //    Description = "使坦克必须优先攻击具有嘲讽技能的目标",
        //    Title_en = "Taunt",
        //    Description_en = "Make tanks have priority to attack targets with taunting skills"
        //});
        //list.Add(new SkillInfo
        //{
        //    Name = "HeavyArmor",
        //    Type = "Defense",
        //    Icon = "Resource/Texture/skill/HeavyArmor",
        //    Title = "重甲",
        //    Description = "受到的伤害百分比减少70%",
        //    Title_en = "Heavy armor",
        //    Description_en = "The percentage of damage taken is reduced by 70%"
        //});
        //list.Add(new SkillInfo
        //{
        //    Name = "Dodge",
        //    Type = "Defense",
        //    Icon = "Resource/Texture/skill/Dodge",
        //    Title = "闪避",
        //    Description = "40%几率闪避一次攻击",
        //    Title_en = "Dodge",
        //    Description_en = "40% chance to dodge an attack"
        //});
        //list.Add(new SkillInfo
        //{
        //    Name = "Revive",
        //    Type = "Defense",
        //    Icon = "Resource/Texture/skill/Revive",
        //    Title = "复生",
        //    Description = "死亡后可复活一次",
        //    Title_en = "Revive",
        //    Description_en = "Can be resurrected once after death"
        //});
        //list.Add(new SkillInfo
        //{
        //    Name = "AntiInjury",
        //    Type = "Defense",
        //    Icon = "Resource/Texture/skill/AntiInjury",
        //    Title = "反伤",
        //    Description = "对命中自身的敌方造成自身受到攻击伤害的50%伤害",
        //    Title_en = "Anti-injury",
        //    Description_en = "Inflict 50% of the damage from the attack on the enemy that hits you"
        //});
        //list.Add(new SkillInfo
        //{
        //    Name = "Recover",
        //    Type = "Defense",
        //    Icon = "Resource/Texture/skill/Recover",
        //    Title = "恢复",
        //    Description = "每回合回复自身血量10%",
        //    Title_en = "Recover",
        //    Description_en = "Recover 10% of its own HP every round"
        //});
        ////list.Add(new SkillInfo { Name = "DeathSound", Type = "Defense", Icon= "Resource/Texture/skill/DeathSound",
        ////    Title = "亡语", Description = "死亡后对我方增加攻击、血量、速度、防御；对敌方造成伤害、降低速度、降低防御等", 
        ////    Title_en = "Death sound", Description_en = "After death, increase attack, blood volume, speed, defense to our side; cause damage to the enemy, reduce speed, reduce defense, etc." });
        //list.Add(new SkillInfo
        //{
        //    Name = "Revenge",
        //    Type = "Defense",
        //    Icon = "Resource/Texture/skill/Revenge",
        //    Title = "还击",
        //    Description = "受到敌方伤害后，会攻击伤害来源的对方坦克",
        //    Title_en = "Revenge",
        //    Description_en = "After receiving damage from the enemy, it will attack the opponent's tank from the source of the damage"
        //});
        #endregion
        
        if (UserInfo != null)
        {
            string jsonstr = JSONhelper.ToJson(new { UserID = UserInfo.UUID });
           
            HttpTool.Instance.Post("aotuser/skillinfo", jsonstr, (string result) =>
            {
                APIResult res = JSONhelper.ToApiResult<AOT_SkillInfo>(result);// JSONhelper.ConvertToObject<APIResult>(result);

                if (res.success == true)
                {
                    SkillList = (List<AOT_SkillInfo>)(res.data);
                    Debug.Log("技能信息获取完毕");
                    InitTankList();
                }
                else
                {
                    Debug.Log(res.message);
                }
            });
        }

    }

    //----获取基础信息----

    
    //获取等级信息
    void GetLevelInfo()
    {
        if (UserInfo != null)
        {
            string jsonstr = JSONhelper.ToJson(new { UserID = UserInfo.UUID });
            HttpTool.Instance.Get("aotuser/levels", (string result) =>
            {
                APIResult res = JSONhelper.ToApiResult<LevelInfo>(result);// JSONhelper.ConvertToObject<APIResult>(result);

                if (res.success == true)
                {
                    Levels = (List<LevelInfo>)(res.data);
                    Debug.Log("等级信息获取完毕");
                }
                else
                {
                    Debug.Log(res.message);
                }
            });
        }

    }
    //获取皮肤信息
    void GetSkinInfo()
    {
        if (UserInfo != null)
        {
            string jsonstr = JSONhelper.ToJson(new { UserID = UserInfo.UUID });
            HttpTool.Instance.Post("aotuser/skininfo", jsonstr, (string result) =>
            {
                APIResult res = JSONhelper.ToApiResult<AOT_SkinInfo>(result);// JSONhelper.ConvertToObject<APIResult>(result);

                if (res.success == true)
                {
                    SkinList = (List<AOT_SkinInfo>)(res.data);
                    Debug.Log("皮肤信息获取完毕");
                }
                else
                {
                    Debug.Log(res.message);
                }
            });
        }

    }

    //获取模型信息
    void GetModelInfo()
    {
        if (UserInfo != null)
        {
            string jsonstr = JSONhelper.ToJson(new { UserID = UserInfo.UUID });
            HttpTool.Instance.Post("aotuser/model", jsonstr, (string result) =>
            {
                APIResult res = JSONhelper.ToApiResult<AOT_Models>(result);// JSONhelper.ConvertToObject<APIResult>(result);

                if (res.success == true)
                {
                    ModelList = (List<AOT_Models>)(res.data);
                    Debug.Log("模型信息获取完毕");
                }
                else
                {
                    Debug.Log(res.message);
                }
            });
        }

    }

    //获取坦克部件信息
    void GetParts()
    {
        if (UserInfo != null)
        {
            string jsonstr = JSONhelper.ToJson(new { UserID = UserInfo.UUID });
            HttpTool.Instance.Post("aotuser/parts", jsonstr, (string result) =>
            {
                APIResult res = JSONhelper.ToApiResult<AOT_Parts>(result);// JSONhelper.ConvertToObject<APIResult>(result);

                if (res.success == true)
                {
                    PartsList = (List<AOT_Parts>)(res.data);
                    Debug.Log("部件信息获取完毕");
                    LoadPartTexture();
                }
                else
                {
                    Debug.Log(res.message);
                }
            });
        }
    }

    //加载远程图片
    void LoadPartTexture()
    {
        if (PartsList.Count > 0)
        {
            foreach (AOT_Parts item in PartsList)
            {
                StartCoroutine(HttpTool.Instance.LoadRemoteImg(item.Cover, 200, 200, (Sprite sp) => {
                    PartsSprite.Add(item.Code, sp);
                    Debug.Log(item.Code + "图片加载完成");
                }));
            }
        }
        
    }


    //获取坦克信息
    void GetTanks()
    {
        if (UserInfo != null)
        {
            string jsonstr = JSONhelper.ToJson(new { UserID = UserInfo.UUID });
            HttpTool.Instance.Post("aotuser/tanks", jsonstr, (string result) =>
            {
                APIResult res = JSONhelper.ToApiResult<AOT_Tanks>(result);// JSONhelper.ConvertToObject<APIResult>(result);

                if (res.success == true)
                {
                    BaseTanks = (List<AOT_Tanks>)(res.data);
                    Debug.Log("坦克信息获取完毕");
                }
                else
                {
                    Debug.Log(res.message);
                }
            });
        }
    }


    //初始化坦克列表
    void InitTankList()
    {
        if (ResourceCtrl.Instance.TankList != null)
        {
            ResourceCtrl.Instance.TankList.Clear();
        }
        // TankList.Add();
        for (int i = 0; i < 50; i++)
        {
            TankProperty tank = new TankProperty((12 + i).ToString().PadLeft(5, '0'), "蝎式坦克", null);
            int atkCount = ResourceCtrl.Instance.SkillList.FindAll(u => u.SkillType == "Attack").Count;
            int defCount = ResourceCtrl.Instance.SkillList.FindAll(u => u.SkillType == "Defense").Count;
            tank.AttackSkill = ResourceCtrl.Instance.SkillList.FindAll(u => u.SkillType == "Attack")[CommonHelper.GetRandom(0, atkCount)];
            tank.DefenseSkill = ResourceCtrl.Instance.SkillList.FindAll(u => u.SkillType == "Defense")[CommonHelper.GetRandom(0, defCount)];

            //tank.AttackSkill = ResourceCtrl.Instance.SkillList.Find(u => u.Name == "Duel");
            //tank.DefenseSkill = ResourceCtrl.Instance.SkillList.Find(u => u.Name == "Revenge");
            ResourceCtrl.Instance.TankList.Add(tank);
        }

    }


    /// <summary>
    /// 组装坦克
    /// </summary>
    /// <param name="Engine">底座部分</param>
    /// <param name="Body">机身部分</param>
    /// <param name="Head">机头部分</param>
    /// <param name="Weapon">武器部分</param>
    /// <returns></returns>
    public GameObject Mount(AOT_Parts Engine, AOT_Parts Body, AOT_Parts Head, AOT_Parts Weapon)
    {
        ResourceCtrl rec = ResourceCtrl.Instance;
        Nodes parentNode;
        LinkNodeInfo parentLink;

        //引擎
        AOT_Models engineModel = rec.ModelList.Find(u => u.Code == Engine.ModelCode);
        AOT_SkinInfo engineSkin = rec.SkinList.Find(u => u.Code == Engine.SkinCode);
        GameObject engineObj = Instantiate(Resources.Load<GameObject>(engineModel.FilePath));
        engineObj.name = engineObj.name.Replace("(Clone)", "");
        engineObj.transform.localPosition = new Vector3(0, 0, 0);
        CommonHelper.ReplaceMaterialByPath(engineObj, engineSkin.MaterialPath);
        engineObj.SetActive(true);

        //机身
        AOT_Models bodyModel = rec.ModelList.Find(u => u.Code == Body.ModelCode);
        AOT_SkinInfo bodySkin = rec.SkinList.Find(u => u.Code == Body.SkinCode);
        parentNode = engineObj.GetComponent<Nodes>();
        parentLink = parentNode.ChildNodes.Find(u => u.LinkType == LinkType.Body);
        GameObject bodyObj = Instantiate(Resources.Load<GameObject>(bodyModel.FilePath), parentLink.LinkNode.transform, false);
        bodyObj.name = bodyObj.name.Replace("(Clone)", "");
        bodyObj.transform.localPosition = new Vector3(0, 0, 0);
        CommonHelper.ReplaceMaterialByPath(bodyObj, bodySkin.MaterialPath);
        bodyObj.SetActive(true);

        //机头
        AOT_Models headModel = rec.ModelList.Find(u => u.Code == Head.ModelCode);
        AOT_SkinInfo headSkin = rec.SkinList.Find(u => u.Code == Head.SkinCode);
        parentNode = bodyObj.GetComponent<Nodes>();
        parentLink = parentNode.ChildNodes.Find(u => u.LinkType == LinkType.Head);
        GameObject headObj = Instantiate(Resources.Load<GameObject>(headModel.FilePath), parentLink.LinkNode.transform, false);
        headObj.name = headObj.name.Replace("(Clone)", "");
        headObj.transform.localPosition = new Vector3(0, 0, 0);
        CommonHelper.ReplaceMaterialByPath(headObj, headSkin.MaterialPath);
        headObj.SetActive(true);

        //武器
        AOT_Models weaponModel = rec.ModelList.Find(u => u.Code == Weapon.ModelCode);
        AOT_SkinInfo weaponSkin = rec.SkinList.Find(u => u.Code == Weapon.SkinCode);
        parentNode = bodyObj.GetComponent<Nodes>();
        LinkNodeInfo weapon_L_Link = parentNode.ChildNodes.Find(u => u.LinkType == LinkType.Weapon_L);
        GameObject weaponObj_L = Instantiate(Resources.Load<GameObject>(weaponModel.FilePath), weapon_L_Link.LinkNode.transform, false);
        weaponObj_L.name = weaponObj_L.name.Replace("(Clone)", "");
        weaponObj_L.transform.localPosition = new Vector3(0, 0, 0);
        CommonHelper.ReplaceMaterialByPath(weaponObj_L, weaponSkin.MaterialPath);
        weaponObj_L.SetActive(true);

        LinkNodeInfo weapon_R_Link = parentNode.ChildNodes.Find(u => u.LinkType == LinkType.Weapon_R);
        GameObject weaponObj_R = Instantiate(Resources.Load<GameObject>(weaponModel.FilePath), weapon_R_Link.LinkNode.transform, false);
        weaponObj_R.name = weaponObj_R.name.Replace("(Clone)", "");
        weaponObj_R.transform.localPosition = new Vector3(0, 0, 0);
        CommonHelper.ReplaceMaterialByPath(weaponObj_R, weaponSkin.MaterialPath);
        weaponObj_R.SetActive(true);


        return engineObj;
    }

    /// <summary>
    /// 获取组装记录
    /// </summary>
    /// <param name="Engine">底座部分</param>
    /// <param name="Body">机身部分</param>
    /// <param name="Head">机头部分</param>
    /// <param name="Weapon">武器部分</param>
    /// <returns></returns>
    public AOT_SetupRecord GetTankSetupRecord(AOT_Parts Engine, AOT_Parts Body, AOT_Parts Head, AOT_Parts Weapon)
    {
        if (Engine != null && Body != null && Head != null && Weapon != null)
        {
            int total = Engine.Level + Body.Level + Head.Level + Weapon.Level;
            int tankLvl = 1;

            #region 确定坦克等级
            if (total >= 4 && total <= 7)
            {
                tankLvl = 1;
            }
            else if (total >= 8 && total <= 11)
            {
                tankLvl = 2;
            }
            else if (total >= 12 && total <= 15)
            {
                tankLvl = 3;
            }
            else if (total >= 16 && total <= 19)
            {
                tankLvl = 4;
            }
            else if (total >= 20)
            {
                tankLvl = 5;
            }
            #endregion

            string Code = (Convert.ToInt32(ResourceCtrl.Instance.TankList.Max(u => u.Code)) + 1).ToString().PadLeft(5, '0');

            //添加组装信息
            AOT_SetupRecord record = new AOT_SetupRecord();

            record.Code = Code;
            record.Level = tankLvl;
            //武器：攻击力、攻击技能
            record.Attack = Weapon.Attack;
            record.AttackSkillCode = Weapon.AttackSkillCode;
            //机身：血量、防御技能
            record.Blood = Body.Blood;
            record.DefenseSkillCode = Body.DefenseSkillCode;
            //底座：速度、承载力
            record.Speed = Engine.Speed;
            record.Bearing = Engine.Bearing;
            //机头：范围、暴击
            record.Range = Head.Range;
            record.Crit = Head.Crit;
            record.Weight = Engine.Weight + Body.Weight + Head.Weight + Weapon.Weight;

            record.LegCode = Engine.Code;
            record.BodyCode = Body.Code;
            record.HeadCode = Head.Code;
            record.WeaponCode = Weapon.Code;
            record.Status = 1;
            if (string.Equals(Engine.SkinCode, Body.SkinCode) && string.Equals(Body.SkinCode, Head.SkinCode) && string.Equals(Head.SkinCode, Weapon.SkinCode))
            {
                record.SameSkin = true;
            }
            else
            {
                record.SameSkin = false;
            }

            return record;
        }
        else
        {
            return null;
        }
    }


    /// <summary>
    /// 添加到坦克列表
    /// </summary>
    /// <param name="record"></param>
    public void InsertToTankList(AOT_SetupRecord record)
    {
        TankProperty tank = new TankProperty();
        tank.Code = record.Code;
        tank.IsSetup = true;
        tank.Attack = (float)record.Attack;
        tank.Blood = (float)record.Blood;
        tank.CritRate = (float)record.Crit;
        tank.Range = (float)record.Range;
        tank.Weight = (float)record.Weight;
        tank.Bearer = (float)record.Bearing;
        tank.Speed = (float)record.Speed;
        if (!string.IsNullOrEmpty(record.AttackSkillCode))
        {
            tank.AttackSkill = ResourceCtrl.Instance.SkillList.Find(u => u.Code == record.AttackSkillCode);
        }
        if (!string.IsNullOrEmpty(record.DefenseSkillCode))
        {
            tank.DefenseSkill = ResourceCtrl.Instance.SkillList.Find(u => u.Code == record.DefenseSkillCode);
        }

        ResourceCtrl.Instance.TankList.Insert(0, tank);

    }
}
