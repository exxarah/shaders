#version 330

in vec2 pass_textureCoord;

out vec4 diffuseColor;

uniform sampler2D textureSampler;

void main() {
    diffuseColor = texture(textureSampler, pass_textureCoord);
    //diffuseColor = vec4(pass_textureCoord.x, pass_textureCoord.y, 0.0f, 1.0f);
}
