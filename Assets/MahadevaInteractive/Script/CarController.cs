using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarController : MonoBehaviour
{
    [Header("Car Parts")]
    public Rigidbody2D BackTire;
    public Rigidbody2D FrontTire;
    public Rigidbody2D CarBody;
    public Rigidbody2D Head; // 👈 Assign your BikeBody (head) here

    [Header("Movement Settings")]
    public float Speed = 300f;
    public float WheelieTorque = 300f;
    public float BrakeForce = 1000f;

    [Header("Fuel Settings")]
    public float maxFuel = 100f;
    public float fuel;
    public float fuelDrainRate = 3f;

    [Header("Coin Settings")]
    public int coins = 0;

    [Header("UI References")]
    public Slider fuelBar;
    public TMP_Text coinText;
    public GameObject gameOverUI; // 👈 Assign your Game Over UI Panel here

    private float movementInput;
    private float tiltInput;
    private bool isBraking;
    private bool isGameOver;

    void Start()
    {
        fuel = maxFuel;
        UpdateFuelUI();
        UpdateCoinUI();
    }

    void Update()
    {
        if (isGameOver || fuel <= 0)
        {
            movementInput = 0;
            tiltInput = 0;
            isBraking = true;
            return;
        }

        movementInput = Input.GetAxis("Vertical");
        tiltInput = Input.GetAxis("Horizontal");
        isBraking = Input.GetKey(KeyCode.Space);
    }

    void FixedUpdate()
    {
        if (isGameOver || fuel <= 0) return;

        BackTire.AddTorque(-movementInput * Speed * Time.fixedDeltaTime);
        FrontTire.AddTorque(-movementInput * Speed * Time.fixedDeltaTime);
        CarBody.AddTorque(-tiltInput * WheelieTorque * Time.fixedDeltaTime);

        if (isBraking)
        {
            BackTire.angularVelocity = Mathf.Lerp(BackTire.angularVelocity, 0, 0.2f);
            FrontTire.angularVelocity = Mathf.Lerp(FrontTire.angularVelocity, 0, 0.2f);
        }

        if (Mathf.Abs(movementInput) > 0.1f)
        {
            fuel -= fuelDrainRate * Time.fixedDeltaTime;
            fuel = Mathf.Clamp(fuel, 0, maxFuel);
            UpdateFuelUI();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isGameOver) return;

        if (other.CompareTag("Coin"))
        {
            coins++;
            UpdateCoinUI();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Fuel"))
        {
            fuel += 25f;
            fuel = Mathf.Clamp(fuel, 0, maxFuel);
            UpdateFuelUI();
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 👇 If the Head hits the ground, game over
        if (collision.collider.CompareTag("Ground") && collision.otherCollider == Head.GetComponent<Collider2D>())
        {
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        isGameOver = true;

        // Stop all movement instantly
        BackTire.angularVelocity = 0;
        FrontTire.angularVelocity = 0;
        CarBody.angularVelocity = 0;
        CarBody.linearVelocity = Vector2.zero;

        if (gameOverUI) gameOverUI.SetActive(true);

        Debug.Log("💀 GAME OVER: Head hit the ground!");
    }

    private void UpdateFuelUI()
    {
        if (fuelBar) fuelBar.value = fuel / maxFuel;
    }

    private void UpdateCoinUI()
    {
        if (coinText) coinText.text = coins.ToString();
    }
}
