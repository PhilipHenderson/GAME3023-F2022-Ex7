using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystems : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public GameObject playerFlameIcon;
    public GameObject playerIceIcon;
    public GameObject enemyFlameIcon;
    public GameObject enemyIceIcon;
    public GameObject playerWindIcon;
    public GameObject enemyWindIcon;

    public Transform playerPlatform;
    public Transform enemyPlatform;

    Unit playerUnit;
    Unit enemyUnit;

    public TMP_Text encounterTxt;

    public BattlePlatform playerHud;
    public BattlePlatform enemyHud;

    public BattleState states;

    // Start is called before the first frame update
    void Start()
    {
        states = BattleState.START;
        StartCoroutine(BattleSetup());
        playerUnit.defendedDmg = playerUnit.dmg / 2;
        enemyUnit.defendedDmg = enemyUnit.dmg / 2;
    }


    IEnumerator BattleSetup()
    {
        GameObject player = Instantiate(playerPrefab, playerPlatform);
        playerUnit = player.GetComponent<Unit>();
        GameObject enemy = Instantiate(enemyPrefab, enemyPlatform);
        enemyUnit = enemy.GetComponent<Unit>();

        // Create More of these and randomize it
        encounterTxt.text = "A Sneaky " + enemyUnit.Unitname + " Has Apreared!";

        playerHud.HudSet(playerUnit);
        enemyHud.HudSet(enemyUnit);

        yield return new WaitForSeconds(2.0f);

        states = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    // Player Menu Coroutines
    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.dmg);

        enemyHud.HpSet(enemyUnit.currentHp);
        encounterTxt.text = "The Attack Was Successfull!";

        yield return new WaitForSeconds(2.0f);

        if (isDead)
        {
            states = BattleState.WON;
            EndBattle();
        }
        else
        {
            states = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }
    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(25);

        playerHud.HpSet(playerUnit.currentHp);
        encounterTxt.text = "The Player Healed!";

        yield return new WaitForSeconds(2.0f);

        states= BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }
    IEnumerator PlayerDefend()
    {
        playerUnit.isDefending = true;
        encounterTxt.text = "Player Gets Into A Defencive Position!";
        yield return new WaitForSeconds(2.0f);
        states = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }
    IEnumerator PlayerFireBlast()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.fireBlastDmg);

        enemyUnit.isOnFire = true;
        enemyFlameIcon.SetActive(true);
        enemyUnit.burningTurnsLeft = 2;
        enemyHud.HpSet(enemyUnit.currentHp);
        encounterTxt.text = "The Fire Blast Was Successful!";

        yield return new WaitForSeconds(2.0f);
        
        if (isDead)
        {
            states = BattleState.WON;
            EndBattle();
        }
        else
        {
            states = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }
    IEnumerator PlayerWindCannon()
    {
        encounterTxt.text = "The Wind Cannon Was Successful!";
        enemyWindIcon.SetActive(true);
        enemyUnit.isBlownAway = true;

        yield return new WaitForSeconds(2.0f);

        states = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }
    IEnumerator PlayerIcePistol()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.icePistolDmg);

        enemyUnit.isFrozen = true;
        enemyIceIcon.SetActive(true);
        enemyUnit.freezingTurnsLeft = 2;
        enemyHud.HpSet(enemyUnit.currentHp);
        encounterTxt.text = "The Ice Pistol Was Successfull!";

        yield return new WaitForSeconds(2.0f);

        if (isDead)
        {
            states = BattleState.WON;
            EndBattle();
        }
        else
        {
            states = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyFireBlast()
    {

        bool isDead = playerUnit.TakeDamage(enemyUnit.fireBlastDmg);

        playerUnit.isOnFire = true;
        playerFlameIcon.SetActive(true);
        playerUnit.burningTurnsLeft = 2;
        playerHud.HpSet(playerUnit.currentHp);
        encounterTxt.text = "The Fire Blast Was Successful!";

        yield return new WaitForSeconds(2.0f);

        if (isDead)
        {
            states = BattleState.LOST;
            EndBattle();
        }
    }
    IEnumerator EnemyWindCannon()
    {
        encounterTxt.text = "The Enemy Used Wind Cannon Successfully!";
        playerWindIcon.SetActive(true);
        playerUnit.isBlownAway = true;

        yield return new WaitForSeconds(2.0f);
    }
    IEnumerator EnemyIcePistol()
    {
        bool isDead = playerUnit.TakeDamage(enemyUnit.icePistolDmg);

        playerUnit.isFrozen = true;
        playerIceIcon.SetActive(true);
        playerUnit.freezingTurnsLeft = 2;
        playerHud.HpSet(playerUnit.currentHp);
        encounterTxt.text = "The Enemy Used Ice Pistol Successfully!";

        yield return new WaitForSeconds(2.0f);

        if (isDead)
        {
            states = BattleState.LOST;
            EndBattle();
        }
    }

    // Enemy Behavior Coroutines
    IEnumerator EnemyTurn()
    {
        // 1. Pre Round - Enemy Status Check
        if (enemyUnit.isBlownAway) // no damage, but cant attack for one turn
        {
            encounterTxt.text = enemyUnit.Unitname + " Has Being Blown Over, They Cannot Attack";
            enemyUnit.isBlownAway = false;
            enemyWindIcon.SetActive(false);
            // 3. Changeing to players turn
            states = BattleState.PLAYERTURN;
            PlayerTurn();
        }
        if (enemyUnit.isOnFire) // where the enemy takes damage from being on fire
            {
                encounterTxt.text = enemyUnit.Unitname + " is being burnt by fire";
                enemyUnit.TakeDamage(playerUnit.burningDmg);
                if (enemyUnit.burningTurnsLeft > 0)
                {
                    enemyUnit.burningTurnsLeft--;
                    if (enemyUnit.burningTurnsLeft == 0)
                    {
                        enemyUnit.isOnFire = false;
                        enemyFlameIcon.SetActive(false);
                    }
                }
            }
        if (enemyUnit.isFrozen) // where the enemy takes damage from being frozen
                {
                    encounterTxt.text = enemyUnit.Unitname + " is being Frozen By Ice";
                    enemyUnit.TakeDamage(playerUnit.freezingDmg);

                    yield return new WaitForSeconds(2.0f);

                    if (enemyUnit.freezingTurnsLeft > 0)
                    {
                        enemyUnit.freezingTurnsLeft--;
                        if (enemyUnit.freezingTurnsLeft == 0)
                        {
                            enemyUnit.isFrozen = false;
                            enemyIceIcon.SetActive(false);
                        }
                    }
                }

        // 2. Main Round - Enemy Logic

        // --Attack Section--
        //CHECKLIST:

        // Flee
        //1.if No Mana && Hp lower then 5 ? Attempt to flee : move to Heal Section

        // Need Heal?
        //2.check to see if hp is above 5 ?
        //  check to see if enemy has enough mana to use Heal ? Heal : move to Abilities Section

        // Abilities Available?
        //3.check to see if enemy has enough mana to use abilities ? use a random ability
        //  : move to Attack Section

        // Attack or Defend
        //4.if health is lower then 50% ? randomize Attack and Defence
        //  : Attack


        //-Attack-
        encounterTxt.text = enemyUnit.Unitname + " Attacks";
        yield return new WaitForSeconds(1.0f);
        bool isDead = playerUnit.TakeDamage(enemyUnit.dmg);
        playerHud.HpSet(playerUnit.currentHp);
        yield return new WaitForSeconds(1.0f);

        //-Heal-
        //add logic for if Hp is getting low, to use heal
        enemyUnit.Heal(25);
        enemyHud.HpSet(enemyUnit.currentHp);
        encounterTxt.text = "The Enemy Healed!";
        yield return new WaitForSeconds(2.0f);

        //-Defend-
        // add logic for random use of defend
        enemyUnit.isDefending = true;
        encounterTxt.text = "Enemy Gets Into A Defencive Position!";
        yield return new WaitForSeconds(2.0f);

        //--Abilities--

        //-Fire Blast-
        StartCoroutine(EnemyFireBlast());

        //Wind Blast
        StartCoroutine(EnemyWindCannon());

        //Ice Pistol
        StartCoroutine(EnemyIcePistol());

        encounterTxt.text = "Please Choose An Action:";

        if (isDead)
        {
            states = BattleState.LOST;
            EndBattle();
        }
        



        
    }

    void PlayerTurn()
    {
        encounterTxt.text = "Please Choose An Action:";
    }

    void EndBattle()
    {
        if (states == BattleState.WON)
        {
            encounterTxt.text = "You Have Defeated The Enemy";
        }
        else if (states == BattleState.LOST)
        {
            encounterTxt.text = "The Enemy Has Defeated You";
        }
    }


    // Action Buttons
    public void OnAttackButton()
    {
        if (states != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }
    public void OnHealButton()
    {
        if (states != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerHeal());
    }
    public void OnDefendButton()
    {
        if (states != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerDefend());
    }

    // Ability Buttons
    public void OnFireBlastButton()
    {
        if (states != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerFireBlast());
    }
    public void OnWindCannonButton()
    {
        if (states != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerWindCannon());
    }
    public void OnIcePistolButton()
    {
        if (states != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerIcePistol());
    }

}
