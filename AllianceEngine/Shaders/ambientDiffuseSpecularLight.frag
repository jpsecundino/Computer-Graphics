#version 330 core
uniform vec3 uLightPos; // define coordenadas de posicao da luz
uniform vec3 uKa; // coeficiente de reflexao ambiente
uniform vec3 uKd; // coeficiente de reflexao difusa

vec3 lightColor = vec3(0.9, 0.9, 0.9);

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
    vec3 ambient = uKa * lightColor;             

    vec3 norm = normalize(out_normal);
    vec3 lightDir = normalize(uLightPos - out_fragPos); 
    float diff = max(dot(norm, lightDir), 0.0); 
    vec3 diffuse = uKd * diff * lightColor;
    
    // calculando reflexao especular
    vec3 viewDir = normalize(uViewPos - out_fragPos);
    vec3 reflectDir = normalize(reflect(-lightDir, norm));
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), uNs);
    vec3 specular = uKs * spec * lightColor;             
    
    vec4 texture = texture(uTexture0, out_texcoord);
    
    // aplicando o modelo de iluminacao
    frag_color = vec4(min(ambient + diffuse, vec3(1.0)) + specular,1.0) * texture; 
    
    
}