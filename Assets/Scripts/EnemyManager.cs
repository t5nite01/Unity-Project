using UnityEngine;

public class EnemyManager : MonoBehaviour
{
  public PlayerHealth playerHealth;  // Reference to the player's heatlh.
  public GameObject player;               // ref to player position.
  private static float difficultyScaler = 1;
  public float spawnTime = 7f;            // How long between each spawn.
  public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.
  private float runtime;

  void Start()
  {
    player = GameObject.Find("Player");
    difficultyScaler = 1;
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
    
    // Filter available spawnpoints
    Transform[] viableSpawns = new Transform[spawnPoints.Length];
    
    for(int i = 1; i < spawnPoints.Length; i++){

      float distanceFromPlayer = Vector3.Distance(spawnPoints[i].position, player.transform.position);
      
      // Spawn enemies if closer than 50m and farther than 10m
      if(distanceFromPlayer > 10 && 50 < distanceFromPlayer){
        viableSpawns[i] = spawnPoints[i];
      }
    }

    int spawnPointIndex = Random.Range(0, viableSpawns.Length);
    
    // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
    GameObject newObject;
    newObject = Instantiate(Resources.Load("Prefabs/Zombie", typeof(GameObject))) as GameObject;
    newObject.GetComponent<EnemyHealth>().setHealthByScale(difficultyScaler);
    newObject.GetComponent<EnemyAttack>().setAttackDamageByScale(difficultyScaler);
    newObject.GetComponent<ZombieController>().setDifficultyScale(difficultyScaler);
    newObject.transform.position = spawnPoints[spawnPointIndex].position;
    newObject.transform.rotation = spawnPoints[spawnPointIndex].rotation;
  }
}