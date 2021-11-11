using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// 管理坦克的武器部分功能，包括获取武器对象、武器枪管，匹配子弹预制体，射击动作动画等
/// </summary>
public class Weapon : MonoBehaviour
{
    //附加在坦克对象上，激活时自动寻找枪管对象
    //根据枪管名称自动匹配子弹对象

    public List<WeaponObjectClass> weapons = new List<WeaponObjectClass>();
    //攻击对象
    public List<GameObject> target = new List<GameObject>();

    private void Awake()
    {
        // Debug.Log( animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot"));
        //武器
        FindWeapons();
        //枪管
        FindBarrels();

       

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Shoot();
        //}
    }
    /// <summary>
    /// 查找武器
    /// </summary>
    void FindWeapons()
    {
        //获取武器位
        foreach (Transform item in GetComponentsInChildren<Transform>())
        {
            //if (item.name.ToLower().IndexOf("mount_weapon_l") > -1 || item.name.ToLower().IndexOf("mount_weapon_r") > -1 || item.name.ToLower().IndexOf("mount_weapon_top") > -1)
            //{
            //    if (item.childCount > 0)
            //    {
            //       // Weapon_L.Add(item.GetChild(0).gameObject);
            //        weapons.Add(new WeaponObjectClass 
            //        {
            //            WeaponObj = item.GetChild(0).gameObject,
            //            Barrels = new List<GameObject>(),
            //            BulletPath="",
            //            BulletPerfab=null,
            //            FirePath="",
            //            FirePerfab=null
            //        });
            //    }
            //}

            switch (item.name)
            {
                case "Weapon_DoubleGun_lvl1":
                case "Weapon_DoubleGun_lvl2":
                case "Weapon_DoubleGun_lvl3":
                case "Weapon_DoubleGun_lvl4":
                case "Weapon_DoubleGun_lvl5":
                case "Weapon_GLauncher_lvl1":
                case "Weapon_GLauncher_lvl2":
                case "Weapon_GLauncher_lvl3":
                case "Weapon_Shock_Rifle_lvl1":
                case "Weapon_Shock_Rifle_lvl2":
                case "Weapon_Shock_Rifle_lvl3":
                case "Weapon_Shock_Rifle_lvl4":
                case "Weapon_Shock_Rifle_lvl5":
                case "Weapon_Shocker_lvl1":
                case "Weapon_Shocker_lvl2":
                case "Weapon_Shocker_lvl3":
                case "Weapon_Shocker_lvl4":
                case "Weapon_Shocker_lvl5":
                case "Weapon_Sniper_lvl1":
                case "Weapon_Sniper_lvl2":
                case "Weapon_Sniper_lvl3":
                case "Weapon_Sniper_lvl4":
                case "Weapon_Sniper_lvl5":
                    weapons.Add(new WeaponObjectClass
                    {
                        WeaponObj = item.gameObject,
                        Barrels = new List<GameObject>(),
                        BulletPath = "",
                        BulletPerfab = null,
                        FirePath = "",
                        FirePerfab = null
                    });
                    break;
            }

        }
        //Debug.Log($"Find {weapons.Count} weapons");
    }
    /// <summary>
    /// 查找武器枪管
    /// </summary>
    void FindBarrels()
    {
        if (weapons.Count > 0)
        {
            foreach (WeaponObjectClass weapon in weapons)
            {
                //获取武器拥有的枪管
                foreach (Transform item in weapon.WeaponObj.transform.GetComponentsInChildren<Transform>())
                {
                    if (item.name.ToLower().IndexOf("barrel_end") > -1)
                    {
                        weapon.Barrels.Add(item.gameObject);
                    }
                }
                //GetBulletPath(weapon);
                GetBulletPerfab(weapon);
            }

        }
    }

    /// <summary>
    /// 获取子弹、火焰预制体路径
    /// </summary>
    void GetBulletPath(WeaponObjectClass weaponobj)
    {
        string name = weaponobj.WeaponObj.name;
        string rootpath = "";//根目录
        switch (name)
        {
            case "Weapon_DoubleGun_lvl1":
                rootpath = "Prefabs/Bullet/Bullet_BlazingRed/Bullet_Big_BlazingRed/";
                weaponobj.FirePath = rootpath + "Bullet_BlazingRed_Big_MuzzleFlare";
                weaponobj.BulletPath = rootpath + "Bullet_BlazingRed_Big_Projectile";
                break;
            case "Weapon_DoubleGun_lvl2":
                rootpath = "Prefabs/Bullet/Bullet_BlazingRed/Bullet_Big_BlazingRed/";
                weaponobj.FirePath = rootpath + "Bullet_BlazingRed_Big_MuzzleFlare";
                weaponobj.BulletPath = rootpath + "Bullet_BlazingRed_Big_Projectile";
                break;
            case "Weapon_DoubleGun_lvl3":
                rootpath = "Prefabs/Bullet/Bullet_BlazingRed/Bullet_Big_BlazingRed/";
                weaponobj.FirePath = rootpath + "Bullet_BlazingRed_Big_MuzzleFlare";
                weaponobj.BulletPath = rootpath + "Bullet_BlazingRed_Big_Projectile";
                break;
            case "Weapon_DoubleGun_lvl4":
                rootpath = "Prefabs/Bullet/Bullet_BlazingRed/Bullet_Big_BlazingRed/";
                weaponobj.FirePath = rootpath + "Bullet_BlazingRed_Big_MuzzleFlare";
                weaponobj.BulletPath = rootpath + "Bullet_BlazingRed_Big_Projectile";
                break;
            case "Weapon_DoubleGun_lvl5":
                rootpath = "Prefabs/Bullet/Bullet_BlazingRed/Bullet_Big_BlazingRed/";
                weaponobj.FirePath = rootpath + "Bullet_BlazingRed_Big_MuzzleFlare";
                weaponobj.BulletPath = rootpath + "Bullet_BlazingRed_Big_Projectile";
                break;
            case "Weapon_GLauncher_lvl1":
                rootpath = "Prefabs/Plasma/Plasma_LightBlue/Plasma_Medium_LightBlue/";
                weaponobj.FirePath = rootpath + "Plasma_LightBlue_Medium_MuzzleFlare";
                weaponobj.BulletPath = rootpath + "Plasma_LightBlue_Medium_Projectile";
                break;
            case "Weapon_GLauncher_lvl2":
                rootpath = "Prefabs/Plasma/Plasma_PoisonGreen/Plasma_Medium_PoisonGreen/";
                weaponobj.FirePath = rootpath + "Plasma_PoisonGreen_Medium_MuzzleFlare";
                weaponobj.BulletPath = rootpath + "Plasma_PoisonGreen_Medium_Projectile";
                break;
            case "Weapon_GLauncher_lvl3":
                rootpath = "Prefabs/Plasma/Plasma_RagingRed/Plasma_Medium_RagingRed/";
                weaponobj.FirePath = rootpath + "Plasma_RagingRed_Medium_MuzzleFlare";
                weaponobj.BulletPath = rootpath + "Plasma_RagingRed_Medium_Projectile";
                break;
            case "Weapon_Shock_Rifle_lvl1":
                rootpath = "Prefabs/Laser/Laser_Blue/Laser_Medium_Blue/";
                weaponobj.FirePath = rootpath + "Laser_Blue_Medium_MuzzleFlare";
                weaponobj.BulletPath = rootpath + "Laser_Blue_Medium_Projectile";
                break;
            case "Weapon_Shock_Rifle_lvl2":
                rootpath = "Prefabs/Laser/Laser_Green/Laser_Medium_Green/";
                weaponobj.FirePath = rootpath + "Laser_Green_Medium_MuzzleFlare";
                weaponobj.BulletPath = rootpath + "Laser_Green_Medium_Projectile";
                break;
            case "Weapon_Shock_Rifle_lvl3":
                rootpath = "Prefabs/Laser/Laser_Orange/Laser_Medium_Orange/";
                weaponobj.FirePath = rootpath + "Laser_Orange_Medium_MuzzleFlare";
                weaponobj.BulletPath = rootpath + "Laser_Orange_Medium_Projectile";
                break;
            case "Weapon_Shock_Rifle_lvl4":
                rootpath = "Prefabs/Laser/Laser_Purple/Laser_Medium_Purple/";
                weaponobj.FirePath = rootpath + "Laser_Purple_Medium_MuzzleFlare";
                weaponobj.BulletPath = rootpath + "Laser_Purple_Medium_Projectile";
                break;
            case "Weapon_Shock_Rifle_lvl5":
                rootpath = "Prefabs/Laser/Laser_Red/Laser_Medium_Red/";
                weaponobj.FirePath = rootpath + "Laser_Red_Medium_MuzzleFlare";
                weaponobj.BulletPath = rootpath + "Laser_Red_Medium_Projectile";
                break;
            case "Weapon_Shocker_lvl1":
                rootpath = "Prefabs/Plasma/Plasma_LightBlue/Plasma_Big_LightBlue/";
                weaponobj.FirePath = rootpath + "Plasma_LightBlue_Big_MuzzleFlare";
                weaponobj.BulletPath = rootpath + "Plasma_LightBlue_Big_Projectile";
                break;
            case "Weapon_Shocker_lvl2":
                rootpath = "Prefabs/Plasma/Plasma_OceanBlue/Plasma_Big_OceanBlue/";
                weaponobj.FirePath = rootpath + "PlasmaOceanBlue_Big_MuzzleFlare";
                weaponobj.BulletPath = rootpath + "PlasmaOceanBlue_Big_Projectile";
                break;
            case "Weapon_Shocker_lvl3":
                rootpath = "Prefabs/Plasma/Plasma_PoisonGreen/Plasma_Big_PoisonGreen/";
                weaponobj.FirePath = rootpath + "Plasma_PoisonGreen_Big_MuzzleFlare";
                weaponobj.BulletPath = rootpath + "Plasma_PoisonGreen_Big_Projectile";
                break;
            case "Weapon_Shocker_lvl4":
                rootpath = "Prefabs/Plasma/Plasma_PurpleHaze/Plasma_Big_PurpleHaze/";
                weaponobj.FirePath = rootpath + "Plasma_PurpleHaze_Big_MuzzleFlare";
                weaponobj.BulletPath = rootpath + "Plasma_PurpleHaze_Big_Projectile";
                break;
            case "Weapon_Shocker_lvl5":
                rootpath = "Prefabs/Plasma/Plasma_RagingRed/Plasma_Big_RagingRed/";
                weaponobj.FirePath = rootpath + "Plasma_RagingRed_Big_MuzzleFlare";
                weaponobj.BulletPath = rootpath + "Plasma_RagingRed_Big_Projectile";
                break;
            case "Weapon_Sniper_lvl1":
                rootpath = "Prefabs/Bullet/Bullet_GoldFire/Bullet_Big_Goldfire/";
                weaponobj.FirePath = rootpath + "Bullet_GoldFire_Big_MuzzleFlare";
                weaponobj.BulletPath = rootpath + "Bullet_GoldFire_Big_Projectile";
                break;
            case "Weapon_Sniper_lvl2":
                rootpath = "Prefabs/Bullet/Bullet_GoldFire/Bullet_Big_Goldfire/";
                weaponobj.FirePath = rootpath + "Bullet_GoldFire_Big_MuzzleFlare";
                weaponobj.BulletPath = rootpath + "Bullet_GoldFire_Big_Projectile";
                break;
            case "Weapon_Sniper_lvl3":
                rootpath = "Prefabs/Bullet/Bullet_GoldFire/Bullet_Big_GoldFire/";
                weaponobj.FirePath = rootpath + "Bullet_GoldFire_Big_MuzzleFlare";
                weaponobj.BulletPath = rootpath + "Bullet_GoldFire_Big_Projectile";
                break;
            case "Weapon_Sniper_lvl4":
                rootpath = "Prefabs/Bullet/Bullet_GoldFire/Bullet_Big_GoldFire/";
                weaponobj.FirePath = rootpath + "Bullet_GoldFire_Big_MuzzleFlare";
                weaponobj.BulletPath = rootpath + "Bullet_GoldFire_Big_Projectile";
                break;
            case "Weapon_Sniper_lvl5":
                rootpath = "Prefabs/Bullet/Bullet_GoldFire/Bullet_Big_GoldFire/";
                weaponobj.FirePath = rootpath + "Bullet_GoldFire_Big_MuzzleFlare";
                weaponobj.BulletPath = rootpath + "Bullet_GoldFire_Big_Projectile";
                break;
            default:
                Debug.Log("default");
                weaponobj.FirePath = "Prefabs/Bullet/Bullet_SilverFlare/Bullet_Medium_SilverFlare/Bullet_SilverFlare_Medium_MuzzleFlare";
                weaponobj.BulletPath = "Prefabs/Bullet/Bullet_SilverFlare/Bullet_Medium_SilverFlare/Bullet_SilverFlare_Medium_Projectile";
                break;
        }

        // weaponobj.FirePath = "Prefabs/Bullet/Bullet_SilverFlare/Bullet_Medium_SilverFlare/Bullet_SilverFlare_Medium_MuzzleFlare";
        // weaponobj.BulletPath = "Prefabs/Bullet/Bullet_SilverFlare/Bullet_Medium_SilverFlare/Bullet_SilverFlare_Medium_Projectile";

    }
    /// <summary>
    /// 获取子弹、火焰预制体
    /// </summary>
    void GetBulletPerfab(WeaponObjectClass weaponobj)
    {
        //weaponobj.FirePerfab = Resources.Load<GameObject>(weaponobj.FirePath);
        //weaponobj.BulletPerfab = Resources.Load<GameObject>(weaponobj.BulletPath);
        string name = weaponobj.WeaponObj.name;
        string rootpath = "";//根目录
        GameObject BulletPerfab=null, FirePerfab=null;
        switch (name)
        {
            case "Weapon_DoubleGun_lvl1":
            case "Weapon_DoubleGun_lvl2":
            case "Weapon_DoubleGun_lvl3":
            case "Weapon_DoubleGun_lvl4":
            case "Weapon_DoubleGun_lvl5":
               CommonHelper.GetBulletPrefab("DoubleGun", out BulletPerfab, out FirePerfab);
                break;
            case "Weapon_GLauncher_lvl1":
            case "Weapon_GLauncher_lvl2":
            case "Weapon_GLauncher_lvl3":
                CommonHelper.GetBulletPrefab("GLauncher", out BulletPerfab, out FirePerfab);
                break;
            case "Weapon_Shock_Rifle_lvl1":
            case "Weapon_Shock_Rifle_lvl2":
            case "Weapon_Shock_Rifle_lvl3":
            case "Weapon_Shock_Rifle_lvl4":
            case "Weapon_Shock_Rifle_lvl5":
                CommonHelper.GetBulletPrefab("Shock_Rifle", out BulletPerfab, out FirePerfab);
                break;
            case "Weapon_Shocker_lvl1":
            case "Weapon_Shocker_lvl2":
            case "Weapon_Shocker_lvl3":
            case "Weapon_Shocker_lvl4":
            case "Weapon_Shocker_lvl5":
                CommonHelper.GetBulletPrefab("Shocker", out BulletPerfab, out FirePerfab);
                break;
            case "Weapon_Sniper_lvl1":
            case "Weapon_Sniper_lvl2":
            case "Weapon_Sniper_lvl3":
            case "Weapon_Sniper_lvl4":
            case "Weapon_Sniper_lvl5":
                CommonHelper.GetBulletPrefab("Sniper", out BulletPerfab, out FirePerfab);
                break;
     
        }
        weaponobj.FirePerfab = FirePerfab;// Resources.Load<GameObject>(weaponobj.FirePath);
        weaponobj.BulletPerfab = BulletPerfab;// Resources.Load<GameObject>(weaponobj.BulletPath);
    }

    //生成子弹副本
    GameObject GetBullet(WeaponObjectClass weaponobj)
    {
        GameObject bullet = null;
        bullet = Instantiate(weaponobj.BulletPerfab);
        BulletBehavior bulletBehavior = bullet.AddComponent<BulletBehavior>();
        if (target.Count>0)
        {
           // bulletBehavior.target = target;
        }

        return bullet;
    }

    /// <summary>
    /// 射击,0.3秒
    /// </summary>
    public void Shoot(GameObject parent=null)
    {
       
        foreach (WeaponObjectClass w in weapons)
        {
            foreach (GameObject item in w.Barrels)
            {
                if (target.Count > 0)
                {
                    //建立动画队列
                    Sequence quence = DOTween.Sequence();
                    foreach (GameObject t in target)
                    {
                        Vector3 t_pos = t.transform.position + new Vector3(0, 3, 0);
                        //添加一个动画
                        quence.Append(
                        transform.parent.DOLookAt(t.transform.position, 0.3f,AxisConstraint.Y).OnComplete(() =>
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                
                                StartCoroutine(CommonHelper.DelayToInvokeDo(() =>
                                {
                                    GameObject bullet = Instantiate(w.BulletPerfab);
                                    if (parent != null) 
                                    {
                                        bullet.transform.SetParent(parent.transform, false);
                                    }
                                    bullet.transform.forward = item.transform.forward;
                                    bullet.transform.position = item.transform.position;
                                    bullet.transform.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 60, ForceMode.Impulse);
                                    bullet.transform.DOMove(t_pos, 0.3f).OnUpdate(() => {
                                        bullet.transform.DOLookAt(t_pos, 0, AxisConstraint.Y);
                                    }).OnComplete(()=> {
                                        DestroyImmediate(bullet);
                                    });
                                  

                                    //bulletBehavior.target = t;
                                    PlayFireAni(w.WeaponObj, item, w.FirePerfab);
                                }, i* 0.1f));
                            }
                          
                        }));
                        //延迟0.2秒
                        quence.AppendInterval(0.3f);
                    }
                    quence.SetLoops(1);

                }
                else
                {

                    GameObject bullet = Instantiate(w.BulletPerfab);
                    if (parent != null)
                    {
                        bullet.transform.SetParent(parent.transform, false);
                    }
                    bullet.transform.forward = item.transform.forward;
                    bullet.transform.position = item.transform.position;
                    bullet.transform.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 40, ForceMode.Impulse);
                    BulletBehavior bulletBehavior = bullet.AddComponent<BulletBehavior>();
                    //bullet.transform.SetParent(item.transform,false);
                    //bullet.SetActive(true);
                    PlayFireAni(w.WeaponObj, item, w.FirePerfab);

                }

                //演示用，坦克被击中30次播放死亡动画
                //if (target != null)
                //{
                //    TankCtrl tankCtrl = target.transform.GetComponent<TankCtrl>();
                //    tankCtrl.HitCount++;
                //    tankCtrl.ShowSkill();
                //    if (tankCtrl.HitCount == 30)
                //    {
                //        tankCtrl.Death();
                //    }
                //}
            }
        }

        WeaponObjectClass wc = weapons[0];
        for (int m = 1; m <= 3; m++)
        {
            StartCoroutine(CommonHelper.DelayToInvokeDo(() =>
            {
                //GameObject sound = new GameObject();
                //AudioSource shootSound = sound.AddComponent<AudioSource>();

                if (wc.WeaponObj.name.IndexOf("Weapon_DoubleGun") > -1)
                {
                    AudioManager.Instance.PlayAudio("Sound/weapon_gatling_006");
                    //shootSound.clip = Resources.Load<AudioClip>("Sound/weapon_gatling_006");
                }
                else if (wc.WeaponObj.name.IndexOf("Weapon_GLauncher") > -1)
                {
                    AudioManager.Instance.PlayAudio("Sound/thump");
                    //shootSound.clip = Resources.Load<AudioClip>("Sound/thump");
                }
                else if (wc.WeaponObj.name.IndexOf("Weapon_Shock_Rifle") > -1)
                {
                    AudioManager.Instance.PlayAudio("Sound/laser");
                    //shootSound.clip = Resources.Load<AudioClip>("Sound/laser");
                }
                else if (wc.WeaponObj.name.IndexOf("Weapon_Shocker") > -1)
                {
                    AudioManager.Instance.PlayAudio("Sound/combustion");
                   // shootSound.clip = Resources.Load<AudioClip>("Sound/combustion");
                }
                else if (wc.WeaponObj.name.IndexOf("Weapon_Sniper") > -1)
                {
                    AudioManager.Instance.PlayAudio("Sound/bullet");
                    //shootSound.clip = Resources.Load<AudioClip>("Sound/bullet");
                }

   
                
            }, m * 0.1f));
        }
        //  SoundMgr.Instance.SkillSound(gameObject, skillname, true);
    }

    //射击时枪口动画
    void PlayFireAni(GameObject weapon, GameObject barrel, GameObject firePrefab)
    {
        if (weapon.name.IndexOf("Weapon_DoubleGun") > -1)
        {
            Animator animator = weapon.transform.GetComponent<Animator>();
            animator.Play("Shoot_Single", -1);
            GameObject fire = Instantiate(firePrefab, barrel.transform, false);
            fire.GetComponent<ParticleSystem>().Play();
            fire.transform.Rotate(new Vector3(0, -90, 0));
            StartCoroutine(CommonHelper.DelayToInvokeDo(() => { DestroyImmediate(fire); }, 0.2f));
        }
        else if (weapon.name.IndexOf("Weapon_GLauncher") > -1)
        {
            Animator animator = weapon.transform.GetComponent<Animator>();
            animator.Play("Shoot", -1);
            GameObject fire = Instantiate(firePrefab, barrel.transform, false);
            fire.GetComponent<ParticleSystem>().Play();
            fire.transform.Rotate(new Vector3(0, -90, 0));
            StartCoroutine(CommonHelper.DelayToInvokeDo(() => { DestroyImmediate(fire); }, 0.2f));
        }
        else if (weapon.name.IndexOf("Weapon_Shock_Rifle") > -1)
        {
            GameObject fire = Instantiate(firePrefab, barrel.transform, false);
            fire.GetComponent<ParticleSystem>().Play();
            fire.transform.Rotate(new Vector3(0, -90, 0));
            StartCoroutine(CommonHelper.DelayToInvokeDo(() => { DestroyImmediate(fire); }, 0.2f));
        }
        else if (weapon.name.IndexOf("Weapon_Shocker") > -1)
        {
            GameObject fire = Instantiate(firePrefab, barrel.transform, false);
            fire.GetComponent<ParticleSystem>().Play();
            fire.transform.Rotate(new Vector3(0, -90, 0));
            StartCoroutine(CommonHelper.DelayToInvokeDo(() => { DestroyImmediate(fire); }, 0.2f));
        }
        else if (weapon.name.IndexOf("Weapon_Sniper") > -1)
        {
            Animator animator = weapon.transform.GetComponent<Animator>();
            animator.Play("Shoot", -1);
            GameObject fire = Instantiate(firePrefab, barrel.transform, false);
            fire.GetComponent<ParticleSystem>().Play();
            fire.transform.Rotate(new Vector3(0, -90, 0));
            StartCoroutine(CommonHelper.DelayToInvokeDo(() => { DestroyImmediate(fire); }, 0.2f));
        }
       

    }

}
