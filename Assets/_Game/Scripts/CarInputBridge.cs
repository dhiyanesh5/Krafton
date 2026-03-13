using UnityEngine;

[RequireComponent(typeof(PrometeoCarController))]
[RequireComponent(typeof(SwipeInputHandler))]
public class CarInputBridge : MonoBehaviour
{
    private PrometeoCarController car;
    private SwipeInputHandler swipeInput;

    private void Awake()
    {
        car = GetComponent<PrometeoCarController>();
        swipeInput = GetComponent<SwipeInputHandler>();
        car.useTouchControls = false;
    }

    private void Update()
    {
        if (car == null || swipeInput == null) return;
        HandleThrottle();
        HandleSteering();
    }

    private void HandleThrottle()
    {
        if (swipeInput.IsTouching)
        {
            CancelInvoke("DecelerateCarProxy");
            car.deceleratingCar = false;
            car.GoForward();
        }
        else
        {
            car.ThrottleOff();
            if (!car.deceleratingCar)
            {
                InvokeRepeating("DecelerateCarProxy", 0f, 0.1f);
                car.deceleratingCar = true;
            }
        }
    }

    private void HandleSteering()
    {
        if (!swipeInput.IsTouching)
        {
            car.ResetSteeringAngle();
            return;
        }

        float turnAngle = Vector3.SignedAngle(
            car.transform.forward,
            swipeInput.joystickDirection,
            Vector3.up
        );

        if (turnAngle > 5f)
            car.TurnRight();
        else if (turnAngle < -5f)
            car.TurnLeft();
        else
            car.ResetSteeringAngle();
    }

    private void DecelerateCarProxy()
    {
        car.DecelerateCar();
    }
}