using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
  private bool close;
  private float difficultyScaler;

  Animator zombieAnimator;
  Animation zombieAnimation;
  Transform player;               // Reference to the player's position.
  PlayerHealth playerHealth;      // Reference to the player's health.
  NavMeshAgent nav;               // Reference to the nav mesh agent.

  void Start()
  {
    
  }

  void Awake()
  {
    // Set up the references.
    player = GameObject.FindGameObjectWithTag("Player").transform;
    playerHealth = player.GetComponent<PlayerHealth>();
    nav = GetComponent<NavMeshAgent>();
    zombieAnimator = GetComponentInChildren<Animator>();
    // Reset 
    difficultyScaler = 1;
    }

  void OnAnimatorMove()
  {
        if (Time.timeScale != 0)    // if game is not paused
        {
            zombieAnimator.speed = 1 * difficultyScaler;
            nav.velocity = zombieAnimator.deltaPosition / Time.unscaledDeltaTime * difficultyScaler;
        }
  }

  // Update is called once per frame
  void Update()
    {
        if (Time.timeScale != 0)    // if game is not paused
        {
            // If the player is dead...
            if (playerHealth.currentHealth <= 0f)
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
    }

  public void Kill(){
    zombieAnimator.ResetTrigger("Attacking");
    zombieAnimator.ResetTrigger("Moving");
    zombieAnimator.SetTrigger("Dead");
    this.enabled = false;
  }

  public void SetDifficultyScale(float scale){
    difficultyScaler = scale;
  }
}
