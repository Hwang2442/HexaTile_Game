using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using TMPro;

namespace HexaGridGame
{
    public class Demo : MonoBehaviour
    {
        [SerializeField] private TileGameManager gameManager;

        [Header("UI")]
        [SerializeField] private GameObject StartPanel;
        [SerializeField] private GameObject EndPanel;

        [Header("Text")]
        [SerializeField] private TextMeshProUGUI infoText;
        [SerializeField] private TextMeshProUGUI stepCountText;

        private void Awake()
        {
            gameManager.gameObject.SetActive(false);

            StartPanel.SetActive(true);
            EndPanel.SetActive(false);
        }

        public void OnClickPlay(int level)
        {
            StartPanel.SetActive(false);

            gameManager.SetLevel(level);
            gameManager.gameObject.SetActive(true);
        }

        public void OnGameClear()
        {
            EndPanel.SetActive(true);
            infoText.text = "CLEAR";
            stepCountText.text = string.Format("STEP COUNT: {0}", gameManager.StepCount);
        }

        public void OnGameFailed()
        {
            EndPanel.SetActive(true);
            infoText.text = "FAIL";
            stepCountText.text = string.Format("STEP COUNT: {0}", gameManager.StepCount);
        }

        public void OnClickReload()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Tools/ScreenCapture")]
        private static void SaveScreenshot()
        {
            ScreenCapture.CaptureScreenshot("Screenshot.png");
        }
#endif

    }
}


