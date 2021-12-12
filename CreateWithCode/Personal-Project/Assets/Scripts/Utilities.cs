using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtilities
{
    public static class Audio
    {
        public static IEnumerator PlaySoundNumTimes(AudioClip sound, AudioSource source, int numTimes)
        {
            for (int i = 0; i < numTimes; i++)
            {
                source.PlayOneShot(sound);

                while (source.isPlaying)
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }
}
