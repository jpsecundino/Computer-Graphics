#version 330 core


in vec2 out_texcoord; // recebido do vertex shader
in vec3 out_normal; // recebido do vertex shader
in vec3 out_fragPos; // recebido do vertex shader

uniform sampler2D uTexture0;
uniform vec3 uLightColor;
uniform vec3 uLightPos; // define coordenadas de posicao da luz
uniform vec3 uKa; // coeficiente de reflexao ambiente
uniform vec3 uKd; // coeficiente de reflexao difusa

out vec4 frag_color;

void main() {
    vec3 ambient = uKa * uLightColor;             

    vec3 norm = normalize(out_normal);                  // normaliza vetores perpendiculares
    vec3 lightDir = normalize(uLightPos - out_fragPos);  // direcao da luz
    float diff = max(dot(norm, lightDir), 0.0);         // verifica limite angular (entre 0 e 90)
    vec3 diffuse = uKd * diff * uLightColor;              // iluminacao difusa
    
    vec4 texture = texture(uTexture0, out_texcoord);
    vec4 result = vec4(min((ambient + diffuse), vec3(1.0)), 1.0) * texture; // aplica iluminacao
    
    frag_color = result;

}