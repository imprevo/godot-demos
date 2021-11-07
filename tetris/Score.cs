using Godot;
using System;

namespace TetrisGame
{
    public class Score : Control
    {
        public int score = 0;

        private Label _label;

        public override void _Ready()
        {
            _label = GetNode<Label>("Label");
            UpdateScoreLabel();
        }

        public void AddScore(int value)
        {
            score += value;
            UpdateScoreLabel();
        }

        private void UpdateScoreLabel()
        {
            _label.Text = "Score: " + score.ToString();
        }
    }
}
