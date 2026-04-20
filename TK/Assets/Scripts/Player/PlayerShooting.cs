using System.Linq;
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
    [SerializeField]
    private Pistol gun5; // laser gun

    private InputAction selectGun1;
    private InputAction selectGun2;
    private InputAction selectGun3;
    private InputAction selectGun4;
    private InputAction selectGun5;

    private int selectedGun = 0;

    private int[] ammos;
    
    [SerializeField]
    private AudioClip pickupSound;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private SpriteRenderer rend;

    [SerializeField]
    private Sprite gunSprite;

    [SerializeField]
    private Sprite pistolSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectGun1 = InputSystem.actions.FindAction("SelectGun1");
        selectGun2 = InputSystem.actions.FindAction("SelectGun2");
        selectGun3 = InputSystem.actions.FindAction("SelectGun3");
        selectGun4 = InputSystem.actions.FindAction("SelectGun4");
        selectGun5 = InputSystem.actions.FindAction("SelectGun5");
        shootAction = InputSystem.actions.FindAction("Attack");

        ammos = GameManager.instance.GetLevelStartAmmos().ToList().ToArray();
        GameManager.instance.SetPlayerShooting(this);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        
        selectedGun = GameManager.instance.SelectedGun;
        if (ammos[selectedGun] == 0)
        {
            selectedGun = 0;
        }
        if (selectedGun == 0 || selectedGun == 4)
        {
            rend.sprite = pistolSprite;
        }
        else
        {
            rend.sprite = gunSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (selectGun1.WasPerformedThisFrame())
        {
            Debug.Log("Gun 1 selected");
            selectedGun = (int)GunType.Pistol;
            GameManager.instance.SelectedGun = selectedGun;
            rend.sprite = pistolSprite;
        }
        else if (selectGun2.WasPerformedThisFrame() && ammos[1] > 0)
        {
            Debug.Log("Gun 2 selected");
            selectedGun = (int)GunType.Shotgun;
            GameManager.instance.SelectedGun = selectedGun;
            rend.sprite = gunSprite;
        }
        else if (selectGun3.WasPerformedThisFrame() && ammos[2] > 0)
        {
            Debug.Log("Gun 3 selected");
            selectedGun = (int)GunType.MachineGun;
            GameManager.instance.SelectedGun = selectedGun;
            rend.sprite = gunSprite;
        }
        else if (selectGun4.WasPerformedThisFrame() && ammos[3] > 0)
        {
            Debug.Log("Gun 4 selected");
            selectedGun = (int)GunType.Sniper;
            GameManager.instance.SelectedGun = selectedGun;
            rend.sprite = gunSprite;
        }
        else if (selectGun5.WasPerformedThisFrame() && ammos[4] > 0)
        {
            Debug.Log("Gun 5 selected");
            selectedGun = (int)GunType.Laser;
            GameManager.instance.SelectedGun = selectedGun;
            rend.sprite = pistolSprite;
        }

        bool hasAmmo = ammos[selectedGun] > 0 || selectedGun == 0;

        if (shootAction.IsPressed() && hasAmmo)
        {
            bool didShoot = false;

            switch(selectedGun)
            {
                case (int)GunType.Pistol:
                    didShoot = gun1.Shoot();
                    break;
                case (int)GunType.Shotgun:
                    didShoot = gun2.Shoot();
                    break;
                case (int)GunType.MachineGun:
                    didShoot = gun3.Shoot();
                    break;
                case (int)GunType.Sniper:
                    didShoot = gun4.Shoot();
                    break;
                case (int)GunType.Laser:
                    didShoot = gun5.Shoot();
                    break;
            }

            if (didShoot)
            {
                ammos[selectedGun] = Mathf.Max(0, ammos[selectedGun] - 1);
            }
        }
    }

    public int[] GetAmmos()
    {
        return ammos;
    }

    public int GetActiveGun()
    {
        return selectedGun;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.tag == "Crate")
        {
            if (other.gameObject.TryGetComponent(out GunCrate crate))
            {
                int gun = (int)crate.GunType;
                ammos[gun] += crate.AmmoAmount;

                Destroy(other.gameObject);
                audioSource.PlayOneShot(pickupSound);
            }
        }
    }
}

public enum GunType
{
    Pistol = 0,
    Shotgun = 1,
    MachineGun = 2,
    Sniper = 3,
    Laser = 4
}