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

    Text shopScoreText, 
        primaryMoreAmmoText, primaryFullAmmoText, 
        secondaryMoreAmmoText, secondaryFullAmmoText;
    Button primaryReloadButton, primaryMoreAmmoButton, primaryFullAmmoButton, 
        secondaryReloadButton, secondaryMoreAmmoButton, secondaryFullAmmoButton;

    bool justEntered;
    int maxBullets = 180;
    int bulletPrice = 10;
    int score, amountOfBulletsToBuy, 
        primaryMoreAmmoPrice, primaryFullAmmoPrice, 
        secondaryMoreAmmoPrice, secondaryFullAmmoPrice;

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
                // Get score text components
                shopScoreText = GameObject.Find("ShopScoreText").GetComponent<Text>();
                primaryMoreAmmoText = GameObject.Find("1st+30AmmoText").GetComponent<Text>();
                primaryFullAmmoText = GameObject.Find("1stFullAmmoText").GetComponent<Text>();
                secondaryMoreAmmoText = GameObject.Find("2nd+30AmmoText").GetComponent<Text>();
                secondaryFullAmmoText = GameObject.Find("2ndFullAmmoText").GetComponent<Text>();
                //Get buttons
                primaryReloadButton = GameObject.Find("1stReloadButton").GetComponent<Button>();
                primaryMoreAmmoButton = GameObject.Find("1st+30AmmoButton").GetComponent<Button>();
                primaryFullAmmoButton = GameObject.Find("1stFullAmmoButton").GetComponent<Button>();
                secondaryReloadButton = GameObject.Find("2ndReloadButton").GetComponent<Button>();
                secondaryMoreAmmoButton = GameObject.Find("2nd+30AmmoButton").GetComponent<Button>();
                secondaryFullAmmoButton = GameObject.Find("2ndFullAmmoButton").GetComponent<Button>();
                // Stop score script
                scoreManager.stop();
                // Show the cursor
                Cursor.lockState = CursorLockMode.None;

                primaryGunScript = gunInventory.primaryGun.GetComponent<GunScript>();
                secondaryGunScript = gunInventory.secondaryGun.GetComponent<GunScript>();
            }
            // Set score to screen
            score = Mathf.RoundToInt(scoreManager.getScore());
            shopScoreText.text = "Score: " + score.ToString();

            if (primaryGunScript != null && secondaryGunScript != null)
            {
                CheckPrices();
            }
        }
    }

    void CheckPrices()
    {
        amountOfBulletsToBuy = Mathf.RoundToInt(maxBullets - primaryGunScript.bulletsIHave);
        primaryFullAmmoPrice = amountOfBulletsToBuy * bulletPrice;

        amountOfBulletsToBuy = 30;
        primaryMoreAmmoPrice = amountOfBulletsToBuy * bulletPrice;
        
        amountOfBulletsToBuy = Mathf.RoundToInt(maxBullets - secondaryGunScript.bulletsIHave);
        secondaryFullAmmoPrice = amountOfBulletsToBuy * bulletPrice;

        amountOfBulletsToBuy = 30;
        secondaryMoreAmmoPrice = amountOfBulletsToBuy * bulletPrice;

        primaryMoreAmmoText.text = "+30 Ammo: \n" + primaryMoreAmmoPrice.ToString();
        primaryFullAmmoText.text = "Full Ammo: \n" + primaryFullAmmoPrice.ToString();
        secondaryMoreAmmoText.text = "+30 Ammo: \n" + secondaryMoreAmmoPrice.ToString();
        secondaryFullAmmoText.text = "Full Ammo: \n" + secondaryFullAmmoPrice.ToString();

        EnableAndDisableButtons();
    }

    void Buy(int scoreAmount, GunScript gun, int bulletAmount)
    {
        if (scoreManager.removeScore(scoreAmount))
        {
            gun.bulletsIHave += bulletAmount;
            if (gun.bulletsIHave > maxBullets)
            {
                gun.bulletsIHave = maxBullets;
            }
        }
        else
        {
            StartCoroutine(ShowMessage("Not enough score to buy", 3f));
        }

        CheckPrices();
    }

    IEnumerator ShowMessage(string message, float delay)
    {
        playerMovement.shopInfoText.SetActive(true);
        Text infoText = playerMovement.shopInfoText.GetComponent<Text>();
        infoText.text = message;
        yield return new WaitForSecondsRealtime(delay);
        playerMovement.shopInfoText.SetActive(false);
    }

    public void ReloadClick(Button button)
    {
        if (button.name.Contains("1st"))
        {
            primaryGunScript.ReloadMethod();
        }
        else if (button.name.Contains("2nd"))
        {
            secondaryGunScript.ReloadMethod();
        }

        CheckPrices();
    }

    public void MoreAmmoClick(Button button)
    {
        if (button.name.Contains("1st"))
        {
            if (primaryGunScript.bulletsIHave + 30 < maxBullets)
            {
                amountOfBulletsToBuy = 30;
                Buy(primaryMoreAmmoPrice, primaryGunScript, amountOfBulletsToBuy);
            }
            else
            {
                StartCoroutine(ShowMessage("More ammo than max - 30", 3f));
            }
        }
        else if (button.name.Contains("2nd"))
        {
            if (secondaryGunScript.bulletsIHave + 30 < maxBullets)
            {
                amountOfBulletsToBuy = 30;
                Buy(secondaryMoreAmmoPrice, secondaryGunScript, amountOfBulletsToBuy);
            }
            else
            {
                StartCoroutine(ShowMessage("More ammo than max - 30", 3f));
            }
        }
    }

    public void FullAmmoClick(Button button)
    {
        if (button.name.Contains("1st"))
        {
            if (primaryGunScript.bulletsIHave < maxBullets)
            {
                amountOfBulletsToBuy = Mathf.RoundToInt(maxBullets - primaryGunScript.bulletsIHave);
                Buy(primaryFullAmmoPrice, primaryGunScript, amountOfBulletsToBuy);
            }
            else
            {
                StartCoroutine(ShowMessage("Ammo already full", 3f));
            }
        }
        else if (button.name.Contains("2nd"))
        {
            if (secondaryGunScript.bulletsIHave < maxBullets)
            {
                amountOfBulletsToBuy = Mathf.RoundToInt(maxBullets - secondaryGunScript.bulletsIHave);
                Buy(secondaryFullAmmoPrice, secondaryGunScript, amountOfBulletsToBuy);
            }
            else
            {
                StartCoroutine(ShowMessage("Ammo already full", 3f));
            }
        }
    }

    public void ExitClick()
    {
        // Resume game
        Time.timeScale = 1;
        // Resume score script
        scoreManager.setScore(score);
        scoreManager.resume();
        // Hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        // Exit shopping
        playerMovement.shopping = false;
        // Allow 
        justEntered = true;
    }

    void EnableAndDisableButtons()
    {
        if (primaryGunScript.bulletsInTheGun == 30)
        {
            primaryReloadButton.interactable = false;
        }
        else
        {
            primaryReloadButton.interactable = true;
        }
        if (secondaryGunScript.bulletsInTheGun == 30)
        {
            secondaryReloadButton.interactable = false;
        }
        else
        {
            secondaryReloadButton.interactable = true;
        }
        if (score < primaryFullAmmoPrice || primaryFullAmmoPrice == 0)
        {
            primaryFullAmmoButton.interactable = false;
        }
        else
        {
            primaryFullAmmoButton.interactable = true;
        }
        if (score < primaryMoreAmmoPrice || primaryMoreAmmoPrice == 0)
        {
            primaryMoreAmmoButton.interactable = false;
        }
        else
        {
            primaryMoreAmmoButton.interactable = true;
        }
        if (score < secondaryFullAmmoPrice || secondaryFullAmmoPrice == 0)
        {
            secondaryFullAmmoButton.interactable = false;
        }
        else
        {
            secondaryFullAmmoButton.interactable = true;
        }
        if (score < secondaryMoreAmmoPrice || secondaryMoreAmmoPrice == 0)
        {
            secondaryMoreAmmoButton.interactable = false;
        }
        else
        {
            secondaryMoreAmmoButton.interactable = true;
        }
    }
}
