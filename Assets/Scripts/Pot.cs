using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class Pot : MonoBehaviour
{
    public int pot_capacity;
    private int alive_capacity;
    private int total_drops_caught;
    private int drops_caught;
    private GameObject dirt;
    private GameObject seed_places_parent;
    public GameObject plant;
    public GameObject petals;
    public GameObject next_living;
    private GameObject pass_help;
    public float plant_top_position;
    private float plant_growth_rate;
    private SpriteRenderer seed_rend;
    private bool sleeping_state = true;
    private int seed_order;
    private CinemachineVirtualCamera vcam;
    float store_vol;

    public int get_drops_in_pot()
    {
        return total_drops_caught;
    }
    public int get_drops_in_sleep()
    {
        return drops_caught;
    }    
    public int get_drops_cap()
    {
        return pot_capacity;
    }
    public void drop_passer(int dco)
    {
        total_drops_caught = dco;
    }
    //is full is used by dropinteract to keep pot from filling infinitely
    public bool is_full()
    {
        return pot_capacity == drops_caught;
    }   
    public bool is_alive_full()
    {
        if (sleeping_state  )
            return false;
        else
        {
            return alive_capacity == total_drops_caught;
        }
    }
    
    //make pots living should only happen once every sleeping pot
    private void make_living()
    {
        if(pot_capacity == drops_caught && sleeping_state && (seed_places_parent != null) && (next_living != null))
        {
            this.gameObject.SetActive(false);
            //drops_caught = 0;
            GameObject live_plant = Instantiate(next_living, this.transform.position, this.transform.rotation);
            //live_plant.GetComponent<Pot>().sleeping_state = false;
            live_plant.GetComponent<Pot>().seed_passer(seed_places_parent.transform.GetChild(0).gameObject);
            live_plant.GetComponent<Pot>().seed_order_passer(seed_order);
            live_plant.GetComponent<Pot>().drop_passer(total_drops_caught);

            //live_plant.GetComponent<Pot>().seed_planter(seed_planted); ;
            sleeping_state = false;
        }
    }
    //this get called by the drop interact script
    public void drop_catch()
    {
        if (drops_caught < pot_capacity && (seed_places_parent != null))
        {
            drops_caught++;
            total_drops_caught++;

            //Vector3 displace_plant = new Vector3(plant.transform.localPosition.x,
            //                                   (plant_growth_rate * drops_caught), plant.transform.localPosition.z);
            //plant.transform.localPosition = displace_plant;
            plant.transform.Translate(0, (plant_growth_rate), 0, Space.Self);
        }
        if(alive_capacity == total_drops_caught)
        {
            petals.transform.GetChild(seed_order).gameObject.SetActive(true);
        }
        if(drops_caught == pot_capacity && next_living == null)
        {
            petals.transform.GetChild(seed_order).gameObject.SetActive(true);
        }
        //camera to follow, will add the latest 
        vcam.GetComponent<CinemachineVirtualCamera>().m_Follow = this.transform;
        make_living();
        plant.GetComponent<AudioSource>().Play();
        Debug.Log("catch plant:" + drops_caught);
    }
    //needs to be run before Start for lving pots 
    // pass the sprite of the seed to the next Pot that spawns.
    public void seed_order_passer(int s_ord)
    {
        this.seed_order = s_ord;
    }
    public void seed_passer(GameObject passed_seed)
    {
        sleeping_state = false;
        pass_help = passed_seed;
        seed_planter_alt();
    }
    private void seed_planter_alt()
    {
        seed_places_parent = this.transform.GetChild(0).GetChild(0).gameObject;
        seed_places_parent.SetActive(true);
        seed_rend = pass_help.GetComponent<SpriteRenderer>();
        //make every child spot have a seed sprite
        foreach (Transform child in seed_places_parent.transform)
        {
            child.GetComponent<SpriteRenderer>().sprite = seed_rend.sprite;
        }
    }
    //when called with the triggerenter and when spawning new pot from old pot
    private void seed_planter(GameObject seedy)
    {
        //Get Seed_Placement parent, set active, then change the sprite for each child
        seed_places_parent = this.transform.GetChild(0).GetChild(0).gameObject;
        seed_places_parent.SetActive(true);
        seed_rend = seedy.transform.GetChild(0).GetComponent<SpriteRenderer>();
        seed_order = seedy.transform.GetSiblingIndex();

        //make every child spot have a seed sprite
        foreach (Transform child in seed_places_parent.transform)
        {
            child.GetComponent<SpriteRenderer>().sprite = seed_rend.sprite;
        }
        //make the old seed inactive, (maybe destroy later)
        seedy.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if seed collides and theres not already a seed then plant seed
        if (collision.gameObject.CompareTag("seed") && (seed_places_parent == null))
            seed_planter(collision.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!this.GetComponent<AudioSource>().isPlaying)
        {
            //get the hit sound to play proportional to the velocity it enters object 
            float velo = this.GetComponent<Rigidbody2D>().GetPointVelocity(Vector2.zero).magnitude;
            this.GetComponent<AudioSource>().volume = store_vol + ((velo)/ 15f);
            this.GetComponent<AudioSource>().Play();
            //this.GetComponent<AudioSource>().volume = store_vol;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        this.GetComponent<AudioSource>().volume = store_vol;
    }

    private void Awake()
    {
        dirt = this.transform.GetChild(0).gameObject;
        plant_growth_rate = plant_top_position / pot_capacity;
        alive_capacity = pot_capacity * 2;
        FindObjectOfType<Level_State>().pot_add(this);
        vcam = GameObject.FindGameObjectWithTag("p_cam").GetComponent<CinemachineVirtualCamera>();
        store_vol = this.GetComponent<AudioSource>().volume;
    }

}
