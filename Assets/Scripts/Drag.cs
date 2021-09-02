using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private bool holding;
    private Vector3 mouse_obj_pos;
    private Vector3 prev_pos;
    private Vector3 prev_mouse_pos;
    private Vector2 cubeRay;
    private float z_layer;
    private List<Collider2D> colls = new List<Collider2D>();
    private Collider2D[] all_colls;
    private float gravity_scale;
    private bool skip_drag;
    private Collider2D other_col;
    private float inertia_base = 951f;
    private float mass_base = 36f;
    //horizantal flip flags
    private bool horz = false;


    private void pick_up_let_go()
    {
        //if left button clicked
        if (Input.GetMouseButtonDown(0))
        {
            //take the mouse posiotin in relation to the camera as a 2d vector
            cubeRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //send a ray from the mouse position to the the zero vector , 
            RaycastHit2D cubeHit = Physics2D.Raycast(cubeRay, Vector2.zero);

            for (int i = 0; i < all_colls.Length; i++)
            {
                //cubeHit will be null if it hits no collider , else it will return an object with the item it hit as a component
                if (cubeHit.collider == all_colls[i])
                {
                    //Debug.Log(cubeRay);
                    holding = true;
/*                    Cursor.SetCursor(hand_texture, Vector2.zero, cur_mode);
*/
                    //turn off gravity while being held
                    this.GetComponent<Rigidbody2D>().gravityScale = 0f;

                    mouse_obj_pos = new Vector3 (cubeRay.x - transform.position.x, cubeRay.y - transform.position.y, transform.position.z);

                    cursor_swap(holding);
                    return;
                }
            }


        }
        //mouse up release object and return gravity
        if (Input.GetMouseButtonUp(0) && holding) 
        {
            this.GetComponent<Rigidbody2D>().gravityScale = gravity_scale;
/*            Cursor.SetCursor(point_texture, Vector2.zero, cur_mode);
*/          holding = false;
            cursor_swap(holding);
        }
    }
    private void cursor_swap(bool hldn)
    {
        if(holding)
        {
            FindObjectOfType<AltDrag>().cursor_grab();
        }
        else
            FindObjectOfType<AltDrag>().cursor_drop();
    }

    private void flipper()
    {
        if (Input.GetButtonUp("Fire2") && holding)
        {
            horz_flip();
        }

    }

    private void drag()
    {
        if (holding)
        {

            Vector3 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Vector3 trial = new Vector3(mouse_pos.x - mouse_obj_pos.x, mouse_pos.y - mouse_obj_pos.y, 0);
            Vector2 mouse_diff = new Vector2((mouse_pos.x - prev_mouse_pos.x) * inertia_base, (mouse_pos.y - prev_mouse_pos.y)* inertia_base);

            Vector2 obj_mouse = new Vector2((  mouse_pos.x - this.transform.position.x) * mass_base*2,
                                               (mouse_pos.y - this.transform.position.y) * mass_base*2);

            this.GetComponent<Rigidbody2D>().AddForce(obj_mouse);

            float rot = Input.GetAxisRaw("Rotat");

            if (rot != 0)
            {
                this.GetComponent<Rigidbody2D>().AddTorque(-100f * rot);
            }

        }
        else
        {
            //this.GetComponent<Rigidbody2D>().gravityScale = gravity_scale;
        }
    }
    public void plant_slapped()
    {
        FindObjectOfType<AltDrag>().plant_slap(this.gameObject);
        this.GetComponent<Rigidbody2D>().gravityScale = gravity_scale;
/*        Cursor.SetCursor(point_texture, Vector2.zero, cur_mode);
*/        holding = false;
    }
    public void horz_flip()
    {
        this.transform.localScale = new Vector3(-1 * this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        horz = !horz;
    }
    public bool horz_check()
    {
        return horz;
    }
    // Start is called before the first frame update
    void Start()
    {
        holding = false;
        z_layer = this.transform.position.z;

        all_colls = this.GetComponentsInChildren<Collider2D>();
        gravity_scale = this.GetComponent<Rigidbody2D>().gravityScale;
        inertia_base = 950f;
        mass_base = 36f;

       // Cursor.SetCursor(point_texture, Vector2.zero, cur_mode);

        //inertia_base = Mathf.Pow((GetComponent<Rigidbody2D>().inertia + 1), 2);
        //mass_base = Mathf.Pow((GetComponent<Rigidbody2D>().mass + 1), 2);

    }
    private void OnDisable()
    {
        FindObjectOfType<AltDrag>().held_disabled(this.gameObject);
    }

    void Update()
    {
        pick_up_let_go();
        flipper();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        drag();
        prev_mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
