using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CardUI:MonoBehaviour
{
    public Image Ilust;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public Image Rarity;

    public void SetHeroData(CardData cardData)
    {
        Ilust.sprite = cardData.Sprite;
        Name.text = cardData.Name;
        Description.text = cardData.Description;
        //Rarity.sprite = cardData.Rarity;
    }
}
