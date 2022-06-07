#version 410

in vec3 color;
in vec3 normal;
in vec3 fragPos;

out vec4 outColor;

void main()
{
	float ambientStrength = 0.3;
    vec3 ambient = ambientStrength * vec3(1, 1, 0);

	vec3 norm = normalize(normal);
	vec3 lightDir = normalize(vec3(1.2, 1.0, 2.0) - fragPos);  

	float diff = max(dot(norm, lightDir), 0.0);
	vec3 diffuse = diff * vec3(1, 1, 1);

	vec3 result = (ambient + diffuse) * vec3(1, 1, 0);

	outColor = vec4(result, 1.0);
}