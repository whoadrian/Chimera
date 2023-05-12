namespace Chimera.AI.Utilities
{
    /// <summary>
    /// Behaviour tree context keys
    /// </summary>
    public static class Context
    {
        /// <summary>
        /// Behaviour tree nodes context keys, for sharing data between nodes.
        /// </summary>
        public enum NodeKey
        {
            None = 0,
            EnemyTarget,
            Destination
        }
        
        /// <summary>
        /// Behaviour tree command context keys, for player-issued commands.
        /// </summary>
        public enum CommandKey
        {
            None = 0,
            MoveToCommand,
            AttackCommand
        }
    }
}