using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.AI;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;                            // The amount of health the player starts the game with.
    public int currentHealth;                                   // The current health the player has.
    public Slider healthSlider;                                 // Reference to the UI's health bar.
    public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
    public AudioClip deathClip;                                 // The audio clip to play when the player dies.
    public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.

    Transform cameraMain;                                       // Reference to the MainCamera's transform.
    Animator anim;                                              // Reference to the Animator component.
    AudioSource playerAudio;                                    // Reference to the AudioSource component.
    PlayerMovementScript playerMovement;                        // Reference to the player's movement.
    MouseLookScript mouseLook;                                  // Reference to the player's mouse look.
    GunInventory gunInventory;                                  // Reference to the PlayerShooting script.
    SkinnedMeshRenderer skinnedMeshRenderer;                    // Reference to the player's Skinned Mesh Renderer.
    bool isDead;                                                // Whether the player is dead.
    bool damaged;                                               // True when the player gets damaged.
    
    private GameObject gameOverPanel;

    private IEnumerator deathCamera;

    void Awake ()
    {
        // Setting up the references.
        cameraMain = transform.Find("Main Camera").transform;
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovementScript> ();
        mouseLook = GetComponent<MouseLookScript> ();
        gunInventory = GetComponentInChildren <GunInventory> ();
        skinnedMeshRenderer = GameObject.Find("Men_4").GetComponent<SkinnedMeshRenderer>();
        
        // Don't show the player body
        skinnedMeshRenderer.enabled = false;

        gameOverPanel = GameObject.Find("GameOverPanel");
        gameOverPanel.SetActive(false);

        // Set the initial health of the player.
        currentHealth = startingHealth;
    }


    void Update ()
    {
        // If the player has just been damaged...
        if(damaged)
        {
            // ... set the colour of the damageImage to the flash colour.
            //damageImage.color = flashColour;
        }
        // Otherwise...
        else
        {
            // ... transition the colour back to clear.
            //damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        // Reset the damaged flag.
        damaged = false;
    }


    public void TakeDamage (int amount)
    {
        // Set the damaged flag so the screen will flash.
        damaged = true;

        // Reduce the current health by the damage amount.
        currentHealth -= amount;

        // Set the health bar's value to the current health.
        healthSlider.value = currentHealth;

        // Play the hurt sound effect.
        //playerAudio.Play ();

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if(currentHealth <= 0 && !isDead)
        {
            // ... it should die.
            Death ();
        }
    }
    
    void Death ()
    {
        // Set the death flag so this function won't be called again.
        isDead = true;

        skinnedMeshRenderer.enabled = true;         // Show the player body

        anim.SetTrigger("Dead");                    // Tell the animator that the player is dead.

        MoveCamera();                               // Move the camera up and rotate to look down

        gameOverPanel.SetActive(true);              // Show the game over panel

        GetScoreAndKills();                         // Get score and kills to score screen.

        GetComponent<ScoreManager>().stop();        // Stop score script

        PreventShopping();                          // Prevent shopping if died inside the shop building

        DisablePlayerInputs();                      // Disable unwanted player input

        Cursor.lockState = CursorLockMode.None;     // Show the cursor

        // Turn off any remaining shooting effects.
        //playerShooting.DisableEffects();

        // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        //playerAudio.clip = deathClip;
        //playerAudio.Play ();
    }

    private Vector3 cameraVelocity = Vector3.zero;
    public IEnumerator DeathCamera(float timeCount, Vector3 deathPosition, Vector3 targetPosition)
    {
        while (true)
        {
            cameraMain.localRotation = Quaternion.Slerp(cameraMain.localRotation, Quaternion.Euler(90, 0, 180), timeCount);
            cameraMain.position = Vector3.SmoothDamp(cameraMain.position, targetPosition, ref cameraVelocity, 2f);
            if (targetPosition.y - cameraMain.position.y <= 1f)
            {
                StopCoroutine(deathCamera);
            }
            timeCount = timeCount + Time.deltaTime * 0.03f;
            yield return 0;
        }
    }

    private void MoveCamera()
    {
        Vector3 deathPosition = cameraMain.position;
        Vector3 targetPosition = new Vector3(deathPosition.x, deathPosition.y + 5f, deathPosition.z);
        deathCamera = DeathCamera(0, deathPosition, targetPosition);
        StartCoroutine(deathCamera);
    }

    private void GetScoreAndKills()
    {
        Text gameOverScoreText = GameObject.Find("GameOverScoreText").GetComponent<Text>();
        Text gameOverKillsText = GameObject.Find("GameOverKillsText").GetComponent<Text>();
        gameOverScoreText.text = "Score: " + Mathf.RoundToInt(GetComponent<ScoreManager>().getScore()).ToString();
        gameOverKillsText.text = "Kills: " + GetComponent<ScoreManager>().getKills().ToString();
    }

    private static void PreventShopping()
    {
        GameObject.FindGameObjectWithTag("Shop").SetActive(false);
        GameObject.FindGameObjectWithTag("Closed Shop").SetActive(false);
        GameObject.Find("ShopClosedText").GetComponent<Text>().enabled = false;
    }

    private void DisablePlayerInputs()
    {
        playerMovement.enabled = false;
        mouseLook.enabled = false;
        gunInventory.DeadMethod();
    }
}