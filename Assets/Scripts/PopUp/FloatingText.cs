using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private float speed = 2f; // Floating speed
    [SerializeField] private float duration = 1f; // Duration of the popup
    [SerializeField] private AnimationCurve sizeCurve; // Animation curve for size variation over time

    private TextMeshPro _textMeshPro;
    private float _timer;
    private float _size;
    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshPro>();
        _textMeshPro.sortingOrder = 999;
    }

    private void Start()
    {
        // Generate random size within the specified range
        _size = Random.Range(1f, 1.5f);
        Destroy(gameObject, duration);
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        // Floating animation using sine function and animation curve
        Vector3 positionOffset = new Vector3(0f, 1f, 0f) * (speed * Time.deltaTime);
        transform.position += positionOffset;

        // Scale the text based on the random size and time-based size variation
        float sizeMultiplier = sizeCurve.Evaluate(_timer / duration);
        transform.localScale = new Vector3(_size * sizeMultiplier, _size * sizeMultiplier, _size * sizeMultiplier);
    }

    public void SetText(string text, Color color)
    {
        _textMeshPro.text = text;
        _textMeshPro.color = color;
    }
}
