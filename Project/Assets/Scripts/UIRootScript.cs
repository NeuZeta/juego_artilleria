using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRootScript : MonoBehaviour {

    public GameObject weaponsOptions, plusBtn, minusBtn, bombBtn, bombSelectedBtn, bulletBtn, bulletSelectedBtn, rocketBtn, rocketSelectedBtn;
    public GameplayManager gameManager;

    private void Awake()
    {
        weaponsOptions.SetActive(false);

        minusBtn.SetActive(false);
        plusBtn.SetActive(true);

        bulletSelectedBtn.SetActive(true);
        bombSelectedBtn.SetActive(false);
        rocketSelectedBtn.SetActive(false);
        bulletBtn.SetActive(false);
        bombBtn.SetActive(true);
        rocketBtn.SetActive(true);
    }

    public void OpenWeaponsOptions()
    {
        weaponsOptions.SetActive(true);
        minusBtn.SetActive(true);
        plusBtn.SetActive(false);
    }

    public void CloseWeaponsOptions()
    {
        weaponsOptions.SetActive(false);
        minusBtn.SetActive(false);
        plusBtn.SetActive(true);
    }

    public void SetBulletAsWeapon()
    {
        gameManager.SetSelectedWeapon(0);

        bulletSelectedBtn.SetActive(true);
        bombSelectedBtn.SetActive(false);
        rocketSelectedBtn.SetActive(false);
        bulletBtn.SetActive(false);
        bombBtn.SetActive(true);
        rocketBtn.SetActive(true);
    }

    public void SetRocketAsWeapon()
    {
        gameManager.SetSelectedWeapon(1);

        bulletSelectedBtn.SetActive(false);
        bombSelectedBtn.SetActive(false);
        rocketSelectedBtn.SetActive(true);
        bulletBtn.SetActive(true);
        bombBtn.SetActive(true);
        rocketBtn.SetActive(false);
    }

    public void SetBombAsWeapon()
    {
        gameManager.SetSelectedWeapon(2);

        bulletSelectedBtn.SetActive(false);
        bombSelectedBtn.SetActive(true);
        rocketSelectedBtn.SetActive(false);
        bulletBtn.SetActive(true);
        bombBtn.SetActive(false);
        rocketBtn.SetActive(true);
    }

}
