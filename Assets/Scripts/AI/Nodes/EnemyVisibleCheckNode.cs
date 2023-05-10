﻿using Chimera.AI.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace Chimera.AI
{
    public class EnemyVisibleCheckNode : Node
    {
        private Transform _actorTransform;

        public EnemyVisibleCheckNode(BehaviourTree tree) : base(tree)
        {
            _actorTransform = _tree.actor.transform;
        }

        public override State Evaluate()
        {
            var enemyTarget = (Transform)_tree.GetContext(Context.EnemyTargetKey);
            if (enemyTarget == null)
            {
                var colliders =
                    Physics.OverlapSphere(_actorTransform.position, _tree.actor.config.fovRange, Layers.ActorLayerMask);
                
                foreach (var c in colliders)
                {
                    if (NavMesh.Raycast(_tree.actor.transform.position, c.transform.position, out _,
                            Layers.DefaultLayerMask))
                    {
                        continue;
                    }
                    
                    var otherActorBehaviour = c.GetComponent<Actor>();
                    if (otherActorBehaviour == null)
                        continue;

                    if (otherActorBehaviour.faction != _tree.actor.faction)
                    {
                        _tree.SetContext(Context.EnemyTargetKey, otherActorBehaviour.transform);

                        _state = State.Success;
                        return _state;
                    }
                }

                _state = State.Failure;
                return _state;
            }
            
            _state = State.Success;
            return _state;
        }
    }
}