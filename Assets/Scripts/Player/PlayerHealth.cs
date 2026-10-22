
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private Image[] hearts;
    [SerializeField] private float respawnDelay = 1f;

    private int currentHealth;

    private PlayerMotor player;
    private bool isRespawning;

    private void Awake()
    {
        player = GetComponent<PlayerMotor>();

        currentHealth = maxHealth;

        UpdateHearts();
    }

    public void TakeDamage(int damage)
    {
        if (player.IsDead || isRespawning)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        UpdateHearts();

        Die();
    }

    public void DieDirectly()
    {
        if (player.IsDead || isRespawning)
            return;

        currentHealth--;

        currentHealth = Mathf.Max(currentHealth, 0);

        UpdateHearts();

        Die();
    }

    private void Die()
    {
        player.IsDead = true;
        player.IsRespawning = true;
        isRespawning = true;

        player.InputManager.LockInput();

        if (player.Rigidbody != null)
        {
            player.Rigidbody.linearVelocity = Vector2.zero;
            player.Rigidbody.angularVelocity = 0f;
        }

        player.StateMachine.ChangeState(new DeathState(player, player.StateMachine));
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);

       
        if (currentHealth <= 0)
        {
            player.IsDead = true;
            player.IsRespawning = false;
            isRespawning = false;

            UiManager.Instance.ShowGameOver();

            yield break;
        }

        Transform respawnPoint = CheckpointManager.Instance.GetRespawnPoint();


        Rigidbody2D rb = player.Rigidbody;

        player.InputManager.LockInput();

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        player.transform.SetPositionAndRotation(respawnPoint.position, respawnPoint.rotation);

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        player.Animator.Rebind();
        player.Animator.Update(0f);

        player.Animator.ResetTrigger("Death");
        player.Animator.ResetTrigger("Attack");

        player.Animator.SetBool("isJumping", false);
        player.Animator.SetFloat("Speed", 0f);

        player.isGrounded = false;
        player.isWallSliding = false;

        player.IsDead = false;

        player.StateMachine.ChangeState(new IdleState(player, player.StateMachine));

        yield return new WaitForFixedUpdate();

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        player.InputManager.LockInput();

        yield return null;

        player.InputManager.UnlockInput();

        player.IsRespawning = false;
        isRespawning = false;
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].gameObject.SetActive(
                i < currentHealth
            );
        }
    }
}
