using UnityEngine;

public class Gap : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth health =
                collision.gameObject.GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.DieDirectly();
            }
        }
    }
}