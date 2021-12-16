using Godot;
using System;

namespace Main
{
    public class InteractiveObject : CollisionShape2D
    {
        [Export]
        public Scene sceneKey = Scene.UNKNOWN;

        public void Interact()
        {
            SceneManager.manager.ChangeScene(sceneKey);
        }
    }
}
