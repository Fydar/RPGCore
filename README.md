<h1>
<img src="./src/icon.png" width="54" height="54" align="left" />
RPGCore
</h1>

[![Build Status](https://github.com/Fydar/RPGCore/workflows/Build/badge.svg)](https://github.com/Fydar/RPGCore/actions?query=workflow%3ABuild)
[![Unit Test Status](https://github.com/Fydar/RPGCore/workflows/Unit%20Tests/badge.svg)](https://github.com/Fydar/RPGCore/actions?query=workflow%3A%22Unit+Tests%22)

**RPGCore** is a toolkit for producing games and mechanics in **C#**.

![RPGCore Main Demo](./img/screenshots/Main.png)

## 📦 Libraries

[![RPGCore.Data](https://img.shields.io/badge/📦-RPGCore.Data-333333.svg)](./src/libs/RPGCore.Data) ![Status: Viable](https://img.shields.io/badge/viable-blue)\
[![RPGCore.DataEditor](https://img.shields.io/badge/📦-RPGCore.DataEditor-333333.svg)](./src/libs/RPGCore.DataEditor) ![Status: Viable](https://img.shields.io/badge/viable-blue)\
[![RPGCore.Events](https://img.shields.io/badge/📦-RPGCore.Events-333333.svg)](./src/libs/RPGCore.Events) ![Status: Viable](https://img.shields.io/badge/viable-blue)\
[![RPGCore.Packages](https://img.shields.io/badge/📦-RPGCore.Packages-333333.svg)](./src/libs/RPGCore.Packages) ![Status: Viable](https://img.shields.io/badge/viable-blue)\
[![RPGCore.Projects](https://img.shields.io/badge/📦-RPGCore.Projects-333333.svg)](./src/libs/RPGCore.Projects) ![Status: Viable](https://img.shields.io/badge/viable-blue)\
[![RPGCore.FileTree](https://img.shields.io/badge/📦-RPGCore.FileTree-333333.svg)](./src/libs/RPGCore.FileTree) ![Status: Work-in-progress](https://img.shields.io/badge/work--in--progress-orange)\
[![RPGCore.World](https://img.shields.io/badge/📦-RPGCore.World-333333.svg)](./src/libs/RPGCore.World) ![Status: Work-in-progress](https://img.shields.io/badge/work--in--progress-orange)\
[![RPGCore.Behaviour](https://img.shields.io/badge/📦-RPGCore.Behaviour-333333.svg)](./src/libs/RPGCore.Behaviour) ![Status: Mockup](https://img.shields.io/badge/mockup-red)\
[![RPGCore.Inventories](https://img.shields.io/badge/📦-RPGCore.Inventories-333333.svg)](./src/libs/RPGCore.Inventories) ![Status: Mockup](https://img.shields.io/badge/mockup-red)\
[![RPGCore.Items](https://img.shields.io/badge/📦-RPGCore.Items-333333.svg)](./src/libs/RPGCore.Items) ![Status: Mockup](https://img.shields.io/badge/mockup-red)\
[![RPGCore.Traits](https://img.shields.io/badge/📦-RPGCore.Traits-333333.svg)](./src/libs/RPGCore.Traits) ![Status: Mockup](https://img.shields.io/badge/mockup-red)

## 🔍 Overview

[![Unity Version: 2019.3.0f6](https://img.shields.io/badge/Unity-2019.3.0f6-333333.svg?logo=unity)](https://unity3d.com/get-unity/download/archive)

At it's core, this project features a behaviour system that's used to create modular items and buffs. The behaviour system is setup using a visual scripting tool, shown below.

![Fire Cape Graph](./img/screenshots/FireCapeGraph.png)

RPGCore is built around a modular behaviour system. One of the core uses for this system is **modular items**.

Below is an item called the "Fire Cape". It applies the Immolate buff to it's owner, which deals damage to nearby enemies.

This graph in the game is interpreted by the tooltip system, which renders the "Fire Cape" tooltip as shown below.

![Fire Cape Tooltip](./img/screenshots/FireCapeTooltip.png)

## License

[![Creative Commons License](https://i.creativecommons.org/l/by-nc/4.0/88x31.png)](http://creativecommons.org/licenses/by-nc/4.0/)

This work is licensed under a [Creative Commons Attribution-NonCommercial 4.0 International License](http://creativecommons.org/licenses/by-nc/4.0/).
