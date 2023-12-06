using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] private GameObject[] items;

    // Generate a random number of item from 1 to 3 And launch him in a random direction
    public void GenerateItem()
    {
        int random = Random.Range(1, 4);
        for (int i = 0; i < random; i++)
        {
            int randomItem = Random.Range(0, items.Length);
            GameObject item = Instantiate(items[randomItem], transform.position, Quaternion.identity);
            // Launch the item in a random angle
            float angle = Random.Range(0, 360);
            Vector2 direction = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
            item.GetComponent<Rigidbody2D>().AddForce(direction * 500);
        }
    }
}