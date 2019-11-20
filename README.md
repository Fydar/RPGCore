# RPGCore

![Build Status](https://github.com/Fydar/RPGCore/workflows/.NET%20Core/badge.svg) ![Build Status](https://github.com/Fydar/RPGCore/workflows/Run%20Unit%20Tests/badge.svg)

RPGCore is a toolkit for producing RPG games and mechanics in C#.

This project is currently using **Unity 2018.2.3f1**.

![RPGCore Main Demo][MainImage]

At it's core, this project features a behaviour system that's used to create modular items and buffs. The behaviour system is setup using a visual scripting tool, shown below.

![Graph Demo][ChargingBuff]

The long-term goal of this project is to produce a toolkit for creating modular and event based mechanics for clients and servers.

## Project Status

**Prototype:** Released\
**Rewrite:** Work-in-progress...

> "Many screenshots of RPGCore are from the "Prototype" project. I don't recommend anyone use this project commercially unless they understand it and are willing to modify it to suit their needs. This code has no multiplayer compatibility (yet) and uses a lot of lambda expressions to accomplish the node connections."

The "Rewrite" project delivers the following features:

- A **high-performance** and extensible node-based **behaviour system**
- Unity-independent code-base that can run on servers
- Intuitive **API for creating custom nodes**

## Case Studies

RPGCore is built around a modular behaviour system. One of the core uses for this system is modular items.

### Fire Cape

Below is an item called the "Fire Cape". It applies the Immolate buff to it's owner, which deals damage to nearby enemies.

![Fire Cape Graph][FireCapeGraph]

This graph in the game is interpreted by the tooltip system, which renders the "Fire Cape" tooltip as shown below.

![Fire Cape Tooltip][FireCapeTooltip]

### Enchantments

The graph system can also be used to add additional behaviours to an item as used in Enchantments.

![Enchantments Tooltip][EnchantmentsTooltip]

### Buffs

The modular behaviour system has other applications, such as buffs and debuffs. Events can be used to trigger behaviours on ticks of the buff, and effects can be applied continuously throughout the duration of the buff.

![Poisoned Debuff][PosionedDebuff]

The remaining duration on the buff can also be used to drive the performance of the buff (such as a slow that weakens over time).

## License

[![License](https://img.shields.io/github/license/Fydar/RPGCore)](https://github.com/Fydar/RPGCore/blob/master/LICENSE)

This work is licensed under the Apache License, Version 2.0, meaning you are free to use this work commercially under the conditions the LICENSE and NOTICE file is included within the source.

[MainImage]: ./docs/screenshots/Main.png

[ChargingBuff]: ./docs/screenshots/ChargingBuff.png
[ImmolateBuff]: ./docs/screenshots/ImmolateBuff.png
[PosionedDebuff]: ./docs/screenshots/PosionedDebuff.png

[PoisonPotion]: ./docs/screenshots/PoisonPotion.png
[FireCapeGraph]: ./docs/screenshots/FireCapeGraph.png

[ItemIcons]: ./docs/screenshots/FireCapeTooltip.png
[FireCapeTooltip]: ./docs/screenshots/FireCapeTooltip.png
[HealthPotion]: ./docs/screenshots/HealthPotion.png
[EnchantmentsTooltip]: ./docs/screenshots/EnchantmentsTooltip.png
