using Naninovel;

[CommandAlias("addScore")]
public class AddScoreCommand : Command
{
    public IntegerParameter ScoreDelta;
    public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        var scoreService = Engine.GetService<ScoreService>();
        scoreService.Score += ScoreDelta.Value;
        return UniTask.CompletedTask;
    }
}
