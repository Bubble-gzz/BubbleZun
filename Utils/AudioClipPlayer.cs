using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleZun.Utils{
    public class AudioClipPlayer : MonoBehaviour, IObjectPoolable
    {
        // Start is called before the first frame update
        public ObjectPool pool{get; set;}
        public AudioSource audioSource;
        public void PlayOneShot(AudioClip clip, float volume = 1f){
            StartCoroutine(PlayOneShotCoroutine(clip, volume));
        }
        public void Play(AudioClip clip, float volume = 1f){
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();
        }
        IEnumerator PlayOneShotCoroutine(AudioClip clip, float volume){
            audioSource.PlayOneShot(clip, volume);
            yield return new WaitForSeconds(clip.length);
            if (pool != null) pool.Recycle(gameObject);
            else Destroy(gameObject);
        }
    }
}