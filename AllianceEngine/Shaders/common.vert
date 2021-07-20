#version 330 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec2 texture_coord;
layout (location = 2) in vec3 normal;

out vec2 out_texcoord;
out vec3 out_fragPos;
out vec3 out_normal;

        
uniform mat4 uModel;
uniform mat4 uViewProjection;  

void main(){
    gl_Position = uViewProjection * uModel * vec4(position, 1.0);
    out_texcoord = texture_coord;
    out_fragPos = (uModel * vec4(vec3(position), 1.0)).xyz;
    out_normal = (uModel * vec4(normal, 0.0)).xyz;
}
