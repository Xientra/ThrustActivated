using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoBehaviour {

    private AudioSource audioSource;
    public bool playing = false;

    public Song[] songs;
    private Song[] songsRandomized;
    private int songIndex = 0;

    private bool endingMusic = false;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        songsRandomized = songs;
        ShuffleSongs();
    }

    void Update() {
        if (GameController.activeInstance.musicIsMuted == false) {
            if (GameController.activeInstance.gameIsRunning == true) {
                playing = true;
            }
            else playing = false;

            if (endingMusic == false) {
                if (GameController.activeInstance.activePlayer != null && GameController.activeInstance.activePlayer.isDead == true) {
                    StartCoroutine(PitchToZero());
                    endingMusic = true;
                }
                else {
                    audioSource.pitch = 1f;
                }
            }

            if (playing == true) {
                ContinuePlaying();
            }
            else {
                audioSource.Stop();
            }
        }
        else {
            playing = false;
            audioSource.Stop();
        }
    }

    private void ContinuePlaying() {
        if (audioSource.isPlaying == false) {
            if (songIndex >= songsRandomized.Length) {
                ShuffleSongs();
                songIndex = 0;
            }
            Song nextSong = songsRandomized[songIndex];
            audioSource.clip = nextSong.clip;
            audioSource.Play();
            songIndex++;
            InGameUI.activeInstance.currentlyPlayingSongText.text = "Currently playing: " + "\n" + nextSong.artist + " - " + nextSong.name;
        }
    }

    private const float PITCH_CHANGE_STEP = 0.01f;

    private IEnumerator PitchToZero() {
        while (audioSource.pitch > 0f) {
            audioSource.pitch -= PITCH_CHANGE_STEP;
            yield return new WaitForSeconds(PITCH_CHANGE_STEP * GameController.TIME_UNTILL_SCENE_RELOAD);
        }
    }

    public void ShuffleSongs() {
        System.Random rng = new System.Random();

        int n = songsRandomized.Length;
        while (n > 1) {
            int k = rng.Next(n--);
            Song temp = songsRandomized[n];
            songsRandomized[n] = songsRandomized[k];
            songsRandomized[k] = temp;
        }
    }

    public void ToggleMusic(bool value) {
        playing = value;
    }

    private void OnDestroy() {
        audioSource.Stop();
        playing = false;
    }
}

[Serializable]
public struct Song {
    public AudioClip clip;
    public string name;
    public string artist;
}