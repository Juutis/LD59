using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ammoCount;
    [SerializeField]
    private bool infinite;
    [SerializeField]
    private GunType gunType;
    [SerializeField]
    private Image infinityIcon;
    [SerializeField]
    private Image gunImage;

    [SerializeField]
    private Sprite disabledGun;
    [SerializeField]
    private Sprite activeGun;
    [SerializeField]
    private Sprite normalGun;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        infinityIcon.gameObject.SetActive(infinite);
        ammoCount.gameObject.SetActive(!infinite);

    }

    // Update is called once per frame
    void Update()
    {
        int ammo = GameManager.instance.GetCurrentAmmos()[(int)gunType];
        bool isActive = GameManager.instance.GetActiveGun() == (int)gunType;
        ammoCount.text = ammo.ToString();
        
        if (ammo <= 0 && gunType != GunType.Pistol)
        {
            gunImage.sprite = disabledGun;
        }
        else if (isActive)
        {
            gunImage.sprite = activeGun;
        }
        else
        {
            gunImage.sprite = normalGun;
        }
    }
}
