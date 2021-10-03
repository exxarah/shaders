#version 330 core

in vec3 position;
in vec2 textureCoord;

out vec2 pass_textureCoord;

void main() {
    gl_Position = vec4(position, 1.0);
    pass_textureCoord = textureCoord;
}
