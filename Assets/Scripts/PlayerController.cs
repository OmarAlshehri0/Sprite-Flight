using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float elapsedTime =0f;

    private int timeScore =0;
    private int bonusScore =0;
    private int totalScore= 0;
    private int highScore= 0;
    public float scoreMultiplier = 10f;
    public float thrustForce= 1f;

    private Rigidbody2D rb;
    public UIDocument uiDocument;

    private Label scoreText;
    private Label highScoreText;
    private Button restartButton;

    public GameObject explosionEffect;
    public GameObject borderParent;
    public GameObject boosterFlame;
    public InputAction moveForward;
    public InputAction lookPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        scoreText =uiDocument.rootVisualElement.Q<Label>("ScoreLabel");

        highScoreText =uiDocument.rootVisualElement.Q<Label>("HighScoreLabel");

        restartButton =uiDocument.rootVisualElement.Q<Button>("RestartButton");

        restartButton.style.display =DisplayStyle.None;

        restartButton.clicked += ReloadScene;

        boosterFlame.SetActive(false);
        moveForward.Enable();
        lookPosition.Enable();

        highScore =PlayerPrefs.GetInt("HighScore",0);

        UpdateScoreUI();
    }

    private void Update()
    {
        elapsedTime +=Time.deltaTime;

        timeScore= Mathf.FloorToInt(elapsedTime * scoreMultiplier);

        totalScore =timeScore +bonusScore;

        if (totalScore > highScore)
        {
            highScore =totalScore;

            PlayerPrefs.SetInt(
                "HighScore",
                highScore
            );
        }

        UpdateScoreUI();

        if (moveForward.IsPressed())
        {
            Vector2 pointerPosition=lookPosition.ReadValue<Vector2>();

            Vector3 worldPosition=Camera.main.ScreenToWorldPoint(pointerPosition);

            Vector2 direction =(worldPosition -transform.position).normalized;

            transform.up =direction;

            rb.AddForce(direction * thrustForce);
        }

        if (moveForward.WasPressedThisFrame())
        {
            boosterFlame.SetActive(true);
        }

        else if (moveForward.WasReleasedThisFrame())
        {
            boosterFlame.SetActive(false);
        }
    }

    public void AddBonusScore(int amount)
    {
        bonusScore += amount;
        totalScore = timeScore+ bonusScore;

        if (totalScore> highScore)
        {
            highScore= totalScore;

            PlayerPrefs.SetInt(
                "HighScore",
                highScore
            );
        }

        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreText.text=
            "Score: "+ totalScore;

        highScoreText.text =
            "Top Score: "+ highScore;
    }

    private void OnCollisionEnter2D(
        Collision2D collision
    )
    {
        Instantiate(
            explosionEffect,
            transform.position,
            transform.rotation
        );

        restartButton.style.display = DisplayStyle.Flex;

        borderParent.SetActive(false);
        PlayerPrefs.Save();
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        moveForward.Disable();
        lookPosition.Disable();
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}
