using UnityEngine;
using UnityEngine.Events;


public class InteractableObject : MonoBehaviour
{   

    [SerializeField] private float interactDistance;   

    private CircleCollider2D _interactCollider;

    [SerializeField] private GameObject interactUI;

    private bool _isInRange;
        
    private void Start()
    {
        _isInRange = false;
        interactUI.SetActive(false);
        _interactCollider = gameObject.AddComponent<CircleCollider2D>();
        _interactCollider.radius = interactDistance;
        _interactCollider.isTrigger = true;
    }

    private void Update()
    {
        if (_isInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isInRange = true;
            interactUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isInRange = false;
            interactUI.SetActive(false);
        }
    }

    private void Interact()
    {
        Debug.Log("Interacting with object");
    }


    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}
