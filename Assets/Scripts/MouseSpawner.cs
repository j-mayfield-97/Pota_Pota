using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSpawner : MonoBehaviour
{
    public GameObject drop;
    private float time_interval;
    private float next;
    // Start is called before the first frame update
    void Start()
    {
        time_interval = 0.5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.F) && (Time.time > next))
        {
            Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(drop, mouse_pos, Quaternion.identity, this.transform);
            next = time_interval + Time.time;
        }
    }
}
