using UnityEngine;
using TMPro;

public class Delivery : MonoBehaviour
{
    [SerializeField] private float _delay = .1f;
    [SerializeField] private TextMeshProUGUI _deliveryNumber;
    [SerializeField] private int _deliveryCount = 0;
    [SerializeField] private AudioClip _packagePickUpSound;
    [SerializeField] private AudioClip _packageDeliverSound;
    [SerializeField] private AudioClip _levelCompleteSound;
    [SerializeField] private ParticleSystem _packageParticles;
    private AudioSource _audioSource;
    private bool _hasPackage = false;
    
    //get access to other scripts
    private Dialogue _dialogue;
    private EndOfLevel _endOfLevel;
    
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        UpdateDeliveryNumber();
        _dialogue = FindFirstObjectByType<Dialogue>();
        _endOfLevel = FindFirstObjectByType<EndOfLevel>();
    }
    
    void UpdateDeliveryNumber()
    {
        _deliveryNumber.text = _deliveryCount.ToString();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        //if you trigger a package and don't have one yet, pick it up
        if (other.CompareTag("Package") && !_hasPackage)
        {
            _hasPackage = true;
            Destroy(other.gameObject, _delay);
            _packageParticles.Play();
            _audioSource.PlayOneShot(_packagePickUpSound);
        }
        
        // if you trigger a customer and have a package
        if (other.CompareTag("Customer") && _hasPackage)
        {
            //check if they have a bubble child
            Transform bubble = other.transform.Find("Exclamation Bubble");
            if (bubble != null)
            {
                //deliver the package, remove bubble, play sound
                _hasPackage = false;
                _packageParticles.Stop();
                Destroy(bubble.gameObject);
                _deliveryCount--;
                UpdateDeliveryNumber();
                _audioSource.PlayOneShot(_packageDeliverSound);
                
                if (other.gameObject.name == "Tutorial Guy")
                {
                    _dialogue.TutorialGuyGotHisIceCreamYay();
                }
                
                //if all deliveries are completed
                if (_deliveryCount == 0)
                {
                    _audioSource.PlayOneShot(_levelCompleteSound);
                    //show end screen
                    _endOfLevel.EndScreen();
                }
            }
        }
    }
}
