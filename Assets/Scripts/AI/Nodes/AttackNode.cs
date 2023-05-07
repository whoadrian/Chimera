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

            _state = State.Running;
            return _state;
        }
    }
}