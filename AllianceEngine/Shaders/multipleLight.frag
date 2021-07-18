#version 330 core

#define MAX_LIGHTS 128

struct LightInfo{
    vec3 Color;
    vec3 Pos;
    float Radius;
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

void main(){
       
    vec4 texture = texture(uTexture0, out_texcoord);
    
    vec3 illumination = vec3(0.0);
    
    //calcula iluminacao
    for(int i = 0; i < MAX_LIGHTS; i++){
        
        vec3 ambient, diffuse, specular;
        
        LightInfo light = uLights[i]; 
        
        //if the object is in the light radius
        if(distance(light.Pos, out_fragPos) <= light.Radius){
        
                ambient = uMaterial.Ka * light.Color;             
                    
                //iluminacao difusa
                vec3 norm = normalize(out_normal);
                vec3 lightDir = normalize(light.Pos - out_fragPos); 
                float diff1 = max(dot(norm, lightDir), 0.0); 
                diffuse = uMaterial.Kd * diff1 * light.Color;
                
                //reflexao especular
                vec3 viewDir = normalize(out_fragPos - uViewPos);
                vec3 reflectDir = normalize(reflect(-lightDir, norm));
                float spec = pow(max(dot(viewDir, reflectDir), 0.0), uMaterial.Ns);
                specular = uMaterial.Ks * spec * light.Color;  
        }else{
                ambient = vec3(0.0);
                diffuse = vec3(0.0);
                diffuse = vec3(0.0); 
        }
        
        illumination += min(ambient + diffuse, vec3(1)) + specular;
    }
    
    frag_color = vec4(illumination, 1.0) * texture;
     
}