namespace OverclockedClash.Combat
{
    /// <summary>
    /// Résultat d'un tour de combat (5 micro-ticks) ou de la fin du combat.
    /// </summary>
    public class CombatResult
    {
        /// <summary> -1 = draw / non terminé, 0 = bot1 gagne, 1 = bot2 gagne </summary>
        public int WinnerIndex { get; set; } = -1;
        /// <summary> Nombre de micro-ticks exécutés ce tour (ou total si fin) </summary>
        public int TicksExecuted { get; set; }
        /// <summary> Combat terminé (un bot détruit) </summary>
        public bool IsFinished { get; set; }
    }
}
