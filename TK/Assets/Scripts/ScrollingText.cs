using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ScrollingText : MonoBehaviour
{
    private InputAction nextLevelAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextLevelAction = InputSystem.actions.FindAction("NextLevel");
    }

    // Update is called once per frame
    void Update()
    {
        var speed = 100.0f;
        if (nextLevelAction.IsPressed())
        {
            speed = speed * 20;
        }
        transform.position += Vector3.up * speed * Time.deltaTime;
        if (transform.position.y > 400)
        {
            SceneManager.LoadScene(1);
        }
    }
}
