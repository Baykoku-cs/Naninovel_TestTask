using Naninovel;

[CommandAlias("addScore")]
public class AddScoreCommand : Command
{
    public IntegerParameter ScoreDelta;
    public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        var scoreService = Engine.GetService<ScoreService>();
        var score = scoreService.GetScore();

        scoreService.SetScore(score + ScoreDelta.Value);

        return UniTask.CompletedTask;
    }
}
