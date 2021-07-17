#version 330 core
uniform vec3 lightPos; // define coordenadas de posicao da luz
uniform vec3 ka; // coeficiente de reflexao ambiente
uniform vec3 kd; // coeficiente de reflexao difusa

vec3 lightColor = vec3(1.0, 1.0, 1.0);

in vec2 fUv; // recebido do vertex shader
in vec3 out_normal; // recebido do vertex shader
in vec3 out_fragPos; // recebido do vertex shader

uniform sampler2D uTexture0;

out vec4 frag_color;

void main()
{
    vec3 ambient = ka * lightColor;
    vec3 test = lightPos * ka * kd;
    
    frag_color = texture(uTexture0, fUv);
    
    if(ka == vec3(0.0,0.0,0.0)){
        frag_color = vec4(test,0.0);
    }else{
        frag_color = vec4(out_normal, 1.0);
    }
}