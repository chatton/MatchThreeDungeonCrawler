using DungeonCrawler.Enemies;
using UnityEngine;

namespace DungeonCrawler.Animations
{
    public class EnemyAnimationController : MonoBehaviour
    {
        private Animator _animator;
        private Enemy _enemy;

        private static readonly int IdleBlock = Animator.StringToHash("IdleBlock");
        private static readonly int Block = Animator.StringToHash("Block");
        private static readonly int Hurt = Animator.StringToHash("Hurt");
        private static readonly int Attack1 = Animator.StringToHash("Attack1");

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _enemy = GetComponent<Enemy>();
        }


        public void Attack()
        {
            _animator.SetTrigger(Attack1);
        }
    }
}