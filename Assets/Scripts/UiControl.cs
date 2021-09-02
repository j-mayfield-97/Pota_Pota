using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UiControl : MonoBehaviour
{
    public GameObject p_menu;
    public GameObject p_panel;
    public GameObject s_menu;
    public GameObject g_menu;
    public GameObject l_menu;
    public GameObject p_button1;
    public GameObject s_button1;
    public GameObject g_button1;
    public GameObject l_button1;
    public TMP_FontAsset[] fonts;
    private TextMeshProUGUI[] defualt_ui;
    private TextMeshPro[] defualt_tmp;

    private void Start()
    {
        p_button1 = FindObjectOfType<PLButt>(true).gameObject;
        s_button1 = FindObjectOfType<MVButt>(true).gameObject;
        g_button1 = FindObjectOfType<NLButt>(true).gameObject;
        l_button1 = FindObjectOfType<RTButt>(true).gameObject;

        //ResoChange(0);
    }
    public void Unpause()
    {
        this.GetComponent<Level_State>().game_pasused = false;
        this.p_menu.SetActive(false);
        Time.timeScale = 1f;
        p_menu.GetComponent<AudioSource>().Play();
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().pitch = 1f;
    }
    public void Restart_Level()
    {
        p_menu.GetComponent<AudioSource>().Play();
        UnityEngine.SceneManagement.Scene active = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        UnityEngine.SceneManagement.SceneManager.LoadScene(active.name);
        Time.timeScale = 1f;
    }
    public void Settings_Menu_Toggle()
    {
        p_menu.GetComponent<AudioSource>().Play();
        //have settings menu change music, font, volume slider, resolution change

        if (!s_menu.activeSelf)
        {
            s_menu.SetActive(true);
            p_panel.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(this.s_button1);

        }
        else
        {
            s_menu.SetActive(false);
            p_panel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(this.p_button1);

        }

    }
    public void Main_Menu()
    {
        p_menu.GetComponent<AudioSource>().Play();
        //have main menu be scene 0
        //AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainMenuSc");
        //UnityEngine.SceneManagement.Scene main_menu = UnityEngine.SceneManagement.SceneManager.GetSceneAt(0);
        //UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuSc");
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        //// do i have to unload previous scene??
    }    
    public void Next_Level()
    {
        int b_index = SceneManager.GetActiveScene().buildIndex;
        //SceneManager.LoadScene(b_index + 1);
        SceneManager.LoadScene(b_index + 1);
    }
    public void Quit_Hard()
    {
        p_menu.GetComponent<AudioSource>().Play();
        //have quit animation?
        //thank you for playing message
        Application.Quit();
    }
    /// <summary>
    /// settings per player master volume
    /// </summary>
    /// <param name="slide_val"></param>
    public void Master_Vol(float slide_val)
    {
        p_menu.GetComponent<AudioSource>().Play();
        AudioListener.volume = slide_val;
        GameObject.Find("SliderV").GetComponent<Slider>().value = slide_val;
        PlayerPrefs.SetFloat("m_vol", slide_val);
        PlayerPrefs.Save();
    }
    public void Music_Vol(float slide_val)
    {
        p_menu.GetComponent<AudioSource>().Play();
        //just link the camera as public
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().volume = 0.15f * slide_val;
        GameObject.Find("SliderMV").GetComponent<Slider>().value = slide_val;
        PlayerPrefs.SetFloat("s_vol", slide_val);
        PlayerPrefs.Save();
    }
    public void get_defaults_tmp()
    {
        TextMeshProUGUI[] All_TMPtext = FindObjectsOfType<TextMeshProUGUI>(true);

        defualt_ui = new TextMeshProUGUI[All_TMPtext.Length];

        for(int i = 0; i < All_TMPtext.Length; i++) 
        {
            defualt_ui[i] = All_TMPtext[i];
        }

        TextMeshPro[] All_TMP3dtext = FindObjectsOfType<TextMeshPro>(true);
        defualt_tmp = new TextMeshPro[All_TMP3dtext.Length];

        for(int i = 0; i < All_TMP3dtext.Length; i++) 
        {
            defualt_tmp[i] = All_TMP3dtext[i];
        }

    }
    //if order of texts changes im screwed
    private void set_defaults_fonts()
    {
        TextMeshProUGUI[] All_TMPUItext = FindObjectsOfType<TextMeshProUGUI>(true);
        TextMeshPro[] All_TMP3dtext = FindObjectsOfType<TextMeshPro>(true);

        if((All_TMPUItext.Length == defualt_ui.Length) ||(All_TMP3dtext.Length == defualt_tmp.Length))
        {
            Debug.LogError("Big Whoopsie Texts cant return to defaults : sizes dont match");
        }

        for (int i = 0; i < All_TMPUItext.Length; i++)
        {
            All_TMPUItext[i].font = defualt_ui[i].font;
            All_TMPUItext[i].enableAutoSizing = true;
        }

        for (int i = 0; i < All_TMP3dtext.Length; i++)
        {
            All_TMP3dtext[i].font = defualt_tmp[i].font;
            All_TMPUItext[i].enableAutoSizing = true;

        }
    }
    public void FontChange(int choice)
    {
        p_menu.GetComponent<AudioSource>().Play();

        if(choice == 0)
        {
            //set_defaults_fonts();
            return;
        }

        TextMeshProUGUI[] All_TMPUItext = FindObjectsOfType<TextMeshProUGUI>(true);

        foreach(TextMeshProUGUI text in All_TMPUItext)
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
        //Component d1 = GameObject.Find("DropdownFont").GetComponent<Dropdown>();
        GameObject.Find("DropdownFont").GetComponent<TMP_Dropdown>().value = choice;
        PlayerPrefs.SetInt("font_int", choice);
        PlayerPrefs.Save();
    }
    public void ResoChange(int choice1)
    {
        p_menu.GetComponent<AudioSource>().Play();

        switch(choice1)
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

        GameObject.Find("DropdownReso").GetComponent<TMP_Dropdown>().value = choice1;
        PlayerPrefs.SetInt("resolution", choice1);
        PlayerPrefs.Save();
    }
    public void FullScreenToggle(bool choice2)
    {
        p_menu.GetComponent<AudioSource>().Play();
        int choice_int;
        if(choice2)
        {
            choice_int = 1;
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.MaximizedWindow);
        }
        else
        {
            choice_int = 0;
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);
        }


        GameObject.Find("ToggleFull").GetComponent<Toggle>().isOn = choice2;
        PlayerPrefs.SetInt("fullscreen", choice_int);
        PlayerPrefs.Save();
    }
}
