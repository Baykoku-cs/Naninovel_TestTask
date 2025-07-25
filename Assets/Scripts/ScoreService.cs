using UnityEngine;
using Naninovel;

[InitializeAtRuntime]
public class ScoreService : IEngineService, IStatefulService<GameStateMap>, IStatefulService<GlobalStateMap>
{
    [System.Serializable]
    private class ScoreState { public int Score; }

    public event System.Action<int, bool> OnScoreChanged;
    private int _score;

    public void SetScore(int value, bool showUi = true)
    {
        _score = Mathf.Max(value, 0);
        OnScoreChanged?.Invoke(_score, showUi);
    }
    public int GetScore() => _score;

    public UniTask InitializeServiceAsync()
    {
        SetScore(0, false);
        return UniTask.CompletedTask;
    }

    public void ResetService()
    {
        SetScore(0, false);
    }

    public void DestroyService()
    {
    }

    public void SaveServiceState(GlobalStateMap mapState)
    {
        var scoreState = new ScoreState()
        {
            Score = _score
        };
        mapState.SetState(scoreState);
    }

    public UniTask LoadServiceStateAsync(GlobalStateMap mapState)
    {
        var scoreState = mapState.GetState<ScoreState>();
        if (scoreState is null) return UniTask.CompletedTask;

        SetScore(scoreState.Score, false);
        return UniTask.CompletedTask;
    }

    public void SaveServiceState(GameStateMap mapState)
    {
        var scoreState = new ScoreState()
        {
            Score = _score
        };
        mapState.SetState(scoreState);
    }

    public UniTask LoadServiceStateAsync(GameStateMap mapState)
    {
        var scoreState = mapState.GetState<ScoreState>();
        if (scoreState is null) return UniTask.CompletedTask;

        SetScore(scoreState.Score, false);
        return UniTask.CompletedTask;
    }
}
