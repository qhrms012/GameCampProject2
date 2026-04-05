using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{

    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;


    public AudioMixer audioMixer;
    public AudioMixerGroup bgmGroup;
    public AudioMixerGroup sfxGroup;


    AudioSource bgmSource;
    AudioSource sfxSource;

    private void Awake()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        bgmSource.outputAudioMixerGroup = bgmGroup;
        sfxSource.outputAudioMixerGroup = sfxGroup;

        bgmSource.loop = true;
    }

    public void PlayBgm(Bgm bgm, bool isOn)
    {
        if (isOn)
        {
            bgmSource.clip = bgmClips[(int)bgm];
            bgmSource.Play();
        }
        else
        {
            bgmSource.Stop();
        }
    }
    public void PlaySfx(Sfx sfx)
    {
        sfxSource.PlayOneShot(sfxClips[(int)sfx]);
    }
    public void SetMasterVolume(float value)
    {
        Debug.Log("Slider °ª: " + value);
        value = Mathf.Clamp(value, 0.0001f, 1f);
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }

    public void SetBgmVolume(float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);
        audioMixer.SetFloat("BGMVolume", Mathf.Log10(value) * 20);
    }

    public void SetSfxVolume(float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    }

    public void SetHighPass(bool isOn)
    {
        if (isOn)
        {
            audioMixer.SetFloat("HighPassCutoff", 5000f);
        }
        else
        {
            audioMixer.SetFloat("HighPassCutoff", 10f);
        }
    }


}
public enum Bgm
{
    Main
}

public enum Sfx
{
    Shot,
    Hit,
    Dead,
    Clear
}