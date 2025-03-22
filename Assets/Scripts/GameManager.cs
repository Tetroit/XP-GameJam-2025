using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] Button startButton;
    public int score = 0;
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
        startButton.SetEnabled(false);
        start.Invoke();
    }
    public UnityEvent start;
    public UnityEvent OnComplete;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
