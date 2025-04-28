using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource sfxAudioSource, bgmAudioSource;

    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }

    public void PlaySound(AudioClip clip){
        sfxAudioSource.PlayOneShot(clip);
        bgmAudioSource.PlayOneShot(clip);
    }
}
