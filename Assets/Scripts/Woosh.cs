using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Woosh : MonoBehaviour
{
    public float force;
    // Start is called before the first frame update
    private void OnCollisionStay2D(Collision2D collision)
    {

        if(collision.gameObject.CompareTag("drops"))
        {
            Vector2 force_vec = new Vector2(force, 0f);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(force_vec);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("drops"))
        {
            Vector2 force_vec = new Vector2(force, 0f);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(force_vec);
        }
    }
}
