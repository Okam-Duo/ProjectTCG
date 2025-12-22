using TMPro;
using UnityEngine;

public class CardPack_Information_UI : MonoBehaviour
{
    public void ShowCardPackInformation(int id)
    {
        gameObject.SetActive(true);
        CardData CardData = StaticDataManager.GetCardData(id);
        informationtext.text = CardData.Name;
    }

    public TextMeshProUGUI informationtext;

}
