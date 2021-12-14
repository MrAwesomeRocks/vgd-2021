using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Assorted utilities for the game.
/// </summary>
namespace GameUtilities {
    /// <summary>
    /// Audio utilities
    /// </summary>
    public static class Audio {
        /// <summary>
        /// A coroutine to play a <paramref name="sound"/> from an audio <paramref name="source"/> a specified number of times.
        /// </summary>
        /// <param name="sound">The AudioClip to play.</param>
        /// <param name="source">The AudioSource to play from.</param>
        /// <param name="numTimes">How many times to play the sound.</param>
        /// <returns>A coroutine that can be started.</returns>
        public static IEnumerator PlaySoundNumTimes(AudioClip sound, AudioSource source, int numTimes) {
            for (int i = 0; i < numTimes; i++) {
                source.PlayOneShot(sound);

                while (source.isPlaying) {
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }
}
