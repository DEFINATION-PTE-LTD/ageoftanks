using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleMode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Awake() 
    {
        transform.Find("btn_solo").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
            AudioManager.Instance.PlayBtnAudio();
            SceneManager.LoadScene("SoloSelect");
        });

        transform.Find("btn_regiment").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
            AudioManager.Instance.PlayBtnAudio();
            SceneManager.LoadScene("RegimentSelect");
        });


    }
}
