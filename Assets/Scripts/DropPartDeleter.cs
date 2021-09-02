using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DropPartDeleter : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
    }
    private void self_delete()
    {
        if(this.GetComponent<ParticleSystem>().isStopped)
        {
            Destroy(this.gameObject);

        }
    }
    private void Update()
    {
        self_delete();
    }
}
