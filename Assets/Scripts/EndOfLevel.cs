using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class EndOfLevel : MonoBehaviour
{
    [SerializeField] public GameObject _endScreen;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _crashText;
    private float _totalTime;
    private bool _timeStopped = false;
    
    //get access to other script
    private Driver _driver;
    private SceneLoader _sceneLoader;
    
    void Start()
    {
        _endScreen.SetActive(false);
        _totalTime = 0;
        _timeStopped = false;
        
        _driver = FindFirstObjectByType<Driver>();
        _sceneLoader = FindFirstObjectByType<SceneLoader>();
    }
    
    void Update()
    {
        if (!_timeStopped)
        {
            _totalTime += Time.deltaTime;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && _endScreen.activeInHierarchy)
        {
            _sceneLoader.LoadNextScene();
        }
    }
    
    // I stole this from the internet, couldn't figure out how to do it myself
    string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(_totalTime / 60f);
        int seconds = Mathf.FloorToInt(_totalTime % 60f);
        int milliseconds = Mathf.FloorToInt((_totalTime * 1000f) % 1000f);
        return $"{minutes:00}:{seconds:00}.{milliseconds:000}";
    }
    
    public void EndScreen()
    {
        _timeStopped = true;
        _timeText.text = GetFormattedTime();
        _crashText.text = _driver._crashCount.ToString();
        _endScreen.SetActive(true);
    }
}
