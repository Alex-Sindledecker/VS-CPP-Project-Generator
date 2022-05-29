#include <iostream>
#include <fstream>
#include <string>
#include <sstream>

#include <glad/glad.h>
#include <glm/gtc/matrix_transform.hpp>
#include <GLFW/glfw3.h>

//Returns 1 if succesfull
int init(GLFWwindow** window)
{
	if (glfwInit() == GLFW_FALSE)
	{
		std::cout << "Failed to init glfw!" << std::endl;
		return 0;
	}

	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 4);
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 1);
	*window = glfwCreateWindow(1280, 720, "Testing", NULL, NULL);
	if (!window)
	{
		std::cout << "Failed to create window!" << std::endl;
		return 0;
	}
	glfwMakeContextCurrent(*window);

	if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress))
	{
		std::cout << "Failed to load opengl!" << std::endl;
		return 0;
	}

	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

	glEnable(GL_DEPTH_TEST);

	return 1;
}

void terminate(GLFWwindow* window)
{
	glfwDestroyWindow(window);
	glfwTerminate();
}

GLuint loadShader(const char* shaderSource, GLenum type, char infoLog[512])
{
	GLuint shader;
	int sucess;

	std::ifstream file(shaderSource);
	std::stringstream buffer;
	buffer << file.rdbuf();
	std::string bufferStr = buffer.str();
	const char* shaderFileContents = bufferStr.c_str();

	shader = glCreateShader(type);
	glShaderSource(shader, 1, &shaderFileContents, NULL);
	glCompileShader(shader);
	glGetShaderiv(shader, GL_COMPILE_STATUS, &sucess);
	if (!sucess)
	{
		glGetShaderInfoLog(shader, sizeof(infoLog), NULL, infoLog);
		std::cout << infoLog << std::endl;
	}
	file.close();

	return shader;
}

GLuint loadShaders(const char* vertexFile, const char* fragmentFile)
{
	GLuint program, vertexShader, fragmentShader;
	char infoLog[512];
	int sucess;

	vertexShader = loadShader(vertexFile, GL_VERTEX_SHADER, infoLog);
	fragmentShader = loadShader(fragmentFile, GL_FRAGMENT_SHADER, infoLog);

	program = glCreateProgram();
	glAttachShader(program, vertexShader);
	glAttachShader(program, fragmentShader);
	glLinkProgram(program);

	glGetProgramiv(program, GL_LINK_STATUS, &sucess);
	if (!sucess)
	{
		glGetProgramInfoLog(program, sizeof(infoLog), NULL, infoLog);
		std::cout << infoLog << std::endl;
	}

	glDetachShader(program, vertexShader);
	glDetachShader(program, fragmentShader);
	glDeleteShader(vertexShader);
	glDeleteShader(fragmentShader);

	return program;
}

GLfloat vertices[] = {
	-0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
	0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
	0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
	0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
	-0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
	-0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,

	-0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
	0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
	0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
	0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
	-0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
	-0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,

	-0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
	-0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
	-0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
	-0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
	-0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
	-0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,

	0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
	0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
	0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
	0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
	0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
	0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,

	-0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
	0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
	0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
	0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
	-0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
	-0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,

	-0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
	0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
	0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
	0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
	-0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
	-0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f
};

int main()
{
	GLFWwindow* window = nullptr;

	if (!init(&window))
		return -1;


	glClearColor(0.1, 0.1, 0.1, 1.0);

	GLuint shaderProgram = loadShaders("vertex.glsl", "fragment.glsl");

	GLuint vao, vbo;

	glGenVertexArrays(1, &vao);
	glBindVertexArray(vao);
	glGenBuffers(1, &vbo);
	glBindBuffer(GL_ARRAY_BUFFER, vbo);
	glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);
	glEnableVertexAttribArray(0);
	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, sizeof(GLfloat) * 6, (void*)0);
	glEnableVertexAttribArray(1);
	glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, sizeof(GLfloat) * 6, (void*)(3 * sizeof(GLfloat)));
	glBindVertexArray(0);
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glDeleteBuffers(1, &vbo);

	glm::vec3 cameraPos(4, 3, 3), cameraFront(0, 0, -1), cameraUp(0, 1, 0), direction;
	float yaw = 2015.f, pitch = 0.f;
	float lastX = 1280.f / 2.f, lastY = 720.f / 2.f;

	while (!glfwWindowShouldClose(window))
	{
		float dt = glfwGetTime();
		glfwSetTime(0);
		double xpos, ypos;
		glfwGetCursorPos(window, &xpos, &ypos);
		glfwSetCursorPos(window, 1280.f / 2.f, 720.f / 2.f);

		float xoffset = 1280.f / 2.f - xpos;
		float yoffset = 720.f / 2.f - ypos;

		float sensitivity = 0.1f;
		xoffset *= sensitivity;
		yoffset *= sensitivity;

		yaw -= xoffset;
		pitch += yoffset;

		if (pitch > 89.0f)
			pitch = 89.0f;
		if (pitch < -89.0f)
			pitch = -89.0f;

		glm::vec3 direction;
		direction.x = cos(glm::radians(yaw)) * cos(glm::radians(pitch));
		direction.y = sin(glm::radians(pitch));
		direction.z = sin(glm::radians(yaw)) * cos(glm::radians(pitch));
		cameraFront = glm::normalize(direction);

		const float cameraSpeed = 5 * dt; // adjust accordingly
		if (glfwGetKey(window, GLFW_KEY_W) == GLFW_PRESS)
			cameraPos += cameraSpeed * cameraFront;
		if (glfwGetKey(window, GLFW_KEY_S) == GLFW_PRESS)
			cameraPos -= cameraSpeed * cameraFront;
		if (glfwGetKey(window, GLFW_KEY_A) == GLFW_PRESS)
			cameraPos -= glm::normalize(glm::cross(cameraFront, cameraUp)) * cameraSpeed;
		if (glfwGetKey(window, GLFW_KEY_D) == GLFW_PRESS)
			cameraPos += glm::normalize(glm::cross(cameraFront, cameraUp)) * cameraSpeed;
		if (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS)
			glfwSetWindowShouldClose(window, true);

		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

		glm::mat4 model(1.f);
		glm::mat4 mvp = glm::perspective(glm::radians(45.f), 1280.f / 720.f, 0.1f, 100.f) *
						glm::lookAt(cameraPos, cameraPos + cameraFront, cameraUp) *
						model;

		glUseProgram(shaderProgram);
		glUniformMatrix4fv(glGetUniformLocation(shaderProgram, "mvp"), 1, GL_FALSE, &mvp[0][0]);
		glUniformMatrix4fv(glGetUniformLocation(shaderProgram, "model"), 1, GL_FALSE, &model[0][0]);
		glBindVertexArray(vao);
		glDrawArrays(GL_TRIANGLES, 0, 36);
		glBindVertexArray(0);

		glfwSwapBuffers(window);
		glfwPollEvents();
	}

	glDeleteVertexArrays(1, &vao);

	terminate(window);

	return 0;
}