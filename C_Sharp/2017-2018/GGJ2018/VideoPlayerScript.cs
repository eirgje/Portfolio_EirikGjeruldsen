using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoPlayerScript : MonoBehaviour {

	public GameObject moviePlayer;
	public int nextLevel;
	public int currentVideo;

    public float SingleVideo = 0f;

	public VideoClip[] mVideoClips = new VideoClip[10];
	public float[] mVideoDurations = new float[10];
		
	// Use this for initialization
	void Start () {
        if (GetComponent<XMLSerializationExample>() != null)
        {
            currentVideo = GetComponent<XMLSerializationExample>().GetTheCorrectInteger();
            moviePlayer.GetComponent<VideoPlayer>().clip = mVideoClips[currentVideo - 1];

            StartCoroutine(WaitForMovieToEnd(mVideoDurations[currentVideo - 1]));

        }
        else
        {
            moviePlayer.GetComponent<VideoPlayer>().clip = mVideoClips[0];

            StartCoroutine(WaitForMovieToEnd(mVideoDurations[0]));
        }
		moviePlayer.GetComponent<VideoPlayer> ().Play ();
		

	}

		IEnumerator WaitForMovieToEnd(float duration)
		{
		yield return new WaitForSeconds(duration);
        Camera.main.transform.GetChild(0).GetComponent<FadeScript>().FadeOut();
		}		
}
