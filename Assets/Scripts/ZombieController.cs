using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ZombieController : MonoBehaviour
{
  public bool alive;
  GameObject Zombie;
  Animator zombieAnimator;
  Transform player;               // Reference to the player's position.
  PlayerHealth playerHealth;      // Reference to the player's health.
  EnemyHealth enemyHealth;        // Reference to this enemy's health.
  NavMeshAgent nav;               // Reference to the nav mesh agent.
                                  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    // If the enemy and the player have health left...
    //if(enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
    if (true)
    {
      // ... set the destination of the nav mesh agent to the player.
      nav.SetDestination(player.position);
    
    }
    // Otherwise...
    else
    {
      // ... disable the nav mesh agent.
      nav.enabled = false;
    }
  }

  void Awake()
  {
    // Set up the references.
    player = GameObject.FindGameObjectWithTag("Player").transform;
    playerHealth = player.GetComponent<PlayerHealth>();
    enemyHealth = GetComponent<EnemyHealth>();
    nav = GetComponent<NavMeshAgent>();
    Zombie = nav.GetComponentInParent<GameObject>();
    zombieAnimator = Zombie.GetComponentInChildren<Animator>();
  }
}
