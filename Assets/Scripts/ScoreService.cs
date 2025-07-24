using UnityEngine;
using Naninovel;

[InitializeAtRuntime]
public class ScoreService : IEngineService
{
    private int _score;
    public int Score 
    { 
        get => _score; 
        set 
        { 
            _score = Mathf.Max(value, 0); 
        } 
    }

    public void DestroyService()
    {
    }

    public UniTask InitializeServiceAsync()
    {
        _score = 0;
        return UniTask.CompletedTask;
    }

    public void ResetService()
    {
        _score = 0;
    }
}
