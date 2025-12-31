# ZenCode Generator Strategy

## Overview
The ZenCode Generator is a tool designed to generate full CRUDs for the ABP.io ecosystem, overcoming limitations of the ABP Suite.

## Phase 1: Frontend Entity Designer (Current Focus)
The primary goal is to build a robust visual editor for entities and their relationships.

### Strategy
1.  **Canvas-First Approach**: Start with an empty canvas using **React Flow**.
2.  **Basic Modeling**: Implement an "Add Entity" button to create nodes on the canvas.
3.  **Detailed Modeling**: Progressively add support for:
    -   Fields and data types.
    -   Validations.
    -   UI hints (screen width, datepickers, etc.).
    -   Relationships (1:1, 1:N, N:N).
4.  **Visual Excellence**: Maintain a premium, dynamic design throughout development.

## Phase 2: Backend Generation
Once the visual modeling is refined, we will implement the backend generation logic.
-   **AST Manipulation**: Use Roslyn for C# and ts-morph for TypeScript.
-   **Partial Classes & Protected Regions**: Ensure safe and idempotent code injection.
-   **Single Source of Truth**: Use the JSON schema generated from the frontend at `zencode-generator/` as the metadata source.
