#version 330 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec2 texture_coord;
layout (location = 2) in vec3 normal;

out vec2 fUv;
out vec3 out_fragPos;
out vec3 out_normal;
        
uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;        

void main(){
    gl_Position = uProjection * uView * uModel * vec4(position,1.0);
    fUv = vec2(texture_coord);
    out_fragPos = vec3(position);
    out_normal = (uModel * vec4(normal, 0.0)).xyz;
}
