using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class ActManager : MonoBehaviour
{
    public static ActManager Instance { get; private set; }
    [SerializeField] Acts[] acts;
    public string currentQuest;

    public enum ActState
    {
        None,
        ActStart,
        ActInProgress,
        ActDone,
        ActEnd
    }

    public int CurrentAct = 0;
    public ActState CurrentState = ActState.None;

    // События для уведомления об изменении состояния акта
    public event Action<ActState> OnActStateChanged;
    public event Action OnNextAct;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Если нужно, чтобы объект пережил перезагрузку сцены
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (CurrentState == ActState.ActDone)
        {
            DoneAct();
            Debug.Log("ACT DONE EVENT");
        }
    }

    public void StartAct()
    {
        ActManager.Instance.SetActState(ActManager.ActState.ActStart);
        Acts currentactObject = acts[CurrentAct];
        currentQuest = currentactObject.questSceneName;

        foreach (GameObject person in currentactObject.actors)
        {
            Instantiate(person);
        }
    }

    public void DoneAct()
    {
        Acts currentactObject = acts[CurrentAct];
        currentQuest = currentactObject.questSceneName;
        foreach (GameObject person in currentactObject.endActors)
        {
            Instantiate(person);
        }
    }

    public void StartQuest()
    {
        if (!string.IsNullOrEmpty(currentQuest)) {
            SceneManager.LoadScene(currentQuest);
        }
        
    }

    public void DoneQuest()
    {

    }


    public void SetActState(ActState state)
    {
        CurrentState = state;
        Debug.Log($"Act {CurrentAct} state changed to: {CurrentState}");

        // Уведомление подписчиков об изменении состояния акта
        OnActStateChanged?.Invoke(CurrentState);
    }

    public void NextAct()
    {
        CurrentAct++;
        CurrentState = ActState.None;
        Debug.Log($"Moving to Act {CurrentAct}");

        // Уведомление подписчиков о переходе к следующему акту
        OnNextAct?.Invoke();
    }
}