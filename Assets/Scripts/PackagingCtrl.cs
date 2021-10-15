using UnityEngine;


/// <summary>
/// 控制坦克的拆分、组合过程的动画播放
/// </summary>
public class PackagingCtrl : MonoBehaviour
{
    /// <summary>
    /// 拍摄物体
    /// </summary>
    public GameObject GodEye;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void Awake()
    {
        animator = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.O))
        {
            animator.Play("B7top_open_ani");
            GodEye.transform.GetComponent<Animator>().enabled = true;
        }
        if (Input.GetKey(KeyCode.C))
        {
            animator.Play("B7top_close_ani");
        }

    }
}
