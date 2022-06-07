#version 410

layout (location = 0) in vec3 inPos;
layout (location = 1) in vec3 inNormal;

uniform mat4 mvp;
uniform mat4 model;

out vec3 color;
out vec3 normal;
out vec3 fragPos;

void main()
{
	gl_Position = mvp * vec4(inPos, 1.0);
	color = inPos;
	normal = inNormal;
	fragPos = mat3(transpose(inverse(model))) * inNormal;
}