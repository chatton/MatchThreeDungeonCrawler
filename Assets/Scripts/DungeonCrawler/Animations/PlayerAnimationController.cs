using DungeonCrawler.PlayerCharacter;
using UnityEngine;

namespace DungeonCrawler.Animations
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private Animator _animator;
        private Player _player;

        private static readonly int IdleBlock = Animator.StringToHash("IdleBlock");
        private static readonly int Block = Animator.StringToHash("Block");
        private static readonly int Hurt = Animator.StringToHash("Hurt");

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _player = GetComponent<Player>();
        }

        private void Start()
        {
            _player.OnPlayerStatsChanged += UpdatePlayerAnimations;
        }

        private void UpdatePlayerAnimations(Player player)
        {
            _animator.SetBool(IdleBlock, player.Defence > 0);
        }

        public void ReceiveDamage(int damage)
        {
            // the player blocked all the incoming damage
            // we just play the block animation
            if (_player.Defence >= damage)
            {
                _animator.SetTrigger(Block);
            }
            // otherwise we are actually taking damage, play the hit animation
            else
            {
                _animator.SetTrigger(Hurt);
            }
        }
    }
}