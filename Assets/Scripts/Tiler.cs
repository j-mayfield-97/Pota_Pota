using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiler : MonoBehaviour
{
    public GameObject geometry_skin;
    private GameObject geometry;
    // Start is called before the first frame update
    void Start()
    {
        geometry = GameObject.Find("geometry");
        if(geometry)
        {
            apply_skin();
        }
    }

    void apply_skin()
    {
        foreach(Transform rect in geometry.transform)
        {
            GameObject new_tile = Instantiate(geometry_skin, rect);
            //determin if the rectangle is vertical
            if(rect.localScale.y > rect.localScale.x)
            {
                //if yes then if it is on the negative x side of the global origin rotate flip and flip
                if (new_tile.transform.position.x < 0f)
                    new_tile.transform.Rotate(new Vector3(0f, 0f, -90f));
                else
                    new_tile.transform.Rotate(new Vector3(0f, 0f, 90f));
            }
        }
    }
}
