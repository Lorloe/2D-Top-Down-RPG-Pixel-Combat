using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    private Image image;

    private void Awake() {
        image = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false; // Hide the cursor by disable the operating system cursor
        if (Application.isPlaying)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined; // Lock the cursor within the game window
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // transform.position = cursorPos;
        Vector2 cursorPos = Input.mousePosition;
        image.rectTransform.position = cursorPos;

        // if (!Application.isPlaying) { return; }

        // Cursor.visible = false;
    }
}
