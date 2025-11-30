using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroUI : MonoBehaviour
{
    [Header("컴포넌트 참조")]
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _healthText;


    private Vector3 _originPosition;


    private void Update()
    {
        transform.position = _originPosition;
    }


    public void SetData(HeroData heroData)
    {
        _image.sprite = heroData.Sprite;
        _healthText.text = heroData.MaxHealth.ToString();
    }

    public void SetOriginPosition(Vector3 position)
    {
        _originPosition = position;
    }

    public void SetHealth(int newHealth, bool enableAnimationByDamageValue = false)
    {
        _healthText.text = newHealth.ToString();

        if (newHealth <= 0)
        {
            HandleDieAnimation();
        }
    }

    public void HandleDieAnimation()
    {
        gameObject.SetActive(false);
    }
}
