**This repo is the Desktop front end for CombatCritters.**

**Our main repository can be found [here]
(https://github.com/InternetEnemies/CombatCritters).**

# Getting Started
This project supports **Mac Catalyst** exclusively. The app is designed to deliver a consistent experience on Mac device, including both Apple Silicon and Intel-based Macs.

## Prerequisites

Before you begin, ensure you have the following installed

- .NET SDK 8.0 or newer
- **Mac Catalyst Workload** for .NET MAUI
- Visual Studio Code
  - Install the **C# extension** and **.NET MAUI support** using the **C# Dev Kit** extension.
- Xcode (for Mac Catalyst development)

> Note: Visual Studio 2022 for Mac is no longer supported. Use **Visual Studio Code** with the appropriate extension for development.

## Front end Setup

### 1. Cloning the Repository

First, clone the Desktop frontend repository to your local machine

```bash
git clone https://github.com/InternetEnemies/combatcritters-maui.git
```

Navigate into the project directory

```bash
cd combatcritters-maui
```
### 2. Build the Project:

```bash
dotnet build -f net8.0-maccatalyst
```

### 3. Running the Project
```
dotnet run -f net8.0-maccatalyst
```

You can also run and debug the project from Visual Studio Code by using the built-in terminal or setting up a debug configuration in the launch.json file.
