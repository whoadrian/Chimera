using Chimera.AI.Utilities;
using UnityEngine;

namespace Chimera.AI
{
    public class EnemyVisibleCheckNode : Node
    {
        private Transform _characterTransform;

        public EnemyVisibleCheckNode(BehaviourTree tree) : base(tree)
        {
            _characterTransform = _tree.character.transform;
        }

        public override State Evaluate()
        {
            var enemyTarget = GetContext(Context.EnemyTargetKey);
            if (enemyTarget == null)
            {
                var colliders =
                    Physics.OverlapSphere(_characterTransform.position, _tree.character.config.fovRange, Layers.CharacterLayerMask);
                
                foreach (var c in colliders)
                {
                    var otherCharacterBehaviour = c.GetComponent<Character>();
                    if (otherCharacterBehaviour == null)
                        continue;

                    if (otherCharacterBehaviour.config.faction != _tree.character.config.faction)
                    {
                        GetRootParent().SetContext(Context.EnemyTargetKey, otherCharacterBehaviour.transform);

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