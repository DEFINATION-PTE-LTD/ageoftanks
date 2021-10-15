using UnityEngine;

public class TankPKCtrl : MonoBehaviour
{
    /// <summary>
    /// 行走速度
    /// </summary>
    public int WalkSpeed = 5;
    /// <summary>
    /// 拐弯速度
    /// </summary>
    public int RotaSpeed = 50;
    /// <summary>
    /// 炮台旋转速度
    /// </summary>
    public int RoundSpeed = 40;
    public int HitCount = 0;

    //爆炸效果
    GameObject boomPrefab;
    //燃烧效果
    GameObject firePrefab;

    //动画状态机
    Animator animator;
    //上身部位
    GameObject MountTop;

    public bool DeathStatus = false;

    private void Awake()
    {
        animator = transform.GetComponent<Animator>();
        Debug.Log(animator.runtimeAnimatorController.name);
        //爆炸效果
        boomPrefab = Resources.Load<GameObject>("Effects/WFX_Nuke"); //transform.Find("Effects/WFX_Nuke").gameObject;
        //燃烧效果
        firePrefab = Resources.Load<GameObject>("Effects/WFX_Fire Natural Broad (Black Smoke)"); //transform.Find("Effects/WFX_Fire Natural Broad (Black Smoke)").gameObject;


        //获取上身部位
        foreach (Transform item in GetComponentsInChildren<Transform>())
        {
            if (item.name.ToLower() == "mount_top")
            {
                MountTop = item.gameObject;
                break;
            }
        }

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        KeyListener();
    }

    //前进、后退、左拐弯、右拐弯、发射
    //炮台方向调整左、右、上下
    //蝎式切换行走状态、切换滚轮状态
    //    input.GetAxis用法：
    //(GetAxis("Mouse X"),
    //GetAxis("Mouse Y"),
    //GetAxis("Mouse ScrollWheel"),
    //GetAxis("Vertical "),
    //GetAxis("Horizontal "),
    //GetAxis 是个方法，需要传参数，参数为string类型，参数如下：
    //一：触屏类
    //    1.Mouse X                鼠标沿着屏幕X移动时触发
    //    2.Mouse Y                鼠标沿着屏幕Y移动时触发
    //    3.Mouse ScrollWheel      当鼠标滚动轮滚动时触发
    //二：键盘操作类
    //    1.Vertical 对应键盘上面的上下箭头，当按下上或下箭头时触发
    //    2.Horizontal 对应键盘上面的左右箭头，当按下左或右箭头时触发

    /// <summary>
    /// 键盘监听
    /// </summary>
    void KeyListener()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        #region 蝎式坦克键盘监听
        string animatorName = animator.runtimeAnimatorController.name;
        if (animatorName == "Legs_Spider_Hvy" || animatorName == "Legs_Spider_Lt_Ctrl" || animatorName == "Legs_Spider_Med_Ctrl")
        {
            //上箭头-前进
            if (v > 0)
            {
                if (h == 0)
                {
                    if (v == 1)
                    {
                        animator.Play("Roller_Roll");
                    }
                    else
                    {
                        animator.Play("Walk");
                    }

                }
                else if (h > 0)
                {
                    animator.Play("Roller_Roll_Turn_R");
                    //transform.Translate(Vector3.forward * Time.deltaTime * WalkSpeed);
                    // transform.Rotate(new Vector3(0, Time.deltaTime * RotaSpeed, 0));
                }
                else if (h < 0)
                {
                    animator.Play("Roller_Roll_Turn_L");
                    // transform.Translate(Vector3.forward * Time.deltaTime * WalkSpeed);
                    //transform.Rotate(new Vector3(0, -Time.deltaTime * RotaSpeed, 0));
                }
            }
            else if (v < 0)
            {
                // transform.Translate(Vector3.back * Time.deltaTime * WalkSpeed);
                if (h == 0)
                {
                    animator.Play("Walk_Back");
                }
                else if (h > 0)
                {
                    animator.Play("Roller_Roll_Turn_R");
                    //transform.Translate(Vector3.back * Time.deltaTime * WalkSpeed);
                    //transform.Rotate(new Vector3(0, Time.deltaTime * RotaSpeed, 0));
                }
                else if (h < 0)
                {
                    animator.Play("Roller_Roll_Turn_L");
                    // transform.Translate(Vector3.back * Time.deltaTime * WalkSpeed);
                    //transform.Rotate(new Vector3(0, -Time.deltaTime * RotaSpeed, 0));
                }
            }
            else if (v == 0)
            {
                if (h == 0)
                {
                    animator.Play("Idle");
                }
                else if (h > 0)
                {
                    animator.Play("Roller_Roll_Turn_R");
                    //transform.Rotate(new Vector3(0, Time.deltaTime * RotaSpeed, 0));
                }
                else if (h < 0)
                {
                    animator.Play("Roller_Roll_Turn_L");
                    //transform.Rotate(new Vector3(0, -Time.deltaTime * RotaSpeed, 0));
                }
            }

            ////下箭头-后退
            //if (Input.GetKey(KeyCode.DownArrow))
            //{
            //    //transform.Translate(Vector3.back * Time.deltaTime * WalkSpeed);
            //    animator.Play("Walk_Back");

            //}
            ////左箭头-左转
            //if (Input.GetKey(KeyCode.LeftArrow))
            //{
            //    //transform.Rotate(new Vector3(0, -Time.deltaTime * RotaSpeed, 0));
            //    animator.Play("Turn_L");
            //}
            ////右箭头-右转
            //if (Input.GetKey(KeyCode.RightArrow))
            //{
            //    //transform.Rotate(new Vector3(0, Time.deltaTime * RotaSpeed, 0));
            //    animator.Play("Turn_R");

            //}

            //Q键-炮口向左
            if (Input.GetKey(KeyCode.Q))
            {
                if (MountTop.transform.rotation.y > -60)
                {
                    MountTop.transform.Rotate(new Vector3(0, -Time.deltaTime * RoundSpeed, 0));
                }
            }
            //E键-炮口向右
            if (Input.GetKey(KeyCode.E))
            {
                if (MountTop.transform.rotation.y < 60)
                {
                    MountTop.transform.Rotate(new Vector3(0, Time.deltaTime * RoundSpeed, 0));
                }
            }

        }

        #endregion

        #region 短履带坦克键盘监听
        else if (animatorName == "Legs_Tracks_Lvl0_Ctrl" || animatorName == "Legs_Tracks_Lvl1_Ctrl" || animatorName == "Legs_Tracks_Lvl2_Ctrl" || animatorName == "Legs_Tracks_Lvl3_Ctrl")
        {

            //上箭头-前进
            if (v > 0)
            {
                animator.Play("Roll");
                transform.Translate(Vector3.forward * Time.deltaTime * WalkSpeed);
                if (h > 0)
                {
                    transform.Rotate(new Vector3(0, Time.deltaTime * RotaSpeed, 0));
                }
                else if (h < 0)
                {
                    transform.Rotate(new Vector3(0, -Time.deltaTime * RotaSpeed, 0));
                }
            }
            else if (v < 0)
            {
                transform.Translate(Vector3.back * Time.deltaTime * WalkSpeed);
                animator.Play("Roll");
                if (h > 0)
                {
                    transform.Rotate(new Vector3(0, Time.deltaTime * RotaSpeed, 0));
                }
                else if (h < 0)
                {
                    transform.Rotate(new Vector3(0, -Time.deltaTime * RotaSpeed, 0));
                }
            }
            else if (v == 0)
            {
                if (h == 0)
                {
                    animator.Play("Idle");
                }
                else if (h > 0)
                {
                    transform.Rotate(new Vector3(0, Time.deltaTime * RotaSpeed, 0));
                }
                else if (h < 0)
                {
                    transform.Rotate(new Vector3(0, -Time.deltaTime * RotaSpeed, 0));
                }
            }
        }
        #endregion

        #region 其他坦克键盘监听
        else
        {
            //上箭头-前进
            if (v > 0)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * WalkSpeed);
                if (h > 0)
                {
                    transform.Rotate(new Vector3(0, Time.deltaTime * RotaSpeed, 0));
                }
                else if (h < 0)
                {
                    transform.Rotate(new Vector3(0, -Time.deltaTime * RotaSpeed, 0));
                }
            }
            else if (v < 0)
            {
                transform.Translate(Vector3.back * Time.deltaTime * WalkSpeed);
                if (h > 0)
                {
                    transform.Rotate(new Vector3(0, Time.deltaTime * RotaSpeed, 0));
                }
                else if (h < 0)
                {
                    transform.Rotate(new Vector3(0, -Time.deltaTime * RotaSpeed, 0));
                }
            }
            else if (v == 0)
            {
                //if (h == 0)
                //{
                //    animator.Play("Idle");
                //}

                if (h > 0)
                {
                    transform.Rotate(new Vector3(0, Time.deltaTime * RotaSpeed, 0));
                }
                else if (h < 0)
                {
                    transform.Rotate(new Vector3(0, -Time.deltaTime * RotaSpeed, 0));
                }
            }
        }
        #endregion


        if (Input.GetKey(KeyCode.L))
        {
            Death();
        }
    }

    /// <summary>
    /// 死亡效果，播放冒烟效果
    /// </summary>
    public void Death()
    {

        DeathStatus = true;
        if (animator != null)
        {
            animator.Play("Death");
        }

        GameObject boom = Instantiate(boomPrefab, transform, false);
        boom.SetActive(true);
        GameObject fire = Instantiate(firePrefab, transform, false);
        fire.transform.localScale = new Vector3(2, 2, 2);
        fire.transform.GetComponent<ParticleSystem>().loop = true;
        fire.SetActive(true);
    }

    public void ShowSkill()
    {
        Transform obj = transform.parent.Find("Skill");
        if (obj != null)
        {
            if (DeathStatus == false)
            {
                obj.gameObject.SetActive(true);

                Invoke("HideSkill", 2f);
            }
        }
    }
    public void HideSkill()
    {
        Transform obj = transform.parent.Find("Skill");
        if (obj != null)
        {
            obj.gameObject.SetActive(false);
        }
    }

}
