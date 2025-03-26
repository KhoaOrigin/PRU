using UnityEngine;

public class Level5CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorNormal;
    [SerializeField] private Texture2D cursorShoot;
    [SerializeField] private Texture2D cursorReload;
    private Vector2 hotspot = new Vector2(16, 48);

    private Level5WeaponScript weapon;
    private bool wasReloading = false; // Track previous reload state


    void Start()
    {
        Cursor.SetCursor(cursorNormal, hotspot, CursorMode.Auto);
        weapon = FindAnyObjectByType<Level5WeaponScript>();
    }


    void Update()
    {
        if (weapon == null) return;

        // Check if currently reloading
        if (weapon.IsReloading())
        {
            Cursor.SetCursor(cursorReload, hotspot, CursorMode.Auto);
            wasReloading = true; // Track that we were in reloading state
            return; // Stop further checks while reloading
        }

        // If reloading just finished, update cursor based on whether the shoot button is held
        if (wasReloading)
        {
            wasReloading = false; // Reset flag
            if (Input.GetMouseButton(0))
            {
                Cursor.SetCursor(cursorShoot, hotspot, CursorMode.Auto); // If still shooting, set shoot cursor
            }
            else
            {
                Cursor.SetCursor(cursorNormal, hotspot, CursorMode.Auto); // Otherwise, reset to normal
            }
            return; // Prevent further updates this frame
        }

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(cursorShoot, hotspot, CursorMode.Auto);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(cursorNormal, hotspot, CursorMode.Auto);
        }
    }
}
