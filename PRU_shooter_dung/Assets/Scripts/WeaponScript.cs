using UnityEngine;
using UnityEngine.UI;

public class WeaponScript : MonoBehaviour
{
    private float rotateOffset = 180f;
    [SerializeField] private Image bulletBar;
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] public float shotDelay = 0.15f;
    private SpriteRenderer spriteRenderer;
    private float nextShot;
    [SerializeField] public int maxAmmo = 15;
    public int currentAmmo;
    private Vector3 originalScale;

    [SerializeField] private float reloadTime = 2f; // Time to reload in seconds
    private float reloadStartTime;
    private bool isReloading = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
        currentAmmo = maxAmmo;
        RenderBulletBar();
    }

    void Update()
    {
        if (isReloading)
        {
            UpdateReloadUI();
            CheckReloadComplete();
            return;
        }

        RotateGun();
        Shoot();

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReload();
        }
    }


    void RotateGun()
    {
        if (Input.mousePosition.x < 0 || Input.mousePosition.y < 0 ||
            Input.mousePosition.x > Screen.width || Input.mousePosition.y > Screen.height)
        {
            return;
        }

        Vector3 displacement = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotateOffset);

        if (angle < -90 || angle > 90)
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        }
        else
        {
            transform.localScale = new Vector3(originalScale.x, -originalScale.y, originalScale.z);
        }
    }


    void Shoot()
    {
        if (Input.GetMouseButton(0) && currentAmmo > 0 && Time.time > nextShot)
        {
            nextShot = Time.time + shotDelay;

            Vector3 bulletDirection = firePos.position;
            // Rotate the fire position along with the gun's rotation
            Vector3 rotatedBulletPosition = RotatePosition(firePos.position, transform.position, transform.rotation);


            Instantiate(bulletPrefabs, firePos.position, firePos.rotation);
            currentAmmo--;
            RenderBulletBar();
        }
    }

    Vector3 RotatePosition(Vector3 position, Vector3 pivot, Quaternion rotation)
    {
        // Calculate the rotated position of firePos relative to the gun's pivot (rotation)
        Vector3 dir = position - pivot;  // Get the direction vector from the pivot (gun) to firePos
        dir = rotation * dir;            // Rotate this direction by the gun's current rotation
        return pivot + dir;              // Return the new position after rotation
    }

    void StartReload()
    {
        if (currentAmmo == maxAmmo || isReloading) return; // No need to reload if already full or reloading

        isReloading = true;
        reloadStartTime = Time.time; // Start reload timer
    }

    void CheckReloadComplete()
    {
        if (Time.time - reloadStartTime >= reloadTime) // If reload time has passed
        {
            currentAmmo = maxAmmo; // Refill ammo
            isReloading = false; // Allow shooting again
            RenderBulletBar();
        }
    }

    void UpdateReloadUI()
    {
        if (bulletBar == null) return;

        float elapsed = Time.time - reloadStartTime;
        bulletBar.fillAmount = Mathf.Lerp(0, 1, elapsed / reloadTime); // Smoothly increase UI fill
    }

    protected void RenderBulletBar()
    {
        if (bulletBar == null) return;

        bulletBar.fillAmount = (float)currentAmmo / maxAmmo; // Correct ammo display
    }

    public bool IsReloading()
    {
        return isReloading; // This tells the CursorManager if the weapon is reloading
    }

}
