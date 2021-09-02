using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuClick : MonoBehaviour
{
    private GameObject state;
    public Event ev;
    public int order;

    private void Menu_Op() 
    {
        switch (order)
        {
            case 0:
                state.GetComponent<MainMenuControl>().First_level();
                break;
            case 1:
                state.GetComponent<MainMenuControl>().Settings_Menu_Toggle();
                break;
            case 2:
                state.GetComponent<MainMenuControl>().Level_Select_Toggle();
                break;
            case 3:
                    state.GetComponent<MainMenuControl>().Credits_Enable();
                break;
            case 4:
                state.GetComponent<MainMenuControl>().Quit_Hard();
                break;
            case 5:
                state.GetComponent<MainMenuControl>().ControlsToggle();
                break;
            default:
                Debug.LogError("Option not added");
                break;
        }

    }
    public void Control_Click() 
    {
        Menu_Op();
    }
    private void Click_Check()
    {
        if((Input.GetButtonUp("Fire1")) && this.gameObject.activeInHierarchy)
        {
            Vector2 cubeRay;
            //take the mouse posiotin in relation to the camera as a 2d vector
            cubeRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //send a ray from the mouse position to the the zero vector , 
            RaycastHit2D cubeHit = Physics2D.Raycast(cubeRay, Vector2.zero);
            if (cubeHit.collider == this.transform.GetComponentInChildren<Collider2D>())
            {
                Menu_Op();
            }

        }

    }
    private void Start()
    {
        state = GameObject.Find("State");
        if (!state)
            Debug.LogError("No State");
    }

    // Update is called once per frame
    void Update()
    {
        Click_Check();
    }
}
