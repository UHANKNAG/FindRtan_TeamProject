using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class My_AudioManager : MonoBehaviour
{
    public static My_AudioManager instance;

    AudioSource audioSource;
    public AudioClip clip;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = this.clip;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
