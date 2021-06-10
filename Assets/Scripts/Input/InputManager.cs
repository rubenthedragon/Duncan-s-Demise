using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class InputManager : MonoBehaviour
{

    [field: SerializeField] public Keymapping keymapping { get; set; }
    public static InputManager Instance { get; set; } = null;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (InputManager.Instance == null)
        {
            InputManager.Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private delegate bool KeyEvent(KeyCode keyCode);
    public bool CheckKeyPressed(List<KeyCode> keys, KeyEventType keyEventType = KeyEventType.Down)
    {
        KeyEvent keyEvent = null;
        switch (keyEventType)
        {
            case (KeyEventType.Hold):
                keyEvent = Input.GetKey;
                break;
            case (KeyEventType.Down):
                keyEvent = Input.GetKeyDown;
                break;
            case (KeyEventType.Up):
                keyEvent = Input.GetKeyUp;
                break;
        }
        return keys.Any(k => keyEvent(k));
    }
}