using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
  private bool close;
  GameObject zombie;
  Animator zombieAnimator;
  Animation zombieAnimation;
  Transform player;               // Reference to the player's position.
  PlayerHealth playerHealth;      // Reference to the player's health.
  EnemyHealth enemyHealth;        // Reference to this enemy's health.
  NavMeshAgent nav;               // Reference to the nav mesh agent.
                                  // Use this for initialization
  void Start()
  {

  }

    void OnAnimatorMove()
  {
      nav.velocity = zombieAnimator.deltaPosition / Time.deltaTime;
  }

  // Update is called once per frame
  void Update()
    {
        // Switch between idle and walk
        // If the enemy and the player have health left...
        //if(enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)

        // If the player is dead...
        if(playerHealth.currentHealth <= 0f)
        {
            nav.enabled = false;
            zombieAnimator.ResetTrigger("Moving");
            zombieAnimator.ResetTrigger("Attacking");
        }
        else
        {
            if (close == false && 1.8 < Vector3.Distance(new Vector3(player.position.x, player.position.y, player.position.z),
                                      new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z)))
            {
                // ... set the destination of the nav mesh agent to the player.
                nav.enabled = true;

                nav.SetDestination(player.position);
                zombieAnimator.SetTrigger("Moving");
            }
            else
            {
                zombieAnimator.ResetTrigger("Moving");
                close = true;
            }

            // Otherwise...
            if (close == true && 2.5 > Vector3.Distance(new Vector3(player.position.x, player.position.y, player.position.z),
                                    new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z)))
            {
                zombieAnimator.SetTrigger("Attacking");
                nav.enabled = false;
            }
            else
            {
                close = false;
                zombieAnimator.ResetTrigger("Attacking");
            }
        }
    }

  void Awake()
  {
    // Set up the references.
    player = GameObject.FindGameObjectWithTag("Player").transform;
    playerHealth = player.GetComponent<PlayerHealth>();
    //enemyHealth = GetComponent<EnemyHealth>();
    nav = GetComponent<NavMeshAgent>();
    zombieAnimator = GetComponentInParent<Animator>();
  }
  public void kill(){
    zombieAnimator.ResetTrigger("Attacking");
    zombieAnimator.ResetTrigger("Moving");
    zombieAnimator.SetTrigger("Dead");
    this.enabled = false;
  }
}
