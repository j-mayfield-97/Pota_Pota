using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanFill : MonoBehaviour
{
    public GameObject water;
    public GameObject water_sprite;
    public GameObject counter_text;
    public GameObject spout_droplet;
    public GameObject droplet_point;
    private GameObject can_drops_parent;
    public float time_interval;
    private bool tipped;
    private bool flipped;
    private bool horz = false;
    public int can_capacity;
    //the amount of cans drops allowed on screen
    [Tooltip("The amount of cans drops allowed on screen")]
    public int drops_limit;
    private int drops_in_can;
    private float next_spawn_time;
    private float mass_step;
    private float initial_mass;
    private bool can_flip;
    private float store_vol;
    void tip_check()
    {
        horz = this.GetComponent<Drag>().horz_check();
        float z_rot = this.transform.rotation.eulerAngles.z;
        float z_rot_ = this.transform.rotation.z;

        if ((z_rot >= 60f && z_rot <= 120f && !horz) || (z_rot <= 300f && z_rot >= 240f && horz))
            tipped = true;
        else
            tipped = false;

        if ((z_rot > 120f && z_rot < 220f && !horz) || (z_rot < 240f && z_rot > 100f && horz))
            flipped = true;
        else
        {
            flipped = false;
            can_flip = true;
        }

    }
    public bool get_can_flip()
    {
        return can_flip;
    }
    public int get_drops_in_can()
    {
        return drops_in_can;
    }
    public int get_can_capacity()
    {
        return can_capacity;
    }
    public bool is_full()
    {
        return can_capacity == drops_in_can;
    }
    public void drop_catch()
    {
        if(drops_in_can < can_capacity && can_flip)
        {
            drops_in_can++;
            //move the renderer  without changing the gameobject of the water_sprite  according to how many drops are in can
            //the amount of displacement steps will need to be changesd manually if the scale changes.
            //calculated by taking the highest point of where water_sprite should be minus the lowest point and divide that by the can_capacity
            //currently 2.8 high , 2.0 low  - local positions
            water_sprite.GetComponent<Renderer>().enabled = true;
            float displace_step = 0.08f;
            Vector3 displace_water = new Vector3(water_sprite.transform.localPosition.x, 
                                                2.5f + (displace_step * drops_in_can), water_sprite.transform.localPosition.z);
            water_sprite.transform.localPosition = displace_water;
            //increase the mass of the can with each drop. 0.3f is arbitrary
            this.gameObject.GetComponent<Rigidbody2D>().mass += mass_step;
        }
        Debug.Log("catch:" + drops_in_can);
    }

    void remove_drop()
    {
        if(can_drops_parent.transform.childCount >= drops_limit)
            Destroy(can_drops_parent.transform.GetChild(0).gameObject);
    }
    void pour()
    {
        if((can_drops_parent.transform.childCount <= drops_limit) && (drops_in_can > 0))
        {
            GameObject n_drop = Instantiate(spout_droplet, droplet_point.transform.position, droplet_point.transform.rotation, can_drops_parent.transform);
            n_drop.GetComponent<Rigidbody2D>().AddForce(this.GetComponent<Rigidbody2D>().velocity);
            drops_in_can--;
            //move water sprite // maybe better to treat it how plant sprite gets treated
            float displace_step = 0.08f;
            Vector3 displace_water = new Vector3(water_sprite.transform.localPosition.x,
                                                2.0f + (displace_step * drops_in_can), water_sprite.transform.localPosition.z);
            water_sprite.transform.localPosition = displace_water;
            //lessen mass with each pour
            this.gameObject.GetComponent<Rigidbody2D>().mass -= mass_step;
        }
    }

    void water_display()
    {
        if (drops_in_can > 0)
        {
            water_sprite.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            water_sprite.GetComponent<Renderer>().enabled = false;
        }
            counter_text.GetComponent<TextMeshPro>().text = "" + drops_in_can;
    }

    void flip_empty()
    {
        //spawn dropletts at each child with the droppoint tag
        foreach(Transform child in this.transform)
        {
            if(child.CompareTag("drop_point") && (can_drops_parent.transform.childCount <= (drops_limit + this.transform.childCount)))
                Instantiate(spout_droplet, child.transform.position, child.transform.rotation, can_drops_parent.transform);
        }
        //lessen mass 
        this.gameObject.GetComponent<Rigidbody2D>().mass = initial_mass;
        drops_in_can = 0;
        can_flip = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!this.GetComponent<AudioSource>().isPlaying)
        {
            float velo = this.GetComponent<Rigidbody2D>().GetPointVelocity(Vector2.zero).magnitude;
            this.GetComponent<AudioSource>().volume = store_vol + ((velo) / 40f);
            this.GetComponent<AudioSource>().Play();
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        water_sprite.GetComponent<Renderer>().enabled = false;
        tipped = false;
        flipped = false;
        can_drops_parent = new GameObject("drops_parent_spout");
        drops_in_can = 0;
        Debug.Log("start:" + drops_in_can);
        next_spawn_time = Time.time + time_interval;
        initial_mass = this.gameObject.GetComponent<Rigidbody2D>().mass;
        mass_step =  initial_mass / can_capacity;

        store_vol = this.GetComponent<AudioSource>().volume;
    }

    // Update is called once per frame
    void Update()
    {
        tip_check();
        water_display();

        //functions that need pauses
        if(Time.time > next_spawn_time)
        {
            remove_drop();
           
            if (tipped)
            {            
                pour();
                next_spawn_time = time_interval + Time.time;
            }
            if(flipped && (drops_in_can > 0) && can_flip)
            {
                flip_empty();
            }

        }

    }
}
