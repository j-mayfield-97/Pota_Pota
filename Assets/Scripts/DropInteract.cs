using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropInteract : MonoBehaviour
{
    public GameObject water_obj;
    private GameObject can_obj;
    public GameObject dirt_obj;
    public GameObject particle;
    private bool used;
    private Vector3 curr_loc;
    private Vector3 prev_loc;
    private Color rand;

    private void test_line()
    {
        curr_loc = this.transform.position;

        Debug.DrawLine(curr_loc, prev_loc, rand, 100000f, false);

        prev_loc = curr_loc;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
            bool water_hit = collision.gameObject.CompareTag(water_obj.tag);
        if (water_hit && !used)
        {
            can_obj = collision.gameObject.transform.parent.parent.gameObject;
            if(!can_obj.GetComponent<CanFill>().is_full() && can_obj.GetComponent<CanFill>().get_can_flip())
            {
                can_obj.GetComponent<CanFill>().drop_catch();
                used = true;
                this.gameObject.SetActive(false);

            }
        }
        bool dirt_hit = collision.gameObject.CompareTag("dirt");
        if (dirt_hit && !used)
        {
            can_obj = collision.gameObject.transform.parent.gameObject;
            if(!can_obj.GetComponent<Pot>().is_full())
            {
                can_obj.GetComponent<Pot>().drop_catch();
                used = true;
                this.gameObject.SetActive(false);

            }
        }

    }
    private void OnDestroy()
    {
    }
    private void OnDisable()
    {
        Instantiate(particle, this.transform.position, this.transform.rotation);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //test_line();
    }

    // Start is called before the first frame update
    void Start()
    {
        used = false;
        prev_loc = this.transform.position;
        rand = Random.ColorHSV();
    }
}
