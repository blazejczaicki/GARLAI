using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TopShooter
{
    public class Menu : MonoBehaviour
    {
        public GameObject mainMenuHolder;
        public GameObject optionsMenuHolder;

        public Slider[] volumeSliders;
        public Toggle[] resolutionToggles;
        public Toggle fullscreenToggle;
        public int[] screenWidths;
        int activeScreenResIndex;
        
        [SerializeField] private TMP_InputField IterationLimit;
        [SerializeField] private TMP_InputField StartIteration;
        [SerializeField] private TMP_InputField GenerationLimit;
        [SerializeField] private TMP_InputField enemySpeed;
        [SerializeField] private TMP_InputField attackSpeed;
        [SerializeField] private TMP_InputField playerSpeed;
        [SerializeField] private TMP_InputField time;
        
        [SerializeField] private Toggle RandomGenerationMode;

        [SerializeField] private TextMeshProUGUI info;

        void Start()
        {
            activeScreenResIndex = PlayerPrefs.GetInt("screen res index");
            bool isFullscreen = (PlayerPrefs.GetInt("fullscreen") == 1) ? true : false;

            for (int i = 0; i < resolutionToggles.Length; i++)
            {
                resolutionToggles[i].isOn = i == activeScreenResIndex;
            }

            fullscreenToggle.isOn = isFullscreen;
        }

		private void Update()
		{
            LoadAgain();
		}

		public void LoadAgain()
		{
            if (SceneComunicator.instance.isChanged && SceneComunicator.instance.iterations> SceneComunicator.instance.currentIT)
            {
                if (SceneComunicator.instance.sceneTaker == SceneTaker.DT)
                {
                    SceneComunicator.instance.sceneTaker = SceneTaker.DT;
                    SceneManager.LoadScene(1);
                }
                else if (SceneComunicator.instance.sceneTaker == SceneTaker.BN)
                {
                    BayesNet();
                }
                else if (SceneComunicator.instance.sceneTaker == SceneTaker.ML)
                {
                    MLagents();
                }
            }
		}

        public void Stop()
        {
            SceneComunicator.instance.isChanged = false;
        }

        public void Play()
        {
			try
			{
                SceneComunicator.instance.iterations = int.Parse(IterationLimit.text);
                SceneComunicator.instance.currentIT = int.Parse(StartIteration.text);
                SceneComunicator.instance.generationLimits = int.Parse(GenerationLimit.text);
                SceneComunicator.instance.playerSpeed = float.Parse(playerSpeed.text);
                SceneComunicator.instance.enemySpeed = float.Parse(enemySpeed.text);
                SceneComunicator.instance.attackSpeed = float.Parse(attackSpeed.text);
                SceneComunicator.instance.time = float.Parse(time.text);
                SceneComunicator.instance.randomMode = RandomGenerationMode.isOn;

                SceneComunicator.instance.sceneTaker = SceneTaker.DT;
                SceneManager.LoadScene(1);
			}
            catch(FormatException e)
			{
                info.text = "Not number values";
			}
        } 
        
        public void DecisionTree()
        {
            
        }

        public void BayesNet()
        {
            //SceneManager.LoadScene(2);
        }

        public void MLagents()
        {
            //SceneManager.LoadScene(3);
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void OptionsMenu()
        {
            mainMenuHolder.SetActive(false);
            optionsMenuHolder.SetActive(true);
        }

        public void MainMenu()
        {
            mainMenuHolder.SetActive(true);
            optionsMenuHolder.SetActive(false);
        }

        public void SetScreenResolution(int i)
        {
            if (resolutionToggles[i].isOn)
            {
                activeScreenResIndex = i;
                float aspectRatio = 16 / 9f;
                Screen.SetResolution(screenWidths[i], (int)(screenWidths[i] / aspectRatio), false);
                PlayerPrefs.SetInt("screen res index", activeScreenResIndex);
                PlayerPrefs.Save();
            }
        }

        public void SetFullscreen(bool isFullscreen)
        {
            for (int i = 0; i < resolutionToggles.Length; i++)
            {
                resolutionToggles[i].interactable = !isFullscreen;
            }

            if (isFullscreen)
            {
                Resolution[] allResolutions = Screen.resolutions;
                Resolution maxResolution = allResolutions[allResolutions.Length - 1];
                Screen.SetResolution(maxResolution.width, maxResolution.height, true);
            }
            else
            {
                SetScreenResolution(activeScreenResIndex);
            }

            PlayerPrefs.SetInt("fullscreen", ((isFullscreen) ? 1 : 0));
            PlayerPrefs.Save();
        }

        public void SetMasterVolume(float value)
        {
            AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Master);
        }

        public void SetMusicVolume(float value)
        {
            AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Music);
        }

        public void SetSfxVolume(float value)
        {
            AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Sfx);
        }
    }
}
