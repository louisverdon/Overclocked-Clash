namespace OverclockedClash.Combat
{
    /// <summary>
    /// Inputs joueur pour un micro-tick (A, B, C).
    /// </summary>
    public struct PlayerInput
    {
        public bool InputA;
        public bool InputB;
        public bool InputC;

        public PlayerInput(bool a, bool b, bool c)
        {
            InputA = a;
            InputB = b;
            InputC = c;
        }
    }
}
