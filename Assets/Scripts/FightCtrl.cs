using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FightCtrl : MonoBehaviour
{
    public GameObject UIPanel;
    public GameObject Pointer;
    /// <summary>
    /// 对战台
    /// </summary>
    public GameObject Platform;
    /// <summary>
    /// 模型位置
    /// </summary>
    public List<GameObject> PlayerA_Pos;
    public List<GameObject> PlayerB_Pos;
    /// <summary>
    /// 坦克对象
    /// </summary>
    public List<GameObject> PlayerA_Tanks;
    public List<GameObject> PlayerB_Tanks;

    /// <summary>
    /// 玩家信息
    /// </summary>
    public Player PlayerA, PlayerB;

    List<FightOrder> OrderList;

    //public int index = 0;
    public int CountDownTime = 5; //倒计时
    public int Round = 1; //回合数
    public int FightIndex = -1; //当前战斗方的下标

    private void Awake()
    {
        InitPlatform();
        InitTank();
        StartTransfer();
        InitPlayer();
        GetFightOrder();

        UIPanel.transform.Find("btnBack").SetAsLastSibling();
        UIPanel.transform.Find("btnBack").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            Debug.Log("back click");
            SceneManager.LoadSceneAsync("IndexScene");
            GC.Collect();
        });
        //5秒后开始
        InvokeRepeating("TimePlay", 1f, 1f);

        StartCoroutine(CommonHelper.DelayToInvokeDo(() => { 
           
            UIPanel.transform.Find("txt_time_center").GetComponent<Text>().DOText("Fight", 0.5f).OnComplete(()=> {
                UIPanel.transform.Find("txt_time_center").gameObject.SetActive(false);
                NextRound();
            });

        },6f));

        
    }

    void TimePlay() 
    {
        Transform timePanel = UIPanel.transform.Find("txt_time_center");
        timePanel.SetAsLastSibling();
        timePanel.GetComponent<Text>().text = CountDownTime.ToString();
        timePanel.gameObject.SetActive(true);
        timePanel.transform.DOScale(new Vector3(1, 1, 1), 0.5f).From(new Vector3(0, 0, 0));
        CountDownTime--;
        if (CountDownTime == 0) 
        {
            CancelInvoke("TimePlay");
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //AutoShoot();
        foreach (FightOrder fightItem in OrderList)
        {
            GameObject bloodbar = UIPanel.transform.Find("bloodbar").gameObject;
            Transform bar = UIPanel.transform.Find("bloodbar_" + fightItem.Code);
            bar.position = Camera.main.WorldToScreenPoint(fightItem.Tank.transform.position + new Vector3(0, 13, 0));
           
        }

    }

  
    //战斗平台初始化
    public void InitPlatform()
    {
        int count = Platform.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            GameObject child = Platform.transform.GetChild(i).gameObject;
            if (child.name.Contains("A"))
            {
                PlayerA_Pos.Add(child);
            }
            else
            {
                PlayerB_Pos.Add(child);
            }
            
        }
    }

    //进入场景后随机匹配对战双方的坦克，后期通过数据拉取
    public void InitTank()
    {
        //获取所有坦克的预制体
        int tankcount = ResourceCtrl.Instance.ResourceRoot.transform.Find("Tanks").childCount;
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < tankcount; i++)
        {
            Transform tr=  ResourceCtrl.Instance.ResourceRoot.transform.Find("Tanks").GetChild(i);
            if (tr != null) 
            {

                list.Add(tr.gameObject);
                //CommonHelper.AddEffect("Effect"+CommonHelper.GetRandom(1,22), tr.gameObject);
            }
            
        }

        
        //分给对战双方
        //foreach (GameObject pos in PlayerA_Pos)
        //{
        //    //随机分配
        //    int index = CommonHelper.GetRandom(0, list.Count);
        //    GameObject tank = Instantiate(list[index]);
        //    tank.AddComponent<Weapon>();
        //    tank.SetActive(false);
        //    tank.transform.SetParent(pos.transform.Find("Tank"), false);
        //    PlayerA_Tanks.Add(tank);
        //}

        //玩家A
        for (int i = 0; i < ResourceCtrl.Instance.SelectList.Count; i++)
        {
            TankProperty item = ResourceCtrl.Instance.SelectList[i];

            GameObject tank = Instantiate(list.Find(u => u.name == item.Code));
            tank.AddComponent<Weapon>();
            tank.SetActive(false);
            tank.transform.SetParent(PlayerA_Pos[i].transform.Find("Tank"), false);
           
            CommonHelper.ReplaceMaterial(tank);
            
            item.TankObject = tank;
            PlayerA_Tanks.Add(tank);
        }


        //玩家B随机分配
        foreach (GameObject pos in PlayerB_Pos)
        {
            //随机分配
            int index = CommonHelper.GetRandom(0, list.Count);
            GameObject tank = Instantiate(list[index]);
            tank.AddComponent<Weapon>();
            tank.SetActive(false);
            tank.transform.SetParent(pos.transform.Find("Tank"), false);
            CommonHelper.ReplaceMaterial(tank);
            PlayerB_Tanks.Add(tank);
        }

    }

    //开启传送阵
    public void StartTransfer()
    {
        GameObject transfer = CommonHelper.GetPrefabs("skill", "Transfer");

        //建立动画队列
        Sequence quence = DOTween.Sequence();

        foreach (GameObject pos in PlayerA_Pos)
        {
            int num = PlayerA_Pos.IndexOf(pos);
            StartCoroutine(CommonHelper.DelayToInvokeDo(() =>
            {
                GameObject newObj = Instantiate(transfer);
                newObj.transform.SetParent(pos.transform, false);
                newObj.SetActive(true);
                Destroy(newObj, 5f);
                GameObject tank = PlayerA_Tanks[num];
                tank.SetActive(true);
                tank.transform.DOScale(new Vector3(1, 1, 1), 0.1f).From(new Vector3(0, 0, 0)).SetEase(Ease.InOutExpo);
            }, (num + 1) * 0.2f));
        }
        foreach (GameObject pos in PlayerB_Pos)
        {
            int num = PlayerB_Pos.IndexOf(pos);
            StartCoroutine(CommonHelper.DelayToInvokeDo(() =>
            {
                GameObject newObj = Instantiate(transfer);
                newObj.transform.SetParent(pos.transform, false);
                newObj.SetActive(true);
                Destroy(newObj, 5f);
                GameObject tank = PlayerB_Tanks[num];
                tank.SetActive(true);
                tank.transform.DOScale(new Vector3(1, 1, 1), 0.1f).From(new Vector3(0, 0, 0)).SetEase(Ease.InOutExpo);


            }, (num + 1) * 0.2f));
        }

    }

    //初始化玩家数据
    public void InitPlayer()
    {
        List<TankProperty> tankPropertiesA = new List<TankProperty>();
        //foreach (GameObject item in PlayerA_Tanks)
        //{
        //    int num = PlayerA_Tanks.IndexOf(item);
        //    string id = (num + 1).ToString().PadLeft(2, '0');
        //    tankPropertiesA.Add(new TankProperty("坦克"+id, "蝎式坦克", item,num));
        //}

        tankPropertiesA = ResourceCtrl.Instance.SelectList;
        PlayerA = new Player
        {
            Name = "玩家A",
            Photo = "",
            Hero = new HeroProperty()
            {
                Code = "A1",
                Name = "英雄A",
                Speed = 10,
                Attack = 10,
                Defense = 10,
                CritRate = 5,
                Blood = 100
            },
            TankList = tankPropertiesA
        };

        List<TankProperty> tankPropertiesB = new List<TankProperty>();
        foreach (GameObject item in PlayerB_Tanks)
        {
            int num = PlayerB_Tanks.IndexOf(item);
            string id = (num + 1).ToString().PadLeft(5, '0');
            TankProperty tank = new TankProperty(id, "蝎式坦克", item);
            tank.AttackSkill = ResourceCtrl.Instance.SkillList.FindAll(u=>u.Type=="Attack")[CommonHelper.GetRandom(0,13)];
            tank.DefenseSkill = ResourceCtrl.Instance.SkillList.FindAll(u => u.Type == "Defense")[CommonHelper.GetRandom(0, 8)];
            tankPropertiesB.Add(tank);
        }
        PlayerB = new Player
        {
            Name = "玩家B",
            Photo = "",
            Hero = new HeroProperty()
            {
                Code = "B1",
                Name = "英雄B",
                Speed = 10,
                Attack = 10,
                Defense = 10,
                CritRate = 5,
                Blood = 100
            },
            TankList = tankPropertiesB
        };
    }

    /// <summary>
    /// 按照攻击速度进行排序
    /// </summary>
    public void GetFightOrder()
    {
        OrderList = new List<FightOrder>();
        foreach (TankProperty item in PlayerA.TankList)
        {
            OrderList.Add(new FightOrder {
                Player = PlayerA,
                Tank = item.TankObject,
                Code = item.Code,
                Speed = PlayerA.Hero.Speed + item.Speed,
                Attack = PlayerA.Hero.Attack + item.Attack,
                CritRate = PlayerA.Hero.CritRate + item.CritRate,
                //Defense = PlayerA.Hero.Defense + item.Defense,
                Blood = PlayerA.Hero.Blood + item.Blood,
               // HitRate = item.HitRate,
                Range = item.Range,
                AttackSkill = item.AttackSkill,
                DefenseSkill = item.DefenseSkill
            }); ;
        }
        foreach (TankProperty item in PlayerB.TankList)
        {
            OrderList.Add(new FightOrder
            {
                Player = PlayerB,
                Tank = item.TankObject,
                Code = item.Code,
                Speed = PlayerB.Hero.Speed + item.Speed,
                Attack = PlayerB.Hero.Attack + item.Attack,
                CritRate = PlayerB.Hero.CritRate + item.CritRate,
                //Defense = PlayerB.Hero.Defense + item.Defense,
                Blood = PlayerB.Hero.Blood + item.Blood,
                //HitRate = item.HitRate,
                Range = item.Range,
                AttackSkill = item.AttackSkill,
                DefenseSkill = item.DefenseSkill

            });
        }

        OrderList = OrderList.OrderByDescending(u => u.Speed).ToList();

        foreach (FightOrder item in OrderList)
        {
            RefreshBloodBar(item);
            
            SetupDefenseSkills(item);
            SetupAttackSkills(item);
          
        }

        
    }

    /// <summary>
    /// 为当前攻击方随机设置攻击目标
    /// </summary>
    /// <param name="fightOrder"></param>
    /// <returns></returns>
    public List<FightOrder> SetTarget(FightOrder fightOrder)
    {
        List<FightOrder> list = new List<FightOrder>();
        Weapon w = fightOrder.Tank.GetComponent<Weapon>();

        if (w.target != null && w.target.Count > 0)
        {
            w.target.Clear();
        }


        int count = 1;// CommonHelper.GetRandom(1, 7);
                      //对方存活坦克
        List<FightOrder> livelist = OrderList.FindAll(u => u.Player.Name != fightOrder.Player.Name && u.Death == false);
        if (count >= livelist.Count)
        {
            foreach (FightOrder item in livelist)
            {
                w.target.Add(item.Tank);
                list = livelist;
            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                FightOrder newor = livelist[CommonHelper.GetRandom(0, livelist.Count)];
                GameObject newtar = newor.Tank;
                while (w.target.IndexOf(newtar) > -1)
                {
                    newor = livelist[CommonHelper.GetRandom(0, livelist.Count)];
                    newtar = newor.Tank;
                }
                w.target.Add(newtar);

                list.Add(newor);
            }
        }


        return list;
    }

   

    /// <summary>
    /// 下个回合
    /// </summary>
    void NextRound() 
    {
        GameObject roundPanel = UIPanel.transform.Find("txt_round_center").gameObject;
         UIPanel.transform.Find("txt_round_center").SetAsLastSibling();
        Text txt_round = roundPanel.transform.GetComponent<Text>();
        txt_round.text = "Round " + Round;
        UIPanel.transform.Find("txt_round").GetComponent<Text>().text= "Round " + Round;
        roundPanel.SetActive(true);
        roundPanel.transform.DOScale(new Vector3(1, 1, 1), 0.5f).From(new Vector3(0, 0, 0));
        StartCoroutine(CommonHelper.DelayToInvokeDo(() =>
        {
            UIPanel.transform.Find("txt_round_center").gameObject.SetActive(false);
            int nextIndex = GetNextFightIndex();
            Fight(nextIndex);
        },2f));
        
    }


    /// <summary>
    /// 显示减血数字效果
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="value">数值</param>
    /// <param name="type">0：减血 1：加血</param>
    /// <param name="crit">是否暴击</param>
    void ShowBlood(GameObject target,float value,int type=0,bool crit=false)
    {
        GameObject txtblood = UIPanel.transform.Find("txt_blood").gameObject;
        GameObject newblood = Instantiate(txtblood, UIPanel.transform, false);
        newblood.transform.position = Camera.main.WorldToScreenPoint(target.transform.position + new Vector3(0, CommonHelper.GetRandom(3,7), 0));
        Text val = newblood.transform.Find("txt_val").GetComponent<Text>();
        val.text =(type==0?"":"+")+ value.ToString();
        val.color = (type== 0 ? Color.white : Color.green); 
        newblood.SetActive(true);
        if (crit == true) 
        {
            newblood.transform.Find("critimg").gameObject.SetActive(true);
        }
        newblood.transform.DOMove(Camera.main.WorldToScreenPoint(target.transform.position + new Vector3(-0.5f, CommonHelper.GetRandom(17, 25), 0)), 0.5f);//上飘
        newblood.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).OnComplete(()=> {
            val.DOColor(new Color(val.color.r, val.color.g, val.color.b, 0), 0.8f).OnComplete(() => {
                DestroyImmediate(newblood);
            }); //消失
        });//字体放大
        
    }

    /// <summary>
    /// 刷新血条
    /// </summary>
    /// <param name="fightItem"></param>
    void RefreshBloodBar(FightOrder fightItem)
    {
        GameObject bloodbar = UIPanel.transform.Find("bloodbar").gameObject;
        Transform bar = UIPanel.transform.Find("bloodbar_" + fightItem.Code);
        Slider slider;
        if (bar == null)
        {
            bar = Instantiate(bloodbar, UIPanel.transform, false).transform;
            bar.gameObject.name = "bloodbar_" + fightItem.Code;
            bar.gameObject.SetActive(true);
            slider = bar.GetComponent<Slider>();
            slider.maxValue = fightItem.Player.Hero.Blood + fightItem.Player.TankList.Find(u => u.TankObject == fightItem.Tank).Blood;
            slider.minValue = 0;
        }

       // bar.position = Camera.main.WorldToScreenPoint(fightItem.Tank.transform.position + new Vector3(0, 13, 0));
        slider = bar.GetComponent<Slider>();
        slider.value = fightItem.Blood;
        if (slider.value <= slider.maxValue * 0.33)
        {
            bar.Find("Fill Area/Fill").GetComponent<Image>().color = Color.red;
        }
        else if (slider.value <= slider.maxValue * 0.66)
        {
            bar.Find("Fill Area/Fill").GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            bar.Find("Fill Area/Fill").GetComponent<Image>().color = Color.green;
        }
    }

    /// <summary>
    /// 暴击判定
    /// </summary>
    /// <param name="rate">概率</param>
    /// <returns></returns>
    bool IsCrit(float rate) 
    {
        int random = CommonHelper.GetRandom(1, 100);
        return random <= rate;
    }

    /// <summary>
    /// 是否命中判定
    /// </summary>
    /// <param name="rate">概率</param>
    /// <returns></returns>
    bool IsHit(float rate)
    {
        int random = CommonHelper.GetRandom(1, 100);
        return random <= rate;
    }

    /// <summary>
    /// 获取下一个攻击方
    /// </summary>
    /// <returns></returns>
    int GetNextFightIndex() 
    {
        if (FightIndex >= OrderList.Count-1) {
            FightIndex = -1;
        }
        int index = FightIndex+1;    
        while(OrderList[index].Death == true)
        { 
            //一个回合之后进入下一回合
            if (index == OrderList.Count - 1)
            {
                index = -1;
                break;
            }
            else
            {
                index++;
            }
        }
        FightIndex = index;
        return index;
    }

    //轮流对战，递归轮流执行操作
    void Fight(int index)
    {
        FightOrder fightitem = OrderList[index];

        #region 判定是否有恢复技能
        if (fightitem.DefenseSkill != null)
        {
            if (fightitem.DefenseSkill.Name.ToLower() == "recover")
            {
                Recover recover = fightitem.Tank.GetComponent<Recover>();
                if (recover != null)
                {
                    if (recover.Trigger() == true)
                    {
                        float maxBlood = fightitem.Player.Hero.Blood + fightitem.Player.TankList.Find(u => u.TankObject == fightitem.Tank).Blood;
                        float addBlood = (int)Math.Round( maxBlood * recover.Value);
                        if (addBlood + fightitem.Blood > maxBlood)
                        {
                            fightitem.Blood = maxBlood;
                        }
                        else
                        {
                            fightitem.Blood += addBlood;
                        }
                        ShowBlood(fightitem.Tank, addBlood, 1);
                    }
                }
            }
        }
        
        #endregion


        List<FightOrder> targets = SetTarget(fightitem);
        //普通攻击
        SimpleAttack(fightitem, targets);
       
        //判断是否一方全部阵亡，若全部阵亡则不再执行攻击，否则继续。当一个回合结束后，开始下一轮
        if (OrderList.FindAll(u => u.Player.Name == "玩家A" && u.Death == false).Count == 0 || OrderList.FindAll(u => u.Player.Name == "玩家B" && u.Death == false).Count == 0)
        {

            UIPanel.transform.Find("txt_round_center").gameObject.SetActive(true);
            UIPanel.transform.Find("txt_round_center").SetAsLastSibling();
            UIPanel.transform.Find("txt_round_center").GetComponent<Text>().text = "GAME  OVER";
            UIPanel.transform.Find("btnBack").gameObject.SetActive(true);
           
        }
        else
        {
            if (index >= OrderList.Count - 1)
            {
                Round++;
                NextRound();
            }
            else
            {
                int nextindex = GetNextFightIndex();
                if (nextindex == -1)
                {
                    Round++;
                    NextRound();
                }
                else
                {
                    StartCoroutine(CommonHelper.DelayToInvokeDo(() =>
                    {
                        Fight(nextindex);
                    }, 1.2f));
                }

            }

        }


    }


    //普通攻击
    void SimpleAttack(FightOrder fightitem, List<FightOrder> targets,bool isskill=false)
    {
        if (fightitem.Death == false)
        {
            Weapon w = fightitem.Tank.GetComponent<Weapon>();
           // List<FightOrder> targets = SetTarget(fightitem);
            if (targets.Count > 0)
            {
                w.Shoot(fightitem.Tank);
                //被攻击坦克普通攻击血量减少操作
                for (int j = 0; j < targets.Count; j++)
                {
                    FightOrder target = targets[j];


                    bool iscrit = IsCrit(fightitem.CritRate);
                    float attack =(int)Math.Round(iscrit ? fightitem.Attack * 2 : fightitem.Attack);

                    #region 防御技能触发判定
                    if (target.DefenseSkill != null)
                    {
                        switch (target.DefenseSkill.Name.ToLower())
                        {
                            //圣盾
                            case "holyshield":
                                HolyShield holyShield = target.Tank.GetComponent<HolyShield>();
                                if (holyShield != null)
                                {
                                    if (holyShield.Trigger() == true)
                                    {
                                        continue;
                                    }
                                }
                                break;
                            //重甲
                            case "heavyarmor":
                                HeavyArmor heavyArmor = target.Tank.GetComponent<HeavyArmor>();
                                if (heavyArmor != null)
                                {
                                    if (heavyArmor.Trigger() == true)
                                    {
                                        //伤害减少百分比
                                        attack = (int)Math.Round((1 - heavyArmor.Value) * attack);
                                    }
                                }
                                break;
                            //闪避
                            case "dodge":
                                Dodge dodge = target.Tank.GetComponent<Dodge>();
                                if (dodge != null)
                                {
                                    if (dodge.Trigger() == true)
                                    {
                                        //闪避触发
                                        //未命中
                                        showMiss(target);
                                        continue;
                                    }
                                }
                                break;
                            //嘲讽
                            case "taunt":
                                Taunt taunt = target.Tank.GetComponent<Taunt>();
                                if (taunt != null)
                                {
                                    if (taunt.Trigger() == true)
                                    {

                                    }
                                }
                                break;
                            //还击
                            case "revenge":
                                Revenge revenge = target.Tank.GetComponent<Revenge>();
                                if (revenge != null)
                                {
                                    if (revenge.Trigger() == true)
                                    {
                                        //随机一个攻击对象
                                        SimpleAttack(target, SetTarget(target), true);
                                    }
                                }
                                break;
                            //反伤
                            case "antiinjury":
                                AntiInjury antiinjury = target.Tank.GetComponent<AntiInjury>();
                                if (antiinjury != null)
                                {
                                    antiinjury.AttackTank = fightitem.Tank;
                                    if (antiinjury.Trigger() == true)
                                    {
                                        //反伤
                                        float replay_atk = (int)Math.Round(attack * antiinjury.Value);
                                        fightitem.Blood -= replay_atk;
                                        ShowBlood(fightitem.Tank, replay_atk, 0, false);
                                        RefreshBloodBar(fightitem);

                                    }
                                }
                                break;
                                //复生(死亡时判定)
                                //case "revive":
                                //    Revive revive = target.Tank.GetComponent<Revive>();
                                //    if (revive != null)
                                //    {
                                //        if (revive.Trigger() == true)
                                //        {
                                //            continue;
                                //        }
                                //    }
                                //    break;
                                //恢复
                                //case "recover":
                                //    Recover recover = target.Tank.GetComponent<Recover>();
                                //    if (recover != null)
                                //    {
                                //        if (recover.Trigger() == true)
                                //        {
                                //            continue;
                                //        }
                                //    }
                                //    break;
                        }

                    }
                    //攻击技能效果
                    //AttackSkills(fightitem, targets);
                    #endregion

                    #region 攻击技能特效触发
                    if (fightitem.AttackSkill != null && isskill==false)
                    {
                        switch (fightitem.AttackSkill.Name.ToLower())
                        {
                            case "batter":
                                Batter batter = fightitem.Tank.GetComponent<Batter>();
                                if (batter != null)
                                {
                                    batter.TargetTank = target.Tank;
                                    if (batter.EffectAttack(()=> { SimpleAttack(fightitem, targets, true); }) == true)
                                    {
                                        continue;
                                    }
                                }
                                break;
                            case "thump":
                                Thump thump = fightitem.Tank.GetComponent<Thump>();
                                if (thump != null)
                                {
                                    thump.TargetTank = target.Tank;
                                    thump.EffectAttack();

                                    attack = (int)Math.Round(attack * thump.Value);
                                }
                                break;
                        }
                    }
                    #endregion

                    target.Blood -= attack;// - target.Defense;
                    ShowBlood(target.Tank, attack, 0, iscrit);
                    RefreshBloodBar(target);
                    if (target.Blood <= 0)
                    {
                        target.Blood = 0;
                        target.Death = true;
                        //死亡爆炸效果
                        GameObject Deathprefab = CommonHelper.GetPrefabs("skill", "Death");
                        GameObject death = Instantiate(Deathprefab);
                        death.SetActive(true);
                        death.transform.SetParent(target.Tank.transform.parent.parent, false);
                        target.Tank.transform.GetChild(0).GetComponent<Animator>().Play("Death");


                        #region 死亡后判定是否有复生技能和亡语技能
                        if (target.DefenseSkill != null)
                        {
                            if (target.DefenseSkill.Name.ToLower() == "revive")
                            {
                                Revive revive = target.Tank.GetComponent<Revive>();
                                if (revive != null)
                                {
                                    if (revive.Trigger() == true)
                                    {
                                        StartCoroutine(CommonHelper.DelayToInvokeDo(() =>
                                        {
                                                //恢复20%血量，移除死亡效果，恢复正常状态
                                                target.Tank.transform.GetChild(0).GetComponent<Animator>().Play("Idle");
                                            Destroy(death);
                                            target.Death = false;
                                            target.Blood = (target.Player.Hero.Blood + target.Player.TankList.Find(u => u.TankObject == target.Tank).Blood) * 0.2f;
                                            RefreshBloodBar(target);
                                        }, 1f));
                                    }
                                }
                            }
                            else if (target.DefenseSkill.Name.ToLower() == "deathsound")
                            {
                                DeathSound deathsound = target.Tank.GetComponent<DeathSound>();
                                if (deathsound != null)
                                {
                                    if (deathsound.Trigger() == true)
                                    {
                                        //亡语效果

                                    }
                                }
                            }

                        }
                        #endregion


                    }
                    //攻击是否为技能
                    if (isskill == false)
                    {
                       // AttackSkills(fightitem, new List<FightOrder>() { target });
                    }

                    Debug.Log($"{target.Player.Name},{target.Title}被攻击减少{ fightitem.Attack}血量，剩余{target.Blood }血量");
                }


            }
        }
        //if (times > 1) 
        //{
        //    StartCoroutine(CommonHelper.DelayToInvokeDo(() =>
        //    {
        //        SimpleAttack(fightitem, targets, times - 1);
        //    }, 0.5f));
        //}
    }

    //显示miss 未命中
    void showMiss(FightOrder target)
    {
        //闪避触发
        //未命中
        GameObject txtblood = UIPanel.transform.Find("txt_blood").gameObject;
        GameObject miss = Instantiate(txtblood, UIPanel.transform, false);
        miss.transform.position = Camera.main.WorldToScreenPoint(target.Tank.transform.position + new Vector3(0, 2, 0));
        Text val = miss.transform.Find("txt_val").GetComponent<Text>();
        val.text = "MISS";
        val.color = Color.red;
        miss.SetActive(true);


        miss.transform.DOMove(Camera.main.WorldToScreenPoint(target.Tank.transform.position + new Vector3(-0.5f, CommonHelper.GetRandom(16, 25), 0)), 0.5f);//上飘
        miss.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).OnComplete(() => {
            val.DOColor(new Color(val.color.r, val.color.g, val.color.b, 0), 0.8f).OnComplete(() => {
                DestroyImmediate(miss);
            }); //消失
        });//字体放大

  
    }

    
    //防御技能挂载
    void SetupDefenseSkills(FightOrder fromItem)
    {
        if (fromItem.DefenseSkill != null)
        {
            string skillname = fromItem.DefenseSkill.Title;
            if (!string.IsNullOrEmpty(skillname))
            {
                switch (fromItem.DefenseSkill.Name.ToLower())
                {
                    case "holyshield":
                        HolyShield holyshield = fromItem.Tank.AddComponent<HolyShield>();
                        holyshield.Total = 4;
                        holyshield.Effected = 0;
                        holyshield.Tank = fromItem.Tank;
                        break;
                    case "heavyarmor":
                        HeavyArmor heavyArmor = fromItem.Tank.AddComponent<HeavyArmor>();
                        heavyArmor.Value = 0.7f;
                        heavyArmor.Effected = 0;
                        heavyArmor.Tank = fromItem.Tank;
                        break;
                    case "dodge":
                        Dodge dodge = fromItem.Tank.AddComponent<Dodge>();
                        dodge.Value = 0.4f;
                        dodge.Effected = 0;
                        dodge.Tank = fromItem.Tank;
                        break;
                    case "revive":
                        Revive revive = fromItem.Tank.AddComponent<Revive>();
                        revive.Total = 1;
                        revive.Effected = 0;
                        revive.Tank = fromItem.Tank;
                        break;
                    case "recover":
                        Recover recover = fromItem.Tank.AddComponent<Recover>();
                        recover.Value = 0.1f;
                        recover.Effected = 0;
                        recover.Tank = fromItem.Tank;
                        break;
                    case "taunt":
                        Taunt taunt = fromItem.Tank.AddComponent<Taunt>();
                        //taunt.Value = 0.1f;
                        taunt.Effected = 0;
                        taunt.Tank = fromItem.Tank;
                        break;
                    case "deathsound":
                        DeathSound deathsound = fromItem.Tank.AddComponent<DeathSound>();
                        deathsound.Total = 1;
                        deathsound.Effected = 0;
                        deathsound.Tank = fromItem.Tank;
                        break;
                    case "revenge":
                        Revenge revenge = fromItem.Tank.AddComponent<Revenge>();
                        //revenge.Total = 1;
                        revenge.Effected = 0;
                        revenge.Tank = fromItem.Tank;
                        break;
                    case "antiinjury":
                        AntiInjury antiinjury = fromItem.Tank.AddComponent<AntiInjury>();
                        antiinjury.Value = 0.5f;
                        antiinjury.Effected = 0;
                        antiinjury.Tank = fromItem.Tank;
                        break;
                    default:
                        //获取技能
                        GameObject skillprefab = CommonHelper.GetPrefabs("skill", "Defense/" + skillname);
                        GameObject skill = Instantiate(skillprefab, fromItem.Tank.transform.parent.parent, false);
                        skill.SetActive(true);
                        break;
                }
             
            }
        }
    }


    void SetupAttackSkills(FightOrder fromItem)
    {
        if (fromItem.AttackSkill != null)
        {
            string skillname = fromItem.AttackSkill.Title;
            if (!string.IsNullOrEmpty(skillname))
            {
                switch (fromItem.AttackSkill.Name.ToLower())
                {
                    //连击
                    case "batter":
                        Batter batter = fromItem.Tank.AddComponent<Batter>();
                        batter.Value = 3;//连续攻击3次
                        batter.Effected = 0;
                        batter.FromTank = fromItem.Tank;
                        break;
                    //重击
                    case "thump":
                        Thump thump = fromItem.Tank.AddComponent<Thump>();
                        thump.Value = 2; //2倍攻击
                        thump.Effected = 0;
                        thump.FromTank = fromItem.Tank;
                        break;

                }
            }
        }
    }


    //技能触发 攻击者、被攻击者
    void AttackSkills(FightOrder fromItem,List<FightOrder> targets)
    {
        if (fromItem.AttackSkill == null) 
        {
            return;
        }
        string skillname = fromItem.AttackSkill.Title;
        Weapon wea = fromItem.Tank.GetComponent<Weapon>();

        //获取技能
        GameObject skillprefab = CommonHelper.GetPrefabs("skill", "Attack/" + skillname);
        foreach (FightOrder target in targets)
        {
            GameObject skill = Instantiate(skillprefab, target.Tank.transform.parent.parent, false);
            skill.SetActive(true);

            StartCoroutine(CommonHelper.DelayToInvokeDo(() => { DestroyImmediate(skill); }, 5f));
            //for (int i = 1; i <= hitcount - 1; i++)
            //{
            //    float delay = i * 0.5f; //延迟时间
            //    StartCoroutine(CommonHelper.DelayToInvokeDo(() => { SimpleAttack(fromItem, targets, true); }, delay));
            //}
        }


        switch (skillname)
        {
            //case "连击":
            //    //连续发起多次攻击（2-6次）次数在获得是就已固定
            //    int hitcount =3;
            //    foreach (FightOrder target in targets)
            //    {
            //        GameObject skill = Instantiate(skillprefab, target.Tank.transform.parent.parent, false);
            //        skill.SetActive(true);
            //        Destroy(skill, 5f);

            //        for (int i = 1; i <= hitcount-1; i++)
            //        {
            //           float delay = i * 0.5f; //延迟时间
            //           StartCoroutine(CommonHelper.DelayToInvokeDo(()=> { SimpleAttack(fromItem, targets, true); }, delay));
            //        }
            //    }
            //    break;
            case "重击":
                //对敌方造成多倍攻击伤害（200 % -600 %）
                

                break;
            case "穿甲":

                break;
        }

        //Destroy(skill, 5f);
    }



}
