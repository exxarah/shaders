#version 330 core

out vec4 fragColor;

in vec4 vertexColor;    // in from vert. same name and type

void main() {
    fragColor = vertexColor;
}
