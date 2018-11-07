using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ZombieController : MonoBehaviour
{
  private bool close;
  GameObject zombie;
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
    if (close == false && 1.8 < Vector3.Distance(new Vector3(player.position.x, player.position.y, player.position.z),
                             new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z)))
    {
      // ... set the destination of the nav mesh agent to the player.
      nav.enabled = true;
      nav.SetDestination(player.position);
      zombieAnimator.Play("walk");
      //zombieAnimator.SetTrigger("Moving");
      close = false;
    } else {
      close = true;
    }

    // Otherwise...
    if(close == true && 2.5 > Vector3.Distance(new Vector3(player.position.x, player.position.y, player.position.z),
                            new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z)))
    {

      //cooldown tbd
      zombieAnimator.Play("attack");
      // playerHealth.TakeDamage(10);
      nav.enabled = false;
    } else {
      close = false;
    }
  }

  void Awake()
  {
    // Set up the references.
    player = GameObject.FindGameObjectWithTag("Player").transform;
    //playerHealth = player.GetComponent<PlayerHealth>();
    //enemyHealth = GetComponent<EnemyHealth>();
    nav = GetComponent<NavMeshAgent>();
    zombieAnimator = GetComponentInParent<Animator>();
  }
}
