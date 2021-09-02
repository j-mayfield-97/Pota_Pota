using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuControl : MonoBehaviour
{
    public float rot_speed;
    public GameObject p_menu;
    public GameObject p_panel;
    public GameObject s_menu;
    public GameObject credit_screen;
    public GameObject p_button1;
    public GameObject s_button1;
    public GameObject back_button1;
    public GameObject Petal_Levels;
    public GameObject control_panel;
    public TMP_FontAsset[] fonts;
    private GameObject butt;
    private GameObject sel_col;

    private void Start()
    {
        butt = GameObject.FindGameObjectWithTag("dirt");
        butt.SetActive(false);
        sel_col = FindObjectOfType<SelCol>().gameObject;
        ResoChange(1);
    }
    private void Update()
    {
        float rot = Input.GetAxis("Rotat");
        float cw = Input.GetAxis("Clock");
        float ccw = Input.GetAxis("Counter");

        if(Input.GetAxis("HorzDpad") != 0)
        {
            if(butt.activeInHierarchy)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(this.butt);
            }

        }

        Rotate_Petals(rot);
        Rotate_Petals(cw - ccw);
    }
    private void Rotate_Petals(float rotation)
    {
        if((rotation != 0) && (p_panel.activeInHierarchy || Petal_Levels.activeInHierarchy))
        {
            rotation *= rot_speed * Time.deltaTime;

            p_panel.transform.Rotate(0, 0, rotation);
            Petal_Levels.transform.rotation = p_panel.transform.rotation;

            EventSystem.current.SetSelectedGameObject(null);
        }
    }
    public void First_level()
    {
        p_menu.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene(1);
    }
    public void Next_Level()
    {
        int b_index = SceneManager.GetActiveScene().buildIndex;
        //SceneManager.LoadScene(b_index + 1);
        SceneManager.LoadScene(b_index + 1);
    }

    public void Settings_Menu_Toggle()
    {
        p_menu.GetComponent<AudioSource>().Play();
        //have settings menu change music, font, volume slider, resolution change

        if (!s_menu.activeInHierarchy)
        {
            s_menu.SetActive(true);
            p_panel.SetActive(false);
            Petal_Levels.SetActive(false);
            sel_col.GetComponent<SelCol>().Deselect();
            sel_col.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(this.s_button1);

        }
        else
        {
            s_menu.SetActive(false);
            p_panel.SetActive(true);
            Petal_Levels.SetActive(false);
            sel_col.SetActive(true);
            sel_col.GetComponent<SelCol>().Deselect();
            EventSystem.current.SetSelectedGameObject(null);

            /*            EventSystem.current.SetSelectedGameObject(null);
                        EventSystem.current.SetSelectedGameObject(this.p_button1);
            */
        }

    }
    public void Credits_Enable()
    {
        p_menu.GetComponent<AudioSource>().Play();

        credit_screen.SetActive(true);
    }
    public void Level_Select_Toggle()
    {
        p_menu.GetComponent<AudioSource>().Play();

        if (!Petal_Levels.activeInHierarchy)
        {
            Petal_Levels.SetActive(true);
            p_panel.SetActive(false);
            if (butt)
                butt.SetActive(true);

        }
        else
        {
            Petal_Levels.SetActive(false);
            p_panel.SetActive(true);
            if (butt)
                butt.SetActive(false);

            EventSystem.current.SetSelectedGameObject(null);
        }

    }
    public void Level_Loader(int choice)
    {
        p_menu.GetComponent<AudioSource>().Play();
        SceneManager.LoadSceneAsync(choice);
    }
    public void Master_Vol(float slide_val)
    {
        p_menu.GetComponent<AudioSource>().Play();
        AudioListener.volume = slide_val;
        PlayerPrefs.SetFloat("m_vol", slide_val);
        PlayerPrefs.Save();
    }
    public void Music_Vol(float slide_val)
    {
        p_menu.GetComponent<AudioSource>().Play();
        //just link the camera as public
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().volume = 0.15f * slide_val;
        PlayerPrefs.SetFloat("s_vol", slide_val);
        PlayerPrefs.Save();
    }
    public void FontChange(int choice)
    {

        p_menu.GetComponent<AudioSource>().Play();

        TextMeshProUGUI[] All_TMPGUItext = FindObjectsOfType<TextMeshProUGUI>(true);

        foreach (TextMeshProUGUI text in All_TMPGUItext)
        {
            text.font = fonts[choice];
            text.enableAutoSizing = true;

        }

        TextMeshPro[] All_TMP3dtext = FindObjectsOfType<TextMeshPro>(true);

        foreach (TextMeshPro text in All_TMP3dtext)
        {
            text.font = fonts[choice];
            text.enableAutoSizing = true;

        }
        PlayerPrefs.SetFloat("font_name", choice);
        PlayerPrefs.Save();
    }
    public void Quit_Hard()
    {
        p_menu.GetComponent<AudioSource>().Play();
        //have quit animation?
        //thank you for playing message
        Application.Quit();
    }
    public void ResoChange(int choice1)
    {
        p_menu.GetComponent<AudioSource>().Play();

        switch (choice1)
        {
            case 0:
                Screen.SetResolution(920, 400, Screen.fullScreen);
                break;
            case 1:
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                break;
            case 2:
                Screen.SetResolution(2560, 1440, Screen.fullScreen);
                break;
            default:
                break;
        }

        PlayerPrefs.SetInt("resolution", choice1);
        PlayerPrefs.Save();
    }

    public void ControlsToggle()
    {
        p_menu.GetComponent<AudioSource>().Play();
        if (!control_panel.activeInHierarchy)
        {
            control_panel.SetActive(true);
            p_panel.SetActive(false);
            Petal_Levels.SetActive(false);
            sel_col.GetComponent<SelCol>().Deselect();
            sel_col.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(this.back_button1);

        }
        else
        {
            control_panel.SetActive(false);
            p_panel.SetActive(true);
            Petal_Levels.SetActive(false);
            sel_col.SetActive(true);
            sel_col.GetComponent<SelCol>().Deselect();
            EventSystem.current.SetSelectedGameObject(null);

            /*            EventSystem.current.SetSelectedGameObject(null);
                        EventSystem.current.SetSelectedGameObject(this.p_button1);
            */
        }

    }
    public void FullScreenToggle(bool choice2)
    {
        p_menu.GetComponent<AudioSource>().Play();
        int choice_int;
        if (choice2)
        {
            choice_int = 1;
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.MaximizedWindow);
        }
        else
        {
            choice_int = 0;
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);
        }


        PlayerPrefs.SetInt("fullscreen", choice_int);
        PlayerPrefs.Save();
    }

}
