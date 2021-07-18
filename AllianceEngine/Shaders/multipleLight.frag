#version 330 core

#define MAX_LIGHTS 128

struct LightInfo{
    vec3 Color;
    vec3 Pos;
    float Radius;
    float Ia;
    float Il;
    float Is;
};

struct MaterialInfo {
    vec3 Ka; // coeficiente de reflexao ambiente
    vec3 Kd; // coeficiente de reflexao difusa
    vec3 Ks; // coeficiente de reflexao especular
    float Ns; // expoente de reflexao especular
};

uniform LightInfo uLights[MAX_LIGHTS];
uniform MaterialInfo uMaterial;
uniform vec3 uViewPos; // define coordenadas com a posicao da camera/observador

in vec2 out_texcoord; // recebido do vertex shader
in vec3 out_fragPos; // recebido do vertex shader
in vec3 out_normal; // recebido do vertex shader

uniform sampler2D uTexture0;

out vec4 frag_color;

vec3 calculateAmbLight(float intensity, LightInfo light){
    return uMaterial.Ka * light.Ia * light.Color * intensity;
}

vec3 calculateDiffLight(float intensity, LightInfo light){
    vec3 norm = normalize(out_normal);
    vec3 lightDir = normalize(light.Pos - out_fragPos); 
    float diff1 = max(dot(norm, lightDir), 0.0); 
    return uMaterial.Kd * diff1 * light.Il * light.Color * intensity;
}

vec3 calculateSpecLight(float intensity, LightInfo light){
    vec3 norm = normalize(out_normal);
    vec3 viewDir = normalize(uViewPos - out_fragPos);
    vec3 lightDir = normalize(light.Pos - out_fragPos); 
    vec3 reflectDir = normalize(reflect(-lightDir, norm));
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), uMaterial.Ns);
    return uMaterial.Ks * spec * light.Is * light.Color * intensity;  
}

void main(){
       
    vec4 texture = texture(uTexture0, out_texcoord);
    
    LightInfo globalLight = uLights[0];
    
    vec3 globalIllum = calculateAmbLight(1, globalLight) + calculateDiffLight(1, globalLight) + calculateSpecLight(1, globalLight);
    
    vec3 illumination = globalIllum;
        
    //calcula iluminacao
    for(int i = 1; i < MAX_LIGHTS; i++){
          
        LightInfo light = uLights[i]; 
        
        float dist = distance(light.Pos, out_fragPos);
                
        float intensity = (dist < 0.00001) ? 1 : 1/dist; 

        vec3 diffuse = calculateDiffLight(intensity, light);
        vec3 specular = calculateSpecLight(intensity, light);
                         
        //this adjustment prevents color saturation
        illumination += diffuse + specular;
    }
    
    frag_color = vec4(illumination, 1.0) * texture;
}