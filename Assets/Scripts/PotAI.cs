using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    the direction the pot wants to move in will be chosen between different options  with indivdual weights 
 
 */


public class PotAI : MonoBehaviour
{
    private Vector2 mouse_pos;
    public GameObject arm;
    private float[] arm_rotation_limits = {45f, 310f};
    public Vector3 arm_torque;
    public float fear_const;
    float distance_p;
    float direction_x;
    Vector2 force_vec = Vector2.zero;
    Vector2 prev_force = Vector2.zero;
    private static float inert_const;
    private GameObject pipe_parent;
    private GameObject drop_parent;
    private GameObject w_can;
    private Transform closest_drop;
    private Transform closest_pipe_transform;
    private float drop_distance;
    private float distance_m;
    private float time_increment = 2f;
    private float next_time;
    private float c_d_distance;
    private float decision_interval = 1f;
    private float next_d_time = 0f;
    private int attract_mark = 1;
    delegate Vector2 FearDelegate();
    FearDelegate the_fear;
    void rotate_arm()
    {
        float arm_z = arm.transform.localRotation.eulerAngles.z;
        if (arm_z > arm_rotation_limits[0] && arm_z < 180f)
        {
            arm_torque = -arm_torque;
            arm_z = arm_rotation_limits[0] - 1f; 
        }
        else if ( arm_z < arm_rotation_limits[1] && arm_z >= 180f)
        {
            arm_torque = -arm_torque;
            arm_z = arm_rotation_limits[1] + 1f;

        }
        Vector3 scaler = new Vector3(1, 1, Time.deltaTime);
        arm.transform.Rotate(Vector3.Scale(arm_torque,scaler));
    }

    Vector2 fear_mouse()
    {
        //mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse_pos = w_can.transform.position;

        distance_m = Vector2.Distance(mouse_pos, this.transform.position) + 1f;
        direction_x = this.transform.position.x - mouse_pos.x;

        if (direction_x > 0f)
            direction_x = fear_const;
        else
            direction_x = -fear_const;

        ///reNAMe this
        if (distance_m != 0f)
        {
            force_vec = new Vector2(inert_const / (direction_x * distance_m * attract_mark), 0);

        }

        return force_vec;
    }
    float closest_pipe()
    {
        float pipe_distance = 5000f;
        float closest = 6000f;

        for (int i = 0; i < pipe_parent.transform.childCount; i++)
        {
            pipe_distance = Vector2.Distance(pipe_parent.transform.GetChild(i).transform.position, this.transform.position);
            if (pipe_distance < closest)
            {
                closest = pipe_distance;
                //restrict the changing of targets to every 2 seconds

                closest_pipe_transform = pipe_parent.transform.GetChild(i);

            }
        }

        return closest;
    }
    Vector2 fear_pipe()
    {
        //if pipe is on then stay away
        if(pipe_parent != null)
            if(pipe_parent.transform.childCount > 0 )
            {
            distance_p = closest_pipe();
            direction_x = this.transform.position.x - closest_pipe_transform.position.x;

            if (direction_x > 0f)
                direction_x = 0.1f;
            else
                direction_x = -0.1f;

            if (distance_p != 0f)
                force_vec = new Vector2(inert_const / (direction_x * distance_p), 0);

            }
            else
                force_vec = Vector2.zero;

        return force_vec;
    }

    //i think only run this if other fears are far away
    ///This can be generalized to accept a parent game object and find any closest child
    float closest_droplet()
    {
        drop_distance = 5000f;
        float closest = 6000f;
        float pipe_droplets = 0;

        for (int i = 0; i < drop_parent.transform.childCount; i++)
        {
            pipe_droplets += drop_parent.transform.GetChild(i).childCount;
            if ( pipe_droplets == 0)
                continue;

            for(int j = 0; j < drop_parent.transform.GetChild(i).childCount; j++)
            {
                //get the ranform of the indexed drop and check if its active
                drop_distance = Vector2.Distance(drop_parent.transform.GetChild(i).GetChild(j).transform.position, this.transform.position);
                if(drop_distance < closest && drop_parent.transform.GetChild(i).GetChild(j).gameObject.activeSelf)
                {
                    closest = drop_distance;
                    closest_drop = drop_parent.transform.GetChild(i).GetChild(j);
                    
                }
            }
        }
        
        return closest;
    }
    Vector2 fear_droplets()
    {
        if(drop_parent != null)
            if (drop_parent.transform.childCount > 0 )
            {
                //restrict the changing of directions to every 2 seconds

                c_d_distance = closest_droplet();
                if (closest_drop == null)
                    return new Vector2(0, 0);

                direction_x = this.transform.position.x - closest_drop.transform.position.x;


                if (direction_x > 0f)
                    direction_x = fear_const;
                else
                    direction_x = -fear_const;

                if (c_d_distance != 0f)
                {
                    force_vec = new Vector2((inert_const / (direction_x * c_d_distance)), 0);

                }
            }
            else
                force_vec = Vector2.zero; 

        return force_vec;
    }
    void stuck_check()
    {
        if (force_vec.x + prev_force.x < 1f)
        {
            if (next_time > 0)
            {
                next_time -= Time.deltaTime;
            }
            else
            {
                bool sign = Random.Range(-1, 1) < 0f;
                float num = 1f;
                if (sign)
                    num = -1f;
                this.GetComponent<Rigidbody2D>().AddForce(new Vector2(inert_const * 50 * num, inert_const*inert_const/4f));
                Debug.Log("STUCK:" + num);
                next_time = time_increment;
            }
        }
    }

    void keep_upright()
    {

    }
    void calculate_fear()
    {
        //fear_mouse();
        //fear_pipe();
        //fear_droplets();
        Debug.Log("mouse:" + fear_mouse().x + " pipe:" + fear_pipe().x +" drop:" + fear_droplets().x );
    }
    //it wont have to run every equation each frame.
    void decide_fear()
    {
        Vector2 force_important = Vector2.zero;
        //every time interval change the_fear function
        if(next_d_time > 0)
        {
            force_important = the_fear();
            //Debug.Log("delegate");
            next_d_time -= Time.deltaTime;
        }
        else
        {
            Vector2 mouse_importance = fear_mouse(); // * new Vector2 (1.2f, 1f); 
            Vector2 drop_importance = fear_droplets();
            Vector2 pipe_importance = fear_pipe();
        
            if(mouse_importance.magnitude > drop_importance.magnitude && mouse_importance.magnitude > pipe_importance.magnitude)
            {
                force_important = mouse_importance;
                the_fear = fear_mouse;
                //Debug.Log("mouse");
            }
            else if(drop_importance.magnitude > pipe_importance.magnitude)
            {
                the_fear = fear_droplets;
                force_important = drop_importance;
                //Debug.Log("drop: " + force_important.x);
            }
            else
            {
                the_fear = fear_pipe;
                force_important = pipe_importance;
                //Debug.Log("pipe");
            }
            next_d_time = decision_interval;
        }
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(force_important);
    }
    void attract_flip()
    {
        if(this.gameObject.GetComponent<PinkPot>())
        {
            attract_mark = -1;
        }
    }

    //this might work better in a new script on plant arm
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Drag>() != null && collision.gameObject.GetComponent<Drag>().enabled)
        {
            collision.gameObject.GetComponent<Drag>().plant_slapped();
        }
    }

    // Start is called before the first frame update
    ///test that start will run as soon as its enabled during runtime
    void Start()
    {
        //disable the drag script and prevent player from holding it
        this.gameObject.GetComponent<Drag>().enabled = false;
        Destroy(this.gameObject.GetComponent<Drag>());

        //Collider2D[] cl = arm.GetComponentsInChildren<Collider2D>();
        //Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), arm.GetComponent<Collider2D>(), true);
        inert_const = this.GetComponent<Rigidbody2D>().inertia * 2;
        next_time = 2f;
        attract_flip();
        pipe_parent = GameObject.FindGameObjectWithTag("pipes_parent");
        drop_parent = GameObject.FindGameObjectWithTag("drops_parent");
        w_can = GameObject.FindGameObjectWithTag("w_can");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        force_vec = Vector2.zero;

        decide_fear();
        keep_upright();

       // stuck_check();
        rotate_arm();
    }

}
