using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleMode : MonoBehaviour
{
    public Dropdown dropdown;
   
    
    // Start is called before the first frame update
    void Start()
    {
        dropdown.value = ResourceCtrl.Instance.QualityVal;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Awake() 
    {
       
        dropdown.onValueChanged.AddListener((int q) => {
            ResourceCtrl.Instance.QualityVal = q;
        });

        transform.Find("btn_solo").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
            AudioMgr.Instance.PlayBtnAudio();
            SceneManager.LoadScene("SoloSelect");
            System.GC.Collect();
        });

        transform.Find("btn_regiment").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
            AudioMgr.Instance.PlayBtnAudio();
            SceneManager.LoadScene("RegimentSelect");
            System.GC.Collect();

        });


    }
}
