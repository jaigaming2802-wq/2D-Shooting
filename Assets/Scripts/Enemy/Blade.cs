using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float moveSpeed = 3f;

    private Transform target;

    private void Start()
    {
        target = pointA;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector2.Distance(
            transform.position,
            target.position
        ) < 0.01f)
        {
            target = target == pointA
                ? pointB
                : pointA;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth health =
                collision.GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.TakeDamage(1);
            }
        }
    }
}