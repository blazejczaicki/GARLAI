using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TopShooter
{
    public class MusicManager : MonoBehaviour
    {
        public AudioClip mainTheme;
        public AudioClip menuTheme;

        int sceneIndex=-1;

        void Start()
        {
            Debug.Log("xd");
            OnLevelWasLoaded(0);
        }


        void OnLevelWasLoaded(int index)
        {
            int newSceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (newSceneIndex != sceneIndex)
            {
                sceneIndex = newSceneIndex;
                Invoke("PlayMusic", .2f);
            }
        }

        void PlayMusic()
        {
            AudioClip clipToPlay = null;

            if (sceneIndex == 0)
            {
                clipToPlay = menuTheme;
            }
            else if (sceneIndex == 1)
            {
                clipToPlay = mainTheme;
            }
            Debug.Log(clipToPlay.name);
            if (clipToPlay != null)
            {
                AudioManager.instance.PlayMusic(clipToPlay, 2);
                Invoke("PlayMusic", clipToPlay.length);
            }

        }
    }
}
