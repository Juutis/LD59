using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> alternatives;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var i = Random.Range(0, alternatives.Count);
        foreach(var alternative in alternatives)
        {
            alternative.SetActive(false);
        }
        alternatives[i].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
