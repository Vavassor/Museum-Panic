using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bone : MonoBehaviour
{
    public List<AudioClip> placeSounds;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (gameObject.tag == "Stuck")
        {
            return;
        }
        if (gameObject.tag == "Sticky" && collision.collider.tag == "Stuck")
        {
            gameObject.tag = "Stuck";
            GameObject player = GameObject.FindWithTag("Player");
            PlayerController controller = player.GetComponent<PlayerController>();
            controller.LetGoOfObject();
            SoundUtils.PlaySoundNonrepeating(audioSource, placeSounds);
        }
    }
}