using _Game.Scripts.AISystem;
using _Game.Scripts.DaySystem;
using _Game.Scripts.Dialogues;
using _Game.Scripts.Player;
using Game.ServiceLocator;
using UnityEngine;

public class OldZone : MonoBehaviour
{
    [SerializeField] private AIView _aiView;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision enter" + other.collider.name);
        if (other.transform.TryGetComponent<PlayerView>(out var player))
        {
            player.transform.position = new Vector3(player.transform.position.x - 0.5f, player.transform.position.y);
            Dialogue dialogue = _aiView.GetDialogue("you_shall_not_pass");
            G.Get<DayController>().NextTimeDay();
            player.Stop();
            
            G.Get<DialogueSystem>().StartDialogue(dialogue, _aiView);
        }
    }
}
