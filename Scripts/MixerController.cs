using UnityEngine;
using UnityEngine.Audio;

public class MixerController : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    //synchronize handle of slider and mixer handle
    public void SetSoundLevel( float soundLevel)
    {
        mixer.SetFloat("MasterVolume", soundLevel);
    }
}
