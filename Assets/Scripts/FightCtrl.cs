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

    List<FightItem> OrderList;

    //public int index = 0;
    public int CountDownTime = 5; //倒计时
    public int Round = 0; //回合数
    public int FightIndex = -1; //当前战斗方的下标
    
    List<FightItem> DuelList = new List<FightItem>(); //决斗中的坦克

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
            
            SceneManager.LoadSceneAsync("IndexScene");
            System.GC.Collect();
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
       
            foreach (FightItem fightItem in OrderList)
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
            GameObject tank =null; 

            if (item.Code.CompareTo("00012") >= 0 && item.Code.CompareTo("00024") <= 0)
            {
                tank = Instantiate(list.Find(u => u.name == item.Code));
                CommonHelper.ReplaceMaterial(tank, "black");
            }
            else if (item.Code.CompareTo("00025") >= 0 && item.Code.CompareTo("00036") <= 0)
            {
                tank = Instantiate(list.Find(u => u.name == (Convert.ToInt32(item.Code)-13).ToString().PadLeft(5,'0')));
                CommonHelper.ReplaceMaterial(tank, "blue");
            }
            else if (item.Code.CompareTo("00037") >= 0 && item.Code.CompareTo("00049") <= 0)
            {
                tank = Instantiate(list.Find(u => u.name == (Convert.ToInt32(item.Code) - 25).ToString().PadLeft(5, '0')));
                CommonHelper.ReplaceMaterial(tank, "yellow");
            }
            else if (item.Code.CompareTo("00050") >= 0 && item.Code.CompareTo("00062") <= 0)
            {
                tank = Instantiate(list.Find(u => u.name == (Convert.ToInt32(item.Code) - 38).ToString().PadLeft(5, '0')));
                CommonHelper.ReplaceMaterial(tank, "red");
            }

            tank.AddComponent<Weapon>();
            tank.SetActive(false);
            tank.transform.SetParent(PlayerA_Pos[i].transform.Find("Tank"), false);

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
            CommonHelper.ReplaceMaterial(tank,"black");
            PlayerB_Tanks.Add(tank);
        }

    }

    //开启传送阵
    public void StartTransfer()
    {
        GameObject transfer = CommonHelper.GetPrefabs("skill", "Transfer");

     
        foreach (GameObject pos in PlayerA_Pos)
        {
            int num = PlayerA_Pos.IndexOf(pos);
            StartCoroutine(CommonHelper.DelayToInvokeDo(() =>
            {
                GameObject newObj = Instantiate(transfer);
                newObj.transform.SetParent(pos.transform, false);
                newObj.SetActive(true);
                StartCoroutine(CommonHelper.DelayToInvokeDo(() => { DestroyImmediate(newObj); }, 5f));
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
                StartCoroutine(CommonHelper.DelayToInvokeDo(() => { DestroyImmediate(newObj); }, 5f));
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
        OrderList = new List<FightItem>();
        foreach (TankProperty item in PlayerA.TankList)
        {
            OrderList.Add(new FightItem {
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
                DefenseSkill = item.DefenseSkill,
                Buffs = new List<Buff>()
            });; ;
        }
        foreach (TankProperty item in PlayerB.TankList)
        {
            OrderList.Add(new FightItem
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
                DefenseSkill = item.DefenseSkill,
                Buffs = new List<Buff>()

            });
        }

        OrderList = OrderList.OrderByDescending(u => u.Speed).ToList();

        foreach (FightItem item in OrderList)
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
    /// <param name="count">目标数</param>
    /// <param name="ignoreTaunt">是否无视嘲讽</param>
    /// <returns></returns>
    public List<FightItem> SetTarget(FightItem fightOrder)
    {
        List<FightItem> list = new List<FightItem>();
  
        int count = 1;
        //对方存活坦克
        List<FightItem> livelist = OrderList.FindAll(u => u.Player.Name != fightOrder.Player.Name && u.Death == false && DuelList.Contains(u)==false);


        string atkName = "";
        if (fightOrder.AttackSkill != null)
        {
            atkName = fightOrder.AttackSkill.Name;
        }
        if (atkName.ToLower() == "scattering")
        {
            count = fightOrder.Tank.GetComponent<Scattering>().Value;
        }


        //逐个击破 攻击攻击力最弱的
        if (atkName.ToLower() == "destroyweak")
        {
            //按攻击力排序，第一个就是攻击力最弱的
            FightItem fo = livelist.OrderBy(u => u.Attack).FirstOrDefault();
            if (fo != null)
            {
                list.Add(fo);
            }
        }
        //攻无不克 攻击攻击力最强的
        else if (atkName.ToLower() == "invincible")
        {
            //按攻击力排序，第一个就是攻击力最弱的
            FightItem fo = livelist.OrderByDescending(u => u.Attack).FirstOrDefault();
            if (fo != null)
            {
                list.Add(fo);
            }
        }
        else
        {
            //具有帝国指令debuff的敌方
            List<FightItem> directive = new List<FightItem>();
            foreach (FightItem item in livelist)
            {
                if (item.Buffs.FindIndex(u => u.Disable == false && u.Name == "empiredirective") > -1)
                {
                    directive.Add(item);
                }
            }


            //具有嘲讽技能的敌方
            List<FightItem> tauntlist = new List<FightItem>();
            foreach (FightItem item in livelist)
            {
                if (item.DefenseSkill != null)
                {
                    if (item.DefenseSkill.Name.ToLower() == "taunt")
                    {
                        tauntlist.Add(item);
                    }
                }
            }


            //优先级：帝国指令>嘲讽
            if (directive.Count > 0)
            {
                if (count == directive.Count)
                {
                    list.AddRange(directive);
                }
                else if (count > directive.Count)
                {
                    //先全选帝国指令
                    list.AddRange(directive);
                }
                //小于帝国指令总数
                else
                {
                    list.AddRange(directive.GetRange(0, count));
                }
            }



            //从嘲讽中选
            if (count > list.Count)
            {
                //嘲讽列表中排除帝国指令
                List<FightItem> tauntlist2 = tauntlist.FindAll(u => directive.Contains(u) == false);
                if (tauntlist2.Count > 0)
                {
                    //足够
                    if (count - list.Count == tauntlist2.Count)
                    {
                        list.AddRange(tauntlist2);
                    }
                    //不够
                    else if (count - list.Count > tauntlist2.Count)
                    {
                        list.AddRange(tauntlist2);
                    }
                    //多余
                    else
                    {
                        list.AddRange(tauntlist2.GetRange(0, count - list.Count));
                    }

                }
            }


            //从其余存活中选择
            if (count > list.Count)
            {
                List<FightItem> livelist2 = livelist.FindAll(u => directive.Contains(u) == false && tauntlist.Contains(u) == false);

                if (livelist2.Count > 0)
                {
                    //足够
                    if (count - list.Count == livelist2.Count)
                    {
                        list.AddRange(livelist2);
                    }
                    //不够
                    else if (count - list.Count > livelist2.Count)
                    {
                        list.AddRange(livelist2);
                    }
                    //多余
                    else
                    {
                        list.AddRange(livelist2.GetRange(0, count - list.Count));
                    }

                }
            }
        }

        return list;
    }

   

    /// <summary>
    /// 下个回合
    /// </summary>
    void NextRound() 
    {
        if (Round == 0) { Round++; }
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
    /// <param name="type">0：减血 1：加血 2:魔法伤害</param>
    /// <param name="crit">是否暴击</param>
    void ShowBlood(GameObject target,float value,int type=0,bool crit=false)
    {
        GameObject txtblood = UIPanel.transform.Find("txt_blood").gameObject;
        GameObject newblood = Instantiate(txtblood, UIPanel.transform, false);
        newblood.transform.position = Camera.main.WorldToScreenPoint(target.transform.position + new Vector3(0, CommonHelper.GetRandom(3,7), 0));
        Text val = newblood.transform.Find("txt_val").GetComponent<Text>();
        val.text =(type==1?"+":"-")+ value.ToString();
        switch (type)
        {
            case 0:
                val.color = Color.white;
                break;
            case 1:
                val.color = Color.green;
                break;
            case 2:
                val.color = Color.magenta;
                break;
        }
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
    void RefreshBloodBar(FightItem fightItem)
    {
        GameObject bloodbar = UIPanel.transform.Find("bloodbar").gameObject;
        Transform bar = UIPanel.transform.Find("bloodbar_" + fightItem.Code);
        Slider slider;
        if (bar == null)
        {
            bar = Instantiate(bloodbar, UIPanel.transform, false).transform;
            bar.gameObject.name = "bloodbar_" + fightItem.Code;
            StartCoroutine(CommonHelper.DelayToInvokeDo(() => { bar.gameObject.SetActive(true); }, 6f));//延迟显示血条
            slider = bar.GetComponent<Slider>();
            TankProperty tp = fightItem.Player.TankList.Find(u => u.TankObject == fightItem.Tank);
            slider.maxValue = fightItem.Player.Hero.Blood + tp.Blood;
            slider.minValue = 0;
            bar.transform.Find("atk/txt_val").GetComponent<Text>().text = (fightItem.Player.Hero.Attack + tp.Attack).ToString();
            bar.transform.Find("speed/txt_val").GetComponent<Text>().text = (fightItem.Player.Hero.Speed + tp.Speed).ToString();
            bar.transform.Find("crit/txt_val").GetComponent<Text>().text = (fightItem.Player.Hero.CritRate + tp.CritRate).ToString()+"%";
            bar.transform.Find("code/txt_val").GetComponent<Text>().text = "#" + tp.Code;
            if (fightItem.AttackSkill != null)
            {
                bar.transform.Find("skills/atk").GetComponent<RawImage>().texture = CommonHelper.LoadSkillImage(fightItem.AttackSkill.Name);
                
            }
            bar.transform.Find("skills/atk").gameObject.SetActive(fightItem.AttackSkill != null);
            if (fightItem.DefenseSkill != null)
            {
                bar.transform.Find("skills/def").GetComponent<RawImage>().texture = CommonHelper.LoadSkillImage(fightItem.DefenseSkill.Name);
                
            }
            bar.transform.Find("skills/def").gameObject.SetActive(fightItem.DefenseSkill != null);
        }

       // bar.position = Camera.main.WorldToScreenPoint(fightItem.Tank.transform.position + new Vector3(0, 13, 0));
        slider = bar.GetComponent<Slider>();
        slider.value =  fightItem.Blood<=0?0: fightItem.Blood;
        bar.Find("Fill Area/txt_Val").GetComponent<Text>().text = slider.value.ToString();
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
        FightItem fightitem = OrderList[index];

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
                        int addBlood = (int)Math.Round( maxBlood * recover.Value);
                        if (addBlood + fightitem.Blood > maxBlood)
                        {
                            fightitem.Blood = maxBlood;
                        }
                        else
                        {
                            fightitem.Blood += addBlood;
                        }
                        ShowBlood(fightitem.Tank, addBlood, 1);
                        RefreshBloodBar(fightitem);
                    }
                }
            }
        }

        #endregion

        #region 判定是否有Debuff
        //燃烧
        Buff rs = fightitem.Buffs.Find(u => u.Name == "combustion" && u.Disable == false);

        if (rs != null)
        {
            rs.EffectCount++;
            float maxBlood = fightitem.Player.Hero.Blood + fightitem.Player.TankList.Find(u => u.TankObject == fightitem.Tank).Blood;
            int rsAtk = (int)Math.Round(maxBlood * rs.Value);
            fightitem.Blood -= rsAtk;
            ShowBlood(fightitem.Tank, rsAtk, 2);
            RefreshBloodBar(fightitem);
            CheckDeath(fightitem);
            
        }


        #endregion

        if (fightitem.Death == false)
        {
            //冻结
            Buff dj = fightitem.Buffs.Find(u => u.Name == "frozen" && u.Disable == false);
            if (dj != null)
            {
                dj.EffectCount = 1;
                dj.Disable = true;
                StartCoroutine(CommonHelper.DelayToInvokeDo(() => { DestroyImmediate(dj.BuffObject); }, 1f));
                //游戏是否结束
                StartCoroutine(CommonHelper.DelayToInvokeDo(() =>
                {
                    CheckGameStatus(index);
                }, 0.3f));
            }
            else
            {
                List<FightItem> targets = SetTarget(fightitem);
                int targetIndex = 0;
                foreach (FightItem item in targets)
                {
                    //普通攻击
                    StartCoroutine(CommonHelper.DelayToInvokeDo(() => { SimpleAttack(fightitem, item, targets.Count>1?true:false); }, targetIndex * 0.5f));
                    targetIndex++;
                }
                //游戏是否结束
                StartCoroutine(CommonHelper.DelayToInvokeDo(() =>
                {
                    CheckGameStatus(index);
                }, targets.Count * 0.5f));
            }
        }
        else 
        {
            //游戏是否结束
            StartCoroutine(CommonHelper.DelayToInvokeDo(() =>
            {
                CheckGameStatus(index);
            }, 1f));
        }
       


    }


    //普通攻击
    void SimpleAttack(FightItem fightitem, FightItem target,bool isskill=false)
    {
        if (fightitem.Death == false)
        {
            Weapon w = fightitem.Tank.GetComponent<Weapon>();
            w.target = new List<GameObject>() { target.Tank };
            w.Shoot(fightitem.Tank);
            //被攻击坦克普通攻击血量减少操作


            bool iscrit = IsCrit(fightitem.CritRate);
            int attack = (int)Math.Round(iscrit ? fightitem.Attack * 2 : fightitem.Attack);



            #region Buff效果生效
            //穿甲
            Buff buff = target.Buffs.Find(u => u.Name == "piercing" && u.Disable == false);
            if (buff != null)
            {
                attack = (int)Math.Round(attack * (1 + buff.Value));
                buff.Disable = true;
                buff.EffectCount++;
                StartCoroutine(CommonHelper.DelayToInvokeDo(() => { DestroyImmediate(buff.BuffObject); }, 1f));
            }

            #endregion

            #region 防御技能触发判定
            if (target.DefenseSkill != null)
            {
                //禁魔 有禁魔状态的不能使用技能
                Buff jinmo = target.Buffs.Find(u => u.Name == "forbidden" && u.Disable == false);
                if (jinmo != null)
                {
                    jinmo.EffectCount++;
                }
                else
                {
                   // StartCoroutine(CommonHelper.DelayToInvokeDo(() =>
                   // {
                        switch (target.DefenseSkill.Name.ToLower())
                        {

                            //圣盾
                            case "holyshield":
                                HolyShield holyShield = target.Tank.GetComponent<HolyShield>();
                                if (holyShield != null)
                                {
                                    if (holyShield.Trigger() == true)
                                    {
                                        CommonHelper.ShowSkillIcon(target.Tank, target.DefenseSkill.Name, UIPanel);
                                        return;
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
                                        CommonHelper.ShowSkillIcon(target.Tank, target.DefenseSkill.Name, UIPanel);
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
                                        CommonHelper.ShowSkillIcon(target.Tank, target.DefenseSkill.Name, UIPanel);
                                        //闪避触发
                                        //未命中
                                        showMiss(target);
                                        return;
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
                                        CommonHelper.ShowSkillIcon(target.Tank, target.DefenseSkill.Name, UIPanel);
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
                                        CommonHelper.ShowSkillIcon(target.Tank, target.DefenseSkill.Name, UIPanel);
                                    //攻击伤害来源的坦克
                                    StartCoroutine(CommonHelper.DelayToInvokeDo(() => { SimpleAttack(target, fightitem, true); }, 0.5f)); 
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
                                        CommonHelper.ShowSkillIcon(target.Tank, target.DefenseSkill.Name, UIPanel);
                                        //反伤
                                        int replay_atk = (int)Math.Round(attack * antiinjury.Value);
                                        fightitem.Blood -= replay_atk;
                                        ShowBlood(fightitem.Tank, replay_atk, 0, false);
                                        RefreshBloodBar(fightitem);

                                    }
                                }
                                break;

                        }
                    //}, 0.3f));
                }
            }

            #endregion

            #region 攻击技能特效触发
            if (fightitem.AttackSkill != null && isskill == false)
            {
                //禁魔 有禁魔状态的不能使用技能
                Buff jinmo = fightitem.Buffs.Find(u => u.Name == "forbidden" && u.Disable == false);
                if (jinmo != null)
                {
                    jinmo.EffectCount++;
                }
                else
                {
                    switch (fightitem.AttackSkill.Name.ToLower())
                    {
                        case "batter":
                            Batter batter = fightitem.Tank.GetComponent<Batter>();
                            if (batter != null)
                            {
                                batter.TargetTank = target.Tank;
                                if (batter.EffectAttack(() => { SimpleAttack(fightitem, target, true); }) == true)
                                {
                                    CommonHelper.ShowSkillIcon(fightitem.Tank, fightitem.AttackSkill.Name, UIPanel);
                                    return;
                                }
                            }
                            break;
                        case "thump":
                            Thump thump = fightitem.Tank.GetComponent<Thump>();
                            if (thump != null)
                            {
                                CommonHelper.ShowSkillIcon(fightitem.Tank, fightitem.AttackSkill.Name, UIPanel);
                                thump.TargetTank = target.Tank;
                                thump.EffectAttack();

                                attack = (int)Math.Round(attack * thump.Value);
                            }
                            break;
                        case "piercing":
                            Piercing piercing = fightitem.Tank.GetComponent<Piercing>();
                            if (piercing != null)
                            {
                                CommonHelper.ShowSkillIcon(fightitem.Tank, fightitem.AttackSkill.Name, UIPanel);
                                piercing.TargetTank = target.Tank;
                                piercing.EffectAttack();
                                //buff标记
                                GameObject pojia = Instantiate(Resources.Load<GameObject>("Skills/Buff/pojia"), target.Tank.transform.parent, false);
                                //添加buff
                                target.Buffs.Add(new Buff
                                {
                                    TankCode = target.Code,
                                    TankObject = target.Tank,
                                    FromTankCode = fightitem.Code,
                                    FromTankObject = fightitem.Tank,
                                    BuffObject = pojia,
                                    Name = "piercing",
                                    BuffType = "debuff",
                                    Value = piercing.Value,
                                    EffectCount = 0,
                                    Disable = false
                                });
                            }
                            break;
                        //燃烧
                        case "combustion":
                            Combustion combustion = fightitem.Tank.GetComponent<Combustion>();
                            if (combustion != null)
                            {
                                CommonHelper.ShowSkillIcon(fightitem.Tank, fightitem.AttackSkill.Name, UIPanel);
                                combustion.TargetTank = target.Tank;
                                combustion.EffectAttack();
                                //只挂载一个燃烧效果
                                if (target.Buffs.Find(u => u.Name == "combustion" && u.Disable == false) == null)
                                {
                                    //buff标记
                                    
                                    GameObject ranshao = GameObject.Instantiate(CommonHelper.GetPrefabs("skill", "Attack/燃烧"), target.Tank.transform.parent, false);
                                    //添加buff
                                    target.Buffs.Add(new Buff
                                    {
                                        TankCode = target.Code,
                                        TankObject = target.Tank,
                                        FromTankCode = fightitem.Code,
                                        FromTankObject = fightitem.Tank,
                                        BuffObject = ranshao,
                                        Name = "combustion",
                                        BuffType = "debuff",
                                        Value = combustion.Value,
                                        EffectCount = 0,
                                        Disable = false
                                    });
                                }
                            }
                            break;
                        //散射
                        case "scattering":
                            Scattering scattering = fightitem.Tank.GetComponent<Scattering>();
                            if (scattering != null)
                            {
                                CommonHelper.ShowSkillIcon(fightitem.Tank, fightitem.AttackSkill.Name, UIPanel);
                                scattering.TargetTank = target.Tank;
                                scattering.EffectAttack();

                            }
                            break;
                        //逐个击破
                        case "destroyweak":
                            Destroyweak destroyweak = fightitem.Tank.GetComponent<Destroyweak>();
                            if (destroyweak != null)
                            {
                                CommonHelper.ShowSkillIcon(fightitem.Tank, fightitem.AttackSkill.Name, UIPanel);
                                destroyweak.TargetTank = target.Tank;
                                destroyweak.EffectAttack();

                            }
                            break;
                        //攻无不克
                        case "invincible":
                            Invincible invincible = fightitem.Tank.GetComponent<Invincible>();
                            if (invincible != null)
                            {
                                CommonHelper.ShowSkillIcon(fightitem.Tank, fightitem.AttackSkill.Name, UIPanel);
                                invincible.TargetTank = target.Tank;
                                invincible.EffectAttack();

                            }
                            break;
                        //必杀
                        case "kill":
                            Kill kill = fightitem.Tank.GetComponent<Kill>();
                            if (kill != null)
                            {

                                //概率判定成功
                                if (CommonHelper.IsHit(kill.Value))
                                {
                                    CommonHelper.ShowSkillIcon(fightitem.Tank, fightitem.AttackSkill.Name, UIPanel);
                                    kill.TargetTank = target.Tank;
                                    kill.EffectAttack();
                                    attack = (int)target.Blood;

                                }

                            }
                            break;
                        //吸血
                        case "suckblood":
                            SuckBlood suckblood = fightitem.Tank.GetComponent<SuckBlood>();
                            if (suckblood != null)
                            {

                                CommonHelper.ShowSkillIcon(fightitem.Tank, fightitem.AttackSkill.Name, UIPanel);
                                suckblood.TargetTank = target.Tank;
                                suckblood.EffectAttack();
                                int suckvalue = (int)Math.Round(attack * suckblood.Value);
                                float maxBlood = fightitem.Player.Hero.Blood + fightitem.Player.TankList.Find(u => u.TankObject == fightitem.Tank).Blood;
                                if (suckvalue + fightitem.Blood > maxBlood)
                                {
                                    fightitem.Blood = maxBlood;
                                }
                                else
                                {
                                    fightitem.Blood += suckvalue;
                                }
                                ShowBlood(fightitem.Tank, suckvalue, 1);
                                RefreshBloodBar(fightitem);

                            }
                            break;
                        //冰冻
                        case "frozen":
                            Frozen frozen = fightitem.Tank.GetComponent<Frozen>();
                            if (frozen != null)
                            {
                                CommonHelper.ShowSkillIcon(fightitem.Tank, fightitem.AttackSkill.Name, UIPanel);
                                frozen.TargetTank = target.Tank;
                                frozen.EffectAttack();
                                //只挂载一个冰冻效果
                                if (target.Buffs.Find(u => u.Name == "frozen" && u.Disable == false) == null)
                                {
                                    //buff标记
                                    GameObject dongjie = GameObject.Instantiate(CommonHelper.GetPrefabs("skill", "Attack/冰冻"), target.Tank.transform.parent, false);
                                    //添加buff
                                    target.Buffs.Add(new Buff
                                    {
                                        TankCode = target.Code,
                                        TankObject = target.Tank,
                                        FromTankCode = fightitem.Code,
                                        FromTankObject = fightitem.Tank,
                                        BuffObject = dongjie,
                                        Name = "frozen",
                                        BuffType = "debuff",
                                        Value = frozen.Value,
                                        EffectCount = 0,
                                        Disable = false
                                    });
                                }
                            }
                            break;
                        //禁魔
                        case "forbidden":
                            Forbidden forbidden = fightitem.Tank.GetComponent<Forbidden>();
                            if (forbidden != null)
                            {
                                CommonHelper.ShowSkillIcon(fightitem.Tank, fightitem.AttackSkill.Name, UIPanel);
                                forbidden.TargetTank = target.Tank;
                                forbidden.EffectAttack();
                                //只挂载一个冰冻效果
                                if (target.Buffs.Find(u => u.Name == "forbidden" && u.Disable == false) == null)
                                {
                                    //buff标记
                                    GameObject chengmo = Instantiate(Resources.Load<GameObject>("Skills/Buff/chengmo"), target.Tank.transform.parent, false);
                                    //添加buff
                                    target.Buffs.Add(new Buff
                                    {
                                        TankCode = target.Code,
                                        TankObject = target.Tank,
                                        FromTankCode = fightitem.Code,
                                        FromTankObject = fightitem.Tank,
                                        BuffObject = chengmo,
                                        Name = "forbidden",
                                        BuffType = "debuff",
                                        Value = forbidden.Value,
                                        EffectCount = 0,
                                        Disable = false
                                    });
                                }
                            }
                            break;
                        //协同开火
                        case "cofire":
                            Cofire cofire = fightitem.Tank.GetComponent<Cofire>();
                            if (cofire != null)
                            {
                                //从存活的坦克中选择指定数量的进行一次攻击
                                List<FightItem> livelist = OrderList.FindAll(u => u.Player.Name == fightitem.Player.Name && u.Death == false && u.Code!=fightitem.Code);
                                List<FightItem> atklist = new List<FightItem>();
                                if (cofire.Value >= livelist.Count)
                                {
                                    atklist = livelist;
                                }
                                else
                                {
                                    atklist = livelist.GetRange(0, cofire.Value);
                                }
                                if (atklist.Count > 0)
                                {
                                    CommonHelper.ShowSkillIcon(fightitem.Tank, fightitem.AttackSkill.Name, UIPanel);
                                    cofire.TargetTank = target.Tank;
                                    cofire.EffectAttack(atklist);

                         
                                    for (int i = 0; i < atklist.Count; i++)
                                    {
                                        FightItem item = atklist[i];
                                        StartCoroutine(CommonHelper.DelayToInvokeDo(() => { SimpleAttack(item, SetTarget(item)[0], true); }, (i + 1) * 0.3f));
                                    }
                                    
                                }
                            }
                            break;
                        //帝国指令
                        case "empiredirective":
                            EmpireDirective empiredirective = fightitem.Tank.GetComponent<EmpireDirective>();
                            if (empiredirective != null)
                            {

                                CommonHelper.ShowSkillIcon(fightitem.Tank, fightitem.AttackSkill.Name, UIPanel);
                                empiredirective.TargetTank = target.Tank;
                                //empiredirective.EffectAttack();
                                //只挂载一个指令效果
                                if (target.Buffs.Find(u => u.Name == "empiredirective" && u.Disable == false) == null)
                                {
                                    //buff标记
                                    GameObject ed = GameObject.Instantiate(CommonHelper.GetPrefabs("skill", "Attack/帝国指令"), target.Tank.transform.parent, false);
                                    
                                    //添加buff
                                    target.Buffs.Add(new Buff
                                    {
                                        TankCode = target.Code,
                                        TankObject = target.Tank,
                                        FromTankCode = fightitem.Code,
                                        FromTankObject = fightitem.Tank,
                                        BuffObject = ed,
                                        Name = "empiredirective",
                                        BuffType = "debuff",
                                        EffectCount = 0,
                                        Disable = false
                                    });
                                }

                            }
                            break;
                        //决斗
                        case "duel":
                            Duel duel = fightitem.Tank.GetComponent<Duel>();
                            if (duel != null)
                            {
                                CommonHelper.ShowSkillIcon(fightitem.Tank, fightitem.AttackSkill.Name, UIPanel);
                                duel.TargetTank = target.Tank;
                                duel.EffectAttack();
                                StartCoroutine(CommonHelper.DelayToInvokeDo(() => { DuelFight(target, fightitem); }, 0.5f));
                                if (DuelList == null) 
                                {
                                    DuelList = new List<FightItem>();
                                }
                                DuelList.Add(fightitem);
                                DuelList.Add(target);
                            }
                            break;
                    }
                }
            }
            #endregion

            target.Blood -= attack;
            ShowBlood(target.Tank, attack, 0, iscrit);
            RefreshBloodBar(target);

            CheckDeath(target);
        }
        //if (times > 1) 
        //{
        //    StartCoroutine(CommonHelper.DelayToInvokeDo(() =>
        //    {
        //        SimpleAttack(fightitem, targets, times - 1);
        //    }, 0.5f));
        //}
    }

    //决斗递归函数
    void DuelFight(FightItem attacker,FightItem target) 
    {
        Weapon w = attacker.Tank.GetComponent<Weapon>();
        w.target = new List<GameObject>() { target.Tank };
        w.Shoot(attacker.Tank);
        bool iscrit = IsCrit(attacker.CritRate);
        int attack = (int)Math.Round(iscrit ? attacker.Attack * 2 : attacker.Attack);
        target.Blood -= attack;
        ShowBlood(target.Tank, attack, 0, iscrit);
        RefreshBloodBar(target);

        if (CheckDeath(target) == false)
        {
            StartCoroutine(CommonHelper.DelayToInvokeDo(() => { DuelFight(target, attacker); }, 0.5f));
        }
        else
        {
            StartCoroutine(CommonHelper.DelayToInvokeDo(() =>
            {
                DuelList.Remove(attacker);
                DuelList.Remove(target);
            }, 1f));
        }
    }


    /// <summary>
    /// 检查游戏状态 继续或是结束
    /// </summary>
    void CheckGameStatus(int index)
    {

        //判断是否一方全部阵亡，若全部阵亡则不再执行攻击，否则继续。当一个回合结束后，开始下一轮
        if (OrderList.FindAll(u => u.Player.Name == "玩家A" && u.Death == false).Count == 0 || OrderList.FindAll(u => u.Player.Name == "玩家B" && u.Death == false).Count == 0)
        {

            UIPanel.transform.Find("txt_round_center").gameObject.SetActive(true);
            UIPanel.transform.Find("txt_round_center").SetAsLastSibling();
            if (OrderList.FindAll(u => u.Player.Name == "玩家A" && u.Death == false).Count == 0)
            {
                UIPanel.transform.Find("txt_round_center").GetComponent<Text>().text = "Defeated";
            }
            else
            {
                UIPanel.transform.Find("txt_round_center").GetComponent<Text>().text = "VICTORY";
            }

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


    /// <summary>
    /// 判定是否死亡
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    bool CheckDeath(FightItem target)
    {
        if (target.Blood <= 0)
        {
            target.Blood = 0;
            target.Death = true;
            //死亡爆炸效果
            GameObject death = Instantiate(CommonHelper.GetPrefabs("skill", "Death"));
            death.SetActive(true);
            death.transform.SetParent(target.Tank.transform.parent.parent, false);
            //StartCoroutine(CommonHelper.DelayToInvokeDo(() => { DestroyImmediate(death); }, 4f));
            Animator t_animator = target.Tank.transform.GetChild(0).GetComponent<Animator>();
            if (t_animator != null && t_animator.gameObject.activeInHierarchy)
            {
                t_animator.Play("Death");
            }

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
                            target.Death = false;
                            target.Blood = (int)((target.Player.Hero.Blood + target.Player.TankList.Find(u => u.TankObject == target.Tank).Blood) * 0.2f);
                            CommonHelper.ShowSkillIcon(target.Tank, target.DefenseSkill.Name, UIPanel);
                            StartCoroutine(CommonHelper.DelayToInvokeDo(() =>
                            {
                                //恢复20%血量，移除死亡效果，恢复正常状态
                                Animator animator = target.Tank.transform.GetChild(0).GetComponent<Animator>();
                                if (animator != null && animator.gameObject.activeInHierarchy)
                                {
                                    animator.Play("Idle");
                                }

                                DestroyImmediate(death);
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
                            CommonHelper.ShowSkillIcon(target.Tank, target.DefenseSkill.Name, UIPanel);
                            //亡语效果

                        }
                    }
                }

            }
            #endregion

            //死亡后移除身上的特效
            if (target.Death == true)
            {
                if (target.DefenseSkill != null)
                {
                    Transform sk = target.Tank.transform.parent.Find(target.DefenseSkill.Title + "(Clone)");
                    if (sk != null)
                    {
                        DestroyImmediate(sk.gameObject);
                    }
                }
                //移除buff效果
                if (target.Buffs.Count > 0)
                {
                    foreach (Buff item in target.Buffs.FindAll(u=>u.Disable==false))
                    {
                        item.Disable = true;
                        DestroyImmediate(item.BuffObject);
                    }
                }
            }

            
        }
        return target.Death;
    }


    //显示miss 未命中
    void showMiss(FightItem target)
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
    void SetupDefenseSkills(FightItem fromItem)
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
                        //嘲讽
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


    void SetupAttackSkills(FightItem fromItem)
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
                    //穿甲
                    case "piercing":
                        Piercing piercing = fromItem.Tank.AddComponent<Piercing>();
                        piercing.Value = 0.5f; //50%附加伤害
                        piercing.Effected = 0;
                        piercing.FromTank = fromItem.Tank;
                        break;
                    //燃烧
                    case "combustion":
                        Combustion combustion = fromItem.Tank.AddComponent<Combustion>();
                        combustion.Value = 0.1f; //10%附加伤害
                        combustion.Effected = 0;
                        combustion.FromTank = fromItem.Tank;
                        break;
                    //散射
                    case "scattering":
                        Scattering scattering = fromItem.Tank.AddComponent<Scattering>();
                        scattering.Value = 3;//同时攻击3个目标
                        scattering.Effected = 0;
                        scattering.FromTank = fromItem.Tank;
                        break;
                    //逐个击破
                    case "destroyweak":
                        Destroyweak destroyweak = fromItem.Tank.AddComponent<Destroyweak>();
                        destroyweak.Effected = 0;
                        destroyweak.FromTank = fromItem.Tank;
                        break;
                    //攻无不克
                    case "invincible":
                        Invincible invincible = fromItem.Tank.AddComponent<Invincible>();
                        invincible.Effected = 0;
                        invincible.FromTank = fromItem.Tank;
                        break;
                    //必杀
                    case "kill":
                        Kill kill = fromItem.Tank.AddComponent<Kill>();
                        kill.Value = 25f;
                        kill.Effected = 0;
                        kill.FromTank = fromItem.Tank;
                        break;
                    //吸血
                    case "suckblood":
                        SuckBlood suckblood = fromItem.Tank.AddComponent<SuckBlood>();
                        suckblood.Value = 0.4f;
                        suckblood.Effected = 0;
                        suckblood.FromTank = fromItem.Tank;
                        break;
                    //冰冻
                    case "frozen":
                        Frozen frozen = fromItem.Tank.AddComponent<Frozen>();
                        frozen.Value =1;
                        frozen.Effected = 0;
                        frozen.FromTank = fromItem.Tank;
                        break;
                    //禁魔
                    case "forbidden":
                        Forbidden forbidden = fromItem.Tank.AddComponent<Forbidden>();
                        forbidden.Value = 0;
                        forbidden.Effected = 0;
                        forbidden.FromTank = fromItem.Tank;
                        break;
                    //协同开火
                    case "cofire":
                        Cofire cofire = fromItem.Tank.AddComponent<Cofire>();
                        cofire.Value = 3;
                        cofire.Effected = 0;
                        cofire.FromTank = fromItem.Tank;
                        break;
                    //帝国指令
                    case "empiredirective":
                        EmpireDirective empiredirective = fromItem.Tank.AddComponent<EmpireDirective>();
                        empiredirective.Effected = 0;
                        empiredirective.FromTank = fromItem.Tank;
                        break;
                    //决斗
                    case "duel":
                        Duel duel = fromItem.Tank.AddComponent<Duel>();
                        duel.Effected = 0;
                        duel.FromTank = fromItem.Tank;
                        break;
                }
            }
        }
    }



}
