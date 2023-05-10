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
            var enemyTarget = (Transform)_tree.GetContext(Context.EnemyTargetKey);
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

            _tree.SetContext(Context.DestinationKey, enemyTarget.position);
            
            _state = State.Running;
            return _state;
        }
    }
}