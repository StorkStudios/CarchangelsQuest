using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputAdapter : MonoBehaviour
{
    [SerializeField]
    private CarEngine carEngine;

    private GameEndManager gameEndManager;
    private PauseHandler pauseHandler;

    private void Start()
    {
        gameEndManager = GameEndManager.Instance;
        pauseHandler = PauseHandler.Instance;
    }

    public void AccelerationInput(InputAction.CallbackContext context)
    {
        if (pauseHandler.IsPaused || gameEndManager.HasGameEnded)
        {
            carEngine.gasPedal.Value = 0;
            return;
        }

        carEngine.gasPedal.Value = context.ReadValue<float>();
    }

    public void SteeringInput(InputAction.CallbackContext context)
    {
        if (pauseHandler.IsPaused || gameEndManager.HasGameEnded)
        {
            return;
        }

        carEngine.steeringWheel.Value = -context.ReadValue<float>();
    }

    public void HandbreakInput(InputAction.CallbackContext context)
    {
        if (pauseHandler.IsPaused || gameEndManager.HasGameEnded)
        {
            carEngine.handbreak.Value = false;
            return;
        }

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
        if (pauseHandler.IsPaused || gameEndManager.HasGameEnded)
        {
            carEngine.horn.Value = false;
            return;
        }

        if (context.started)
        {
            carEngine.horn.Value = true;
        }
        else if (context.canceled)
        {
            carEngine.horn.Value = false;
        }
    }

    public void PauseInput(InputAction.CallbackContext context)
    {
        if (gameEndManager.HasGameEnded)
        {
            return;
        }

        if (context.started)
        {
            pauseHandler.TogglePause();
        }
    }
}
