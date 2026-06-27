using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//this dialogue script is on the tutorial guy NPC
public class Dialogue : MonoBehaviour
{
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private string[] _dialogueLines;
    [SerializeField] private GameObject _iceCreamGate;
    [SerializeField] private GameObject _westGate;

    private bool _hasIceCream = false;
    private int _currentLine = 0;
    private Scene _currentLevel;
    
    void Start()
    {
        //dont show dialogue on start
        _dialoguePanel.SetActive(false);
        //find what level it is
        _currentLevel = SceneManager.GetActiveScene();
    }
    
    //if the player enters and gate is alive, dialoauge 0 and 1
    //if the player enters and gate is dead, dialouge 2 and 3
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) //if the player enters the collider
        {
            if (_currentLevel.buildIndex == 1)
            {
                if (_currentLine <= 1 || _hasIceCream)
                {
                    _dialoguePanel.SetActive(true); //turn on dialogue panel
                    ShowLine(); //show current line of dialogue
                }
            }

            if (_currentLevel.buildIndex == 2)
            {	
                _dialoguePanel.SetActive(true);
                ShowLine();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) //if the player exits the collider
        {
            _dialoguePanel.SetActive(false); //turn off dialogue panel
        }
    }
    
    void Update()
    {
        //check if space was pressed, only one frame per key press
        if (Keyboard.current.spaceKey.wasPressedThisFrame && _dialoguePanel.activeInHierarchy)
        {
            if (_currentLevel.buildIndex == 1)
            {
                TutorialDialogue();
            }

            if (_currentLevel.buildIndex == 2)
            {
                LevelOneDialogue();
            }
        }
    }

    void ShowLine()
    {
        _dialogueText.text = _dialogueLines[_currentLine]; //set the text to the current line
    }

    public void TutorialGuyGotHisIceCreamYay()
    {
        _hasIceCream = true;
    }

    void TutorialDialogue()
    {
        if (_currentLine < _dialogueLines.Length - 1)
        {
            _currentLine++;
            ShowLine();
            if (_currentLine == 2 && !_hasIceCream)
            {
                _iceCreamGate.SetActive(false);
                _dialoguePanel.SetActive(false);
            }

            if (_currentLine == 4)
            {
                _westGate.SetActive(false);
            }
        }
        else
        {
            _dialoguePanel.SetActive(false);
        }
    }

    void LevelOneDialogue()
    {
        if (_currentLine < _dialogueLines.Length - 1)
        {
            _currentLine++;
            ShowLine();
        }
        else
        {
            _dialoguePanel.SetActive(false);
        }
    }
}

