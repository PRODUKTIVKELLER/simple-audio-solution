using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPitch : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Apply Random Pitch Range")]
    [Range(-3f, 3f)] public float minPitchRange;
    [Range(-3f, 3f)] public float maxPitchRange;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ApplyRandomPitch()
    {
        audioSource.pitch = (Random.Range(minPitchRange,maxPitchRange));
    }
}
