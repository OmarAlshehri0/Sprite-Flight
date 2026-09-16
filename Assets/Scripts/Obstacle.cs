using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float minSize = 0.5f;
    public float maxSize = 2.0f;
    Rigidbody2D rb;
    public float minSpeed = 50f;
    public float maxSpeed = 150f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      float randomSize = Random.Range(minSize, maxSize);
      transform.localScale = new Vector3(randomSize, randomSize, 1);

      rb = GetComponent<Rigidbody2D>();

      float randomSpeed = Random.Range(minSpeed, maxSpeed);
      Vector2 randomDirection = Random.insideUnitCircle;
      rb.AddForce(randomDirection * randomSpeed);


    }

    // Update is called once per frame
    void Update()
    {

    }
}
