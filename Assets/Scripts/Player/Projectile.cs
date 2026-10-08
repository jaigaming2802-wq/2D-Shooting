using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    private Animator animator;
    private Collider2D projectileCollider;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private float speed = 8f;

    private float direction;
    private bool hasHit;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        projectileCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        FireBallMove();
    }
   
    public void SetDirection(int newDirection)
    {
        direction = newDirection;
        hasHit = false;

        projectileCollider.enabled = true;

        animator.Play("Fireball", 0, 0f);

        if (direction > 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    public void FireBallMove()
    {
        if (hasHit)
            return;

        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit)
            return;

        hasHit = true;
        projectileCollider.enabled = false;
        animator.SetTrigger("Explode");
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}