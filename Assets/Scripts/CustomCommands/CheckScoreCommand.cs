using Naninovel;
using UnityEngine;

[CommandAlias("checkScore")]
public class CheckScoreCommand : Command
{
    [RequiredParameter]
    public IntegerParameter ScoreThreshold;
    [RequiredParameter]
    public StringParameter IfLabel;
    [RequiredParameter]
    public StringParameter ElseLabel;
    public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        var scoreService = Engine.GetService<ScoreService>();
        var score = scoreService.GetScore();
        Debug.Log($"{score} > {ScoreThreshold}");
        var next = score > ScoreThreshold ? IfLabel : ElseLabel;

        var player = Engine.GetService<ScriptPlayer>();
        player.PlayFromLabel(next); // перейти к нужной метке

        return UniTask.CompletedTask;
    }
}
