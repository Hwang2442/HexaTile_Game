using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace HexaGridGame
{
    public class Demo : MonoBehaviour
    {
        public TileGameManager gameManager;

        [Space]
        public GameObject panel;

        [Space]
        public Slider sliderX;
        public Slider sliderY;
        public Slider sliderObstacle;

        [Space]
        public Text textValueX;
        public Text textValueY;
        public Text textValueObstacle;

        private void Start()
        {
            
        }

        public void SliderValueSyncX(float x)
        {
            textValueX.text = x.ToString();

            sliderObstacle.maxValue = Mathf.CeilToInt(sliderX.value * sliderY.value * 0.5f) - 1;
            sliderObstacle.value = 0;
        }
        public void SliderValueSyncY(float y)
        {
            textValueY.text = y.ToString();

            sliderObstacle.maxValue = Mathf.CeilToInt(sliderX.value * sliderY.value * 0.5f) - 1;
            sliderObstacle.value = 0;
        }
        public void SliderValueSyncObstacle(float val)
        {
            textValueObstacle.text = val.ToString();
        }

        public void OnClickPlay(int level)
        {
            panel.SetActive(false);

            gameManager.SetLevel(level);
            gameManager.gameObject.SetActive(true);
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


