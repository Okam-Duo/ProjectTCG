using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;
public class CardUI:MonoBehaviour
{
    public Image Ilust;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public Image Rarity;
    public Sprite commonSprite;
    public Sprite rareSprite;
    public Sprite epicSprite;
    public Sprite legendarySrpite;

    public void SetHeroData(CardData cardData)
    {
        Ilust.sprite = cardData.Sprite;
        Name.text = cardData.Name;
        Description.text = cardData.Description;
        switch (cardData.Rarity)
        {
            case CardRarity.Common:
                Rarity.sprite = commonSprite;
                break;
            case CardRarity.Rare:
                Rarity.sprite = rareSprite;
                break;
            case CardRarity.Epic: 
                Rarity.sprite = epicSprite;
                break;
            case CardRarity.Legendary:
                Rarity.sprite = legendarySrpite;
                break;
        }
    }
}
