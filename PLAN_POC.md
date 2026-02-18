# 🎯 Plan d'Action - POC Overclocked Clash

## Vue d'ensemble

Ce document détaille le plan d'action pour réaliser un premier POC fonctionnel du système de construction et combat de bots modulaires.

**Objectif du POC** : Pouvoir construire un bot simple, le valider, et le faire combattre contre une cible fixe dans une simulation abstraite.

---

## 📋 Phases de développement

### Phase 1 : Modèle de données (Fondations)
**Objectif** : Définir toutes les structures de données nécessaires

#### 1.1 Créer les fichiers JSON de définition
- [ ] `Assets/Data/pieces.json` - Définitions des pièces (Canon, Radar, Roue, etc.)
- [ ] `Assets/Data/modules.json` - Définitions des modules modificateurs
- [ ] `Assets/Data/logicNodes.json` - Définitions des composants logiques
- [ ] `Assets/Data/unit_core.json` - Définition de l'unité centrale
- [ ] `Assets/Data/stats.json` - Liste des statistiques modifiables

#### 1.2 Créer les classes C# de base
- [ ] `Scripts/Data/PieceDefinition.cs` - Classe pour charger les définitions JSON
- [ ] `Scripts/Data/ModuleDefinition.cs`
- [ ] `Scripts/Data/LogicNodeDefinition.cs`
- [ ] `Scripts/Data/PortDefinition.cs` - Structure pour les ports (type, nom, direction)
- [ ] `Scripts/Data/StatDefinition.cs` - Enum/struct pour les stats

#### 1.3 Système de chargement JSON
- [ ] `Scripts/Data/DataLoader.cs` - Singleton pour charger tous les JSON au démarrage
- [ ] Tests unitaires basiques pour vérifier le chargement

**Livrable** : Toutes les données peuvent être chargées depuis JSON et utilisées en C#

---

### Phase 2 : Architecture de base (Cœur du système)
**Objectif** : Implémenter les classes principales qui représentent un bot en mémoire

#### 2.1 Système de ports
- [ ] `Scripts/Core/PortType.cs` - Enum (Bool, Number, Event, HUD_Number)
- [ ] `Scripts/Core/PortInstance.cs` - Instance d'un port avec valeur courante et connexions
  - Propriétés : Type, Nom, Valeur, Liste de connexions
  - Méthodes : Connect(), Disconnect(), GetValue()

#### 2.2 Instance de pièce
- [ ] `Scripts/Core/PieceInstance.cs` - Représente une pièce dans un bot
  - Propriétés : Definition, HP courant, HP max, Ports (inputs/outputs)
  - Méthodes : TakeDamage(), IsDestroyed(), GetPort()

#### 2.3 Unité centrale
- [ ] `Scripts/Core/UnitCore.cs` - Hérite de PieceInstance
  - Propriétés : Énergie stockée, Énergie max, Inputs joueur (A/B/C)
  - Méthodes : ConsumeEnergy(), AddEnergy(), GetPlayerInput()

#### 2.4 Instance de bot
- [ ] `Scripts/Core/BotInstance.cs` - Contient toutes les pièces et le graphe logique
  - Propriétés : Liste de pièces, Unité centrale, Graphe de connexions
  - Méthodes : AddPiece(), ConnectPorts(), IsDestroyed() (si unité centrale détruite)
  - Méthode : Clone() pour les tests

**Livrable** : On peut créer un bot en code avec des pièces et les connecter

---

### Phase 3 : Système de circuits logiques
**Objectif** : Implémenter les composants logiques (AND, OR, NOT, comparateurs, etc.)

#### 3.1 Classe de base pour nœuds logiques
- [ ] `Scripts/Logic/LogicNode.cs` - Classe abstraite de base
  - Méthode abstraite : `void Evaluate()` - Recalcule les sorties
  - Propriétés : Ports d'entrée/sortie

#### 3.2 Implémenter les nœuds booléens
- [ ] `Scripts/Logic/Nodes/AndNode.cs`
- [ ] `Scripts/Logic/Nodes/OrNode.cs`
- [ ] `Scripts/Logic/Nodes/NotNode.cs`
- [ ] `Scripts/Logic/Nodes/ToggleNode.cs` - Toggle/Latch
- [ ] `Scripts/Logic/Nodes/RelayNode.cs` - Répéteur booléen

#### 3.3 Implémenter les comparateurs
- [ ] `Scripts/Logic/Nodes/CompareNode.cs` - Comparaisons >, <, =, !=
  - Propriété : Type de comparaison (enum)

#### 3.4 Implémenter les opérateurs mathématiques
- [ ] `Scripts/Logic/Nodes/AddNode.cs`
- [ ] `Scripts/Logic/Nodes/SubtractNode.cs`
- [ ] `Scripts/Logic/Nodes/MultiplyNode.cs`
- [ ] `Scripts/Logic/Nodes/DivideNode.cs`
- [ ] `Scripts/Logic/Nodes/DelayNode.cs` - Retarde un signal

#### 3.5 Gestionnaire de graphe logique
- [ ] `Scripts/Logic/LogicGraph.cs` - Gère tous les nœuds logiques
  - Méthode : `void EvaluateAll()` - Évalue tous les nœuds dans l'ordre
  - Méthode : `void PropagateSignals()` - Propage les valeurs dans le graphe

**Livrable** : On peut créer des circuits logiques et les évaluer

---

### Phase 4 : Modules modificateurs de stats
**Objectif** : Implémenter les modules qui modifient dynamiquement les stats des pièces

#### 4.1 Classe de base pour modules
- [ ] `Scripts/Modules/StatModule.cs` - Classe abstraite
  - Propriétés : Pièce cible, Signal d'activation (bool), Stat modifiée
  - Méthode abstraite : `float Modify(float baseValue)` - Retourne la valeur modifiée

#### 4.2 Implémenter les modules génériques
- [ ] `Scripts/Modules/MultiplierModule.cs` - Multiplie une stat par un facteur
- [ ] `Scripts/Modules/AdderModule.cs` - Ajoute une valeur à une stat
- [ ] `Scripts/Modules/DividerModule.cs` - Divise une stat
- [ ] `Scripts/Modules/ClampModule.cs` - Limite entre min et max
- [ ] `Scripts/Modules/MinMaxModule.cs` - Retourne min ou max

#### 4.3 Système de calcul de stats
- [ ] `Scripts/Core/StatCalculator.cs` - Calcule les stats finales d'une pièce
  - Méthode : `float GetFinalStat(PieceInstance piece, string statName)`
  - Prend en compte tous les modules actifs sur la pièce
  - Ne s'applique que si le signal d'activation est ON

**Livrable** : Les modules peuvent modifier les stats des pièces dynamiquement

---

### Phase 5 : Système énergétique
**Objectif** : Gérer la consommation et le stockage d'énergie

#### 5.1 Gestion de l'énergie dans UnitCore
- [ ] Étendre `UnitCore.cs` avec :
  - Méthode : `bool TryConsumeEnergy(float amount)` - Retourne false si pas assez
  - Méthode : `void RechargeEnergy(float amount)` - Recharge (pour générateurs)

#### 5.2 Pièces énergétiques
- [ ] `Scripts/Pieces/BatteryPiece.cs` - Étend la capacité max
- [ ] `Scripts/Pieces/GeneratorPiece.cs` - Génère de l'énergie passivement
- [ ] `Scripts/Pieces/SolarPanelPiece.cs` - Génère de l'énergie (variante)

#### 5.3 Consommation dans les pièces
- [ ] Ajouter consommation dans `PieceInstance.Execute()` (méthode abstraite)
- [ ] Vérifier l'énergie avant d'exécuter une action

**Livrable** : Le système d'énergie fonctionne et limite les actions

---

### Phase 6 : Moteur de combat (CombatEngine)
**Objectif** : Simulation déterministe tour par tour avec micro-ticks

#### 6.1 Classe CombatEngine
- [ ] `Scripts/Combat/CombatEngine.cs` - Moteur principal
  - Propriétés : Liste de bots, Tick actuel, État du combat
  - Méthode : `void Initialize(BotInstance bot1, BotInstance bot2)`
  - Méthode : `void ExecuteMicroTick(Dictionary<int, PlayerInput> inputs)` - Un micro-tick
  - Méthode : `CombatResult ExecuteFullTurn()` - 5 micro-ticks = 1 tour

#### 6.2 Cycle d'un micro-tick (dans CombatEngine)
- [ ] Lire les inputs joueurs
- [ ] Propager les signaux dans tous les graphes logiques
- [ ] Calculer les stat modifiers actifs pour toutes les pièces
- [ ] Exécuter les comportements des pièces (méthode abstraite `Execute()`)
- [ ] Appliquer les dégâts et vérifier les destructions
- [ ] Consommer l'énergie
- [ ] Vérifier les conditions de fin (bot détruit)

#### 6.3 Comportements des pièces
- [ ] `Scripts/Pieces/PieceBehavior.cs` - Classe abstraite
  - Méthode : `void Execute(PieceInstance piece, CombatContext context)`
- [ ] `Scripts/Pieces/WeaponBehavior.cs` - Tire des projectiles
- [ ] `Scripts/Pieces/MovementBehavior.cs` - Déplace le bot
- [ ] `Scripts/Pieces/RadarBehavior.cs` - Détecte les cibles

#### 6.4 Système de dégâts
- [ ] `Scripts/Combat/DamageSystem.cs` - Gère les dégâts
  - Méthode : `void ApplyDamage(PieceInstance target, float damage)`
  - Propagation des interruptions si pièce détruite

#### 6.5 Déterministe
- [ ] Utiliser un système de seed fixe pour les calculs
- [ ] Pas de Random() non contrôlé
- [ ] Ordre d'exécution fixe

**Livrable** : Un combat peut être simulé micro-tick par micro-tick

---

### Phase 7 : Validation des bots (TestEngine)
**Objectif** : Tester automatiquement si un bot est fonctionnel

#### 7.1 TestEngine
- [ ] `Scripts/Testing/TestEngine.cs` - Moteur de test
  - Méthode : `TestResult ValidateBot(BotInstance bot)`
  - Clone le bot
  - Spawn une cible fixe (20 PV, immobile)
  - Exécute 200 micro-ticks
  - Vérifie si la cible est détruite

#### 7.2 Cible de test
- [ ] `Scripts/Testing/TestTarget.cs` - Cible simple pour les tests
  - 20 PV, pas de mouvement, pas de défense

#### 7.3 Résultat de test
- [ ] `Scripts/Testing/TestResult.cs` - Structure de résultat
  - Propriétés : IsValid, DamageDealt, TicksExecuted, ErrorMessage

#### 7.4 Intégration dans BotInstance
- [ ] Ajouter propriété `bool IsValidated` dans `BotInstance`
- [ ] Méthode : `void Validate()` - Lance le test et met à jour IsValidated

**Livrable** : On peut valider automatiquement qu'un bot fonctionne

---

### Phase 8 : Éditeur basique (Interface minimale)
**Objectif** : Interface simple pour construire et tester un bot

#### 8.1 Scène d'éditeur
- [ ] Créer `Scenes/BotEditor.unity`
- [ ] UI minimale avec :
  - Liste des pièces disponibles
  - Zone de construction (grille simple)
  - Bouton "Valider le bot"
  - Bouton "Tester en combat"

#### 8.2 Scripts d'éditeur
- [ ] `Scripts/Editor/BotBuilder.cs` - Gère la construction en mode éditeur
  - Méthode : `void AddPiece(string pieceId, Vector2 position)`
  - Méthode : `void ConnectPorts(PortInstance port1, PortInstance port2)`
  - Méthode : `BotInstance BuildBot()` - Crée le BotInstance final

#### 8.3 Visualisation des connexions
- [ ] Afficher les connexions entre ports (lignes simples)
- [ ] Mode "câblage" pour connecter les ports

#### 8.4 Scène de test
- [ ] Créer `Scenes/CombatTest.unity`
- [ ] Afficher les résultats du combat (logs console pour l'instant)

**Livrable** : Interface basique pour construire un bot et le tester

---

## 🎯 Objectifs du POC

### Minimum viable
1. ✅ Charger les définitions depuis JSON
2. ✅ Créer un bot avec 3-4 pièces (Unité centrale + Canon + Radar + Roue)
3. ✅ Connecter quelques ports entre pièces
4. ✅ Valider le bot (test automatique)
5. ✅ Simuler un combat contre une cible fixe
6. ✅ Le bot doit pouvoir détruire la cible en 200 micro-ticks

### Nice to have (si temps)
- Interface éditeur visuelle plus avancée
- Plus de types de pièces
- Système de sauvegarde/chargement de bots
- Visualisation du combat (logs détaillés)

---

## 📁 Structure de dossiers proposée

```
Assets/
├── Data/                    # Fichiers JSON
│   ├── pieces.json
│   ├── modules.json
│   ├── logicNodes.json
│   └── unit_core.json
├── Scripts/
│   ├── Data/               # Classes de chargement JSON
│   ├── Core/               # BotInstance, PieceInstance, PortInstance, UnitCore
│   ├── Logic/              # Système de circuits logiques
│   │   └── Nodes/         # AND, OR, NOT, comparateurs, etc.
│   ├── Modules/            # Modules modificateurs de stats
│   ├── Pieces/             # Comportements des pièces
│   ├── Combat/             # CombatEngine, DamageSystem
│   ├── Testing/            # TestEngine, TestTarget
│   └── Editor/             # Scripts d'éditeur
└── Scenes/
    ├── BotEditor.unity
    └── CombatTest.unity
```

---

## 🔄 Ordre de priorité recommandé

1. **Phase 1** → Fondations indispensables
2. **Phase 2** → Architecture de base nécessaire pour tout le reste
3. **Phase 6** → Moteur de combat (peut être simplifié au début)
4. **Phase 7** → Validation (nécessaire pour tester)
5. **Phase 3** → Circuits logiques (peut être simplifié)
6. **Phase 4** → Modules (peut être simplifié)
7. **Phase 5** → Énergie (peut être simplifié)
8. **Phase 8** → Éditeur (interface minimale suffit)

---

## 📝 Notes importantes

- **Déterministe** : Tous les calculs doivent être déterministes pour permettre les replays
- **Micro-ticks** : 5 micro-ticks = 1 tour
- **Validation obligatoire** : Un bot non validé ne peut pas être utilisé en combat
- **Unité centrale** : Si détruite → bot détruit (même si d'autres pièces restent)
- **Modules** : Doivent être physiquement attachés à la pièce qu'ils modifient
- **Ports** : Seuls les ports de même type peuvent être connectés

---

## 🚀 Prochaines étapes

1. Commencer par la Phase 1 (modèle de données)
2. Créer les fichiers JSON de base avec quelques pièces simples
3. Implémenter le chargement JSON
4. Tester avec un bot minimal en code
5. Itérer sur les phases suivantes

