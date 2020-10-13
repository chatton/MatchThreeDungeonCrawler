using System.Collections.Generic;
using MatchThree.Board;
using UnityEngine;

namespace MatchThree.Gems
{
    public class Gem : MonoBehaviour
    {
        [SerializeField] private MatchType matchType;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private GemAction gemAction;

        public MatchType MatchType => matchType;
        internal GameBoard GameBoard { get; set; }
        internal GemSource GemSource { get; set; }
        

        public bool HasReachedTargetPosition => Vector3.Distance(transform.localPosition, _targetPosition) <= 0.01f;

        private Vector3 _targetPosition;

        public void UpdatePosition((int, int) oldPosition, (int, int) newPosition)
        {
            _targetPosition = new Vector3(newPosition.Item1 * transform.localScale.x,
                newPosition.Item2 * transform.localScale.y, 0);

            // if the object is disabled just immediately teleport to the new position
            if (!gameObject.activeSelf)
            {
                transform.localPosition = _targetPosition;
            }
        }

        public void ReturnToPool()
        {
            GemSource.ReturnToPool(this);
        }

        public void OnMatch(Dictionary<GemEffectType, GemResult> gemMatchResults)
        {
            gemAction.OnMatch(gemMatchResults);
            ReturnToPool();
        }

        private void Update()
        {
            if (_targetPosition == Vector3.zero)
            {
                return;
            }

            transform.localPosition =
                Vector3.MoveTowards(transform.localPosition, _targetPosition, Time.deltaTime * moveSpeed);
        }

        public void OnMouseDown()
        {
            GameBoard.SelectGem(this);
        }

        public void SetPosition((int, int) valueTuple)
        {
            transform.localPosition = new Vector3(valueTuple.Item1 * transform.localScale.x,
                valueTuple.Item2 * transform.localScale.y, 0);
        }
    }
}