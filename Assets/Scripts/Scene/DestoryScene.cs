using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryScene : MonoBehaviour
{
    public List<GameObject> gameObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //销毁时清除所有游戏物体
    private void OnDestroy()
    {
        foreach (GameObject item in gameObjects)
        {
            DestroyImmediate(item);
        }
    }
}
