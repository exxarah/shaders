#version 330

in vec2 pass_textureCoord;
in vec3 surfaceNormal;
in vec3 toLightVector;

out vec4 out_Color;

uniform sampler2D modelTexture;
uniform vec3 lightColor;

void main() {
    // vec4 diffuseColor = texture(modelTexture, pass_textureCoord);
    // vec4 diffuseColor = vec4(pass_textureCoord.x, pass_textureCoord.y, 0.0f, 1.0f);
    vec4 diffuseColor = vec4(1.0f, 0.5f, 0.31f, 1.0f);
    
    vec3 unitNormal = normalize(surfaceNormal);
    vec3 unitLightVector = normalize(toLightVector);
    float nDot1 = dot(unitNormal, unitLightVector);
    float brightness = max(nDot1, 0.0f);
    vec4 brightnessColor = vec4(brightness * lightColor, 1.0f);
    
    out_Color = diffuseColor * brightnessColor;
}
