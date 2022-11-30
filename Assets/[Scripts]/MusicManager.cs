//Singleton Music Manager class to handle playing out crossfading music, which persists 

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public enum TrackID
    {
        DAYWORLD,
        NIGHTWORLD
    }

    [Tooltip("Track Order SHould Line up with trackID")]
    [SerializeField]
    AudioClip[] tracks;

    //Hidden Constructor
    private MusicManager() { }

    private static MusicManager instance = null;
    public static MusicManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<MusicManager>();
                SceneManager.sceneLoaded += instance.OnSceneLoaded;
            }
            return instance;

        }

        private set { instance = value; }
    }

    [Tooltip("One Track For Crossfading")]
    [SerializeField]
    AudioSource musicSource1;

    [Tooltip("One Track For Crossfading, the order is abitrary")]
    [SerializeField]
    AudioSource musicSource2;


    void Start()
    {
        //On start, in inastance is null, this will set our original. if its aleady been set, this will return that one
        MusicManager original = Instance;

        //if i want to have a musicmanager living int he scene, i need to make sure only one stays at a time...
        MusicManager[] managers = FindObjectsOfType<MusicManager>();
        foreach (MusicManager manager in managers)
        {
            if (manager != original)
            {
                Destroy(manager.gameObject);
                
            }
        }

        if (this == original)
        {
            DontDestroyOnLoad(gameObject);
        }


    }

    void OnSceneLoaded(Scene newScene, LoadSceneMode loadMode)
    {
        //if (newScene.name == "MainMenu")
        //{
        //    CrossFadeTo(TrackID.MAINMENU);
        //}
        if (newScene.name == "DayWorld")
        {
            CrossFadeTo(TrackID.DAYWORLD);
        }
        if (newScene.name == "NightWorld")
        {
            CrossFadeTo(TrackID.NIGHTWORLD);
        }
        //if (newScene.name == "Town")
        //{
        //    CrossFadeTo(TrackID.TOWN);
        //}
    }
    //Add a mothod for :
    //1. playing a track immediatly
    //2. crossfading between current tracks and a new goal track
    //3. Fading out a track
    //4.Fading in a track
    //5. Dip-To-Black transition where current track fades to 0, then Goal track fades in from 0

    /// <summary>
    /// Stop everything and play on source 1
    /// </summary>
    /// <param name="whichTrackToPlay"></param>

    public void PlayTrackSolo(TrackID whichTrackToPlay)
    {
        musicSource1.Stop();
        musicSource2.Stop();
        musicSource1.clip = tracks[(int)whichTrackToPlay];
        musicSource1.Play();
    }


    ///<summary>
    ///Assuming one track is alreading playing, we crossfade ro end with anoher track playing solo on a figgerent source
    ///</summary>
    ///<param name="goalTrack"></param>

    public void CrossFadeTo(TrackID goalTrack, float transitionDurationSec = 3.0f)
    {
        //old track will fade out new track will fade in
        AudioSource oldTrack = musicSource1;
        AudioSource newTrack = musicSource2;

        if(musicSource1.isPlaying)
        {
            oldTrack = musicSource1;
            newTrack = musicSource2;
        } else if (musicSource2.isPlaying)
        {
            oldTrack = musicSource2;
            newTrack = musicSource1;
        }

        newTrack.clip = tracks[(int)goalTrack];
        newTrack.Play();

        StartCoroutine(CrossFadeCoroitine(oldTrack, newTrack,transitionDurationSec));
    }

    private IEnumerator CrossFadeCoroitine(AudioSource oldTrack, AudioSource newTrack, float transitionDurationSec)
    {
        float time = 0.0f;
        while (time < transitionDurationSec)
        {
            float tValue = time / transitionDurationSec;

            //volume from 0 to 1 over duration
            newTrack.volume = tValue;
            oldTrack.volume = 1.0f - tValue;

            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();

        }

        oldTrack.Stop();
        oldTrack.volume = 0.3f;

    }
}
