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
        var svc = Engine.GetService<ScoreService>();
        Debug.Log($"{svc.Score} > {ScoreThreshold}");
        var next = svc.Score > ScoreThreshold ? IfLabel : ElseLabel;

        var player = Engine.GetService<ScriptPlayer>();
        player.PlayFromLabel(next); // перейти к нужной метке

        return UniTask.CompletedTask;
    }
}
