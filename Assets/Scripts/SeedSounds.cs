using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedSounds : MonoBehaviour
{
    AudioSource shared_seed_sound;

    private void Start()
    {
        shared_seed_sound = this.transform.parent.GetComponent<AudioSource>();

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!shared_seed_sound.isPlaying)
        {
            shared_seed_sound.Play();
        }
    }
}
