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

    Animator anim;                                              // Reference to the Animator component.
    AudioSource playerAudio;                                    // Reference to the AudioSource component.
    PlayerMovementScript playerMovement;                        // Reference to the player's movement.
    MouseLookScript mouseLook;                                  // Reference to the player's mouse look.
    GunInventory gunInventory;                                  // Reference to the PlayerShooting script.
    bool isDead;                                                // Whether the player is dead.
    bool damaged;                                               // True when the player gets damaged.
    
    private GameObject gameOverPanel;

    void Awake ()
    {
        // Setting up the references.
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovementScript> ();
        mouseLook = GetComponent<MouseLookScript> ();
        gunInventory = GetComponentInChildren <GunInventory> ();

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

        // Tell the animator that the player is dead.
        anim.SetTrigger("Dead");

        // Move the camera up and rotate to look down
        Transform cameraMain = transform.Find("Main Camera").transform;
        Vector3 cameraPosition = new Vector3(cameraMain.position.x, cameraMain.position.y + 5f, cameraMain.position.z);
        Quaternion cameraRotation = Quaternion.Euler(90f, cameraMain.rotation.y, cameraMain.rotation.z);
        cameraMain.SetPositionAndRotation(cameraPosition, cameraRotation);

        gameOverPanel.SetActive(true);
        // Get score and kills to score screen.
        Text gameOverScoreText = GameObject.Find("GameOverScoreText").GetComponent<Text>();
        Text gameOverKillsText = GameObject.Find("GameOverKillsText").GetComponent<Text>();
        gameOverScoreText.text = "Score: " + Mathf.RoundToInt(GetComponent<ScoreManager>().getScore()).ToString();
        gameOverKillsText.text = "Kills: " + GetComponent<ScoreManager>().getKills().ToString();
        // Stop score script
        GetComponent<ScoreManager>().stop();
        // Disable unwanted player input
        playerMovement.enabled = false;
        mouseLook.enabled = false;
        gunInventory.DeadMethod();
        Cursor.lockState = CursorLockMode.None;

        // Turn off any remaining shooting effects.
        //playerShooting.DisableEffects();

        // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        //playerAudio.clip = deathClip;
        //playerAudio.Play ();
    }       
}