using UnityEngine;
using UnityEngine.AI;
public class EnemyHealth : MonoBehaviour
{
  private float difficulty;
  public int startingHealth = 100;            // The amount of health the enemy starts the game with.
  public int attackDamage = 10;
  public int currentHealth;                   // The current health the enemy has.
  public float sinkSpeed = 2f;              // The speed at which the enemy sinks through the floor when dead.
  [HideInInspector] public int scoreValue = 50;                 // The amount added to the player's score when the enemy dies.
  public AudioClip deathClip;                 // The sound to play when the enemy dies.
  public AudioClip damageSound;
  public AudioClip walkSound;
  Animator anim;                              // Reference to the animator.
  AudioSource enemyAudio;                     // Reference to the audio source.
  ParticleSystem hitParticles;                // Reference to the particle system that plays when the enemy is damaged.
  CapsuleCollider capsuleCollider;            // Reference to the capsule collider.
  Rigidbody rigidBody;
  EnemyAttack enemyAttack; 
  ZombieController zombieController;
  ScoreManager scoreManager;
  private AudioSource zombieAS;
  
  bool isDead;                                // Whether the enemy is dead.
  bool isSinking;                             // Whether the enemy has started sinking through the floor.
  
  void Awake()
  {
    // Setting up the references.
    zombieAS = GetComponent<AudioSource>();
    anim = GetComponent<Animator>();
    enemyAudio = GetComponent<AudioSource>();
    hitParticles = GetComponentInChildren<ParticleSystem>();
    capsuleCollider = GetComponent<CapsuleCollider>();
    rigidBody = GetComponent<Rigidbody>();
    enemyAttack = GetComponent<EnemyAttack>();
    zombieController = GetComponent<ZombieController>();
    scoreManager = GameObject.FindWithTag("Player").GetComponent<ScoreManager>();

    // Setting the current health when the enemy first spawns.
    currentHealth = startingHealth;
  }

  void Update()
  {
    // If the enemy should be sinking...
    if (isSinking)
    {
      // ... move the enemy down by the sinkSpeed per second.
      transform.Translate(0,-1 * sinkSpeed * Time.deltaTime,0);
    }
  }


  public void TakeDamage(int amount, Vector3 hitPoint)
  {
    // If the enemy is dead...
    if (isDead)
      // ... no need to take damage so exit the function.
      return;

    // Play the hurt sound effect.
    zombieAS.PlayOneShot(damageSound);

    // Reduce the current health by the amount of damage sustained.
    currentHealth -= amount;

    // Set the position of the particle system to where the hit was sustained.
    //hitParticles.transform.position = hitPoint;

    // And play the particles.
    //hitParticles.Play();

    // If the current health is less than or equal to zero...
    if (currentHealth <= 0)
    {
      // ... the enemy is dead.
      Death();
    }
  }

  public void setHealthByScale(float scale){
    float newHealth = (currentHealth * scale);
    currentHealth = Mathf.RoundToInt(newHealth);
  }

  void Death()
  {
    isDead = true;
    zombieController.Kill();
    // Turn the collider into a trigger so shots can pass through it.
    capsuleCollider.isTrigger = true;
    rigidBody.useGravity = false;
    scoreManager.addKillAndScore(scoreValue);

    // Change the audio clip of the audio source to the death clip and play it (this will stop the hurt clip playing).
    zombieAS.PlayOneShot(deathClip);

    StartSinking();
  }

  public void StartSinking()
  {
    // Find and disable the Nav Mesh Agent.
    GetComponent<NavMeshAgent>().enabled = false;

    // Find the rigidbody component and make it kinematic (since we use Translate to sink the enemy).
    GetComponent<Rigidbody>().isKinematic = true;

    //start enemy sink after 1 second
    Invoke("sink", 1f);
  
    // After 2 seconds destory the enemy.
    Destroy(gameObject, 2f);
  }
  private void sink(){
    // The enemy should now sink.
    isSinking = true;
  }
}