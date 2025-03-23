using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] Button startButton;

    public int score = 0;
    bool enableTimer = false;
    bool resetScore = false;

    public float timer = 60f;
    public float maxTimer = 60f;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timerText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        //startButton.SetEnabled(false);
        start.Invoke();
        timer = maxTimer;
        enableTimer = true;
    }
    public UnityEvent start;
    public UnityEvent OnComplete;
    public UnityEvent OnLose;
    public UnityEvent onReset;

    private void OnEnable()
    {
        //startButton.clicked += (StartGame);
    }
    private void OnDisable()
    {
        //startButton.clicked -= (StartGame);
    }
    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (enableTimer)
            timer -= Time.deltaTime;
        if (timerText != null)
        {
            timerText.text = ((int)timer).ToString();
        }
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }

        if (timer <= 0 && enableTimer)
        {
            enableTimer = false;
            OnLose?.Invoke();
            resetScore = true;
            Invoke(nameof(OnReset), 2f);
        }
    }
    private void OnReset()
    {
        if (resetScore)
            score = 0;

        enableTimer = true;
        timer = maxTimer;
        onReset?.Invoke();
    }
    public void Completed()
    {
        score++;
        enableTimer = false;
        timer = maxTimer;
        OnComplete?.Invoke();
        resetScore = false;
        Invoke(nameof(OnReset), 1f);
    }
}
