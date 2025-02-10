using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class Movement : MonoBehaviour
{
    void SetDirection(Vector2 direction)
    {
        
    }

    void SetDirection(InputAction.CallbackContext ctx)
    {
        if (ctx.phase ==UnityEngine.InputSystem.InputActionPhase.Performed)
        {
            print ($"asdasdasd {ctx.ReadValue<Vector2>()}");
        }
    }
}

