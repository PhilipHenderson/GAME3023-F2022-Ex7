using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Unit Info")]
    public string Unitname;
    public int Unitlvl;
    public int dmg;
    public int defendedDmg = 2;
    public int maxHp;
    public int currentHp;
    public int maxMp;
    public int currentMp;

    [Header("Fire Info")]
    public int fireBlastDmg;
    public int burningDmg;
    public int burningTurnsLeft;
    public int fireMpCost;

    [Header("Ice Info")]
    public int icePistolDmg;
    public int freezingDmg;
    public int freezingTurnsLeft;
    public int iceMpCost;

    [Header("Wind Info")]
    public int BlownAwayTurnsLeft;
    public int windMpCost;

    [Header("Heal Info")]
    public int healMpCost;

    [Header("Bool Checks")]
    public bool isDefending = false;
    public bool isOnFire = false;
    public bool isBlownAway = false;
    public bool isFrozen = false;

    public bool TakeDamage(int dmg)
    {
        if (isDefending)
        {
            currentHp -= dmg / defendedDmg;
            isDefending = false;
            if (currentHp <= 0) return true;
            else return false;            
        }
        else
        {
            currentHp -= dmg;
            if (currentHp <= 0) return true;
            else return false;
        }
    }

    public void Heal(int amount)
    {
        currentHp += amount;
        if(currentHp > maxHp)
            currentHp = maxHp;
    }
}
