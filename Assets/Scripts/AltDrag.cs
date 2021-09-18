using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltDrag : MonoBehaviour
{
    private GameObject selected_object;
    public GameObject selected_marker_fab;
    private GameObject selected_marker;
    private GameObject held_object;
    public GameObject held_marker_fab;
    private GameObject held_marker;

    public Sprite hand_sprite;
    public Texture2D point_texture;
    public Texture2D hand_texture;
    private CursorMode cur_mode = CursorMode.ForceSoftware;

    private float graviy_scale;
    //private float inertia_base = 951f;
    //private float mass_base = 36f;
    public float stick_constant;

    private Vector3 prev_mouse_pos;
    private Vector2 cubeRay;

    private bool controller_on;

    private Component[] draggables;

    private const float controller_select_interval = 0.25f;
    private float next_time = 0f;


    private enum dpad
    {
        LEFT, RIGHT, UP, DOWN
    };

    // Start is called before the first frame update
    private void Rotate_Held()
    {
        if(held_object)
        {
            float rot = Input.GetAxis("Clock") - Input.GetAxis("Counter");
            held_object.GetComponent<Rigidbody2D>().AddTorque(rot * Time.deltaTime * 10000f);
        }

    }
    private void drag_mouse()
    {

    }

    private void drag_controller()
    {
        float vert_ = Input.GetAxis("VertStick");
        float horz_ = Input.GetAxis("HorzStick");


        if(held_object)
        {
            Rigidbody2D r_bod = held_object.GetComponent<Rigidbody2D>();
            Vector2 force = new Vector2( horz_ * Time.deltaTime * stick_constant, vert_ * Time.deltaTime * stick_constant);
            if (r_bod && force.magnitude > 0)
            {
                r_bod.AddForce(force);
            }
        }

    }
    private void First_Drag_Visible()
    {
        draggables = FindObjectsOfType<Drag>();

        foreach (Component index in draggables)
        {
            if (index.gameObject.GetComponentInChildren<Renderer>().isVisible)
            {
                selected_object = index.gameObject;
            }
        }
        selected_marker.transform.SetParent(selected_object.transform);
        selected_marker.transform.localPosition = new Vector3(0, 0, -2f);

    }
    // function based on edward roe post - https://forum.unity.com/threads/clean-est-way-to-find-nearest-object-of-many-c.44315/
    private Transform Closest_Draggable(List<GameObject> drags)
    {
        Transform best_target = null;
        float closest_distance_sqr = Mathf.Infinity;
        Vector3 current_position = selected_object.transform.position;


        foreach (GameObject potential_target in drags)
        {

            Vector3 directionToTarget = potential_target.transform.position - current_position;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closest_distance_sqr)
            {
                closest_distance_sqr = dSqrToTarget;
                best_target = potential_target.transform;
            }
        }

        return best_target;
    }

    private bool children_visible(GameObject go)
    {
        bool ret = false;
        Renderer[] rends = go.GetComponentsInChildren<Renderer>();

        foreach(Renderer rend in rends)
        {
            ret = ret || rend.isVisible;
        }

        return ret;
    }
    private GameObject closest_select_visible(dpad dir)
    {
        draggables = FindObjectsOfType<Drag>();
        List<GameObject> res_list = new List<GameObject>();
        GameObject ret;

        foreach(Component index in draggables)
        {
            //make this a switch instead
            if(dir == dpad.LEFT)
            {
                if(children_visible(index.gameObject) && (index.gameObject.transform.position.x < selected_object.transform.position.x) )
                {
                    res_list.Add(index.gameObject);
                }
            }
            else if(dir == dpad.RIGHT)
            {
                if(children_visible(index.gameObject) && (index.gameObject.transform.position.x > selected_object.transform.position.x))
                {
                    res_list.Add(index.gameObject);
                }
            }
            else if(dir == dpad.UP)
            {
                if(children_visible(index.gameObject) && (index.gameObject.transform.position.y > selected_object.transform.position.y))
                {
                    //reject if there y distance is extremely close
                    if (Mathf.Abs(index.gameObject.transform.position.y - selected_object.transform.position.y) < 0.001f)
                        continue;

                    res_list.Add(index.gameObject);
                }
            }
            else if(dir == dpad.DOWN)
            {
                if (children_visible(index.gameObject) && (index.gameObject.transform.position.y < selected_object.transform.position.y))
                {
                    //reject if there y distance is extremely close
                    if (Mathf.Abs(index.gameObject.transform.position.y - selected_object.transform.position.y) < 0.001f)
                        continue;
                    res_list.Add(index.gameObject);
                }
            }
        }
        if (res_list.Count == 0)
            return selected_object;

        ret = Closest_Draggable(res_list).gameObject;
        //reset time
        next_time = controller_select_interval;
        //set select amrker to new object
        selected_marker.SetActive(true);
        selected_marker.transform.SetParent(ret.transform);
        selected_marker.transform.localPosition = new Vector3(0, 0, -2f);
        //deactivate held marker, and drop boject
        controller_drop();



        return ret;
    }
    private void selector_controller()
    {
        float hzdp = Input.GetAxis("HorzDpad");
        float vtdp = Input.GetAxis("VertDpad");
        if (selected_object || hzdp != 0 || vtdp != 0)
        {
            if (hzdp > 0)
            {
                selected_object = closest_select_visible(dpad.RIGHT);
            }
            else if (hzdp < 0)
            {
                selected_object = closest_select_visible(dpad.LEFT);
            }
            else if (vtdp < 0)
            {
                selected_object = closest_select_visible(dpad.DOWN);
            }
            else if (vtdp > 0)
            {
                selected_object = closest_select_visible(dpad.UP);
            }

        }
        else
            First_Drag_Visible();
    }
    private void controller_drop()
    {
        if (held_object)
        {
            if (held_object.activeInHierarchy)
            {
                held_object.GetComponent<Rigidbody2D>().gravityScale = graviy_scale;
                held_marker.SetActive(false);
                selected_marker.SetActive(true);
                held_object = null;

            }
            else
                held_object = null;
        }

    }
    public void plant_slap(GameObject go)
    {
        if(held_object == go)
        {
            controller_drop();
        }
    }
    public void held_disabled(GameObject go)
    {
        if(held_object == go)
        {
            //cant change parent while , current parent is being disable
            /*            held_marker.transform.parent = null;
                        selected_marker.transform.parent = null;
            */
            held_marker = Instantiate(held_marker_fab);
            held_marker.SetActive(false);
            selected_marker = Instantiate(selected_marker_fab);
            controller_drop();
            First_Drag_Visible();
        }
    }
    private void pick_up_controller()
    {
        bool grab_button = Input.GetButtonDown("Grab");
        if(grab_button)
        {
            //if already holding then drop
            if(held_object)
            {
                controller_drop();
            }
            else
            {
                if (selected_object)
                {
                    //switch pipe instead of hold
                    if(selected_object.GetComponent<WaterSwitch>())
                    {
                        selected_object.GetComponent<WaterSwitch>().switch_parent_pipe();
                        return;
                    }

                    held_object = selected_object;
                    graviy_scale = held_object.GetComponent<Rigidbody2D>().gravityScale;

                    held_object.GetComponent<Rigidbody2D>().gravityScale = 0f;
                    held_marker.SetActive(true);
                    //set select amrker to new object
                    held_marker.transform.SetParent(held_object.transform);
                    held_marker.transform.localPosition = new Vector3(0, 0, -2f);
                    //deactivate held marker, and drop boject
                    selected_marker.SetActive(false);

                }

            }
            next_time = controller_select_interval;
        }
    }

    private void pick_up_mouse()
    {
    }
    public void cursor_grab()
    {
        Cursor.SetCursor(hand_texture, Vector2.zero, cur_mode);
    }
    public void cursor_drop()
    {
        Cursor.SetCursor(point_texture, Vector2.zero, cur_mode);
    }
    public void flip_controller()
    {
        bool flp = Input.GetButtonDown("Flip");
        if (flp)
        {
            if(held_object)
            {
               held_object.GetComponent<Drag>().horz_flip();
                next_time = controller_select_interval;
            }
        }
    }
    void Start()
    {
        Cursor.SetCursor(point_texture, Vector2.zero, cur_mode);
        held_marker = Instantiate(held_marker_fab);
        selected_marker = Instantiate(selected_marker_fab);
        draggables = FindObjectsOfType<Drag>();
    }
    void Update()
    {
        next_time -= Time.deltaTime;
        if(next_time <= 0f)
        {
            selector_controller();
            pick_up_controller();
            flip_controller();
        }
        pick_up_mouse();
    }
    void FixedUpdate()
    {
        drag_controller();
        drag_mouse();
        Rotate_Held();
        

        prev_mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

}
