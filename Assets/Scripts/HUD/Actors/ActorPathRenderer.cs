using Chimera.AI;
using Chimera.AI.Utilities;
using UnityEngine;

namespace Chimera
{
    public class ActorPathRenderer : MonoBehaviour
    {
        public LineRenderer lineRenderer;
        
        private BehaviourTree _actorBehaviourTree;
        private Actor _actor;
        private Vector3[] _linePositions = new[] {Vector3.zero, Vector3.zero};
        
        private void Awake()
        {
            _actor = transform.parent.GetComponent<Actor>();
            _actorBehaviourTree = transform.parent.GetComponent<BehaviourTree>();
        }
        
        private void Update()
        {
            if (_actor == null || _actorBehaviourTree == null)
            {
                return;
            }

            if (!_actor.Selected)
            {
                lineRenderer.enabled = false;
                return;
            }
            
            var destinationData = _actorBehaviourTree.GetNodesContext(Context.Nodes.DestinationKey);
            if (destinationData == null)
            {
                lineRenderer.enabled = false;
                return;
            }
            
            _linePositions[0] = _actorBehaviourTree.transform.position;
            _linePositions[1] = (Vector3)destinationData;
            _linePositions[1].y = _actorBehaviourTree.transform.position.y;

            if (Vector3.SqrMagnitude(_linePositions[0] - _linePositions[1]) < 1)
            {
                lineRenderer.enabled = false;
                return;
            }

            lineRenderer.enabled = true;
            lineRenderer.SetPositions(_linePositions);
        }
    }
}