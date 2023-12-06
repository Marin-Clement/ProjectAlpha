using UnityEngine;

public class PlayerClone : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private float Duration = 0.5f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // change opacity
        Color color = spriteRenderer.color;
        color.a = 0.5f;
        spriteRenderer.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        Color color = spriteRenderer.color;
        color.a -= 1f * Time.deltaTime;
        spriteRenderer.color = color;
        transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f) * Time.deltaTime;
        if (Duration > 0)
        {
            Duration -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}