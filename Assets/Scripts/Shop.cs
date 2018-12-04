using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    GameObject player;
    PlayerMovementScript playerMovement;
    MouseLookScript mouseLook;
    GunScript primaryGunScript, secondaryGunScript;
    GunInventory gunInventory;
    ScoreManager scoreManager;

    bool justEntered;
    int maxBullets = 180;

    // Use this for initialization
    void Start()
    {
        justEntered = true;
        
        player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovementScript>();
        mouseLook = player.GetComponent<MouseLookScript>();
        gunInventory = player.GetComponent<GunInventory>();
        scoreManager = player.GetComponent<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update only when shopping
        if(playerMovement.shopping == true)
        {
            // Only allow the script to pass here once in one shopping time
            if (justEntered)
            {
                justEntered = false;
                // Pause game
                Time.timeScale = 0;
                // Get score to score screen.
                Text shopScoreText = GameObject.Find("ShopScoreText").GetComponent<Text>();
                shopScoreText.text = "Score: " + Mathf.RoundToInt(scoreManager.getScore()).ToString();
                // Stop score script
                scoreManager.stop();
                // Show the cursor
                Cursor.lockState = CursorLockMode.None;

                primaryGunScript = gunInventory.primaryGun.GetComponent<GunScript>();
                secondaryGunScript = gunInventory.secondaryGun.GetComponent<GunScript>();
            }
        }
    }

    public void ReloadClick()
    {
        primaryGunScript.ReloadMethod();
        secondaryGunScript.ReloadMethod();
    }

    public void MoreAmmoClick()
    {
        if (primaryGunScript.bulletsIHave < maxBullets)
        {
            primaryGunScript.bulletsIHave += 30;
            if (primaryGunScript.bulletsIHave > maxBullets)
            {
                primaryGunScript.bulletsIHave = maxBullets;
            }
        }

        if (secondaryGunScript.bulletsIHave < maxBullets)
        {
            secondaryGunScript.bulletsIHave += 30;
            if (secondaryGunScript.bulletsIHave > maxBullets)
            {
                secondaryGunScript.bulletsIHave = maxBullets;
            }
        }
    }

    public void FullAmmoClick()
    {
        if (primaryGunScript.bulletsIHave < maxBullets)
        {
            primaryGunScript.bulletsIHave = maxBullets;
        }

        if (secondaryGunScript.bulletsIHave < maxBullets)
        {
            secondaryGunScript.bulletsIHave = maxBullets;
        }
    }

    public void ExitClick()
    {
        // Resume game
        Time.timeScale = 1;
        // Resume score script
        scoreManager.resume();
        // Hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        // Exit shopping
        playerMovement.shopping = false;
        // Allow 
        justEntered = true;
    }
}
