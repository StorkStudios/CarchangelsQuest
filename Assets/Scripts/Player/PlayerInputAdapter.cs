using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputAdapter : MonoBehaviour
{
    [SerializeField]
    private CarEngine carEngine;

    public void AccelerationInput(InputAction.CallbackContext context)
    {
        carEngine.gasPedal.Value = context.ReadValue<float>();
    }

    public void SteeringInput(InputAction.CallbackContext context)
    {
        carEngine.steeringWheel.Value = -context.ReadValue<float>();
    }

    public void HandbreakInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            carEngine.handbreak.Value = true;
        }
        else if (context.canceled)
        {
            carEngine.handbreak.Value = false;
        }
    }

    public void CarHornInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            carEngine.horn.Value = true;
        }
        else if (context.canceled)
        {
            carEngine.horn.Value = false;
        }
    }
}
