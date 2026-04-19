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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        infinityIcon.gameObject.SetActive(infinite);
        ammoCount.gameObject.SetActive(!infinite);
    }

    // Update is called once per frame
    void Update()
    {
        ammoCount.text = GameManager.instance.GetCurrentAmmos()[(int)gunType].ToString();
    }
}
