using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looker : MonoBehaviour
{
    public GameObject target;
    private float distance = 200.0f;

    private float xSpeed = 250.0f;
    private float ySpeed = 120.0f;
    private float scrollSpeed = 30;

    private float yMinLimit = 0;
    private float yMaxLimit = 100;

    float x = 50.0f;
    float y = 15.0f;

    void Start()
    {
        //var angles = transform.eulerAngles;
        //x = angles.y;
        //y = angles.x;
    }

    float prevDistance;

    void LateUpdate()
    {
        if (distance < 50) distance = 50.0f;
        if (distance > 500) distance = 500f;
        distance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        if (target && (Input.GetMouseButton(0) || Input.GetMouseButton(1)))
        {
            var pos = Input.mousePosition;
            var dpiScale = 1f;
            if (Screen.dpi < 1) dpiScale = 1;
            if (Screen.dpi < 200) dpiScale = 1;
            else dpiScale = Screen.dpi / 200f;

            if (pos.x < 380 * dpiScale && Screen.height - pos.y < 250 * dpiScale) return;

            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;

            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);
            var rotation = Quaternion.Euler(y, x, 0);
            var position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.transform.position;
            transform.rotation = rotation;
            transform.position = position;

        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
            //Screen.lockCursor = false;
        }

        if (Math.Abs(prevDistance - distance) > 0.001f)
        {
            prevDistance = distance;
            var rot = Quaternion.Euler(y, x, 0);
            var po = rot * new Vector3(0.0f, 0.0f, -distance) + target.transform.position;
            transform.rotation = rot;
            transform.position = po;
        }
    }

    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
