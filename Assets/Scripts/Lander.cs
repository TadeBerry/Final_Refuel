using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    private bool isImmortal;
    [SerializeField] private float spawnProtectionDuration = 1.5f;
    private SpriteRenderer spriteRenderer;

    private const float GRAVITY_NORMAL = 0.7f;

    public static Lander Instance { get; private set;}



    public event EventHandler OnUpForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler<OnCoinPickupEventArgs> OnCoinPickup;
    public class OnCoinPickupEventArgs : EventArgs
    {
        public CoinPickup coinPickup;
    }

    public event EventHandler<OnFuelPickupEventArgs> OnFuelPickup;
    public class OnFuelPickupEventArgs : EventArgs
    {
        public FuelPickup fuelPickup;
    }

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public event EventHandler<OnLandedEventArgs> OnLanded;
    public class OnLandedEventArgs : EventArgs
    {
        public LandingType landingType;
        public int score;
        public float dotVector;
        public float LandingSpeed;
        public float scoreMultiplier;
    }

    public enum LandingType
    {
        Success,
        WrongLandingArea,
        TooSteepAngle,
        TooFastLanding,
    }

    public enum State
    {
        WaitingToStart,
        Normal,
        GameOver,
    }

    private Rigidbody2D landerRigidbody2D;
    private float fuelAmount = 10f;
    private float fuelAmountMax = 10f;
    private State state;


public void TriggerSpawnProtection()
{
    StopAllCoroutines();
    StartCoroutine(SpawnProtectionRoutine());
}

private System.Collections.IEnumerator SpawnProtectionRoutine()
{
    isImmortal = true;
    float timer = 0;
    float blinkSpeed = 0.1f;

    while (timer < spawnProtectionDuration)
    {
        Color tempColor = spriteRenderer.color;
        tempColor.a = (tempColor.a == 1f) ? 0.2f : 1f; 
        spriteRenderer.color = tempColor;

        yield return new WaitForSeconds(blinkSpeed);
        timer += blinkSpeed;
    }

    Color finalColor = spriteRenderer.color;
    finalColor.a = 1f;
    spriteRenderer.color = finalColor;
    
    isImmortal = false;
}


    private void Awake()
    {
        Instance = this;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();  //for blinking
        fuelAmount = fuelAmountMax;
        state = State.WaitingToStart;

        landerRigidbody2D = GetComponent<Rigidbody2D>();
        landerRigidbody2D.gravityScale = 0f;
    }


    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);

        switch (state)
        {
            default:
            case State.WaitingToStart:
            if (GameInput.Instance.IsUpActionPressed() ||
            GameInput.Instance.IsLeftActionPressed() || 
            GameInput.Instance.IsRightActionPressed() ||
            GameInput.Instance.GetMovementInputVector2() != Vector2.zero)
        {
            //Pressing any input
            landerRigidbody2D.gravityScale = GRAVITY_NORMAL;
            SetState(State.Normal);
        }
            break;
            case State.Normal:

            if (fuelAmount <= 0f)
        {
            return;
        }

        if (GameInput.Instance.IsUpActionPressed() ||
            GameInput.Instance.IsLeftActionPressed() || 
            GameInput.Instance.IsRightActionPressed() ||
            GameInput.Instance.GetMovementInputVector2() != Vector2.zero)
        {
            //Pressing any input
            ConsumeFuel();
        }

        float gamepadDeadzone = 0.4f;
        if (GameInput.Instance.IsUpActionPressed() || GameInput.Instance.GetMovementInputVector2().y > gamepadDeadzone)
            {
                float force = 700f;
                landerRigidbody2D.AddForce(force * transform.up * Time.deltaTime);
                OnUpForce?.Invoke(this, EventArgs.Empty);
            }
        if (GameInput.Instance.IsLeftActionPressed() || GameInput.Instance.GetMovementInputVector2().x < -gamepadDeadzone)
            {
                float turnSpeed = +100f;
                landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
                OnLeftForce?.Invoke(this, EventArgs.Empty);
            }
        if (GameInput.Instance.IsRightActionPressed() || GameInput.Instance.GetMovementInputVector2().x > gamepadDeadzone)
            {
                float turnSpeed = -100f;
                landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
                OnRightForce?.Invoke(this, EventArgs.Empty);
            }
            break;
        case State.GameOver:
            break;
        }

        
    }
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        {
        if (isImmortal) return;
        }
        if (collision2D.gameObject.TryGetComponent(out StartPad startPad))
        {
        return;
        }


        if (!collision2D.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            Debug.Log("Crashed on the terrain!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
        { 
            landingType = LandingType.WrongLandingArea,
            dotVector = 0f,
            LandingSpeed = 0f,
            scoreMultiplier = 0,
            score = 0,
        });
        SetState(State.GameOver);
        return;
        }
        float softLandingVelocityMagnitude = 4f;
        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;
        if (relativeVelocityMagnitude > softLandingVelocityMagnitude)
        {
            //Landed too hard!
            Debug.Log("Landed too hard!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
        { 
            landingType = LandingType.TooFastLanding,
            dotVector = 0f,
            LandingSpeed = relativeVelocityMagnitude,
            scoreMultiplier = 0,
            score = 0,
        });
        SetState(State.GameOver);
            return;
        }

        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minDotVector = 0.90f;
        if (dotVector < minDotVector)
        {
            //Landed on a too steep angle!
            Debug.Log("Landed at a bad angle!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
        { 
            landingType = LandingType.TooSteepAngle,
            dotVector = dotVector,
            LandingSpeed = relativeVelocityMagnitude,
            scoreMultiplier = 0,
            score = 0,
        });
        SetState(State.GameOver);
            return;
        }

        Debug.Log("Successful landing!");

        float maxScoreAmountLandingAngle = 100;
        float scoreDotVectorMiltiplier = 10f;
        float landingAngleScore = maxScoreAmountLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMiltiplier * maxScoreAmountLandingAngle;

        float MaxScoreAmountLandingSpeed = 100;
        float landingSpeedScore = (softLandingVelocityMagnitude - relativeVelocityMagnitude) * MaxScoreAmountLandingSpeed;

        Debug.Log("landingAngleScore: " + landingAngleScore);
        Debug.Log("landingspeedScore: " + landingSpeedScore);

        int score = Mathf.RoundToInt((landingAngleScore + landingSpeedScore) * landingPad.GetScoreMultiplier());

        Debug.Log("score: " + score);
        OnLanded?.Invoke(this, new OnLandedEventArgs
        { 
            landingType = LandingType.Success,
            dotVector = dotVector,
            LandingSpeed = relativeVelocityMagnitude,
            scoreMultiplier = landingPad.GetScoreMultiplier(),
            score = score,
        });
        SetState(State.GameOver);
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.TryGetComponent(out FuelPickup fuelPickup))
        {
            float addFuelAmount = 10f;
            fuelAmount += addFuelAmount;
            if (fuelAmount > fuelAmountMax)
            {
                fuelAmount = fuelAmountMax;
            }
            //OnFuelPickup?.Invoke(this, EventArgs.Empty);
            OnFuelPickup?.Invoke(this,new OnFuelPickupEventArgs {fuelPickup = fuelPickup});
            fuelPickup.DestroySelf();
        }

        if (collider2D.gameObject.TryGetComponent(out CoinPickup coinPickup))
        {
            //OnCoinPickup?.Invoke(this, EventArgs.Empty);
            OnCoinPickup?.Invoke(this,new OnCoinPickupEventArgs {coinPickup = coinPickup});
            coinPickup.DestroySelf();
        }
    }
    
    private void SetState(State state)
    {
        this.state = state;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        { 
            state = state,
        });
    }

    private void ConsumeFuel()
    {
        float fuelConsumptionAmount = 0.5f;
        fuelAmount -= fuelConsumptionAmount * Time.deltaTime;
    }

    public float GetFuel()
    {
        return fuelAmount;
    }

    public float GetFuelAmountNormalized()
    {
        return fuelAmount / fuelAmountMax;
    }

    public float GetSpeedX()
    {
        return landerRigidbody2D.linearVelocityX;
    }

    public float GetSpeedY()
    {
        return landerRigidbody2D.linearVelocityY;
    }

    public State GetState() 
    {
    return state; 
    }

    public void TriggerBulletCrash()
    {
    if (state == State.GameOver) return; //don't die twice

    Debug.Log("Lander shot down!");

    OnLanded?.Invoke(this, new OnLandedEventArgs
    {
        landingType = LandingType.WrongLandingArea,
        dotVector = 0f,
        LandingSpeed = 0f,
        scoreMultiplier = 0,
        score = 0,
    });

    SetState(State.GameOver); 
    }

    public void ResetLander() 
{
    state = State.WaitingToStart; 
    fuelAmount = fuelAmountMax;

    if (landerRigidbody2D != null)
    {
        landerRigidbody2D.gravityScale = 0f;
        landerRigidbody2D.linearVelocity = Vector2.zero;
        landerRigidbody2D.angularVelocity = 0f;
    }
}

}