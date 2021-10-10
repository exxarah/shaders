#version 330

struct Material {
    sampler2D diffuseTexture;
    vec3 diffuseFactor;
    vec3 specularFactor;
    float shininessFactor;
};

struct Light {
    vec3 position;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

in vec2 pass_textureCoord;
in vec3 surfaceNormal;
in vec3 toLightVector;
in vec3 toCameraVector;

out vec4 out_Color;

uniform Material material;
uniform Light light;

void main() {
    vec3 baseColor = vec3(texture(material.diffuseTexture, pass_textureCoord)) * material.diffuseFactor;
    
    // ambient
    vec3 ambient = light.ambient * baseColor;
    
    // diffuse
    vec3 unitNormal = normalize(surfaceNormal);
    vec3 unitLightVector = normalize(toLightVector);
    float diff = max(dot(unitNormal, unitLightVector), 0.0f);
    vec3 diffuse = light.diffuse * diff * baseColor;
    
    // specular
    vec3 unitCameraVector = normalize(toCameraVector);
    vec3 reflectDir = reflect(-unitLightVector, unitNormal);
    float spec = pow(max(dot(unitCameraVector, reflectDir), 0.0f), material.shininessFactor);
    vec3 specular = light.specular * (spec * material.specularFactor);
    
    vec3 result = ambient * diffuse + specular;
    out_Color = vec4(result, 1.0f);
}
