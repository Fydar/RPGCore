# RPGCore

## Introduction

RPGCore is a framework for producing RPG games and mechanics in Unity.

It's currently using **Unity 2018.2.3f1**.

![RPGCore Main Demo][MainImage]

At it's core, it features a behaviour system that's used to create items and buffs. The behaviour system is setup using a visual scripting tool, shown below.

![Graph Demo][ChargingBuff]

The longterm goal of this project is to produce a framework that creates modular and event based mechanics for clients and servers.

### Still a Protoype

RPGCore in it's current state is very much a prototype. I don't recommend anyone use this project commercially unless they understand it and are willing to modify it to suit their needs. This code has no multiplayer compatibility (yet) and uses a lot of lambda expressions to acomplish the node connections - so performance when running over 500 characters may vary.

I am currently in the process of rewriting RPGCore to run on servers and not rely on ScriptableObjects.

### Todo

- Rewrite inventory and slots systems.
- Remove lambda expressions and rework nodes system.
- Create a Unity-independant version that can run on servers.
- Make node behaviours server authoritive.
- Create an app that can be used to edit the graphs outside of Unity.

## Items

RPGCore is built around a modular behaviour system. One of the core uses for this system is modular items.

### Fire Cape

Below is an item called the "Fire Cape". It applies the Immolate buff to it's owner, which deals damage to nearby enemies.

![Fire Cape Graph][FireCapeGraph]

This graph in the game is interpreted by the tooltip system, which renders the "Fire Cape" tooltip as shown below.

![Fire Cape Tooltip][FireCapeTooltip]

## Enchantments

The graph system can also be used to add additional behaviours to an item as used in Enchantments.

![Enchantments Tooltip][EnchantmentsTooltip]

## Buffs

The modular behaviour system has other applications, such as buffs and debuffs. Events can be used to trigger behaviours on ticks of the buff, and effects can be applied continuously throughout the duration of the buff.

![Posioned Debuff][PosionedDebuff]

The remaining duration on the buff can also be used to drive the performance of the buff (such as a slow that weakens over time).

## License

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