#version 330 core

////////////////////////
// Luz #1
////////////////////////
uniform vec3 uLightColor1;
uniform vec3 uLightPos1; // define coordenadas de posicao da luz
uniform float uLightRadius1;

////////////////////////
// Luz #2
////////////////////////
uniform vec3 uLightColor2;
uniform vec3 uLightPos2; // define coordenadas de posicao da luz
uniform float uLightRadius2;

uniform vec3 uKa; // coeficiente de reflexao ambiente
uniform vec3 uKd; // coeficiente de reflexao difusa


// parametros da iluminacao especular
uniform vec3 uViewPos; // define coordenadas com a posicao da camera/observador
uniform vec3 uKs; // coeficiente de reflexao especular
uniform float uNs; // expoente de reflexao especular

in vec2 out_texcoord; // recebido do vertex shader
in vec3 out_fragPos; // recebido do vertex shader
in vec3 out_normal; // recebido do vertex shader

uniform sampler2D uTexture0;

out vec4 frag_color;

void main(){
    
    vec3 ambient1, ambient2, diffuse1,diffuse2, specular1, specular2; 
    
    ////////////////////////
    // Iluminacao #1
    ////////////////////////
    
    if(distance(uLightPos1, out_fragPos) <= uLightRadius1){
        //iluminacao ambiente
        ambient1 = uKa * uLightColor1;             
    
        //iluminacao difusa
        vec3 norm = normalize(out_normal);
        vec3 lightDir = normalize(uLightPos1 - out_fragPos); 
        float diff1 = max(dot(norm, lightDir), 0.0); 
        diffuse1 = uKd * diff1 * uLightColor1;
        
        //reflexao especular
        vec3 viewDir1 = normalize(out_fragPos - uViewPos);
        vec3 reflectDir1 = normalize(reflect(-lightDir, norm));
        float spec1 = pow(max(dot(viewDir1, reflectDir1), 0.0), uNs);
        specular1 = uKs * spec1 * uLightColor1;   
    }else{
        ambient1 = vec3(0.0);
        diffuse1 = vec3(0.0);
        diffuse1 = vec3(0.0);
    }
    

    ////////////////////////
    // Iluminacao #2
    ////////////////////////
    
    if(distance(uLightPos2, out_fragPos) <= uLightRadius2){
    
        //iluminacao ambiente
        ambient2 = uKa * uLightColor2;             
    
        //iluminacao difusa
        vec3 norm = normalize(out_normal);
        vec3 lightDir2 = normalize(uLightPos2 - out_fragPos); 
        float diff2 = max(dot(norm, lightDir2), 0.0); 
        diffuse2 = uKd * diff2 * uLightColor2;
        
        //reflexao especular
        vec3 viewDir2 = normalize(out_fragPos - uViewPos);
        vec3 reflectDir2 = normalize(reflect(-lightDir2, norm));
        float spec2 = pow(max(dot(viewDir2, reflectDir2), 0.0), uNs);
        specular2 = uKs * spec2 * uLightColor2;          
        
    }else{
        ambient2 = vec3(0.0);      
        diffuse2 = vec3(0.0);
        diffuse2 = vec3(0.0);
    }
   
    ////////////////////////
    // Aplicando modelo
    ////////////////////////
    vec4 texture = texture(uTexture0, out_texcoord);
    frag_color = vec4(min(ambient1 + ambient2 + diffuse1 + diffuse2, vec3(1.0)) + specular1 + specular2, 1.0) * texture; 
}