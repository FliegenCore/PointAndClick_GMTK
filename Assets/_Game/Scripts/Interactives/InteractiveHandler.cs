using _Game.Scripts.Player;
using Game.ServiceLocator;

namespace _Game.Scripts.Interactives
{
    public class InteractiveHandler : IService
    {
        private PlayerController _playerController;
        
        private Interactive _lastInteractive;
        
        public void Initialize()
        {
            _playerController = G.Get<PlayerController>();
        }
        
        public void Interact(Interactive interactive, float position)
        {
            _lastInteractive = interactive;

            _playerController.Move(position, interactive);
        }
    }
}