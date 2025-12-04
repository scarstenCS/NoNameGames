using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;
public class SceneSwitcher : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Assign in Inspector

    public string nextSceneName;
    public string streamingFileName = "EerieVideo.mp4";

    public DialogueManager dm;

    public DialogueSequence dialogue;

    void Awake()
    {
        if (videoPlayer != null)
        {
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = Path.Combine(Application.streamingAssetsPath, streamingFileName);
            videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
            videoPlayer.loopPointReached += OnVideoFinished;
            StartCoroutine(AutoPlayMuted());  
        }
        else
        {
            Debug.LogError("VideoPlayer is not assigned in the Inspector.");
        }
    }
    System.Collections.IEnumerator AutoPlayMuted()
    {
        videoPlayer.Prepare();
        while (videoPlayer != null && !videoPlayer.isPrepared)
            yield return null;

        if (videoPlayer != null)
        {
            videoPlayer.SetDirectAudioMute(0, true); 
            videoPlayer.Play();
        }   
    }

    void OnVideoFinished(VideoPlayer vp)
    { 
        dm.StartSequence(dialogue);
        

    }

    private IEnumerable playCutsceneDialogue()
    {
        Debug.Log("start");
        dm.StartSequence(dialogue);

        yield return new WaitUntil(dm.isDialogueFinished);
        SceneManager.LoadScene(nextSceneName);

    }
}