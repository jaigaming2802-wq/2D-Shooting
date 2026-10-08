
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Vector2 firePointStartPos;
    private GameObject[] projectiles;

    [Header("Fire")]

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int poolSize = 5;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        firePointStartPos = firePoint.localPosition;

        projectiles = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            projectiles[i] = Instantiate(projectilePrefab);
            projectiles[i].SetActive(false);
        }
    }

    private void Update()
    {
        FirePointFlip();
    }

    private void FirePointFlip()
    {
        if (spriteRenderer.flipX)
        {
            firePoint.localPosition = new Vector2(-Mathf.Abs(firePointStartPos.x), firePointStartPos.y);
        }
        else
        {
            firePoint.localPosition = new Vector2(Mathf.Abs(firePointStartPos.x), firePointStartPos.y);
        }
    }

    public void Shoot()
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (!projectiles[i].activeInHierarchy)
            {
                projectiles[i].transform.position = firePoint.position;
                Projectile projectile = projectiles[i].GetComponent<Projectile>();

                if (spriteRenderer.flipX)
                {
                    projectile.SetDirection(-1);
                }

                else
                {
                    projectile.SetDirection(1);
                }

                projectiles[i].SetActive(true);
                break;
            }
        }
    }
}
