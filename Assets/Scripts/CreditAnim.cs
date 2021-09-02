using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditAnim : MonoBehaviour
{
    public GameObject flower_particle_fab;
    private GameObject flower_particle;

    private void OpenCredit()
    {
        flower_particle.SetActive(true);
        //this.transform.parent.gameObject.SetActive(false);
    }
    private void CloseCredit()
    {
        flower_particle.SetActive(false);
        this.transform.parent.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        flower_particle = Instantiate(flower_particle_fab);
        flower_particle.SetActive(false);
    }
    
}
