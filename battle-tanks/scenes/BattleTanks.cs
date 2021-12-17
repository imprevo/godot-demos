using Godot;
using Core;

public class BattleTanks : Node2D
{
    private void OnExitButtonPressed()
    {
        SceneManager.manager.ChangeScene(Scene.MAIN);
    }
}
