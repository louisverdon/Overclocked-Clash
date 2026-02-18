# 🎮 Overclocked Clash

Jeu de construction et combat de bots modulaires avec système de circuits logiques.

## 📋 Description

Construisez des bots pièce par pièce, créez des circuits logiques pour les contrôler, et affrontez-les dans des combats tour par tour déterministes.

## 🚀 Démarrage rapide

1. **Ouvrir le bon dossier dans Unity** : le projet Unity est dans le sous-dossier `Overclocked-Clash/`. Dans Unity Hub → Ouvrir, choisir le dossier **`Overclocked-Clash/Overclocked-Clash`** (celui qui contient `Assets`, `ProjectSettings`, `Packages`), pas la racine du dépôt.
2. Utiliser Unity **6000.1.7f1** ou supérieur.
3. Consulter `PLAN_POC.md` pour le plan d'action détaillé.
3. Commencer par la Phase 1 (modèle de données) - déjà partiellement implémentée

## 📁 Structure du projet

```
Assets/
├── Data/                    # Fichiers JSON de définition
│   ├── pieces.json         # Définitions des pièces
│   ├── modules.json        # Définitions des modules
│   ├── logicNodes.json    # Définitions des nœuds logiques
│   └── stats.json         # Liste des statistiques
├── Scripts/
│   ├── Data/              # Classes de chargement JSON
│   ├── Core/              # BotInstance, PieceInstance, PortInstance
│   ├── Logic/             # Système de circuits logiques
│   ├── Modules/           # Modules modificateurs de stats
│   ├── Pieces/            # Comportements des pièces
│   ├── Combat/            # Moteur de combat
│   ├── Testing/           # Système de validation
│   └── Editor/            # Scripts d'éditeur
└── Scenes/
    ├── BotEditor.unity    # Scène d'édition (à créer)
    └── CombatTest.unity   # Scène de test (à créer)
```

## ✅ État actuel

### Implémenté
- ✅ Structure de dossiers
- ✅ Fichiers JSON de base (pièces, modules, nœuds logiques)
- ✅ Classes de base : `PortType`, `PortInstance`
- ✅ Classes de données : `PieceDefinition`, `ModuleDefinition`, `LogicNodeDefinition`
- ✅ `DataLoader` pour charger les JSON

### À faire
- Voir `PLAN_POC.md` pour la liste complète des phases

## 🎯 Objectif du POC

Pouvoir construire un bot simple, le valider automatiquement, et le faire combattre contre une cible fixe dans une simulation abstraite.

## 📖 Documentation

Consulter `PLAN_POC.md` pour le plan d'action détaillé avec toutes les phases de développement.

## 🔧 Technologies

- Unity 6000.1.7f1
- C#
- JSON pour les définitions data-driven

