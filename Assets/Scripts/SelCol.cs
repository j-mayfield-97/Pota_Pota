using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SelCol : MonoBehaviour
{
    private GameObject selected;
    private GameObject prev_selected;
    private Color orig_col;
    public float color_const;
    private Color c1, c2; 

    public void Deselect()
    {
        if(selected)
        {
            selected = null;
        }
    }
    private void Sel_Marker()
    {
        if(selected)
        {
            SpriteShapeRenderer ren = selected.GetComponent<SpriteShapeRenderer>();
            if (ren)
            {
                ren.color = new Color(orig_col.r, Mathf.PingPong(Time.time, 1f), orig_col.b);
            }

            if((prev_selected != selected) && (prev_selected))
            {
                prev_selected.GetComponent<SpriteShapeRenderer>().color = orig_col;
            }

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject parent = collision.gameObject.transform.parent.gameObject;
        if (parent.GetComponent<LevelClick>() || parent.GetComponent<MenuClick>())
        {
            orig_col = parent.GetComponent<SpriteShapeRenderer>().color;
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject parent = collision.gameObject.transform.parent.gameObject;
        if (parent.GetComponent<LevelClick>() || parent.GetComponent<MenuClick>())
        {
            selected = parent;
        }
    }

    private void Start()
    {
        selected = GameObject.FindGameObjectWithTag("first_pet");
    }
    private void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            if((selected.GetComponent<LevelClick>()) && selected.activeInHierarchy && this.isActiveAndEnabled)
                selected.GetComponent<LevelClick>().Control_Click();

            if((selected.GetComponent<MenuClick>()) && selected.activeInHierarchy && this.isActiveAndEnabled)
                selected.GetComponent<MenuClick>().Control_Click();
        }

        Sel_Marker();
        prev_selected = selected;
    }



}
