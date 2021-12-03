using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackTo : MonoBehaviour
{
    public string BackScene;
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
        transform.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
            AudioMgr.Instance.PlayBtnAudio();
            if (!string.IsNullOrEmpty(BackScene))
            {
                SceneManager.LoadScene(BackScene);
            }
        });
    }
}
