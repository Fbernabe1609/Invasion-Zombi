using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class HumanBehaviour : MonoBehaviour
{
    [SerializeField] private BoxCollider attackingHandCollider;
    [SerializeField] private float playerPositionCheckInterval = 0.5f;
    [SerializeField] private float health = 30f;
    [SerializeField] private AudioClip attackSound, deathSound, hitSound;
    [SerializeField] private AudioSource fxAudioSource, deathAudioSource;

    private NavMeshAgent agent;
    private Animator anim;
    private Transform playerPos;
    private float timer = 0f;
    private bool isAttacking = false;
    private bool isDead = false;
    private bool invincible = false;
    private float nextPlayerPositionCheckTime = 0f;
    public static float additionalDamage = 0f;
    public static int deadHumans = 0;

    private static readonly int IsDead = Animator.StringToHash("IsDead");
    private static readonly int Die1 = Animator.StringToHash("Die");
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Attack = Animator.StringToHash("Attack");

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        attackingHandCollider.enabled = false;

        agent.SetDestination(playerPos.position);
    }

    private void Update()
    {
        if (!isDead)
        {
            SendSpeedToAnim();
            CheckIfAttackPlayer();
            GoToPlayer();
        }
    }

    private void GoToPlayer()
    {
        if (Time.time >= nextPlayerPositionCheckTime)
        {
            agent.SetDestination(playerPos.position);
            nextPlayerPositionCheckTime = Time.time + playerPositionCheckInterval;
        }
    }

    private void SendSpeedToAnim()
    {
        anim.SetFloat(Speed, agent.velocity.magnitude);
    }

    private void CheckIfAttackPlayer()
    {
        if (!isAttacking)
        {
            float stoppingDistanceWithBuffer = agent.stoppingDistance + 1f;
            if (Vector3.Distance(transform.position, playerPos.position) <= stoppingDistanceWithBuffer)
            {
                anim.SetTrigger(Attack);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player Hands") && !isDead)
        {
            TakeDamage(10 + additionalDamage);
        }
    }

    private void TakeDamage(float damage)
    {
        if (!(invincible && isDead))
        {
            PlayHitSound();

            health -= damage;
            invincible = true;
            StartCoroutine(InvincibleCooldown());

            if (health <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        isDead = true;
        deadHumans++;
        PlayDeathSound();
        anim.SetBool(IsDead, true);
        anim.SetTrigger(Die1);
        anim.SetFloat(Speed, 0);
        agent.isStopped = true;
        attackingHandCollider.enabled = false;
        Destroy(gameObject, 5f);
    }
    
    
    IEnumerator IncreaseDamageOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(20f);
            additionalDamage += 10f;
        }
    }

    private void PlaySound(AudioClip sound)
    {
        fxAudioSource.pitch = Random.Range(0.8f, 1.2f);
        fxAudioSource.PlayOneShot(sound);
    }

    private void PlayAttackSound()
    {
        PlaySound(attackSound);
    }

    private void PlayHitSound()
    {
        PlaySound(hitSound);
    }

    private void PlayDeathSound()
    {
        fxAudioSource.Stop();
        deathAudioSource.PlayOneShot(deathSound);
    }

    public void StartingAttackAnim()
    {
        attackingHandCollider.enabled = true;
        isAttacking = true;
    }

    public void EndingAttackAnim()
    {
        attackingHandCollider.enabled = false;
        isAttacking = false;
    }

    IEnumerator InvincibleCooldown()
    {
        yield return new WaitForSeconds(1f);
        invincible = false;
    }
}