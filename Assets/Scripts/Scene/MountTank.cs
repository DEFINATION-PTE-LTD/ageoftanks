using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class MountTank : MonoBehaviour
{

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Awake()
    {
       
        // StartCoroutine(CommonHelper.DelayToInvokeDo(() => { BeginMount(); }, 3f));

    }

    public GameObject BeginMount(AOT_Parts Engine, AOT_Parts Body, AOT_Parts Head, AOT_Parts Weapon) 
    {
        if (transform.Find("NewTank").childCount > 0)
        {
            Debug.Log(transform.Find("NewTank").childCount);
            DestroyImmediate(transform.Find("NewTank").GetChild(0).gameObject);
        }
        GameObject tank =  ResourceCtrl.Instance.Mount(Engine, Body, Head, Weapon);

        tank.transform.SetParent(transform.Find("NewTank"), false);

        return tank;
    }


}
