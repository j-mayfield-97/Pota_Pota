using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geometry : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ///use a set function instead , also figure out how to make members accessible but not public 
        //collision.gameObject.GetComponent<Drag>().geometry_stop = true;
        Debug.Log(collision.gameObject + ":inetersect");
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
       //collision.gameObject.GetComponent<Drag>().geometry_stop = true;
        Debug.Log(collision.gameObject + ":stay");

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //collision.gameObject.GetComponent<Drag>().geometry_stop = false;
        Debug.Log(collision.gameObject + ":exit");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
