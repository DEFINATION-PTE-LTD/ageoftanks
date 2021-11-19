using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 控制物体旋转操作
/// </summary>
public class ObjectRot : MonoBehaviour
{
    //被控制的GameObject
    public GameObject Target;
    public GameObject RoateBtn;
    public int AutoRotaSpeed = 30; //自动旋转速度
    public int HandRotaSpeed = 50; //手动旋转速度
    public GameObject top;
    public bool AutoRotate = false;
    private bool isDown = false;

    /// <summary>
    /// 批量旋转目标
    /// </summary>
    public List<GameObject> Targets;

    // Start is called before the first frame update
    void Start()
    {
        //  transform.LookAt(Target.transform);
        if (isDown)
        {
            Target.transform.Rotate(new Vector3(0, -Time.deltaTime * HandRotaSpeed, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (AutoRotate)
        {
            Rotateing();
        }

    }

    void LateUpdate()
    {
        if (Target)
        {
            //鼠标左键按下
            //if (Input.GetMouseButton(0) && Math.Abs(Input.GetAxis("Mouse X")) > 0.1)
            //{
            //    AutoRotate = false;
            //    Target.transform.Rotate(new Vector3(0, -Input.GetAxis("Mouse X") * Time.deltaTime * HandRotaSpeed, 0));
            //}
            //if (Input.GetKeyUp(KeyCode.Mouse0)) 
            //{
            //    AutoRotate = true;
            //}
            //单指触摸滑动
            //if (Input.touchCount == 1)
            //{
            //    //触摸类型，滑动
            //    if (Input.GetTouch(0).phase == TouchPhase.Moved)
            //    {
            //        if (Input.mousePosition.y < Screen.height / 2)
            //        {
            //            return;
            //        }
            //        Target.transform.Rotate(new Vector3(0, -Input.GetAxis("Mouse X") * Time.deltaTime * HandRotaSpeed, 0));
            //    }

            //}



            // Vector3 scale = Target.transform.localScale;
            // Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
            // Target.transform.localScale = new Vector3(scale.x * Input.GetAxis("Mouse ScrollWheel") * 10, scale.y * Input.GetAxis("Mouse ScrollWheel") * 10, scale.z * Input.GetAxis("Mouse ScrollWheel") * 10);



        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        isDown = true; 
        AutoRotate = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDown = false; 
        AutoRotate = true;
    }

    /// <summary>
    /// 自动旋转
    /// </summary>
    public void Rotateing()
    {
        Target.transform.Rotate(new Vector3(0, Time.deltaTime * AutoRotaSpeed, 0));
        foreach (GameObject item in Targets)
        {
            item.transform.Rotate(new Vector3(0, Time.deltaTime * AutoRotaSpeed, 0));
        }
    }
    public void Scale()
    {

    }

}
