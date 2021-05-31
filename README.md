<h1>
<img src="./src/icon.png" width="54" height="54" align="left" />
RPGCore
</h1>

[![Build Status](https://github.com/Fydar/RPGCore/workflows/Build/badge.svg)](https://github.com/Fydar/RPGCore/actions?query=workflow%3ABuild)
[![Unit Test Status](https://github.com/Fydar/RPGCore/workflows/Unit%20Tests/badge.svg)](https://github.com/Fydar/RPGCore/actions?query=workflow%3A%22Unit+Tests%22)

RPGCore is a toolkit for producing RPG games and mechanics in C#.

![RPGCore Main Demo](./img/screenshots/Main.png)

- [RPGCore.Data](./src/libs/RPGCore.Data/README.md)

## The "Rewrite"

[![Unity Version: 2019.3.0f6](https://img.shields.io/badge/Unity-2019.3.0f6-333333.svg?logo=unity)](https://unity3d.com/get-unity/download/archive) [![Status: Work-in-progress](https://img.shields.io/badge/status-work--in--progress-orange)](https://github.com/Fydar/RPGCore/projects/1)

The "Rewrite" project delivers the following features:

- A **high-performance** and extensible node-based **behaviour system**
- Unity-independent code-base that can run on servers
- Intuitive **API for creating custom nodes**

![Rewrite Editor](./img/wip-editor.png)

## The "Prototype"

[![Unity Version: 2019.3.0f6](https://img.shields.io/badge/Unity-2019.3.0f6-333333.svg?logo=unity)](https://unity3d.com/get-unity/download/archive) ![Status: Work-in-progress](https://img.shields.io/badge/status-released-brightgreen)

> "Many screenshots of RPGCore are from the "Prototype" project. I don't recommend anyone use this project commercially unless they understand it and are willing to modify it to suit their needs. This code has no multiplayer compatibility (yet) and uses a lot of lambda expressions to accomplish the node connections."

At it's core, this project features a behaviour system that's used to create modular items and buffs. The behaviour system is setup using a visual scripting tool, shown below.

RPGCore is built around a modular behaviour system. One of the core uses for this system is **modular items**.

Below is an item called the "Fire Cape". It applies the Immolate buff to it's owner, which deals damage to nearby enemies.

![Fire Cape Graph](./img/screenshots/FireCapeGraph.png)

This graph in the game is interpreted by the tooltip system, which renders the "Fire Cape" tooltip as shown below.

![Fire Cape Tooltip](./img/screenshots/FireCapeTooltip.png)

## License

[![Creative Commons License](https://i.creativecommons.org/l/by-nc/4.0/88x31.png)](http://creativecommons.org/licenses/by-nc/4.0/)

This work is licensed under a [Creative Commons Attribution-NonCommercial 4.0 International License](http://creativecommons.org/licenses/by-nc/4.0/).
