namespace Chimera.AI.Utilities
{
    public static class Context
    {
        public struct Nodes
        {
            public static string EnemyTargetKey => "enemyTarget";
            public static string DestinationKey => "destination";
        }
        
        public struct Commands
        {
            public static string MoveToCommandKey => "moveToCmd";
            public static string AttackCommandKey => "attackCmd";
        }
    }
}