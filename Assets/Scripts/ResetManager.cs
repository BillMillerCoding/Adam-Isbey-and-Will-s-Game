using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetManager : MonoBehaviour
{
    public static ResetManager Instance;
    
    private static List<IResettable> resettables = new List<IResettable>();

    public static void Register(IResettable r)
    {
        resettables.Add(r);
    }


    void Awake()
    {
        Instance = this;
    }

    public void SaveAll()
    {
        Debug.Log("SaveAll() called");
        foreach (var r in resettables)
            r.SaveSnapshot();
    }

    public void ResetAll()
    {
        Debug.Log("ResetAll() called");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (var r in resettables)
            r.RestoreSnapshot();
    }
}