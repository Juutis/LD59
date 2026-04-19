using UnityEngine;
using UnityEngine.InputSystem;

public class Crosshair : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 1));
        transform.position = new Vector3(pos.x, Camera.main.transform.position.y - 1, pos.z);
    }
}
