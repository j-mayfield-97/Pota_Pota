using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FallBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UnityEngine.SceneManagement.Scene active = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        UnityEngine.SceneManagement.SceneManager.LoadScene(active.name);
    }
}
