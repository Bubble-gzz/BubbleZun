using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BubbleZun.Utils{
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public GameObject audioClipPlayerPrefab;
    ObjectPool audioClipPlayerPool;
    [System.Serializable]
    public class AudioClipInfo{
        public string name;
        public AudioClip clip;
    }
    public List<AudioClipInfo> audioClips = new List<AudioClipInfo>();
    public Dictionary<string, AudioClipPlayer> audioClipPlayers = new Dictionary<string, AudioClipPlayer>();
    float volume = 1f;
    void Awake()
    {
        if (instance != null) {
            Destroy(gameObject);
            Debug.Log("AudioManager already exists, destroying new one");
            return;
        }
        instance = this;
        audioClipPlayerPool = new ObjectPool(audioClipPlayerPrefab);
        DontDestroyOnLoad(gameObject);
    }
    public static void PlayOneShot(string name)
    {
        instance._playOneShot(name);
    }
    public static void Play(string name, string playerID = "default")
    {
        instance._play(name, playerID);
    }
    public void _play(string name, string playerID = "default")
    {
        if (!audioClipPlayers.ContainsKey(playerID))
        {
            audioClipPlayers[playerID] = audioClipPlayerPool.GetObject(transform).GetComponent<AudioClipPlayer>();
        }
        audioClipPlayers[playerID].Play(audioClips.Find(clip => clip.name == name).clip, volume);
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