using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DefenseSkill:MonoBehaviour
{
    //技能信息
    public SkillInfo Skill = null;
    //技能属性值，如次数、概率值、百分比
    public float Value = 0;
    //防御坦克
    public GameObject Tank = null;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (Skill.Name.ToLower() == "holyshield")
        {
            HolyShield holyShield = Tank.GetComponent<HolyShield>();
        
        }
    }
    public void Trigger()
    {
        if (Skill.Name.ToLower() == "holyshield")
        {

        }
    }

}

