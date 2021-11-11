using System.Collections;
using System.Collections.Generic;
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
    public List<SkillInfo> SkillList = new List<SkillInfo>();

    public List<AOT_SkinInfo> SkinList = new List<AOT_SkinInfo>();

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
        InitSkillInfo();
        System.GC.Collect();
        StartCoroutine(CommonHelper.DelayToInvokeDo(() => { SceneManager.LoadScene("BattleMode"); }, 5f));
        
    }


    /// <summary>
    /// 获取用户信息
    /// </summary>
    void GetUserInfo()
    {
        if (PlayerPrefs.HasKey("userinfo"))
        {
            string str = PlayerPrefs.GetString("userinfo");
            Debug.Log(str);
            UserInfo = JSONhelper.ConvertToObject<AOT_User>(str);
        }
        else
        {
            Debug.Log("用户信息获取失败");
        }
    }


    /// <summary>
    /// 技能基础信息初始化
    /// </summary>
    void InitSkillInfo()
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
        SkillList = list;
        if (UserInfo != null)
        {
            string jsonstr = JSONhelper.ToJson(new { UserID = UserInfo.UUID });
            List<AOT_SkillInfo> skilllist = new List<AOT_SkillInfo>();
            HttpTool.Instance.Post("aotuser/skillinfo", jsonstr, (string result) =>
            {
                APIResult res = JSONhelper.ConvertToObject<APIResult>(result);// JSONhelper.ConvertToObject<APIResult>(result);

                if (res.success == true)
                {
                    skilllist = JSONhelper.ConvertToObject<List<AOT_SkillInfo>>(JSONhelper.ToJson(res.data));
                }
                else
                {
                    Debug.Log(res.message);
                }
            });
        }

    }



    //----获取基础信息----
    //获取皮肤信息
    void InitSkin()
    {
        //普通（1）、稀罕（2）、史诗（3）、传奇（4）、神奇（5）
        List<AOT_SkinInfo> list = new List<AOT_SkinInfo>();
        list.Add(new AOT_SkinInfo() {
            Code = "Skin00001",
            Level = 1,
            SkinName = "普通",
            Title = "普通",
            MaterialPath = "Materials/A_Spiders_Mat(black)",
            Description = "",
            Title_en = "",
            Description_en = "",
            AttackUp = 1,
            BloodUp = 1,
            SpeedUp = 1,
            RangeUp = 1,
            CirtUp = 1,
            UpType = 1
        });
        list.Add(new AOT_SkinInfo()
        {
            Code = "Skin00002",
            Level = 2,
            SkinName = "稀罕",
            Title = "稀罕",
            MaterialPath = "Materials/A_Spiders_Mat(green)",
            Description = "",
            Title_en = "",
            Description_en = "",
            AttackUp = 1,
            BloodUp = 1,
            SpeedUp = 1,
            RangeUp = 1,
            CirtUp = 1,
            UpType = 1
        });
        list.Add(new AOT_SkinInfo()
        {
            Code = "Skin00003",
            Level = 3,
            SkinName = "史诗",
            Title = "史诗",
            MaterialPath = "Materials/A_Spiders_Mat(blue)",
            Description = "",
            Title_en = "",
            Description_en = "",
            AttackUp = 1,
            BloodUp = 1,
            SpeedUp = 1,
            RangeUp = 1,
            CirtUp = 1,
            UpType = 1
        });
        list.Add(new AOT_SkinInfo()
        {
            Code = "Skin00004",
            Level = 4,
            SkinName = "传奇",
            Title = "传奇",
            MaterialPath = "Materials/A_Spiders_Mat(yellow)",
            Description = "",
            Title_en = "",
            Description_en = "",
            AttackUp = 1,
            BloodUp = 1,
            SpeedUp = 1,
            RangeUp = 1,
            CirtUp = 1,
            UpType = 1
        });
        list.Add(new AOT_SkinInfo()
        {
            Code = "Skin00005",
            Level = 5,
            SkinName = "神奇",
            Title = "神奇",
            MaterialPath = "Materials/A_Spiders_Mat(red)",
            Description = "",
            Title_en = "",
            Description_en = "",
            AttackUp = 1,
            BloodUp = 1,
            SpeedUp = 1,
            RangeUp = 1,
            CirtUp = 1,
            UpType = 1
        });

        SkinList = list;


        if (UserInfo != null)
        {
            string jsonstr = JSONhelper.ToJson(new { UserID = UserInfo.UUID });
            List<AOT_SkinInfo> skinlist = new List<AOT_SkinInfo>();
            HttpTool.Instance.Post("aotuser/skininfo", jsonstr, (string result) =>
            {
                APIResult res = JSONhelper.ConvertToObject<APIResult>(result);// JSONhelper.ConvertToObject<APIResult>(result);

                if (res.success == true)
                {
                    skinlist = JSONhelper.ConvertToObject<List<AOT_SkinInfo>>(JSONhelper.ToJson(res.data));
                }
                else
                {
                    Debug.Log(res.message);
                }
            });
        }

    }

    //获取模型信息

    //获取技能信息

    //获取坦克信息



}
