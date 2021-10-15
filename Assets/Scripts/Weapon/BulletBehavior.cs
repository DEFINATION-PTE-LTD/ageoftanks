using UnityEngine;

/// <summary>
/// 控制子弹的飞行，销毁
/// </summary>
public class BulletBehavior : MonoBehaviour
{
    public int speed = 5;//飞行速度
    public float lifetime = 1;
    // Start is called before the first frame update


    //发射者
    public string player;
    //攻击技能
    public string skill;
    //暴击值大于攻击值时按暴击值计算
    //攻击值
    public int atk = 200;
    //暴击值
    public int citical = 0;

    public GameObject target;


    void Start()
    {
        // Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
        // rigidbody.AddForce(transform.forward * speed, ForceMode.Impulse);
        // rigidbody.useGravity = false;
        //gameObject.AddComponent<BoxCollider>();

    }
    void FixedUpdate()
    {
        lifetime -= Time.deltaTime;
        if (lifetime > 0)
        {
            if (target != null)
            {
                //平滑飞向目标点坐标
                transform.position = Vector3.Lerp(transform.position, target.transform.position + new Vector3(0, 1, 0), Time.deltaTime * speed);
            }
            //this.transform.Translate( transform.forward * Time.deltaTime * speed);
        }
        else
        {
            RecoveryBullet(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.ToLower() == "gameobject")
        {
            RecoveryBullet(gameObject);

            //Debug.Log("碰撞");

            //ParticleSystem particleSystem = collision.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
            //particleSystem.Stop();
            //particleSystem.gameObject.SetActive(true);
            //particleSystem.Play();
        }
    }
    /// <summary>
    /// 回收子弹
    /// </summary>
    /// <param name="bullet"></param>
    void RecoveryBullet(GameObject bullet)
    {
        Destroy(bullet);
    }
}
