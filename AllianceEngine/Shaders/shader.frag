#version 330 core
uniform vec3 lightPos; // define coordenadas de posicao da luz
uniform vec3 ka; // coeficiente de reflexao ambiente
uniform vec3 kd; // coeficiente de reflexao difusa

vec3 lightColor = vec3(0.9, 0.9, 0.9);

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
    
    
    vec4 texture = texture2D(uTexture0, fUv);
    vec4 result = vec4((ambient + diffuse), 1.0) * texture; // aplica iluminacao
    
    frag_color = result;

}