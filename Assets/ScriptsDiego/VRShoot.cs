using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Rendering;
public class VRShoot : MonoBehaviour
{
    [SerializeField] private AudioClip revolverSoundClip;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float bulletSpeed = 10f;

    public void fireBullet(){
    AudioShoot();
    GameObject spawBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
     spawBullet.GetComponent<Rigidbody>().linearVelocity = spawnPoint.forward * bulletSpeed;
    Destroy (spawBullet, 3f);

    }

    public void AudioShoot(){
        AudioManager.instance.PlaySound(revolverSoundClip);
    }
}
