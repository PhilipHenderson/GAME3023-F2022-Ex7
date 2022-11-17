using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattlePlatform : MonoBehaviour
{
    public TMP_Text Name;
    public TMP_Text level;
    public Slider hpSlider;

    public void HudSet(Unit unit)
    {
        Name.text = unit.Unitname;
        level.text = "Lvl " + unit.Unitlvl;
        hpSlider.maxValue = unit.maxHp;
        hpSlider.value = unit.currentHp;

    }

    public void HpSet(int hp)
    {
        hpSlider.value = hp;
    }


}
