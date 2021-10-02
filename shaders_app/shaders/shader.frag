#version 330 core

in vec3 colour;

out vec4 diffuseColor;

void main() {
    diffuseColor = vec4(colour, 1.0);
}
