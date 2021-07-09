# Alliance Engine
This project is a very basic 3D graphics engine with component based system for learning OpenGL and 3D computer graphics contents.

# Controls

## Camera Movement
W - Forward | LShift - Up

A - Left	| LCtrl  - Down

S - Right	| WheelScrollY - Look Up/Down

D - Back	| WheelScrollX - Look Left/Right

## Toogle Polygon Mode
P - Toogles between Fill and Line PolygonMode

## UI Display
Mouse Right Click - Toggles between displaying and not displaying the Hierarchy UI.

# Architecture

## Engine
Engine is the main class of the program, it controls the flow of the application.

## Scene
The scene class has a list of all the active GameObjects and handles the components callbacks.

## RenderPipeline
The RenderPipeline class takes maintains a reference to all the Renderers on the scene to draw them every frame.

## ModelManager
ModelManager return references to Meshes and instantiates new ones only when needed (avoiding memory duplication on the GPU).

## Component System

### GameObject
Every object on the scene is a GameObject that can have components attached to it.

GameObjects have a transform, used to calculate translation, rotation and scale transformations and the model matrix.

### Component
Components create behaviour through 3 callbacks:
- Start: Called while loading window
- Update: Called Every frame before renderering
- Destroy: Called at window close

## UI
We used Dear ImGUI dotnet wrapper to display and change the GameObject's transform data.

# Supported Files
## Shader
This engine supports only glsl shaders and they must be divided into two files (vertex and fragment).

## 3D Models
This engine supports only Wavefront (.obj) files and requires a .mtl file.
You can have multiple objects in the same .obj with different materials, but they will share the same Mesh and model matrix.

# Folder structure
## Models
All the models must be included in this folder and must have a separate folder each in the following hierarchy:

Models/modelName/textures/ -> contains all the texture files

Models/modelName/modelName.mtl

Models/modelName/modelName.obj

## Scripts
This folder contains all scripts in the project. 
The component classes are stored at /Component folder and the Alliance Engine code is stored at /Engine

# Dependencies
* Silk.NET for window, input, OpenGL and dear ImGui.
* SixLabors ImageSharp for reading image files.