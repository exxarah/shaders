#version 330 core

out vec4 fragColor;
in vec3 thisColor;

void main() {
    fragColor = vec4(thisColor, 1.0);
}
