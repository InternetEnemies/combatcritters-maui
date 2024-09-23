# combatcritters-maui .NET MAUI Project (Mac Catalyst Only)

## Table of Content

- [Overview](#overview)
- [Prerequisites](#prerequisites)
- [Setup](#setup)

## Overview

CombatCritters is a cross-platform mobile application built using .NET MAUI. This project supports **Mac Catalyst** exclusively. The app is designed to deliver a consistent experience on Mac device, including both Apple Silicon and Intel-based Macs.

## Prerequisites

Before you begin, ensure you have the following installed

- .NET SDK 8.0 or newer
- **Mac Catalyst Workload** for .NET MAUI
- Visual Studio Code
  - Install the **C# extension** and **.NET MAUI support** using the **C# Dev Kit** extension.
- Xcode (for Mac Catalyst development)

> Note: Visual Studio 2022 for Mac is no longer supported. Use **Visual Studio Code** with the appropriate extension for development.

## Setup

### Cloning the Repository

1. Clone the repository:
   ```bash
   git clone https://github.com/InternetEnemies/combatcritters-maui.git
   cd combatcritters-maui
   ```
2. Install dependencies:

   ```
    dotnet restore
   ```

3. Install Mac Catalyst Workload

   ```
   dotnet workload install maccatalyst
   ```

4. Building the Project

   ```
   dotnet build -f net8.0-maccatalyst
   ```

5. Running the Project
   ```
   dotnet run -f net8.0-maccatalyst
   ```

You can also run and debug the project from Visual Studio Code by using the built-in terminal or setting up a debug configuration in the launch.json file.
