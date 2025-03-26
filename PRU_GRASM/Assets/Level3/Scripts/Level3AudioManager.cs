using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource effectAudioSource;
    [SerializeField] private AudioSource defaulAudioSource;
    [SerializeField] private AudioSource bossAudioSource;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip reloadClip;
    [SerializeField] private AudioClip energyClip;
    public void PlayShootClip()
    {
        effectAudioSource.PlayOneShot(shootClip);
    }
    public void PlayReloadClip()
    {
        effectAudioSource.PlayOneShot(reloadClip);
    }
    public void PlayEnergyClip()
    {
        effectAudioSource.PlayOneShot(energyClip);
    }
    public void PlayBossClip()
    {
        defaulAudioSource.Stop();
        bossAudioSource.Play();
    }
    public void PlayDefaultClip()
    {
        bossAudioSource.Stop();
        defaulAudioSource.Play();
    }
    public void StopAudioGame()
    {
        bossAudioSource.Stop();
        effectAudioSource.Stop();
       defaulAudioSource.Stop();
    }
}
