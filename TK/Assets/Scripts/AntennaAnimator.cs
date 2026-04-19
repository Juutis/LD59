using UnityEngine;

public class AntennaAnimator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, 10.0f * Time.deltaTime);
    }
}
