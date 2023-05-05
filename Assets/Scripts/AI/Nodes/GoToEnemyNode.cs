using Chimera.AI.Utilities;
using UnityEngine;

namespace Chimera.AI
{
    public class GoToEnemyNode : Node
    {
        private Vector3 _lastEnemyPosition = Vector3.zero;        
        public GoToEnemyNode(BehaviourTree tree) : base(tree)
        {
        }
        
        public override State Evaluate()
        {
            var enemyTarget = (Transform)GetContext(Context.EnemyTargetKey);
            if (enemyTarget != null)
            {
                if (Vector3.SqrMagnitude(_lastEnemyPosition - enemyTarget.position) > 0.1f)
                {
                    _tree.character.navMeshAgent.SetDestination(enemyTarget.position);
                    _lastEnemyPosition = enemyTarget.position;
                }
            }

            _state = State.Running;
            return _state;
        }
    }
}