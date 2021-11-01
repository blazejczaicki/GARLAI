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
        [SerializeField] private TMP_InputField attackDistanceThreshold;
        [SerializeField] private TMP_InputField playerSpeed;
        [SerializeField] private TMP_InputField time;        

        [SerializeField] private TMP_InputField IterationLimitBN;
        [SerializeField] private TMP_InputField StartIterationBN;
        [SerializeField] private TMP_InputField GenerationLimitBN;
        [SerializeField] private TMP_InputField enemySpeedBN;
        [SerializeField] private TMP_InputField attackDistanceThresholdBN;
        [SerializeField] private TMP_InputField playerSpeedBN;
        [SerializeField] private TMP_InputField timeBN;

        [SerializeField] private TMP_InputField IterationLimitMLA;
        [SerializeField] private TMP_InputField StartIterationMLA;
        [SerializeField] private TMP_InputField GenerationLimitMLA;
        [SerializeField] private TMP_InputField enemySpeedMLA;
        [SerializeField] private TMP_InputField attackDistanceThresholdMLA;
        [SerializeField] private TMP_InputField playerSpeedMLA;
        [SerializeField] private TMP_InputField timeMLA;

        [SerializeField] private GameObject panelDT;
        [SerializeField] private GameObject panelBN;
        [SerializeField] private GameObject panelMLA;
        
        [SerializeField] private Toggle RandomGenerationMode;
        [SerializeField] private Toggle RandomGenerationModeBN;
        [SerializeField] private Toggle RandomGenerationModeMLA;

        [SerializeField] private TMP_InputField manualIndexBN;
        [SerializeField] private Toggle manualBN;
        [SerializeField] private TMP_InputField manualIndexDT;
        [SerializeField] private Toggle manualDT;

        [SerializeField] private TextMeshProUGUI info;

        [Header("DT manual")]
        [SerializeField] private TMP_InputField enToClose;
        [SerializeField] private TMP_InputField safArDist;
        [SerializeField] private TMP_InputField GoBackDist;
        [SerializeField] private TMP_InputField WallBackDist;
        [SerializeField] private TMP_InputField SpurDist;
        [SerializeField] private TMP_InputField MinSourDist;
        [SerializeField] private TMP_InputField SourWallDist;
        [SerializeField] private TMP_InputField StayTimeUpdt;
        [SerializeField] private TMP_InputField SafetyTimeUpdt;
        [SerializeField] private TMP_InputField SourTimeUpd;
        [SerializeField] private TMP_InputField RetTimeUpdt;
        [SerializeField] private TMP_InputField WallRetreatTimUpdt;

        [SerializeField] private Transform panelManualDT;



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
                    SceneComunicator.instance.sceneTaker = SceneTaker.BN;
                    SceneManager.LoadScene(2);
                }
                else if (SceneComunicator.instance.sceneTaker == SceneTaker.ML)
                {

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
                if (panelDT.activeSelf)
                {
                    SceneComunicator.instance.iterations = int.Parse(IterationLimit.text);
                    SceneComunicator.instance.currentIT = int.Parse(StartIteration.text);
                    SceneComunicator.instance.generationLimits = int.Parse(GenerationLimit.text);
                    SceneComunicator.instance.playerSpeed = float.Parse(playerSpeed.text);
                    SceneComunicator.instance.enemySpeed = float.Parse(enemySpeed.text);
                    SceneComunicator.instance.attackDistanceThreshold = float.Parse(attackDistanceThreshold.text);
                    SceneComunicator.instance.time = float.Parse(time.text);
                    SceneComunicator.instance.randomMode = RandomGenerationMode.isOn;
                    SceneComunicator.instance.sceneTaker = SceneTaker.DT;
                    

                    

                    var isManual = manualDT.isOn;
                    SceneComunicator.instance.manual = isManual;
                    if (isManual)
                    {
                        SceneComunicator.instance.manualIndex = int.Parse(manualIndexDT.text);
                        SceneComunicator.instance.datas.Add(float.Parse(enToClose.text));
                        SceneComunicator.instance.datas.Add(float.Parse(safArDist.text));
                        SceneComunicator.instance.datas.Add(float.Parse(GoBackDist.text));
                        SceneComunicator.instance.datas.Add(float.Parse(WallBackDist.text));
                        SceneComunicator.instance.datas.Add(float.Parse(SpurDist.text));
                        SceneComunicator.instance.datas.Add(float.Parse(MinSourDist.text));
                        SceneComunicator.instance.datas.Add(float.Parse(SourWallDist.text));
                        SceneComunicator.instance.datas.Add(float.Parse(StayTimeUpdt.text));
                        SceneComunicator.instance.datas.Add(float.Parse(SafetyTimeUpdt.text));
                        SceneComunicator.instance.datas.Add(float.Parse(SourTimeUpd.text));
                        SceneComunicator.instance.datas.Add(float.Parse(RetTimeUpdt.text));
                        SceneComunicator.instance.datas.Add(float.Parse(WallRetreatTimUpdt.text));

                        SceneManager.LoadScene(5);
                    }
                    else
                    {
                        SceneManager.LoadScene(1);
                    }
                }
                else if (panelBN.activeSelf)
                {
                    SceneComunicator.instance.iterations = int.Parse(IterationLimitBN.text);
                    SceneComunicator.instance.currentIT = int.Parse(StartIterationBN.text);
                    SceneComunicator.instance.generationLimits = int.Parse(GenerationLimitBN.text);
                    SceneComunicator.instance.playerSpeed = float.Parse(playerSpeedBN.text);
                    SceneComunicator.instance.enemySpeed = float.Parse(enemySpeedBN.text);
                    SceneComunicator.instance.attackDistanceThreshold = float.Parse(attackDistanceThresholdBN.text);
                    SceneComunicator.instance.time = float.Parse(timeBN.text);
                    SceneComunicator.instance.randomMode = RandomGenerationModeBN.isOn;

                    SceneComunicator.instance.sceneTaker = SceneTaker.BN;

                    SceneComunicator.instance.manualIndex = int.Parse(manualIndexBN.text);

                    var isManual = manualBN.isOn;
                    SceneComunicator.instance.manual = isManual;
                    if (isManual)
                    {
                        SceneManager.LoadScene(3);
                    }
                    else
                    {
                        SceneManager.LoadScene(2);
                    }
                }
                else if (panelMLA.activeSelf)
                {
                    SceneComunicator.instance.iterations = int.Parse(IterationLimitMLA.text);
                    SceneComunicator.instance.currentIT = int.Parse(StartIterationMLA.text);
                    SceneComunicator.instance.generationLimits = int.Parse(GenerationLimitMLA.text);
                    SceneComunicator.instance.playerSpeed = float.Parse(playerSpeedMLA.text);
                    SceneComunicator.instance.enemySpeed = float.Parse(enemySpeedMLA.text);
                    SceneComunicator.instance.attackDistanceThreshold = float.Parse(attackDistanceThresholdMLA.text);
                    SceneComunicator.instance.time = float.Parse(timeMLA.text);
                    SceneComunicator.instance.randomMode = RandomGenerationModeMLA.isOn;
                    SceneComunicator.instance.sceneTaker = SceneTaker.ML;
                    SceneManager.LoadScene(4);
                }
            }
            catch(FormatException e)
			{
                info.text = "Not number values";
			}
        }



        public void ManualActiveDT()
		{
            panelManualDT.gameObject.SetActive(!panelManualDT.gameObject.activeSelf);
		}


        public void DecisionTree()
        {
            panelBN.SetActive(false);
            panelDT.SetActive(true);
            panelMLA.SetActive(false);
        }

        public void BayesNet()
        {
            panelBN.SetActive(true);
            panelDT.SetActive(false);
            panelMLA.SetActive(false);
        }

        public void MLagents()
        {
            panelBN.SetActive(false);
            panelDT.SetActive(false);
            panelMLA.SetActive(true);
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
