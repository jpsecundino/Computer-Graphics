#version 330 core
in vec2 fUv;

uniform sampler2D uTexture0;

uniform vec3 uKa;
uniform vec4 uLightColor;

out vec4 FragColor;

void main()
{
	vec4 ambientLight = vec4(uKa, 1) * uLightColor;
	FragColor = ambientLight * texture(uTexture0, fUv);
}