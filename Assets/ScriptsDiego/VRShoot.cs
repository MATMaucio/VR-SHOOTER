using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;

public class VRShoot : MonoBehaviour
{
    [SerializeField] private AudioClip revolverSoundClip;
    public XRInputButtonReader xRInputButton;

    private void Update()
    {
        if(Input.GetButton("XRI_Right_Trigger")){
            AudioShoot();
        }
    }
    public void AudioShoot(){
        AudioManager.instance.PlaySound(revolverSoundClip);
    }
}
