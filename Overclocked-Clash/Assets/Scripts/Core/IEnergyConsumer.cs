namespace OverclockedClash.Core
{
    /// <summary>
    /// Permet de consommer de l'énergie (ex. UnitCore). Utilisé par les pièces dans Execute().
    /// </summary>
    public interface IEnergyConsumer
    {
        bool TryConsumeEnergy(float amount);
    }
}
