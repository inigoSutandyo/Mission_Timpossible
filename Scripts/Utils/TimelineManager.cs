using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class TimelineManager : MonoBehaviour
{
    [SerializeField] private List<PlayableDirector> Timelines;
    [SerializeField] private Transform playerTransform;
    private PlayableDirector currTimeline;

    private int counter;
    private bool isPlayingTimeline;
    private PauseManager pause;

    private void Awake()
    {
        pause = GetComponent<PauseManager>();
    }

    void Start()
    {
        counter = 0;
        isPlayingTimeline = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayingTimeline && currTimeline.state != PlayState.Playing)
        {
            playerTransform.rotation = Quaternion.identity;
            isPlayingTimeline = false;
            pause.ContinueForTimeline();
        }
    }


    public void PlayNextTimeline()
    {
        if (isPlayingTimeline) return;
        if (counter >= Timelines.Count) return;
        
        currTimeline = Timelines[counter];
        Timelines[counter].Play();
        counter++;

        isPlayingTimeline = true;
        pause.PauseForTimeline();
        print("Timeline");
    }
}
