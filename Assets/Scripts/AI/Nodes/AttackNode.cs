﻿using Chimera.AI.Utilities;
using UnityEngine;

namespace Chimera.AI
{
    public class AttackNode : Node
    {
        public override State Evaluate()
        {
            var enemyTarget = (Transform)GetContext(Context.EnemyTargetKey);
            if (enemyTarget != null)
            {
                
            }

            _state = State.Running;
            return _state;
        }
    }
}