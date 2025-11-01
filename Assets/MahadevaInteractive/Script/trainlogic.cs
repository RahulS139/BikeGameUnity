using UnityEngine;

public class TrainLogic : MonoBehaviour
{
    public float Speed = 5f;
    public float Lifetime = 5f;
    private bool startMoving = false;

    void Update()
    {
        if (startMoving)
        {
            transform.Translate(Vector2.left * Speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered by: " + other.name); // 👈 debug check

        if (other.CompareTag("Player"))
        {
            Debug.Log("Train started moving!");
            startMoving = true;
            Destroy(gameObject, Lifetime);
        }
    }
}
