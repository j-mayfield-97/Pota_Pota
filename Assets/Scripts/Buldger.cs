using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buldger : MonoBehaviour
{
    public GameObject circle_prefab;
    private GameObject circle;
    public GameObject pipe;
    protected bool exited;
    // Start is called before the first frame update
    void Start()
    {
        exited = false;
        circle = Instantiate(circle_prefab, this.transform);
    }

    // Update is called once per frame

    private void Update()
    {
        if ((circle.transform.localScale.x * this.transform.localScale.x) <= (pipe.transform.localScale.x ) && circle.activeSelf)
        {
            //Destroy(circle);
            circle.SetActive(false);
        }
    }
    void FixedUpdate()
    {
        Vector3 ls = circle.transform.localScale;
        float shrink_rate = 0.9f;
        if (exited)
        {
            circle.transform.localScale = new Vector3(ls.x * shrink_rate, ls.y * shrink_rate, 1);
        }
        
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        exited = true;
        this.GetComponent<AudioSource>().Play();

    }

}
