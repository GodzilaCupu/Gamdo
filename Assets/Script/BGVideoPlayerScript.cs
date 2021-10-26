using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class BGVideoPlayerScript : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private string videoName;

    private void Start()
    {
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, videoName);
        Debug.Log(videoPlayer.url);
        videoPlayer.playOnAwake = true;
        videoPlayer.isLooping = true;
        videoPlayer.Play();
    }
}
