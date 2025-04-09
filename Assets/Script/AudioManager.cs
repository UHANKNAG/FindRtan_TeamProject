using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    AudioSource audioSource;
    public AudioClip clip;

    private void Awake() {  // 싱글톤
        if (Instance == null) { // 나 혼자일 때때
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {  // 또 존재한다면?
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = this.clip;
        // 변수로 받은 clip을 audioSource 컴포넌트에 있는 clip에 넣어 준다.
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
