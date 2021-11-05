using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
     public void Awake()
    {
       
        transform.Find("btn_login").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            //AudioManager.Instance.PlayBtnAudio();
            string account = transform.Find("Account").GetComponent<InputField>().text;
            string password = transform.Find("Password").GetComponent<InputField>().text;
            if (!string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(password))
            {
         
                string jsonstr = JSONhelper.ToJson(new { Account = account, Password = password });// JSONhelper.ToJson(jsonObj);
                Debug.Log(jsonstr);
                HttpTool.Instance.Post("aotuser/login", jsonstr, (string result)=> {
                    Debug.Log(result);
                    APIResult res = JSONhelper.ConvertToObject<APIResult>(result);// JSONhelper.ConvertToObject<APIResult>(result);
   
                    if (res.success == true)
                    {
                        transform.Find("txt_tip").GetComponent<Text>().text = "";
                        SceneManager.LoadScene("ResourceScene");
                    }
                    else
                    {
                        transform.Find("txt_tip").GetComponent<Text>().text = res.message_en;
                    }
                });
            }


        });

    }

 
}
