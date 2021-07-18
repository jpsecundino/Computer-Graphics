#version 330 core
uniform vec3 lightPos; // define coordenadas de posicao da luz
uniform vec3 ka; // coeficiente de reflexao ambiente
uniform vec3 kd; // coeficiente de reflexao difusa

vec3 lightColor = vec3(1, 1, 1);

// parametros da iluminacao especular
uniform vec3 viewPos; // define coordenadas com a posicao da camera/observador
uniform float ks; // coeficiente de reflexao especular
uniform float ns; // expoente de reflexao especular

in vec2 fUv; // recebido do vertex shader
in vec3 out_normal; // recebido do vertex shader
in vec3 out_fragPos; // recebido do vertex shader

uniform sampler2D uTexture0;

out vec4 frag_color;

void main(){
    vec3 ambient = ka * lightColor;             

    vec3 norm = normalize(out_normal); // normaliza vetores perpendiculares
    vec3 lightDir = normalize(lightPos - out_fragPos); // direcao da luz
    float diff = max(dot(norm, lightDir), 0.0); // verifica limite angular (entre 0 e 90)
    vec3 diffuse = kd * diff * lightColor; // iluminacao difusa
    
    // calculando reflexao especular
    vec3 viewDir = normalize(viewPos - out_fragPos); // direcao do observador/camera
    vec3 reflectDir = normalize(reflect(-lightDir, norm)); // direcao da reflexao
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), ns);
    vec3 specular = ks * spec * lightColor;             
    
    vec4 texture = texture2D(uTexture0, fUv);
    
    if(ka = vec3(0.5,0.5,0.5)){
        // aplicando o modelo de iluminacao
        vec4 result = vec4((ambient + diffuse + specular),1.0) * texture; // aplica iluminacao
        frag_color = result; 
    }else(){
        vec4 result = vec4((ambient + diffuse),1.0) * texture; // aplica iluminacao
        frag_color = result; 
    }
    
}