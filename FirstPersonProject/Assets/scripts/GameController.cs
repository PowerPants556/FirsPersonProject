using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    public static bool isPause;
    [SerializeField] private GameObject[] UIinterface;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private Slider cameraViewSlider;
    private void Start()
    {

    }
}
