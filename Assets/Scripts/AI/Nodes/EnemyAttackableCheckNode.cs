using Chimera.AI.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace Chimera.AI
{
    public class EnemyAttackableCheckNode : Node
    {
        public EnemyAttackableCheckNode(BehaviourTree tree) : base(tree)
        {
        }
        
        public override State Evaluate()
        {
            var enemyTarget = (Transform)_tree.GetNodesContext(Context.Nodes.EnemyTargetKey);
            
            if (enemyTarget != null &&
                !NavMesh.Raycast(_tree.actor.transform.position, enemyTarget.position, out var hitData, Layers.DefaultLayerMask) &&
                Vector3.SqrMagnitude(_tree.actor.transform.position - enemyTarget.position) < _tree.actor.config.attackRange * _tree.actor.config.attackRange)
            {
                _state = State.Success;
                return _state;
            }

            _state = State.Failure;
            return _state;
        }
    }
}