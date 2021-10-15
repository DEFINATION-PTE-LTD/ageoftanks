using UnityEngine;

public class AnimationBindEvent : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //触发事件
    void TriggerFun(AnimationEvent animationEvent)
    {
        GameObject target = (GameObject)animationEvent.objectReferenceParameter;
        string FunName = animationEvent.stringParameter;
        target.SendMessage(FunName);
    }
}
