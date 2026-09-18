<p align="center"> <img src="https://github.com/MichalisTamiolakis/BoFUN/raw/dev/Fullstack/frontend/src/assets/logo.png?raw=true" alt="BoFUN logo" width="200"/> </p>  <h4 align="center"><b>Board games made FUN!</b></h4> <h1 align="center"></h1>

A multiplayer, multi-device party-game platform built for a smart living room, developed for the **HCI Lab's CS469 course**. BoFUN combines an interactive coffee table, a wall-projected display, and every player's own smartphone into a single synchronized board game experience. No controllers, no single-screen bottleneck, everyone plays from their own device while sharing the same table and wall.

## How It Works

BoFUN is made up of three connected components, all kept in sync in real time:

| Component | Role |
|---|---|
| 🎲 **AugmenTable** (Unity) | The physical coffee table surface — dice rolls, board state, and piece placement via touch |
| 🖼️ **SurroundWall** (wall display) | A shared wall projection showing the board, trivia, pictionary prompts, and live statistics |
| 📱 **Smartphone clients** | Each player's controller — join via QR code, answer questions, and interact hands-free |
| 🔌 **Backend** | A real-time socket server that synchronizes game state, scores, and events across every connected device |

## Features

- **Multiplayer, multi-device sync** — table, wall, and every phone stay in sync in real time via a central socket server
- **QR-code onboarding** — players join and control gameplay directly from their own smartphone, no app install or account needed
- **Touch-based tabletop interactions** — dice rolls, board navigation, and piece placement directly on the coffee table surface
- **Voice command recognition** (SRGS grammar) and **text-to-speech narration** for hands-free question reading and game interaction
- **Multiple casual game modes** in one platform (trivia, pictionary-style drawing, and more) sharing the same underlying sync layer
- **Live statistics dashboard** on the wall display, rendered with ApexCharts
- **Configurable settings** — min/max players, round timing, narrator on/off, and more, via JSON config files

## Screenshots

### AugmenTable (Coffee Table)

<img width="480" height="270" alt="02_SelectPlayers" src="https://github.com/user-attachments/assets/fc987336-b984-4c18-b48f-05f7350c4868" />
<img width="480" height="270" alt="07_BoardAfterDiceRoll" src="https://github.com/user-attachments/assets/d7d2d4f9-fbf2-438f-9126-7d43f5872136" />
<img width="480" height="270" alt="08_BoardAfterDiceRollInfo" src="https://github.com/user-attachments/assets/4ea88a26-3f44-407a-89db-9bdfd6e37cc1" />
<img width="480" height="270" alt="09_BoardDrawing" src="https://github.com/user-attachments/assets/85329616-bb65-4b52-84f4-0e2f99ecaf9a" />
<img width="480" height="270" alt="10_Question" src="https://github.com/user-attachments/assets/b4260b13-508b-46b2-9c96-097a6673c363" />
<img width="480" height="270" alt="11_Pantomime" src="https://github.com/user-attachments/assets/f2429eb7-9899-4f88-aba8-6e02c9e00388" />

### SurroundWall (Wall Display)

<img width="480" height="164" alt="01_Homepage" src="https://github.com/user-attachments/assets/3e1125b7-f9df-4b43-9b42-178c4918dea6" />
<img width="480" height="164" alt="06_PictionaryDisplay" src="https://github.com/user-attachments/assets/a8c747c3-321e-4bf2-bb90-0020aa4a8f10" />
<img width="480" height="164" alt="07_TriviaDisplay" src="https://github.com/user-attachments/assets/86885701-ee6f-4b7a-8e5b-c9b6cabc3925" />
<img width="480" height="164" alt="08_Statistics" src="https://github.com/user-attachments/assets/193733b6-a0ae-40a3-9831-c8132870ce25" />

### Smartphone Client

<img width="187" height="406" alt="iPhone 13 mini - 12" src="https://github.com/user-attachments/assets/86e26635-2f80-4e91-8c54-013b6b6244e3" />
<img width="187" height="406" alt="iPhone 13 mini - 6" src="https://github.com/user-attachments/assets/3032256d-a889-4d49-a036-57ca9e6b9ef2" />
<img width="187" height="406" alt="iPhone 13 mini - 9" src="https://github.com/user-attachments/assets/6e464862-fc9a-46f8-b51b-cd8af92c9b84" />
<img width="187" height="406" alt="iPhone 13 mini - 11" src="https://github.com/user-attachments/assets/c1773822-ed5c-43aa-9252-56c828e3da7b" />

## Tech Stack

- **Backend**: Node.js, Express, TypeScript, Socket.IO for real-time state sync, MongoDB (via Mongoose), Inversify for dependency injection
- **Frontend (SurroundWall + smartphone clients)**: Angular 14, `ngx-socket-io`, ApexCharts (live statistics display)
- **AugmenTable**: Unity

## Getting Started

### Prerequisites

- [Node.js](https://nodejs.org/) and npm
- A running MongoDB instance (local or remote)
- [Unity](https://unity.com/) (only needed if you want to rebuild the AugmenTable app from source — a built `BoFUN.exe` is provided for running on the table)

### Setup

1. **Start the backend**
   ```bash
   cd Fullstack/backend
   npm install
   npm run dev
   ```
   Make sure your MongoDB connection details are set (check for a `.env` file or config in `src/`).

2. **Start the frontend**
   ```bash
   cd Fullstack/frontend
   npm install
   ng serve
   ```
   This serves the SurroundWall and smartphone web clients.

3. **Configure the AugmenTable app**
   Navigate to `AugmenTableUnity/bin/BoFUN_Data/StreamingAssets/Settings/`. There you'll find:
   - `GameSettings.json` — min/max players, min/max time per round, narrator toggle, animation options
   - `NetworkSettings.json` — backend and socket server URLs. **Replace `localhost` with your backend's actual address.**

   The `SRGS` folder holds the voice-recognition grammar and the `Sounds` folder holds background audio — both can be customized, but keep the file names unchanged so the app can find them.

4. **Run the table app**
   Launch `BoFUN.exe` from the `bin` folder on the table device.

5. **Open the wall display**
   On the wall device, navigate to `http://<ip-address>:4200/surroundwall`.

6. **Play!** Players scan the QR code shown on the wall/table to join from their smartphones.

## Project Structure

```
BoFUN/
├── AugmenTableUnity/   # Unity project for the interactive coffee table
└── Fullstack/
    ├── frontend/       # Angular app: SurroundWall + smartphone clients
    └── backend/        # Node.js/Express/TypeScript API + Socket.IO server (MongoDB)
```

## Context

BoFUN was built as a project for the **CS469 (Human-Computer Interaction)** course, designed specifically for the smart living room setup at the HCI Lab, University of Crete.

## License

Distributed under the MIT License. See [`LICENSE`](./LICENSE) for details.
