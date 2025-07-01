using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BubbleZun.Utils{
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    GameObject audioClipPlayerPrefab;
    ObjectPool audioClipPlayerPool;
    [System.Serializable]
    public class AudioClipInfo{
        public string name;
        public AudioClip clip;
    }
    public List<AudioClipInfo> audioClips = new List<AudioClipInfo>();
    float volume = 1f;
    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
        audioClipPlayerPool = new ObjectPool(audioClipPlayerPrefab);
        DontDestroyOnLoad(gameObject);
    }
    public static void PlayOneShot(string name)
    {
        instance._playOneShot(name);
    }
    public void _playOneShot(string name, float volume = 1f)
    {
        AudioClipInfo info = audioClips.Find(clip => clip.name == name);
        if (info == null) return;
        AudioClipPlayer player = audioClipPlayerPool.GetObject(transform).GetComponent<AudioClipPlayer>();
        player.PlayOneShot(info.clip, volume);
    }
}
}