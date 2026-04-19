using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI healthAmount;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthAmount.text = ((int)GameManager.instance.GetCurrentHealth()).ToString();
    }
}
