using UnityEngine;

namespace DungeonCrawler.Animations
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private Animator _animator;
        private Player _player;

        private static readonly int IdleBlock = Animator.StringToHash("IdleBlock");

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
    }
}