using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoStream : MonoBehaviour
{
    public GameObject o;
    public RawImage image;
    private AudioSource audioSource;
    private VideoSource videoSource;
    private VideoPlayer videoPlayer;
    public VideoClip video;
    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true;
                
    }

    public IEnumerator playVideo()
    {
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        gameObject.GetComponent<VideoPlayer>().renderMode= VideoRenderMode.CameraNearPlane;

        audioSource = gameObject.AddComponent<AudioSource>();

        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;
        audioSource.Pause();

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = "https://firebasestorage.googleapis.com/v0/b/lideresocialespg.appspot.com/o/NRQA7837.MP4?alt=media&token=e4f298a1-6d69-4556-8577-13dc99a6d20f";
        //videoPlayer.source = VideoSource.VideoClip;

        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        //videoPlayer.clip= video;
        videoPlayer.Prepare();

        WaitForSeconds waitTime = new WaitForSeconds(1);

        while (!videoPlayer.isPrepared)
        {
            print("Preparando el video");
            yield return waitTime;
            break;
        }

        print("Ya se preparo");

        image.texture = videoPlayer.texture;

        videoPlayer.Play();

        audioSource.Play();

        print("Deberia estarse reproduciendo");

        while(videoPlayer.isPlaying)
        {
            yield return null;
        }
        print("Ya dejó de reproducirse");
        
        o.SetActive(false);
    }
}
