namespace Chimera.AI
{
    /// <summary>
    /// Simply stops the navmesh agent.
    /// </summary>
    public class StopMovementNode : Node
    {
        public StopMovementNode(BehaviourTree tree) : base(tree)
        {
        }

        public override State Evaluate()
        {
            if (_tree.actor.navMeshAgent.destination != _tree.actor.transform.position)
            {
                _tree.actor.navMeshAgent.SetDestination(_tree.actor.transform.position);
            }

            _state = State.Success;
            return _state;
        }
    }
}