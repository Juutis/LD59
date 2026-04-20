using UnityEngine;

public class AntennaSwitch : MonoBehaviour
{
    private static int antennaCount;

    [SerializeField]
    private Sprite activated;
    private GameObject player;
    private SpriteRenderer rend;

    [SerializeField]
    private AntennaAnimator anim;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rend = GetComponent<SpriteRenderer>();
        AntennaManager.Instance.Antennas.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        var selfPos = transform.position;
        selfPos.y = 0;
        var playerPos = player.transform.position;
        playerPos.y = 0;
        if (Vector3.Distance(selfPos, playerPos) < 0.5f)
        {
            AntennaManager.Instance.AntennaActivated(this);
            rend.sprite = activated;

            if (anim != null)
            {
                anim.enabled = true;
            }
        }
    }
}
