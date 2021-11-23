using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountLinkEffect : MonoBehaviour
{
    public GameObject beamStartPre;
    public GameObject beamEndPre;
    public GameObject beamPre;
    private GameObject beamStart;
    private GameObject beamEnd;
    private GameObject beam;
    private LineRenderer line;

    [Header("Adjustable Variables")]
    public float beamEndOffset = 1f; //How far from the raycast hit point the end effect is positioned
    public float textureScrollSpeed = 8f; //How fast the texture scrolls along the beam
    public float textureLengthScale = 3; //Length of the beam texture
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        beamStart = Instantiate(beamStartPre, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        beamEnd = Instantiate(beamEndPre, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        beam = Instantiate(beamPre, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        line = beam.GetComponent<LineRenderer>();
        beam.transform.SetParent(transform, false);
        beamStart.transform.SetParent(transform, false);
        beamEnd.transform.SetParent(transform, false);

    }
    private void Awake()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        ShootBeamInDir(transform.position, new Vector3(0, 1, 0));
    }

    void ShootBeamInDir(Vector3 start, Vector3 dir)
    {
        line.positionCount = 2;
        line.SetPosition(0, start);
        beamStart.transform.position = start;

        Vector3 end = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(start, dir, out hit))
            end = hit.point - (dir.normalized * beamEndOffset);
        else
            end = transform.position + (dir * 100);

        beamEnd.transform.position = end;
        line.SetPosition(1, end);

        beamStart.transform.LookAt(beamEnd.transform.position);
        beamEnd.transform.LookAt(beamStart.transform.position);

        float distance = Vector3.Distance(start, end);
        line.sharedMaterial.mainTextureScale = new Vector2(distance / textureLengthScale, 1);
        line.sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0);
    }
}
