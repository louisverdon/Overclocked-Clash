namespace OverclockedClash.Testing
{
    /// <summary>
    /// Résultat d'un test de validation de bot (TestEngine.ValidateBot).
    /// </summary>
    public class TestResult
    {
        /// <summary> Le bot a détruit la cible en 200 micro-ticks. </summary>
        public bool IsValid { get; set; }
        /// <summary> Dégâts infligés à la cible (0 à 20). </summary>
        public float DamageDealt { get; set; }
        /// <summary> Nombre de micro-ticks exécutés (200 max). </summary>
        public int TicksExecuted { get; set; }
        /// <summary> Message d'erreur ou raison de l'échec. </summary>
        public string ErrorMessage { get; set; }
    }
}
