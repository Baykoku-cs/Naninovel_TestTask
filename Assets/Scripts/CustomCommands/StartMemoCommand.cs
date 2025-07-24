using Naninovel;
using Naninovel.Commands;
using Naninovel.UI;
using UnityEngine;

[CommandAlias("startMemo")]
public class StartMemoCommand : Command
{
    public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {

       // 1. Disable Naninovel input.
        var inputManager = Engine.GetService<IInputManager>();
        inputManager.ProcessInput = false;

        // 2. Stop script player.
        var scriptPlayer = Engine.GetService<IScriptPlayer>();
        scriptPlayer.Stop();

        Engine.GetService<UIManager>().GetUI<ContinueInputUI>().Hide();

        // 3. Hide text printer.
        var hidePrinter = new HidePrinter();
        hidePrinter.ExecuteAsync(asyncToken).Forget();


        // 4. Swap Cameras.
        MemoGameCamera.Instance.ToggleCamera(true);
        Engine.GetService<ICameraManager>().Camera.enabled = false;

        return UniTask.CompletedTask;
    }
}
