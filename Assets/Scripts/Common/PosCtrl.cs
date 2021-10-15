using UnityEngine;

public class PosCtrl : MonoBehaviour
{
    public float RotateSpeed = 40;//转向速度
    public float MoveSpeed = 0.03f;//移动速度
    Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        Camera main = GameObject.Find("Main Camera").GetComponent<Camera>();
        GameObject obj = GameObject.Find("GameObject");
        main.transform.LookAt(obj.transform);

    }

    // Update is called once per frame
    void Update()
    {
        //QE左右旋转
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, -RotateSpeed * Time.deltaTime, 0, Space.Self);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, RotateSpeed * Time.deltaTime, 0, Space.Self);
        }
        //前后左右行走
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {

            //z轴
            transform.Translate(Vector3.forward * MoveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            //z轴
            transform.Translate(Vector3.back * MoveSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            //x轴
            transform.Translate(Vector3.left * MoveSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            //x轴
            transform.Translate(Vector3.right * MoveSpeed);
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            //鼠标左键开火
        }

        //上下
        if (Input.GetKey(KeyCode.UpArrow))
        {

            //y轴
            transform.Translate(Vector3.up * MoveSpeed);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {

            //y轴
            transform.Translate(Vector3.down * MoveSpeed);
        }

    }
}
