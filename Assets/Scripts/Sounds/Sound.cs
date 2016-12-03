﻿using System;
using UnityEngine;

public static class Sound {
    public static AudioSource BuildAudioSource(GameObject gameObject, 
        AudioClip audioClip, bool loop = false) {
        AudioSource audioSource
            = gameObject.AddComponent<AudioSource>();
        audioSource.loop = loop;
        audioSource.clip = audioClip;

        return audioSource;
    }

    public static AudioSource BuildFxSource(GameObject gameObject, 
        AudioClip audioClip, string name = null, bool loop = false, 
        float spatialBlend = 1f) {
        AudioSource audioSource 
            = Sound.BuildAudioSource(gameObject, audioClip, loop);
        audioSource.spatialBlend = spatialBlend;
        audioSource.name = name ?? audioClip.name;

        return audioSource;
    }

    public static AudioSource BuildMusicSource(GameObject gameObject,
        AudioClip audioClip, string name = null, bool loop = true) {
        AudioSource audioSource 
            = Sound.BuildAudioSource(gameObject, audioClip, loop);
        audioSource.name = name ?? gameObject.scene.name;

        return audioSource;
    }
}