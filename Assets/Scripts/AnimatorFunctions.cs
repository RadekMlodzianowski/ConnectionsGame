using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
   public AudioSource audioSource;
   public AudioClip[] audioClips;
   public float audioClipsVolume = 1f;

   public void PlayRandomSounds()
   {
      audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)], audioClipsVolume);
   }

}
