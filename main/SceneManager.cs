using Godot;
using System;
using System.Collections.Generic;

namespace Main
{
    public enum Scene
    {
        UNKNOWN,
        MAIN,
        PLATFORMER,
        SNAKE,
        TETRIS,
        TANKS,
    }

    class SceneManager : Node
    {
        public static SceneManager manager { get; private set; }

        public override void _Ready()
        {
            manager = this;
        }

        private Dictionary<Scene, string> scenes = new Dictionary<Scene, string>
        {
            {Scene.MAIN, "res://main/Main.tscn"},
            {Scene.PLATFORMER, "res://platformer/PlatformerScene.tscn"},
            {Scene.SNAKE, "res://snake/SnakeScene.tscn"},
            {Scene.TANKS, "res://battle-tanks/scenes/BattleTanks.tscn"},
            {Scene.TETRIS, "res://tetris/Tetris.tscn"},
        };

        public void ChangeScene(Scene sceneKey)
        {
            if (!scenes.ContainsKey(sceneKey))
            {
                throw new Exception("Unknown scene");
            }

            var path = scenes[sceneKey];
            GetTree().ChangeScene(path);
        }
    }
}
