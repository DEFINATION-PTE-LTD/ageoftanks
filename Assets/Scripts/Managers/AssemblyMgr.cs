using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyMgr: MonoBehaviour
{
    static public AssemblyMgr _instacne = null;


    public static AssemblyMgr Instance
    {
        get
        {
            if (_instacne == null)
            {
                Debug.LogError("AssemblyMgr Awake error");
            }
            return _instacne;
        }
    }

    private void Awake()
    {
        if (_instacne == null)
        {
            DontDestroyOnLoad(gameObject);
            AssemblyMgr._instacne = gameObject.GetComponent<AssemblyMgr>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
