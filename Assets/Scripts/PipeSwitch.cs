using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSwitch : MonoBehaviour
{
    private GameObject drops_parent;
    public GameObject pipe;
    public GameObject droplet_prefab;
    public GameObject p_switch;
    public GameObject button;
    public GameObject spawn_point;
    private bool on_state = false;
    private int individual_drop_limit = 10;
    private int world_drop_limit;
    public float drop_interval;
    private float next_spawn_time;

    private void checkswitch()
    {
        //if left button clicked
        if (Input.GetMouseButtonDown(0))
        {
            //take the mouse posiotin in relation to the camera as a 2d vector
            Vector2 cubeRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //send a ray from the mouse position to the the zero vector , 
            RaycastHit2D cubeHit = Physics2D.Raycast(cubeRay, Vector2.zero);

            //cubeHit will be null if it hits no collider , else it will return an object with the item it hit as a component
            if (cubeHit.collider == p_switch.GetComponent<Collider2D>())
            {
                // Debug.Log("We hit " + cubeHit.collider.name);
                on_state = !on_state;
                button.SetActive(on_state);
                this.GetComponent<AudioSource>().Play();
            }
        }
    }
    public void controller_switch()
    {
        on_state = !on_state;
        button.SetActive(on_state);
        this.GetComponent<AudioSource>().Play();

    }
    public bool get_state()
    {
        return on_state;
    }
    //create a droplet every interval while in on state
    void drop_on_time()
    {
        if(Time.time > next_spawn_time && on_state)
        {
            Instantiate(droplet_prefab, spawn_point.transform.position, spawn_point.transform.rotation, drops_parent.transform);
            next_spawn_time = Time.time + drop_interval;
        }

    }
    //remove drop from world drops
    void remove_drop()
    { 
        if(drops_parent.transform.childCount > individual_drop_limit)
        {
            Destroy(drops_parent.transform.GetChild(0).gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        button.SetActive(on_state);
        //start off with next spawn time being 'in x seconds'
        next_spawn_time = Time.time + drop_interval;
        drops_parent = new GameObject("drops_parent_pipe"+ this.transform.GetSiblingIndex());
        drops_parent.transform.SetParent(GameObject.FindGameObjectWithTag("drops_parent").transform);
    }

    // Update is called once per frame
    void Update()
    {
        checkswitch();
        remove_drop();
        drop_on_time();
    }
}
