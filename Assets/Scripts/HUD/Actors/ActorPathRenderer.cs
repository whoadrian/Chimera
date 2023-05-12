using Chimera.AI;
using Chimera.AI.Utilities;
using UnityEngine;

namespace Chimera
{
    /// <summary>
    /// Displays a line towards an Actor's destination, taken from its behaviour tree data context.
    /// Needs to be a scene child of an actor component.
    /// </summary>
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

            // Only display it when actor is selected
            if (!_actor.Selected)
            {
                lineRenderer.enabled = false;
                return;
            }
            
            // Get destination data from the actor's behaviour tree context
            var destinationData = _actorBehaviourTree.GetNodesContext(Context.NodeKey.Destination);
            if (destinationData == null)
            {
                lineRenderer.enabled = false;
                return;
            }
            
            // Setup line positions
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