using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelClick : MonoBehaviour
{
    private GameObject state;
    private GameObject sel_col;
    public Event ev;
    private bool selected;
    public int order;

    private void Text_And_Pic()
    {
        this.gameObject.GetComponentInChildren<TextMeshPro>().text = "" + order;
        this.gameObject.GetComponentInChildren<TextMeshPro>().alignment = TextAlignmentOptions.Center;

        // place a pic of the level lext to the text
    }
    public void Control_Click()
    {
        state.GetComponent<MainMenuControl>().Level_Loader(order);
    }

    private void Click_Check()
    {
        if (Input.GetButtonUp("Fire1") && this.gameObject.activeInHierarchy)
        {
            Vector2 cubeRay;
            //take the mouse posiotin in relation to the camera as a 2d vector
            cubeRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //send a ray from the mouse position to the the zero vector , 
            RaycastHit2D cubeHit = Physics2D.Raycast(cubeRay, Vector2.zero);


            if (cubeHit.collider == this.transform.GetComponentInChildren<Collider2D>())
            {
                state.GetComponent<MainMenuControl>().Level_Loader(order);
            }

        }

    }
    private void Start()
    {
        state = GameObject.Find("State");
        sel_col = GameObject.Find("sel_col");
        if (!state || !sel_col)
            Debug.LogError("No State or Collider");

        Collider2D one = sel_col.GetComponent<Collider2D>();
        Collider2D two = this.GetComponent<Collider2D>();
        Text_And_Pic();
    }

    // Update is called once per frame
    void Update()
    {
        Click_Check();

/*        if (this.GetComponent<Collider2D>())
        {
        Collider2D[] cols = new Collider2D[10];
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        this.GetComponent<Collider2D>().OverlapCollider(filter, cols);

        }
*/

    }
}
