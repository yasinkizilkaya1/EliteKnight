using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Text NameText;
    public Text HealthText;
    public Text SpeedText;
    public Text EnergyText;
    public Text DefanceText;
    public Text PowerText;
    public Toggle toogle;

    public void UpEnumdateData(CharacterData data)
    {
        name = data.Name;
        NameText.text = data.Name;
        HealthText.text = data.Health.ToString();
        SpeedText.text = data.Speed.ToString();
        EnergyText.text = data.Energy.ToString();
        DefanceText.text = data.Defence.ToString();
        PowerText.text = data.Power.ToString();
    }
}