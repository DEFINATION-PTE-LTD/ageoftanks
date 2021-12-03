using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class AssemblyTimeline : MonoBehaviour
{

    public GameObject tank;
    public Camera headCamera, bodyCamera, wealCamera, wearCamera;
    public RawImage rawImage;
    public GameObject btnSkip;

    private PlayableDirector playableDirector; 
    private readonly Dictionary<string, PlayableBinding> bindingDict = new Dictionary<string, PlayableBinding>(); //动画轨道集合

    private RenderTexture rt;
    

    // Start is called before the first frame update
    void Awake()
    {
        AudioMgr.Instance.PauseBgm();
        btnSkip.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
            gameObject.SetActive(false);
            DestroyImmediate(tank);
            AudioMgr.Instance.PlayBgm();
        });

        playableDirector = GetComponent<PlayableDirector>();
        rt = new RenderTexture(Screen.width, Screen.height, 0);
       
        headCamera.targetTexture = rt;
        bodyCamera.targetTexture = rt;
        wealCamera.targetTexture = rt;
        wearCamera.targetTexture = rt;
        rawImage.texture = rt;
        rawImage.material = Resources.Load<Material>("RenderTextureMat");
        rawImage.gameObject.SetActive(true);
        //开始的时候，储存所有轨道信息，轨道名称作为key，Track作为value，用于动态设置
        foreach (var bind in playableDirector.playableAsset.outputs)
        {
            if (!bindingDict.ContainsKey(bind.streamName))
            {
                bindingDict.Add(bind.streamName, bind);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 动态设置轨道
    /// </summary>
    /// <param name="trackName"></param>
    /// <param name="gameObject"></param>
    public void SetTrackDynamic(string trackName, GameObject gameObject)
    {
        if (bindingDict.TryGetValue(trackName, out PlayableBinding pb))
        {
            if (gameObject.GetComponent<Animator>() == null)
            {
                gameObject.AddComponent<Animator>(); //添加这个才能被动画控制
            }
            else
            {
                if (gameObject.GetComponent<Animator>().enabled == false)
                {
                    gameObject.GetComponent<Animator>().enabled = true;
                }
            }
            
            playableDirector.SetGenericBinding(pb.sourceObject, gameObject);
        }
    }


    /// <summary>
    /// 替换轨道游戏物体
    /// </summary>
    /// <param name="head"></param>
    /// <param name="body"></param>
    /// <param name="weaponL"></param>
    /// <param name="weaponR"></param>
    /// <param name="engine"></param>
    public void SetTracks(GameObject head, GameObject body, GameObject weaponL,GameObject weaponR, GameObject engine)
    {
        tank = engine;
        //transform.Find("TankBox")

        //头部
        SetTrackDynamic("head_Active",head);
        SetTrackDynamic("head_Anim", head);
        //机身
        SetTrackDynamic("body_Active", body);
        SetTrackDynamic("body_Anim", body);
        //武器左
        SetTrackDynamic("weapon_l_Active", weaponL);
        SetTrackDynamic("weapon_l_Anim", weaponL);
        //武器右
        SetTrackDynamic("weapon_r_Active", weaponR);
        SetTrackDynamic("weapon_r_Anim", weaponR);
        //底座
        SetTrackDynamic("tank_rotate", engine);
    }
}
