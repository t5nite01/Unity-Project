using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{ 
  private AudioSource zombieAS;
  public AudioClip attackSound;
  public float timeBetweenAttacks = 0.5f;     // The time in seconds between each attack.
  private int attackDamage = 10;               // The amount of health taken away per attack.
  GameObject player;                          // Reference to the player GameObject.
  PlayerHealth playerHealth;                  // Reference to the player's health.
  EnemyHealth enemyHealth;                    // Reference to this enemy's health.
  bool playerInRange;                         // Whether player is within the trigger collider and can be attacked.
  float timer;                                // Timer for counting up to the next attack.

  void Awake()
  {
    // Setting up the references.
    zombieAS = GetComponent<AudioSource>();
    player = GameObject.FindGameObjectWithTag("Player");
    playerHealth = player.GetComponent<PlayerHealth>();
    enemyHealth = GetComponent<EnemyHealth>();
    // Reset
    attackDamage = 10;
  }

  void OnTriggerEnter(Collider other)
  {
    // If the entering collider is the player...
    if (other.gameObject == player)
    {
      // ... the player is in range.
      playerInRange = true;
    }
  }

  void OnTriggerExit(Collider other)
  {
    // If the exiting collider is the player...
    if (other.gameObject == player)
    {
      // ... the player is no longer in range.
      playerInRange = false;
    }
  }

  void Update()
  {
    // Add the time since Update was last called to the timer.
    timer += Time.deltaTime;

    // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
    if (timer >= timeBetweenAttacks && 2 > Vector3.Distance(new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z),
                        new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z)) && enemyHealth.currentHealth > 0)
    {
      // ... attack.
      timer = 0f;
      Attack();
    }

    // If the player has zero or less health...
    if (playerHealth.currentHealth <= 0)
    {
      // ... tell the animator the player is dead.
      //anim.SetTrigger ("PlayerDead");
    }
  }


  void Attack()
  {
    // Reset the timer.
    timer = 0f;

    // If the player has health to lose...
    if (playerHealth.currentHealth > 0)
    {
      // ... damage the player.
      zombieAS.PlayOneShot(attackSound);
      playerHealth.TakeDamage(attackDamage);
    }
  }


  public void setAttackDamageByScale(float scale)
  {
    attackDamage = Mathf.RoundToInt(attackDamage * scale);
  }
}