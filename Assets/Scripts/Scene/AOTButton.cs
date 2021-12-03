using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AOTButton :MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    //载入时自动获取到组件内的按钮挂载音效组件
    public void Awake()
    {
        
    }

    //点击
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioMgr.Instance.PlayBtnAudio();
    }
    //进入
    public void OnPointerEnter(PointerEventData eventData)
    {
       
    }
    //退出
    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

}
