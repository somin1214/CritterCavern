# 🐾 Critter Quest  
A cute creature-command 2D puzzle game built in Unity  

---

## Overview  
**Critter Quest** is a top-down mini adventure where the player explores stages filled with tiny creatures called *Critters*.  
Your goal is to navigate through challenging maps, interact with different critters, and reach the goal while mastering physics-based mechanics and special abilities.  

This project emphasizes **technical implementation** over art or sound — focusing on advanced Unity systems such as AI behavior, procedural generation, and save/load mechanics.

---

## Gamw Concept
- The player whistles to call nearby Critters.
- Critters follow within the radius and can be sent to interact with objects.
- Each object type behaves differently (rock -> breakable, bridge -> buildable, enemy -> attackable)
- The stage ends when the player reaches the stage goal.

---
## Features  

- **Basic Platformer Mechanics**  
  - Smooth player movement and jumping  
  - Checkpoints and respawn system  
  - Goal detection and stage completion logic  

- **Critter System**  
  - Multiple critter types with unique behaviors (e.g., chase, hide, wander)  
  - Interactions and simple AI logic  

- **Advanced Technical Features (Planned)**  
  - Procedural map generation for varied stages  
  - Save/Load system using JSON or ScriptableObjects  
  - Finite State Machine (FSM) or Behavior Tree–based AI  
  - Object pooling for efficient performance  

---

## Tech Stack  

| Category | Technology |
|-----------|-------------|
| Engine | Unity 2022+ |
| Language | C# |
| Version Control | Git + GitHub |
| Project Management | Trello |
| Build Target | PC / WebGL |

---

## Development Plan  

| Phase | Tasks | Duration |
|-------|--------|-----------|
| **Phase 1** | Core mechanics (movement, collision, goal system) | Week 1 |
| **Phase 2** | Critter implementation and AI behavior | Week 2 |
| **Phase 3** | Procedural generation and data save/load | Week 3 |
| **Phase 4** | Optimization and polish | Week 4 |

---

## Folder Structure  

```

Assets/
├─ Scripts/
│   ├─ Player/
│   ├─ Critters/
│   ├─ Managers/
│   └─ Systems/
├─ Prefabs/
├─ Scenes/
├─ UI/
└─ Resources/

```

---
## System Architecture

```
PlayerController
 ├── Detects Critters within radius
 ├── Issues commands (follow, send, whistle)
 └── Handles player movement

Critter (FSM-based)
 ├── Idle / Follow / Work / Scared
 ├── Transitions via StateManager
 └── Executes IInteractable.Interact()

InteractableObject (Interface)
 ├── Rock : IInteractable
 ├── Bridge : IInteractable
 ├── Enemy : IInteractable
 └── Goal : triggers StageClear

SaveManager
 ├── SaveGameData()
 ├── LoadGameData()
 └── Manages JSON serialization
 
```
---

## Future Enhancements  

- Inventory and item usage system  
- In-game time/day progression  
- Dialogue or event trigger system  
- Skill tree or player upgrades  


---

## License  
This project is for educational and non-commercial use.  
All assets used are either self-made or from free sources.

---
