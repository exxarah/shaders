# Shaders TODO

## Minimum Required
- [X] A window to display stuff in
- [X] Have everything in place for 2d shader practice
  - [X] A square that's ready to have shaders put on it
  - [X] Any library stuff useful for that
- [ ] 2d Experimentation (these aren't for the assignment, but are groundwork)
  - [X] Fractals
  - [ ] Make a bunch of shapes, test out ShaderToy code
  - [ ] Random noise
  - [ ] Get comfortable with GLSL
- [ ] Have everything in place for 3d shader practice
  - [ ] Meshes (as in generate a cube or sphere)
  - [ ] Lights
  - [ ] Cameras
  - [ ] Add a sphere floating in space
  - [ ] Mouse input to rotate the sphere
  - [ ] Mouse input to rotate the light
- [ ] 3d Shaders (the ones for the actual assignment. Note that these are chosen based on the extensions available for the Khronos glTF format)
  - [ ] Wireframe Shader (not for glTF, should be basically built into openGL, from memory)
  - [ ] Metallic-Roughness Shader
  - [ ] Unlit Materials Shader
  - [ ] Specular Glossiness Materials Shader

## Stretch Goals
- [ ] FPS
- [ ] UI to swap between shaders  
- [ ] A bunch of nice UI stuff for live shader editing (nodes basically)
- [ ] Try out some post-processing stuff over top of a 3d scene
- [ ] Mess more with vertex shaders, waves on a plane, etc
- [ ] Export to glTF format (not with lights and cameras or anything, we just care about turning a mesh and images, into a glTF object with shader stuff built in)
- [ ] Import meshes from fbx format, look at [FbxSharp](https://github.com/izrik/FbxSharp)