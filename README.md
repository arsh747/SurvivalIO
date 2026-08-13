# Survival.io

Part of the **MetaPlay: A Multi Game Offline Platform** — a fast-paced survival game focused on reflexes, resource management, and level progression.

## Description
A survival-style game where enemies spawn through a scripted spawn system (no AI pathing — this game does not use AI). The player must survive for a set amount of time; once the survival timer completes, a portal opens that takes the player to the next level.

## Built With
- Unity (Game Engine)
- C#

## Key Features
- **Scripted enemy spawning** (non-AI) — enemies spawn at intervals via spawn script
- **Survive-the-timer** core loop — surviving the required time opens a portal to the next level
- Hunger/stamina depletion system that decreases over time and affects player health
- Health drops when hunger reaches zero
- Third-person / top-down camera for better visibility and control
- Fully offline, progress/scores saved locally via PlayerPrefs

## How to Run
1. Clone the repo
2. Open in Unity Hub
3. Add this folder as a project
4. Open the main scene and press Play

## Testing Notes (from FYP documentation)
- Verified: hunger bar decreases steadily when player is idle
- Verified: player takes damage over time once hunger reaches zero

## Status
Completed — part of MetaPlay FYP (Fall 2025 – Spring 2026)

## Credits
Syed Kazim Raza, M. Arshmaan Attique, Hamza Javed — BS Computer Science, University of Management and Technology, Lahore
