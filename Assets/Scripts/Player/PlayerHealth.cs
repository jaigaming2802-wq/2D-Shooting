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

        currentHealth--;

        UpdateHearts();

        Die();
    }

    public void DieDirectly()
    {
        if (player.IsDead || isRespawning)
            return;

        currentHealth--;

        UpdateHearts();

        Die();
    }

    private void Die()
    {
        player.IsDead = true;
        player.IsRespawning = true;
        isRespawning = true;

        // Stop input immediately
        player.InputManager.LockInput();

        if (player.Rigidbody != null)
        {
            player.Rigidbody.linearVelocity = Vector2.zero;
            player.Rigidbody.angularVelocity = 0f;
        }

        player.StateMachine.ChangeState(
            new DeathState(
                player,
                player.StateMachine
            )
        );

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

            yield break;
        }

        Transform respawnPoint =
            CheckpointManager.Instance.GetRespawnPoint();

        Rigidbody2D rb = player.Rigidbody;

        // Keep input locked
        player.InputManager.LockInput();

        // Stop physics
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Teleport to checkpoint
        player.transform.SetPositionAndRotation(
            respawnPoint.position,
            respawnPoint.rotation
        );

        // Clear velocity again
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Reset Animator
        player.Animator.Rebind();
        player.Animator.Update(0f);

        player.Animator.ResetTrigger("Death");
        player.Animator.ResetTrigger("Attack");

        player.Animator.SetBool("isJumping", false);
        player.Animator.SetFloat("Speed", 0f);

        // Reset player checks
        player.isGrounded = false;
        player.isWallSliding = false;

        // Player is alive
        player.IsDead = false;

        // Start from Idle
        player.StateMachine.ChangeState(
            new IdleState(
                player,
                player.StateMachine
            )
        );

        // Wait for physics frame
        yield return new WaitForFixedUpdate();

        // Stop any movement that happened during respawn
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Clear input one more time
        player.InputManager.LockInput();

        // Wait one frame
        yield return null;

        // Now allow player input
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