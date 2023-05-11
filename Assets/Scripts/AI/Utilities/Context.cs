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
        public struct Nodes
        {
            public static string EnemyTargetKey => "enemyTarget";
            public static string DestinationKey => "destination";
        }
        
        /// <summary>
        /// Behaviour tree command context keys, for player-issued commands.
        /// </summary>
        public struct Commands
        {
            public static string MoveToCommandKey => "moveToCmd";
            public static string AttackCommandKey => "attackCmd";
        }
    }
}