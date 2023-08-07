using UnityEngine;

public class InteractableObject : MonoBehaviour
{   
    // Simple interactable object script for 2d games

    [SerializeField] private float _interactDistance;   
    [SerializeField] private LayerMask _interactLayerMask;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, _interactDistance, Vector2.zero, 0f, _interactLayerMask);
        if (hit.collider != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Interacted with " + hit.collider.gameObject.name);
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _interactDistance);
    }
}
