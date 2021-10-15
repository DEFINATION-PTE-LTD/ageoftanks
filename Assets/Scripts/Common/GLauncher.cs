using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GLauncher武器脚本
/// </summary>
public class GLauncher : MonoBehaviour
{
    //武器gameobject
    public GameObject Weapon;
    //武器的枪管
    public List<GameObject> Barrels;

    List<GameObject> bullets = new List<GameObject>();
    List<GameObject> fires = new List<GameObject>();
    // Start is called before the first frame update
    float shootTime = 0;

    void Start()
    {

        foreach (Transform item in Weapon.GetComponentsInChildren<Transform>())
        {
            if (item.name.ToLower().IndexOf("barrel_end") > -1)
            {
                Barrels.Add(item.gameObject);
                fires.Add(item.GetChild(0).gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }
    private void FixedUpdate()
    {

        if (shootTime > 0)
        {
            shootTime -= Time.deltaTime;
            foreach (GameObject item in fires)
            {
                ParticleSystem particleSystem = item.transform.GetChild(0).GetComponent<ParticleSystem>();
                particleSystem.Stop();
                Debug.Log(particleSystem.isStopped);
                // item.SetActive(false);
            }
        }

    }

    //射击
    void Shoot()
    {
        GameObject bulletPrefab = Resources.Load<GameObject>("Prefabs/Barrel/Projectile_Bullet");
        foreach (GameObject item in Barrels)
        {
            GameObject bullet = Instantiate(bulletPrefab);

            bullet.transform.SetParent(item.transform, false);
            bullet.AddComponent<BulletBehavior>();
            BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();

        }

        foreach (GameObject item in fires)
        {
            // item.SetActive(true);
            ParticleSystem particleSystem = item.transform.GetChild(0).GetComponent<ParticleSystem>();
            particleSystem.Play();


        }
        shootTime = 5f;
    }

}
