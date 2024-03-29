using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    [SerializeField] private float interactDistance;

    [SerializeField] private GameObject interactUI;

    private bool _isInRange;

    protected void Start()
    {
        _isInRange = false;
        interactUI.SetActive(false);
        var interactCollider = gameObject.AddComponent<CircleCollider2D>();
        interactCollider.radius = interactDistance;
        interactCollider.isTrigger = true;
    }

    private void Update()
    {
        if (_isInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }

        SetDetail();
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }

    protected abstract void Interact();

    protected abstract void SetDetail();
}