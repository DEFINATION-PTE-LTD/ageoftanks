using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeCtrl : MonoBehaviour
{
    public UnityEngine.UI.Slider MusicSlider, SoundSlider;
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
        MusicSlider = transform.Find("BGVol").GetComponent<UnityEngine.UI.Slider>();
        SoundSlider = transform.Find("EffectVol").GetComponent<UnityEngine.UI.Slider>();

        MusicSlider.value = AudioManager.Instance.MusicVol;
        SoundSlider.value = AudioManager.Instance.EffectVol;


        MusicSlider.onValueChanged.AddListener((float vol) => {
            AudioManager.Instance.SetVolume(1, vol);
        });

        SoundSlider.onValueChanged.AddListener((float vol) => {
            AudioManager.Instance.SetVolume(2, vol);
        });

        transform.Find("MusicMax").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            MusicSlider.value = 1;
            AudioManager.Instance.SetVolume(1, 1);

        });

        transform.Find("MusicMin").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            MusicSlider.value = 0;
            AudioManager.Instance.SetVolume(1, 0);

        });
        transform.Find("SoundMax").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            SoundSlider.value = 1;
            AudioManager.Instance.SetVolume(2, 1);

        });
        transform.Find("SoundMin").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            SoundSlider.value = 0;
            AudioManager.Instance.SetVolume(2, 0);

        });

    }

}
