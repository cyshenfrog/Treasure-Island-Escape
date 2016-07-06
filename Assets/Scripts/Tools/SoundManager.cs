using UnityEngine;
using System;
using System.Collections;

public class SoundManager : MonoBehaviour {
    private static SoundManager instance;
    private AudioSource bgm;
    private AudioSource sfx1, sfx2;
    private static IEnumerator tween = null;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
        bgm = this.gameObject.AddComponent<AudioSource>();
        sfx1 = this.gameObject.AddComponent<AudioSource>();
        sfx2 = this.gameObject.AddComponent<AudioSource>();
        bgm.loop = true;
        sfx1.loop = sfx2.loop = false;
    }
	public static void PlayBGM(AudioClip clip)
    {
        instance.bgm.clip = clip;
        instance.bgm.Play();
    }
    public static void PlaySfx1(AudioClip clip) {
        instance.sfx1.clip = clip;
        instance.sfx1.Play();
    }
    public static void PlaySfx2(AudioClip clip)
    {
        instance.sfx2.clip = clip;
        instance.sfx2.Play();
    }
    public static void TweenVolume(float from, float to, float time, Action onFinish)
    {
        if (tween != null)
            instance.StopCoroutine(tween);
        tween = VolumeChange(from, to, time, onFinish);
        instance.StartCoroutine(tween);
    }
    private static IEnumerator VolumeChange(float from, float to, float time, Action onFinish)
    {
        float timer = 0;
        while (timer < time)
        {
            timer += Time.deltaTime;
            instance.bgm.volume = Mathf.Lerp(from, to, timer / time);
            yield return null;
        }
        onFinish();
        tween = null;
    }
}
