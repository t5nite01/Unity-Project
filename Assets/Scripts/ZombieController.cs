using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
  private bool close;
  private float difficultyScaler;
  GameObject zombie;
  Animator zombieAnimator;
  Animation zombieAnimation;
  Transform player;               // Reference to the player's position.
  PlayerHealth playerHealth;      // Reference to the player's health.
  EnemyHealth enemyHealth;        // Reference to this enemy's health.
  EnemyAttack enemyAttack;
  NavMeshAgent nav;               // Reference to the nav mesh agent.

  void Start()
  {
    
  }

  void Awake()
  {
    // Set up the references.
    player = GameObject.FindGameObjectWithTag("Player").transform;
    playerHealth = player.GetComponent<PlayerHealth>();
    enemyHealth = GetComponent<EnemyHealth>();
    enemyAttack = GetComponent<EnemyAttack>();
    nav = GetComponent<NavMeshAgent>();
        zombieAnimator = GetComponentInChildren<Animator>();
    // Reset 
    difficultyScaler = 1;
  }

  void OnAnimatorMove()
  {
    zombieAnimator.speed = 1 * difficultyScaler;
    nav.velocity = zombieAnimator.deltaPosition / Time.deltaTime * difficultyScaler;
  }

  // Update is called once per frame
  void Update()
    {
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

  public void kill(){
    zombieAnimator.ResetTrigger("Attacking");
    zombieAnimator.ResetTrigger("Moving");
    zombieAnimator.SetTrigger("Dead");
    this.enabled = false;
  }
  public void setDifficultyScale(float scale){
    difficultyScaler = scale;
  }
}
