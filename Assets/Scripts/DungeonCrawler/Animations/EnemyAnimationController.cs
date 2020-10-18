using DungeonCrawler.Enemies;
using UnityEngine;

namespace DungeonCrawler.Animations
{
    public class EnemyAnimationController : MonoBehaviour
    {
        private Animator _animator;
        private Enemy _enemy;

        private static readonly int Hurt = Animator.StringToHash("Hurt");
        private static readonly int AttackHash = Animator.StringToHash("Attack");

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _enemy = GetComponent<Enemy>();
        }


        public void Attack()
        {
            _animator.SetTrigger(AttackHash);
        }
    }
}