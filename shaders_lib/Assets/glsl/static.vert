#version 330 core

in vec3 position;
in vec2 textureCoord;

out vec2 pass_textureCoord;

uniform mat4 transformationMatrix;
uniform mat4 projectionMatrix;

void main() {
    gl_Position = projectionMatrix * transformationMatrix * vec4(position, 1.0);
    pass_textureCoord = textureCoord;
}
