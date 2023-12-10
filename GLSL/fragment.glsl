#version 330 core
out vec4 FragColor;

in vec2 uv;

void main()
{
    // For simplicity, just output UV coordinates as colors
    FragColor = vec4(1.0, 1.0, 1.0, 1.0);
}