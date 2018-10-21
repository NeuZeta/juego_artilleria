using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour {

    public EnemyAI enemy;
    public PlayerControl hero;

    public Text TextTime;
    public Text TextTurn;

    public GameObject startButton;
    public GameObject attackButton;
    public CameraFollow cameraFollow;
    public GameObject UIRoot;
    private int timeRemaining = 10;
    private GameObject[] UI;

    private const string SELECTED_WEAPON = "Selected Weapon";

    private void Awake()
    {
        SetSelectedWeapon(0);
    }

    void Start() {

        cameraFollow.SetPlayerToFollow(hero.transform);
        enemy.GetComponentInChildren<Gun>().gunFired += SwapTurn;
        hero.GetComponentInChildren<Gun>().gunFired += SwapTurn;
        TextTime.gameObject.SetActive(false);
        TextTurn.gameObject.SetActive(false);
        UIRoot.SetActive(false);

    }

    public void StartGame()
    {
        startButton.GetComponent<Button>().enabled = false;
        hero.hasTurn = true;
        TextTime.gameObject.SetActive(true);
        TextTurn.gameObject.SetActive(true);
        startButton.SetActive(false);
        UIRoot.SetActive(hero.hasTurn);
        InvokeRepeating ("DecreaseTime", 0, 1);
    }

    void SwapTurn()
    {
        StartCoroutine(SwapTurnCoroutine());
    }

    IEnumerator SwapTurnCoroutine()
    {
        CancelInvoke("DecreaseTime");
        hero.hasTurn = !hero.hasTurn;
        UIRoot.SetActive(hero.hasTurn);

        cameraFollow.SetPlayerToFollow(hero.hasTurn ? hero.transform : enemy.transform);
        
        if (!enemy.hasTurn)
        {
            yield return new WaitForSeconds(5f);
            InvokeRepeating("DecreaseTime", 0, 1);
        }
        enemy.hasTurn = !enemy.hasTurn;
        timeRemaining = 10;
        InvokeRepeating("DecreaseTime", 4, 1);
    }

    void DecreaseTime()
    {
        if (hero.hasTurn) TextTurn.text = "Your Turn!";
        if (enemy.hasTurn) TextTurn.text = "Enemy's Turn!";

        timeRemaining--;
        if (timeRemaining == 0)
        {
            SwapTurn();
        }

        TextTime.text = timeRemaining.ToString();
    }

    public void SetSelectedWeapon(int selectedWeapon)
    {
        PlayerPrefs.SetInt(SELECTED_WEAPON, selectedWeapon);
    }

    public int GetSelectedWeapon()
    {
        return PlayerPrefs.GetInt(SELECTED_WEAPON);
    }


}//GameplayManager
