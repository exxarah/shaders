#version 330

struct Material {
    sampler2D diffuseTexture;
    vec3 diffuseFactor;
    vec3 specularFactor;
    float shininessFactor;
};

in vec2 pass_textureCoord;
in vec3 surfaceNormal;
in vec3 toLightVector;
in vec3 toCameraVector;

out vec4 out_Color;

uniform Material material;
uniform vec3 lightColor;

void main() {
    vec3 baseColor = vec3(texture(material.diffuseTexture, pass_textureCoord));
    
    // ambient
    vec3 ambient = lightColor * baseColor;
    
    // diffuse
    vec3 unitNormal = normalize(surfaceNormal);
    vec3 unitLightVector = normalize(toLightVector);
    float diff = max(dot(unitNormal, unitLightVector), 0.0f);
    vec3 diffuse = lightColor * (diff * material.diffuseFactor);
    
    // specular
    vec3 unitCameraVector = normalize(toCameraVector);
    vec3 reflectDir = reflect(-unitLightVector, unitNormal);
    float spec = pow(max(dot(unitCameraVector, reflectDir), 0.0f), material.shininessFactor);
    vec3 specular = lightColor * (spec * material.specularFactor);
    
    vec3 result = ambient * diffuse + specular;
    out_Color = vec4(result, 1.0f);
}
