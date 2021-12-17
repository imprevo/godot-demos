using Godot;
using Core;

namespace Main
{
    public class InteractiveObject : CollisionShape2D
    {
        [Export]
        private NodePath path;

        [Export]
        public Scene sceneKey = Scene.UNKNOWN;

        private ConfirmationDialog _dialog;

        public override void _Ready()
        {
            _dialog = GetNode<ConfirmationDialog>(path);
        }

        public void Interact()
        {
            _dialog.Popup_();
            SubscribeOnDialogSignals();
        }

        private void SubscribeOnDialogSignals()
        {
            _dialog.Connect("confirmed", this, nameof(ActionConfirmed));
            _dialog.Connect("popup_hide", this, nameof(UnsubscribeFromDialogSignals));
        }

        private void UnsubscribeFromDialogSignals()
        {
            _dialog.Disconnect("confirmed", this, nameof(ActionConfirmed));
            _dialog.Disconnect("popup_hide", this, nameof(UnsubscribeFromDialogSignals));
        }

        private void ActionConfirmed()
        {
            _dialog.Hide();
            SceneManager.manager.ChangeScene(sceneKey);
        }
    }
}
