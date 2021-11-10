using Godot;
using System;

namespace TetrisGame
{
    public class Score : Control
    {
        const int MIN_LEVEL = 1;
        const int MAx_LEVEL = 10;
        public int score = 0;
        public int level = 1;

        private Label _scoreLabel;
        private Label _levelLabel;

        public override void _Ready()
        {
            _scoreLabel = GetNode<Label>("ScoreLabel");
            _levelLabel = GetNode<Label>("LevelLabel");
            UpdateLabels();
        }

        public void AddScore(int value)
        {
            score += value;
            level = Mathf.Clamp(score / 8, MIN_LEVEL, MAx_LEVEL);
            UpdateLabels();
        }

        public void ClearScore()
        {
            score = 0;
            level = 0;
            UpdateLabels();
        }

        private void UpdateLabels()
        {
            _scoreLabel.Text = "Score: " + score.ToString();
            _levelLabel.Text = "Level: " + level.ToString();
        }
    }
}
