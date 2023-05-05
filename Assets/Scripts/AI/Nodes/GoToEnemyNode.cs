using Chimera.AI.Utilities;
using UnityEngine;

namespace Chimera.AI
{
    public class GoToEnemyNode : Node
    {
        private Transform _characterTransform;
        
        public GoToEnemyNode(BehaviourTree tree) : base(tree)
        {
            _characterTransform = _tree.character.transform;
        }
        
        public override State Evaluate()
        {
            var enemyTarget = (Transform)GetContext(Context.EnemyTargetKey);
            if (enemyTarget != null)
            {
                if (Vector3.SqrMagnitude(_characterTransform.position - enemyTarget.position) > 0.01f)
                {
                    _characterTransform.position = Vector3.MoveTowards(_characterTransform.position,
                        enemyTarget.position, _tree.character.config.speed * Time.deltaTime);
                    _characterTransform.LookAt(enemyTarget.position);
                }
            }

            _state = State.Running;
            return _state;
        }
    }
}