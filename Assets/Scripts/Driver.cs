using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class Driver : MonoBehaviour
{
    [SerializeField] private float _steerSpeed = 200;
    [SerializeField] private float _moveSpeed = 10;
    [SerializeField] private float _currentSpeed = 10;
    [SerializeField] private float _boostSpeed = 15;
    [SerializeField] private AudioClip _speedPickUpSound;
    [SerializeField] private AudioClip[] _crashSounds;
    [SerializeField] private TextMeshProUGUI _crashNumber;
    [SerializeField] private ParticleSystem _coffeeParticles;
    private AudioSource _audioSource;    
    public int _crashCount = 0;

    private EndOfLevel _endOfLevel;
    
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        UpdateCrashNumber();
        _endOfLevel = FindFirstObjectByType<EndOfLevel>();
    }
    
    void UpdateCrashNumber()
    {
        _crashNumber.text = _crashCount.ToString();
    }
    
    // Update is called once per frame
    void Update()
    {
        float steer = 0f;
        float move = 0f;

        //dont move if the level is over
        if (!_endOfLevel._endScreen.activeInHierarchy)
        {
            if (Keyboard.current.wKey.isPressed)
            {
                move = 1;
            }
            else if (Keyboard.current.sKey.isPressed)
            {
                move = -.6f;
            }
            if (Keyboard.current.aKey.isPressed)
            {
                steer = 1;
            }
            else if (Keyboard.current.dKey.isPressed)
            {
                steer = -1;
            }
        }

        transform.Rotate(0,0, _steerSpeed * steer * Time.deltaTime);
        transform.Translate(0,_currentSpeed * move * Time.deltaTime,0);
    }
    
    //collider = pass through object, activate trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boost"))
        {
            _currentSpeed = _boostSpeed;
            _coffeeParticles.Play();
            Destroy(other.gameObject);
            _audioSource.PlayOneShot(_speedPickUpSound);
        }
    }
    
    //collision = physics system, when you cant pass through an object
    void OnCollisionEnter2D(Collision2D col)
    {
        _currentSpeed = _moveSpeed;
        _coffeeParticles.Stop();
        AudioClip randomCrash = _crashSounds[Random.Range(0, _crashSounds.Length)];
        _audioSource.PlayOneShot(randomCrash);
        _crashCount++;
        UpdateCrashNumber();
    }
}
