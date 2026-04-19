using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    private InputAction shootAction;
    [SerializeField]
    private Pistol gun1;
    [SerializeField]
    private Shotgun gun2;
    [SerializeField]
    private MachineGun gun3;
    [SerializeField]
    private Pistol gun4; // sniper rifle

    private InputAction selectGun1;
    private InputAction selectGun2;
    private InputAction selectGun3;
    private InputAction selectGun4;

    private bool hasGun1 = true;
    private bool hasGun2 = true;
    private bool hasGun3 = true;
    private bool hasGun4 = true;

    private int selectedGun = 0;

    private int[] ammos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectGun1 = InputSystem.actions.FindAction("SelectGun1");
        selectGun2 = InputSystem.actions.FindAction("SelectGun2");
        selectGun3 = InputSystem.actions.FindAction("SelectGun3");
        selectGun4 = InputSystem.actions.FindAction("SelectGun4");
        shootAction = InputSystem.actions.FindAction("Attack");
        ammos = new int[4] { 0, 10, 30, 5 };
    }

    // Update is called once per frame
    void Update()
    {
        if (hasGun1 && selectGun1.WasPerformedThisFrame())
        {
            Debug.Log("Gun 1 selected");
            selectedGun = 0;
        }
        else if (hasGun2 && selectGun2.WasPerformedThisFrame() && ammos[1] > 0)
        {
            Debug.Log("Gun 2 selected");
            selectedGun = 1;
        }
        else if (hasGun3 && selectGun3.WasPerformedThisFrame() && ammos[2] > 0)
        {
            Debug.Log("Gun 3 selected");
            selectedGun = 2;
        }
        else if (hasGun4 && selectGun4.WasPerformedThisFrame() && ammos[3] > 0)
        {
            Debug.Log("Gun 4 selected");
            selectedGun = 3;
        }

        bool hasAmmo = ammos[selectedGun] > 0 || selectedGun == 0;

        if (shootAction.IsPressed() && hasAmmo)
        {
            bool didShoot = false;

            switch(selectedGun)
            {
                case 0:
                    didShoot = gun1.Shoot();
                    break;
                case 1:
                    didShoot = gun2.Shoot();
                    break;
                case 2:
                    didShoot = gun3.Shoot();
                    break;
                case 3:
                    didShoot = gun4.Shoot();
                    break;
            }

            if (didShoot)
            {
                ammos[selectedGun] = Mathf.Max(0, ammos[selectedGun] - 1);
            }
        }
    }
}
