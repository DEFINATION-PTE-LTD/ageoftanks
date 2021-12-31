using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    private string walletaddress = "";
    public void Awake()
    {

        transform.Find("btn_login").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            //AudioManager.Instance.PlayBtnAudio();
            string account = transform.Find("Account").GetComponent<InputField>().text;
            string password = transform.Find("Password").GetComponent<InputField>().text;
            if (!string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(password))
            {
                Dictionary<string, string> pars = new Dictionary<string, string>();
                pars.Add("Account", account);
                pars.Add("Password", password);

                HttpTool.Instance.Post("aotuser/login", pars, (string result) => {
                    //Debug.Log(result);
                    APIResult res = JSONhelper.ToApiResult<AOT_User>(result);// JSONhelper.ConvertToObject<APIResult>(result);

                    if (res.success == true)
                    {
                        transform.Find("txt_tip").GetComponent<Text>().text = "";
                        PlayerPrefs.SetString("userinfo", JSONhelper.ToJson(((List<AOT_User>)res.data)[0]));
                        SceneManager.LoadScene("ResourceScene");
                    }
                    else
                    {
                        transform.Find("txt_tip").GetComponent<Text>().text = res.message_en;
                    }
                });
            }


        });


        transform.Find("btn_play").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {

            ToPlay();
        });

        EventCenter.AddListener<string>(eEventType.ReciveWalletAddress, SetAddress);

    }

    /// <summary>
    /// 保存钱包地址
    /// </summary>
    /// <param name="address"></param>
    public void SetAddress(string address)
    {
        walletaddress = address;
        PlayerPrefs.SetString("walletAddress", address);
    }


    private void OnDestroy()
    {
        EventCenter.RemoveListener<string>(eEventType.ReciveWalletAddress, SetAddress);
    }


    //点击试玩：1、从前端获取到地址参数（）


    //进入游戏
    public void ToPlay()
    {

        if (string.IsNullOrEmpty(walletaddress))
        {
            //通知获取地址
            EventCenter.Broadcast(eEventType.RequestWalletAddress);
        }
        else
        {
            //查询该地址是否在试玩期内

            Dictionary<string, string> pars = new Dictionary<string, string>();
            pars.Add("WalletAddress", walletaddress);
            HttpTool.Instance.Post("deposit/endtime", pars, (string result) => {
                APIResult res = JSONhelper.ConvertToObject<APIResult>(result);
                if (res.success == true)
                {
                    if (Convert.ToDateTime(res.data) < DateTime.Now)
                    {
                        transform.Find("txt_tip").GetComponent<Text>().text = "No trial permission";
                    }
                    else
                    {
                        SceneManager.LoadScene("ResourceScene");
                    }
                }
                else 
                {
                    transform.Find("txt_tip").GetComponent<Text>().text = res.message_en;
                }
            
            });

        }
    }
    
 
}
