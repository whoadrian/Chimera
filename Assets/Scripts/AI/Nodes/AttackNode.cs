using Chimera.AI.Utilities;
using UnityEngine;

namespace Chimera.AI
{
    /// <summary>
    /// Attacks an enemy, if one is present in the nodes context.
    /// </summary>
    public class AttackNode : Node
    {
        public AttackNode(BehaviourTree tree) : base(tree)
        {
        }

        public override State Evaluate()
        {
            // Check the nodes context for an enemy
            var enemyTarget = (Transform)_tree.GetNodesContext(Context.NodeKey.EnemyTarget);
            if (enemyTarget == null)
            {
                // No enemy
                _state = State.Failure;
                return _state;
            }
            
            // Face enemy
            var faceDirection = enemyTarget.position - _tree.actor.transform.position;
            faceDirection.y = 0;
            _tree.actor.transform.rotation = Quaternion.RotateTowards(_tree.actor.transform.rotation,
                Quaternion.LookRotation(faceDirection), _tree.actor.config.angularSpeed * Time.deltaTime);

            // Trigger attack animation
            _tree.actor.animator.SetBool(_tree.actor.config.attackAnimBool, true);
            
            // Set destination of actor to the enemy
            _tree.SetNodesContext(Context.NodeKey.Destination, enemyTarget.position);
            
            _state = State.Running;
            return _state;
        }
    }
}