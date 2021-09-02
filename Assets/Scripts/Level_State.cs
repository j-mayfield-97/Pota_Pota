using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Cinemachine;

public class Level_State : MonoBehaviour
{
    //all of this is optionally visible in ui
    public int current_level;
    public int pots_in_level;
    public int cans_in_level;
    public int pipes_in_level;
    public int drops_in_can;
    public int drops_in_level;
    public int drops_in_pot;
    private int total_cap;
    public string current_filter;
    public bool all_filled;
    public int[] all_pot_drops;
    public float time_level;
    public bool game_pasused = false;
    private GameObject cans;
    private Component can;
    private GameObject main_cam;
    public List <Component> pots;
    private GameObject drops_parent;
    public GameObject c_d_text;
    public GameObject percent_tmp;
    public GameObject win_particle_parent;
    public GameObject loss_particle_parent;
    private CinemachineVirtualCamera cam_pot;
    public string goal;
    public int goal_order;
    private IEnumerator coroutine;
    private bool done = false;
    private bool fail_trigger = false;
    private bool level8_cam = true;
    private delegate bool FillCond();
    FillCond the_fill_cond;



    private void Start()
    {
        //does not currently scale wiht more than one pot
        //get an array of pots and if 0.is_alive_full() && 1.is_alive_full() , then have goal_met = true;
        current_level = SceneManager.GetActiveScene().buildIndex;
        can = FindObjectOfType<CanFill>();
        
        main_cam = GameObject.FindGameObjectWithTag("MainCamera");
        cam_pot = GameObject.FindGameObjectWithTag("p_cam").GetComponent<CinemachineVirtualCamera>();

        pots = new List<Component> (FindObjectsOfType<Pot>());
        pots_in_level = pots.Count;

        drops_parent = GameObject.FindGameObjectWithTag("drops_parent");
        total_cap = pots_total_capacity();

        this.GetComponent<UiControl>().get_defaults_tmp();
    }
    private void Update()
    {
        time_level = Time.timeSinceLevelLoad;
        drops_in_level = count_pipe_drops();
        check_game_paused();

        failed_function();
        check_goals_met(goal_order);
    }
    public void pot_add(Component comp)
    {
        pots.Add(comp);
    }
    public void check_game_paused()
    {
        if (Input.GetButtonUp("Pause"))
        {
            game_pasused = !game_pasused;
            if(game_pasused)
            {
                Time.timeScale = 0f;
                this.GetComponent<UiControl>().p_menu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(this.GetComponent<UiControl>().p_button1);
                main_cam.GetComponent<AudioSource>().pitch = 0.85f;
            }
            else
            {
                Time.timeScale = 1f;
                this.GetComponent<UiControl>().p_menu.SetActive(false);
                main_cam.GetComponent<AudioSource>().pitch = 1f;

            }
        }
    }
    public void check_prefs()
    {
        if(this.GetComponent<UiControl>().p_menu.activeSelf)
        {
            if (PlayerPrefs.HasKey("m_vol"))
                this.GetComponent<UiControl>().Master_Vol(PlayerPrefs.GetFloat("m_vol"));
            if (PlayerPrefs.HasKey("s_vol"))
                this.GetComponent<UiControl>().Music_Vol(PlayerPrefs.GetFloat("s_vol"));
            if (PlayerPrefs.HasKey("font_int"))
                this.GetComponent<UiControl>().FontChange(PlayerPrefs.GetInt("font_int"));
            if (PlayerPrefs.HasKey("resolution"))
                this.GetComponent<UiControl>().FontChange(PlayerPrefs.GetInt("resolution"));
            if (PlayerPrefs.HasKey("fullscreen"))
                this.GetComponent<UiControl>().FontChange(PlayerPrefs.GetInt("fullscreen"));
        }
    }
    public int count_pipe_drops()
    {
        int total = 0;
        foreach(Transform pipe in drops_parent.transform)
        {
            int count = 0;

            foreach (Transform kid in pipe)
            {
                if (kid.gameObject.activeSelf)
                    count++;
            }
            total += count;
        }
        return total;
    }
    public int pots_total_capacity()
    {
        int ret = 0;
        for(int i = 0; i < pots.Count; i++)
        {
            if (pots[i].GetComponent<Pot>().next_living == null)
            {
                ret += pots[i].GetComponent<Pot>().get_drops_cap();
            }
            else
                ret += pots[i].GetComponent<Pot>().get_drops_cap() * 2;
        }
        return ret;
    }    
    public int pots_total_drops()
    {
        int ret = 0;
        for(int i = 0; i < pots.Count; i++)
        {
            if (pots[i].GetComponent<Pot>().gameObject.activeSelf)
            {
                ret += pots[i].GetComponent<Pot>().get_drops_in_pot();
            }
        }
        percent_tmp.GetComponent<TextMeshProUGUI>().text = goal + " : " + ret + " / " + total_cap;
        return ret;
    }    
    /// <summary>
    /// Only use when there are mix spawned of Ai pots and regular pots
    /// </summary>
    /// <returns></returns>
    public bool pots_all_filled()
    {
        //i guess if theres no pots return true?
        bool ret = true;
        for(int i = 0; i < pots.Count; i++)
        {
            if(pots[i].GetComponent<PotAI>() != null)
            {
                ret = ret && pots[i].GetComponent<Pot>().is_alive_full();
            }
            else
                ret = ret && pots[i].GetComponent<Pot>().is_full();
        }
        return ret;
    }
    public bool pots_all_filled_special()
    {
        //i guess if theres no pots return true?
        bool ret = true;
        for(int i = 0; i < pots.Count; i++)
        {
                ret = ret && pots[i].GetComponent<Pot>().is_full();
        }
        return ret;
    }
    public int cans_total()
    {
        return 0;
    }
    public int cans_drops()
    {
        return 0;
    }    
    public int pot_drops()
    {
        return 0;
    }
    private IEnumerator wait_anim(float waitTime)
    {
        cam_pot.GetComponent<CinemachineVirtualCamera>().m_Priority = 20;
        Instantiate(win_particle_parent);
        yield return new WaitForSeconds(waitTime);
        this.GetComponent<UiControl>().g_menu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(this.GetComponent<UiControl>().g_button1);

        print("Coroutine ended: " + Time.time + " seconds");
        cam_pot.GetComponent<CinemachineVirtualCamera>().m_Priority = 1;
    }

    private void fill_goal()
    {
        //update the objective text 
        pots_total_drops();

        if (the_fill_cond() && !done)
        {
            this.GetComponent<UiControl>().g_menu.GetComponent<AudioSource>().Play();
            coroutine = wait_anim(3.0f);
            StartCoroutine(coroutine);

            done = true;
        }
    }
    private IEnumerator case3_cam(float w_time)
    {
        yield return new WaitForSeconds(w_time);
        cam_pot.m_Priority = 9;
        level8_cam = false;
    }
    private void case2_fail_check()
    {
        int drops_left = FindObjectsOfType<DropInteract>().Length;
        int in_pot = pots[0].GetComponent<Pot>().get_drops_in_pot();
        if ((drops_left + in_pot) < total_cap)
        {
            fail_trigger_flag();
        }
    }
    public void fail_trigger_flag()
    {
        fail_trigger = true;
    }
    public void failed_function()
    {
        if(!done && fail_trigger)
        {
            Instantiate(loss_particle_parent);
            this.GetComponent<UiControl>().l_menu.SetActive(true);
            this.GetComponent<UiControl>().l_menu.GetComponent<AudioSource>().Play();
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(this.GetComponent<UiControl>().l_button1);

            done = true;
        }
    }

    public void check_goals_met(int g)
    {
        switch (g)
        {
            case 0:
                the_fill_cond = pots_all_filled;
                fill_goal();
                break;
            case 1:
                the_fill_cond = pots_all_filled_special;
                fill_goal();
                break;
            case 2:
                //level 5 only
                the_fill_cond = pots_all_filled;
                fill_goal();
                case2_fail_check();
                break;
            case 3:
                //level 8 only
                the_fill_cond = pots_all_filled_special;
                fill_goal();
                if(level8_cam)
                {
                    coroutine = case3_cam(2f);
                    StartCoroutine(coroutine);
                }
                break;
            default:
                Debug.Log("fix goal order int");
                break;
        }
    }
}
