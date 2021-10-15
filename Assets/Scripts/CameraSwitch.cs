using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //
        if (Input.GetKey(KeyCode.Keypad1))
        {
            Cinemachine.CinemachineBrain brain = transform.GetComponent<Cinemachine.CinemachineBrain>();


        }
    }


}
