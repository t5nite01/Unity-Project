using UnityEngine;

public class EnemyManager : MonoBehaviour
{
  public PlayerHealth playerHealth;  // Reference to the player's heatlh.
  public GameObject player;               // ref to player position.
  private static float difficultyScaler = 1;
  public GameObject enemy;                // The enemy prefab to be spawned.
  public float spawnTime = 7f;            // How long between each spawn.
  public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.
  private int Spawns;
  private float runtime;
  public static Object zombiePrefab;

  void Start()
  {
    zombiePrefab = Resources.Load("Prefabs/Zombie");
    player = GameObject.Find("Player");
    // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
    InvokeRepeating("Spawn", spawnTime, spawnTime);
  }

  private void Update() {
    
    runtime += Time.deltaTime;

    // Increase difficulty scaler overtime
    if(runtime > 10){
      difficultyScaler += 0.1f;
      spawnTime -= 0.1f;
      runtime = 0;
    }
  } 

  void Spawn()
  {
    // If the player has no health left...
    if (playerHealth.currentHealth <= 0f)
    {
        // Disable spawning
        CancelInvoke("Spawn");
        return;
    }

    // Find a random index between zero and one less than the number of spawn points.
    int spawnPointIndex = Random.Range(0, spawnPoints.Length);
 
    // Spawn enemies if closer than 50m of spawn
    if (50 > Vector3.Distance(spawnPoints[spawnPointIndex].position, player.transform.position))
    {
      // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
      GameObject newObject = Instantiate(Resources.Load("Prefabs/Zombie", typeof(GameObject))) as GameObject;
      newObject.GetComponent<EnemyHealth>().setHealthByScale(difficultyScaler);
      newObject.GetComponent<EnemyAttack>().setAttackDamageByScale(difficultyScaler);
      newObject.GetComponent<ZombieController>().setDifficultyScale(difficultyScaler);
      newObject.transform.position = spawnPoints[spawnPointIndex].position;
      newObject.transform.rotation = spawnPoints[spawnPointIndex].rotation;
      
      // Requeue spawn if not close.
    } else {
      Invoke("Spawn", 0.1f);
    }
  }
}