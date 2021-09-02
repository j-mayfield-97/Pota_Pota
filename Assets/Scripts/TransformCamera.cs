using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TransformCamera : MonoBehaviour
{
    public float speed;

    void Update()
    {

        float hzc = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float vtc = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        if(hzc != 0 || vtc != 0)
        {
            Vector3 vec = new Vector3(hzc, vtc, 0);

            this.transform.position = this.GetComponent<CinemachineVirtualCamera>().State.CorrectedPosition;

            this.transform.Translate(vec);

        }
        

/*
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(0, speed * Time.deltaTime, 0);
        }

        else if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(0, -speed * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(-speed * Time.deltaTime, 0, 0);
        }

        else if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(speed * Time.deltaTime, 0, 0);
        }
*/

    }
}
