# Configuration de l'éditeur de bot (Phase 8)

## Créer les scènes

Dans Unity : **Overclocked Clash > Créer scène BotEditor** et **Overclocked Clash > Créer scène CombatTest**.

Ou manuellement :
1. **BotEditor** : Nouvelle scène → Create Empty → Ajouter le component `BotEditorController`
2. **CombatTest** : Nouvelle scène → Create Empty → Ajouter le component `CombatTestRunner`

## Build Settings

Ajouter les scènes dans **File > Build Settings** : BotEditor (index 0), CombatTest (optionnel).

## Utilisation de l'éditeur

1. Lancer la scène **BotEditor** (ou celle avec BotEditorController)
2. Cliquer sur une pièce dans la liste à gauche pour l'ajouter à la zone de construction
3. **Mode câblage** : Cliquer sur le bouton → clic sur pièce source (ex. Unité centrale) → clic sur pièce cible (ex. Canon pour connecter trigger)
4. **Valider le bot** : Lance le test (cible 20 PV, 200 ticks)
5. **Tester en combat** : Exécute le combat et affiche le résultat dans la zone de statut

## Exemple de bot minimal validé

- Ajouter **Unité Centrale** (unit_core)
- Ajouter **Canon Basique** (canon_basic)
- Mode câblage : connecter une sortie du core (ex. coreHP ou energyLevel) à l'entrée trigger du canon — *pour le POC le trigger est forcé à true dans le test, donc même sans câblage le canon tire si le bot a une unité centrale et un canon*
