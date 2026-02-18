using System.Collections.Generic;
using OverclockedClash.Core;
using OverclockedClash.Pieces;

namespace OverclockedClash.Combat
{
    /// <summary>
    /// Moteur de combat déterministe : 5 micro-ticks = 1 tour. Pas de Random().
    /// </summary>
    public class CombatEngine
    {
        private BotInstance _bot1;
        private BotInstance _bot2;
        private CombatContext _context;
        private DamageSystem _damageSystem;
        private Dictionary<string, PieceBehavior> _behaviors;

        public BotInstance Bot1 => _bot1;
        public BotInstance Bot2 => _bot2;
        public int CurrentTick { get; private set; }
        public bool IsFinished => _bot1 != null && _bot2 != null && (_bot1.IsDestroyed() || _bot2.IsDestroyed());

        /// <summary> Position X du bot 1 (ordre fixe d'exécution). </summary>
        private float _position1X;
        /// <summary> Position X du bot 2. </summary>
        private float _position2X;

        public CombatEngine()
        {
            _damageSystem = new DamageSystem();
            _context = new CombatContext { DamageSystem = _damageSystem };
            _behaviors = new Dictionary<string, PieceBehavior>
            {
                ["weapon"] = new WeaponBehavior(),
                ["radar"] = new RadarBehavior(),
                ["movement"] = new MovementBehavior()
            };
        }

        /// <summary>
        /// Initialise le combat avec deux bots (clones pour ne pas modifier les originaux).
        /// </summary>
        public void Initialize(BotInstance bot1, BotInstance bot2)
        {
            _bot1 = bot1?.Clone() ?? throw new System.ArgumentNullException(nameof(bot1));
            _bot2 = bot2?.Clone() ?? throw new System.ArgumentNullException(nameof(bot2));
            CurrentTick = 0;
            _position1X = 0f;
            _position2X = 10f;
            _context.CooldownRemaining.Clear();
        }

        /// <summary>
        /// Exécute un micro-tick. inputs[0] = joueur 1 (bot1), inputs[1] = joueur 2 (bot2).
        /// </summary>
        public void ExecuteMicroTick(Dictionary<int, PlayerInput> inputs)
        {
            if (_bot1 == null || _bot2 == null) return;
            if (IsFinished) return;

            inputs ??= new Dictionary<int, PlayerInput>();

            InjectInputs(_bot1, inputs.TryGetValue(0, out var in1) ? in1 : default);
            InjectInputs(_bot2, inputs.TryGetValue(1, out var in2) ? in2 : default);

            RegenEnergy(_bot1);
            RegenEnergy(_bot2);

            _bot1.LogicGraph?.EvaluateAll();
            _bot2.LogicGraph?.EvaluateAll();

            DecrementCooldowns();

            _context.CurrentTick = CurrentTick;

            ExecuteBotPieces(_bot1, _bot2, ref _position1X, _position2X);
            if (IsFinished) { CurrentTick++; return; }

            ExecuteBotPieces(_bot2, _bot1, ref _position2X, _position1X);

            CurrentTick++;
        }

        /// <summary>
        /// Exécute 5 micro-ticks avec les mêmes inputs (ou sans input joueur).
        /// </summary>
        public CombatResult ExecuteFullTurn()
        {
            var result = new CombatResult();
            var noInput = new Dictionary<int, PlayerInput>();
            for (int i = 0; i < 5; i++)
            {
                ExecuteMicroTick(noInput);
                result.TicksExecuted++;
                if (IsFinished) break;
            }
            result.IsFinished = IsFinished;
            if (_bot1 != null && _bot1.IsDestroyed()) result.WinnerIndex = 1;
            else if (_bot2 != null && _bot2.IsDestroyed()) result.WinnerIndex = 0;
            return result;
        }

        private static void InjectInputs(BotInstance bot, PlayerInput input)
        {
            if (bot?.Core == null) return;
            bot.Core.GetInput("playerInputA")?.SetValue(input.InputA);
            bot.Core.GetInput("playerInputB")?.SetValue(input.InputB);
            bot.Core.GetInput("playerInputC")?.SetValue(input.InputC);
        }

        private static void RegenEnergy(BotInstance bot)
        {
            if (bot?.Core == null) return;
            bot.Core.RegenEnergy();
            foreach (var p in bot.Pieces)
            {
                if (p is GeneratorPiece gp)
                    bot.Core.RechargeEnergy(gp.GetEnergyPerTick());
                else if (p is SolarPanelPiece sp)
                    bot.Core.RechargeEnergy(sp.GetEnergyPerTick());
            }
        }

        private void DecrementCooldowns()
        {
            var keys = new List<PieceInstance>(_context.CooldownRemaining.Keys);
            foreach (var k in keys)
            {
                int v = _context.CooldownRemaining[k];
                if (v > 0) _context.CooldownRemaining[k] = v - 1;
            }
        }

        private void ExecuteBotPieces(BotInstance current, BotInstance enemy, ref float currentPosX, float enemyPosX)
        {
            _context.CurrentBot = current;
            _context.EnemyBot = enemy;
            _context.BotPositionX = currentPosX;
            _context.EnemyPositionX = enemyPosX;

            foreach (var piece in current.Pieces)
            {
                if (piece.IsDestroyed()) continue;
                if (piece is UnitCore || piece is BatteryPiece || piece is GeneratorPiece || piece is SolarPanelPiece)
                    continue;

                string behaviorName = piece.Definition?.behavior;
                if (string.IsNullOrEmpty(behaviorName) || !_behaviors.TryGetValue(behaviorName, out var behavior))
                    continue;

                behavior.Execute(piece, _context);
            }

            currentPosX = _context.BotPositionX;
        }
    }
}
