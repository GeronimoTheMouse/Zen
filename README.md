# Zen

## Description 
Zen is a simple external CS:GO Multi Hack written from scratch in C# and C++, **without** the use of a copy-pasted SDK . Its difference over other similar programs, is the effort of hiding the low-level ugliness through abstraction. Zen is not considered a perfect project, since more improvements can be made. Additionally, Zen was created and used only with bots, and the creator does not support using  in official matchmaking.

## Features
Zen has the following features:

 - ESP
 - Glow Hack
 - Aimbot
 - Triggerbot
 - No Flash

## Recording
The ESP is being drawn in an external overlay. That gives the ability of cheating while recording your game play, by simply recording only the CS:GO Window.

## Why two projects ?
Zen is made from two projects. The first project, which is located in the "Zen CSGO Hack", is the main project, which contains all the code for the actual hack. The "CSMemLibrary" on the other hand, it's a C++ project that contains all the functions that C# could not call directly without Pinvoke. The reason that it consists of two projects, is that if C# was Pinvoking user32.dll for example, the signature of the Pinvoking could not be obfuscated. This means, that there will be always an easy-to-detect signature in the cheat, no matter what. By having an external library though, both the name of the library and the name of the functions can be changed, making it impossible to create a signature through the Pinvoke.

## Contribution
Any contribution to the project is highly encouraged.