using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroCardUI : MonoBehaviour
{
    public Image Ilust;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI MaxHealth;

    public void SetHeroData(HeroData heroData)
    {
        Ilust.sprite = heroData.Sprite;
        Name.text = heroData.Name;
        Description.text = heroData.Description;
        MaxHealth.text = heroData.MaxHealth.ToString();
    }
}
