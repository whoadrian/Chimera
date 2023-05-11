using Chimera.AI.Utilities;
using UnityEngine;

namespace Chimera.AI
{
    public class AttackNode : Node
    {
        public AttackNode(BehaviourTree tree) : base(tree)
        {
        }

        public override State Evaluate()
        {
            var enemyTarget = (Transform)_tree.GetNodesContext(Context.Nodes.EnemyTargetKey);
            if (enemyTarget == null)
            {
                _state = State.Failure;
                return _state;
            }
            
            // Face enemy
            var faceDirection = enemyTarget.position - _tree.actor.transform.position;
            faceDirection.y = 0;
            _tree.actor.transform.rotation = Quaternion.RotateTowards(_tree.actor.transform.rotation,
                Quaternion.LookRotation(faceDirection), _tree.actor.config.angularSpeed * Time.deltaTime);

            _tree.actor.navMeshAgent.SetDestination(_tree.actor.transform.position);
            _tree.actor.animator.SetBool(_tree.actor.config.attackAnimBool, true);
            _tree.SetNodesContext(Context.Nodes.DestinationKey, enemyTarget.position);
            
            _state = State.Running;
            return _state;
        }
    }
}