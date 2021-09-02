using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    private Component state;
    private int seeds_fallen;
    // Start is called before the first frame update
    void Start()
    {
        state = FindObjectOfType<Level_State>();
        if (!state)
            Debug.LogError("NoState");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.gameObject.GetComponent<Pot>())||(collision.gameObject.GetComponent<CanFill>()))
        {
            state.GetComponent<Level_State>().fail_trigger_flag();

            foreach(SpriteRenderer ren in collision.gameObject.GetComponentsInChildren<SpriteRenderer>())
            {
                ren.color = Color.black;
            }
            
            Debug.Log("loss");
        }
        if(collision.gameObject.CompareTag("seed"))
        {
            ++seeds_fallen;
            foreach (SpriteRenderer ren in collision.gameObject.GetComponentsInChildren<SpriteRenderer>())
            {
                ren.color = Color.black;
            }
            //level 7 & 8 only
            if (state.GetComponent<Level_State>().goal_order == 3)
                state.GetComponent<Level_State>().fail_trigger_flag();

            if (seeds_fallen >= 5)
                state.GetComponent<Level_State>().fail_trigger_flag();
        }
    }
}
