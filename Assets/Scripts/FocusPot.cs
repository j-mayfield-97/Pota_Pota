using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FocusPot : MonoBehaviour
{
    GameObject vcam1;
    GameObject vcam2;

    // Start is called before the first frame update
    void Start()
    {
        vcam1 = GameObject.Find("CM vcam1");
        vcam2 = GameObject.Find("CM vcam2");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            vcam1.GetComponent<CinemachineVirtualCamera>().m_Priority = 1;
        }
        if(Input.GetKeyDown(KeyCode.V))
        {
            vcam2.GetComponent<CinemachineVirtualCamera>().m_Priority = 1;
        }
    }
}
