<h1>
<img src="./src/icon.png" width="54" height="54" align="left" />
RPGCore
</h1>

[![Build Status](https://github.com/Fydar/RPGCore/workflows/Build/badge.svg)](https://github.com/Fydar/RPGCore/actions?query=workflow%3ABuild)
[![Unit Test Status](https://github.com/Fydar/RPGCore/workflows/Unit%20Tests/badge.svg)](https://github.com/Fydar/RPGCore/actions?query=workflow%3A%22Unit+Tests%22)

**RPGCore** is a toolkit for producing games and mechanics in **C#**.

<p align="center">
  <img src="./img/screenshots/Main.png" alt="RPGCore Main Demo"/>
  <sup><i>The Unity demo project.</i></sup>
</p>

## 📦 Libraries

> <sub>[![RPGCore.Data.Polymorphic](https://img.shields.io/badge/📦%20RPGCore-Data.Polymorphic-333333.svg)](./src/libs/RPGCore.Data.Polymorphic) ![Status: Viable](https://img.shields.io/badge/✔-2b83e0)</sub>\
> <sup>_Polymorphic data serialization._</sup>

> <sub>[![RPGCore.DataEditor](https://img.shields.io/badge/📦%20RPGCore-DataEditor-333333.svg)](./src/libs/RPGCore.DataEditor) ![Status: Viable](https://img.shields.io/badge/✔-2b83e0)</sub>\
> <sup>_Data file editing API._</sup>

> <sub>[![RPGCore.Events](https://img.shields.io/badge/📦%20RPGCore-Events-333333.svg)](./src/libs/RPGCore.Events) ![Status: Viable](https://img.shields.io/badge/✔-2b83e0)</sub>\
> <sup>_Value wrappers and collections with events._</sup>

> <sub>[![RPGCore.Packages](https://img.shields.io/badge/📦%20RPGCore-Packages-333333.svg)](./src/libs/RPGCore.Packages) ![Status: Viable](https://img.shields.io/badge/✔-2b83e0)</sub>\
> <sup>_Loading pre-packaged content._</sup>

> <sub>[![RPGCore.Projects](https://img.shields.io/badge/📦%20RPGCore-Projects-333333.svg)](./src/libs/RPGCore.Projects) ![Status: Viable](https://img.shields.io/badge/✔-2b83e0)</sub>\
> <sup>_Authoring pre-packaged content and build pipelines._</sup>

> <sub>[![RPGCore.FileTree](https://img.shields.io/badge/📦%20RPGCore-FileTree-333333.svg)](./src/libs/RPGCore.FileTree) ![Status: Work-in-progress](https://img.shields.io/badge/🚧-ffc62b)</sub>\
> <sup>_File system abstraction with file change events._</sup>

> <sub>[![RPGCore.World](https://img.shields.io/badge/📦%20RPGCore-World-333333.svg)](./src/libs/RPGCore.World) ![Status: Work-in-progress](https://img.shields.io/badge/🚧-ffc62b)</sub>\
> <sup>_Modular Entity-Component-System for games._</sup>

> <sub>[![RPGCore.Behaviour](https://img.shields.io/badge/📦%20RPGCore-Behaviour-333333.svg)](./src/libs/RPGCore.Behaviour) ![Status: Mockup](https://img.shields.io/badge/🔥-e83f3f)</sub>\
> <sup>_Data-driven mechanics._</sup>

> <sub>[![RPGCore.Inventories](https://img.shields.io/badge/📦%20RPGCore-Inventories-333333.svg)](./src/libs/RPGCore.Inventories) ![Status: Mockup](https://img.shields.io/badge/🔥-e83f3f)</sub>\
> <sup>_Inventories that contain, store, and move items._</sup>

> <sub>[![RPGCore.Items](https://img.shields.io/badge/📦%20RPGCore-Items-333333.svg)](./src/libs/RPGCore.Items) ![Status: Mockup](https://img.shields.io/badge/🔥-e83f3f)</sub>\
> <sup>_Modular implementation of RPG items._</sup>

> <sub>[![RPGCore.Traits](https://img.shields.io/badge/📦%20RPGCore-Traits-333333.svg)](./src/libs/RPGCore.Traits) ![Status: Mockup](https://img.shields.io/badge/🔥-e83f3f)</sub>\
> <sup>_RPG character stats and item stats._</sup>

## 🔍 Overview

[![Unity Version: 2019.3.0f6](https://img.shields.io/badge/Unity-2019.3.0f6-333333.svg?logo=unity)](https://unity3d.com/get-unity/download/archive)

At it's core, this project features a behaviour system that's used to create modular items and buffs. The behaviour system is setup using a visual scripting tool, shown below.

<p align="center">
  <img src="./img/screenshots/FireCapeGraph.png" alt="Fire Cape Graph"/>
  <sup><i>The graph editor for the 'Fire Cape' item.</i></sup>
</p>

RPGCore is built around a modular behaviour system. One of the core uses for this system is **modular items**.

Below is an item called the 'Fire Cape'. It applies the Immolate buff to it's owner, which deals damage to nearby enemies.

This graph in the game is interpreted by the tooltip system, which renders the 'Fire Cape' tooltip as shown below.

<p align="center">
  <img src="./img/screenshots/FireCapeTooltip.png" alt="Fire Cape Tooltip"/>
  <sup><i>The tooltip for the 'Fire Cape' item.</i></sup>
</p>

## License

[![Creative Commons License](https://i.creativecommons.org/l/by-nc/4.0/88x31.png)](http://creativecommons.org/licenses/by-nc/4.0/)

This work is licensed under a [Creative Commons Attribution-NonCommercial 4.0 International License](http://creativecommons.org/licenses/by-nc/4.0/).
