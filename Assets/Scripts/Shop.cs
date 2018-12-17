using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    GameObject player;
    PlayerMovementScript playerMovement;
    GunScript primaryGunScript, secondaryGunScript;
    GunInventory gunInventory;
    ScoreManager scoreManager;
    AudioController audioController;

    Text shopScoreText, 
        primaryMoreAmmoText, primaryFullAmmoText,
        secondaryBuyText, secondaryMoreAmmoText, secondaryFullAmmoText;
    Button primaryReloadButton, primaryMoreAmmoButton, primaryFullAmmoButton,
        secondaryBuyButton, secondaryReloadButton, secondaryMoreAmmoButton, secondaryFullAmmoButton;

    bool justEntered;
    int maxBullets = 180;
    int bulletPrice = 10;
    int score, amountOfBulletsToBuy, 
        primaryMoreAmmoPrice, primaryFullAmmoPrice, 
        secondaryMoreAmmoPrice, secondaryFullAmmoPrice;
    int secondaryGunPrice = 2000;

    // Use this for initialization
    void Start()
    {
        justEntered = true;
        
        player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovementScript>();
        gunInventory = player.GetComponent<GunInventory>();
        scoreManager = player.GetComponent<ScoreManager>();
        audioController = GameObject.Find("AudioManager").GetComponent<AudioController>();
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
                // Mute volume
                audioController.mixer.SetFloat("MainVolume", -80);

                Transform[] allChildren = GameObject.Find("ShopPanel").GetComponentsInChildren<Transform>(true);
                foreach (Transform child in allChildren)
                {
                    child.gameObject.SetActive(true);
                }

                // Get GameObjects
                shopScoreText = GameObject.Find("ShopScoreText").GetComponent<Text>();
                primaryMoreAmmoText = GameObject.Find("1st+30AmmoText").GetComponent<Text>();
                primaryFullAmmoText = GameObject.Find("1stFullAmmoText").GetComponent<Text>();

                primaryReloadButton = GameObject.Find("1stReloadButton").GetComponent<Button>();
                primaryMoreAmmoButton = GameObject.Find("1st+30AmmoButton").GetComponent<Button>();
                primaryFullAmmoButton = GameObject.Find("1stFullAmmoButton").GetComponent<Button>();

                primaryGunScript = gunInventory.primaryGun.GetComponent<GunScript>();

                secondaryBuyText = GameObject.Find("2ndBuyText").GetComponent<Text>();
                secondaryBuyButton = GameObject.Find("2ndBuyButton").GetComponent<Button>();

                secondaryMoreAmmoText = GameObject.Find("2nd+30AmmoText").GetComponent<Text>();
                secondaryFullAmmoText = GameObject.Find("2ndFullAmmoText").GetComponent<Text>();

                secondaryReloadButton = GameObject.Find("2ndReloadButton").GetComponent<Button>();
                secondaryMoreAmmoButton = GameObject.Find("2nd+30AmmoButton").GetComponent<Button>();
                secondaryFullAmmoButton = GameObject.Find("2ndFullAmmoButton").GetComponent<Button>();

                if (gunInventory.secondaryGun == null)
                {
                    secondaryBuyText.text = "Buy 2nd Gun: \n" + secondaryGunPrice.ToString();

                    secondaryBuyButton.gameObject.SetActive(true);
                    secondaryReloadButton.gameObject.SetActive(false);
                    secondaryMoreAmmoButton.gameObject.SetActive(false);
                    secondaryFullAmmoButton.gameObject.SetActive(false);
                }
                else
                {
                    secondaryBuyButton.gameObject.SetActive(false);
                    secondaryReloadButton.gameObject.SetActive(true);
                    secondaryMoreAmmoButton.gameObject.SetActive(true);
                    secondaryFullAmmoButton.gameObject.SetActive(true);
                    secondaryGunScript = gunInventory.secondaryGun.GetComponent<GunScript>();
                }
                // Stop score script
                scoreManager.stop();
                // Show the cursor
                Cursor.lockState = CursorLockMode.None;
            }
            // Set score to screen
            score = Mathf.RoundToInt(scoreManager.getScore());
            shopScoreText.text = "Score: " + score.ToString();

            if (primaryGunScript != null)
            {
                CheckPrimaryGunPrices();
            }

            if (secondaryGunScript != null)
            {
                CheckSecondaryGunPrices();
            }
            else
            {
                if (score < secondaryGunPrice)
                {
                    secondaryBuyButton.interactable = false;
                    ChangeImageMaterial(secondaryBuyButton);
                }
            }
        }
    }

    void CheckPrimaryGunPrices()
    {
        amountOfBulletsToBuy = Mathf.RoundToInt(maxBullets - primaryGunScript.bulletsIHave);
        primaryFullAmmoPrice = amountOfBulletsToBuy * bulletPrice;

        amountOfBulletsToBuy = 30;
        primaryMoreAmmoPrice = amountOfBulletsToBuy * bulletPrice;
        
        primaryMoreAmmoText.text = "+30 Ammo: \n" + primaryMoreAmmoPrice.ToString();
        primaryFullAmmoText.text = "Full Ammo: \n" + primaryFullAmmoPrice.ToString();

        PrimaryGunButtons();
    }

    void CheckSecondaryGunPrices()
    {
        amountOfBulletsToBuy = Mathf.RoundToInt(maxBullets - secondaryGunScript.bulletsIHave);
        secondaryFullAmmoPrice = amountOfBulletsToBuy * bulletPrice;

        amountOfBulletsToBuy = 30;
        secondaryMoreAmmoPrice = amountOfBulletsToBuy * bulletPrice;
        
        secondaryMoreAmmoText.text = "+30 Ammo: \n" + secondaryMoreAmmoPrice.ToString();
        secondaryFullAmmoText.text = "Full Ammo: \n" + secondaryFullAmmoPrice.ToString();

        SecondaryGunButtons();
    }

    void BuyAmmo(int scoreAmount, GunScript gun, int bulletAmount)
    {
        if (scoreManager.removeScore(scoreAmount))
        {
            gun.bulletsIHave += bulletAmount;
            if (gun.bulletsIHave > maxBullets)
            {
                gun.bulletsIHave = maxBullets;
            }
        }

        if (gun == primaryGunScript)
        {
            CheckPrimaryGunPrices();
        }
        else if (gun == secondaryGunScript)
        {
            CheckSecondaryGunPrices();
        }
    }

    public void Buy2ndBuyClick(Button button)
    {
        if (scoreManager.removeScore(secondaryGunPrice))
        {
            gunInventory.CreateSecondaryWeapon();
            secondaryGunScript = gunInventory.secondaryGun.GetComponent<GunScript>();

            secondaryReloadButton.gameObject.SetActive(true);
            secondaryMoreAmmoButton.gameObject.SetActive(true);
            secondaryFullAmmoButton.gameObject.SetActive(true);

            secondaryBuyButton.gameObject.SetActive(false);

            CheckSecondaryGunPrices();
        }
    }

    public void ReloadClick(Button button)
    {
        if (button.name.Contains("1st"))
        {
            primaryGunScript.ReloadMethod();
            CheckPrimaryGunPrices();
        }
        else if (button.name.Contains("2nd"))
        {
            secondaryGunScript.ReloadMethod();
            CheckSecondaryGunPrices();
        }
    }

    public void MoreAmmoClick(Button button)
    {
        if (button.name.Contains("1st"))
        {
            if (primaryGunScript.bulletsIHave + 30 < maxBullets)
            {
                amountOfBulletsToBuy = 30;
                BuyAmmo(primaryMoreAmmoPrice, primaryGunScript, amountOfBulletsToBuy);
            }
        }
        else if (button.name.Contains("2nd"))
        {
            if (secondaryGunScript.bulletsIHave + 30 < maxBullets)
            {
                amountOfBulletsToBuy = 30;
                BuyAmmo(secondaryMoreAmmoPrice, secondaryGunScript, amountOfBulletsToBuy);
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
                BuyAmmo(primaryFullAmmoPrice, primaryGunScript, amountOfBulletsToBuy);
            }
        }
        else if (button.name.Contains("2nd"))
        {
            if (secondaryGunScript.bulletsIHave < maxBullets)
            {
                amountOfBulletsToBuy = Mathf.RoundToInt(maxBullets - secondaryGunScript.bulletsIHave);
                BuyAmmo(secondaryFullAmmoPrice, secondaryGunScript, amountOfBulletsToBuy);
            }
        }
    }

    public void ExitClick()
    {
        // Resume game
        Time.timeScale = 1;
        // Unmute volume
        audioController.mixer.SetFloat("MainVolume", PlayerPrefs.GetFloat("MainVolume"));
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

    void PrimaryGunButtons()
    {
        if (primaryGunScript.bulletsInTheGun == 30)
        {
            primaryReloadButton.interactable = false;
            ChangeImageMaterial(primaryReloadButton);
        }
        else
        {
            primaryReloadButton.interactable = true;
            ChangeImageMaterial(primaryReloadButton);
        }
        if (score < primaryFullAmmoPrice || primaryFullAmmoPrice == 0)
        {
            primaryFullAmmoButton.interactable = false;
            ChangeImageMaterial(primaryFullAmmoButton);
        }
        else
        {
            primaryFullAmmoButton.interactable = true;
            ChangeImageMaterial(primaryFullAmmoButton);
        }
        if (score < primaryMoreAmmoPrice || primaryMoreAmmoPrice == 0)
        {
            primaryMoreAmmoButton.interactable = false;
            ChangeImageMaterial(primaryMoreAmmoButton);
        }
        else
        {
            primaryMoreAmmoButton.interactable = true;
            ChangeImageMaterial(primaryMoreAmmoButton);
        }
    }

    void SecondaryGunButtons()
    {
        if (secondaryGunScript.bulletsInTheGun == 30)
        {
            secondaryReloadButton.interactable = false;
            ChangeImageMaterial(secondaryReloadButton);
        }
        else
        {
            secondaryReloadButton.interactable = true;
            ChangeImageMaterial(secondaryReloadButton);
        }
        if (score < secondaryFullAmmoPrice || secondaryFullAmmoPrice == 0)
        {
            secondaryFullAmmoButton.interactable = false;
            ChangeImageMaterial(secondaryFullAmmoButton);
        }
        else
        {
            secondaryFullAmmoButton.interactable = true;
            ChangeImageMaterial(secondaryFullAmmoButton);
        }
        if (score < secondaryMoreAmmoPrice || secondaryMoreAmmoPrice == 0)
        {
            secondaryMoreAmmoButton.interactable = false;
            ChangeImageMaterial(secondaryMoreAmmoButton);
        }
        else
        {
            secondaryMoreAmmoButton.interactable = true;
            ChangeImageMaterial(secondaryMoreAmmoButton);
        }
    }

    void ChangeImageMaterial(Button button)
    {
        if (button.interactable == false)
        {
            button.image.material = Resources.Load("Materials/ButtonDisabledMat", typeof(Material)) as Material;
        }
        else
        {
            button.image.material = Resources.Load("Materials/ButtonMat", typeof(Material)) as Material;
        }
    }
}
