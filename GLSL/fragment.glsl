#version 330 core

out vec4 FragColor;

in vec3 theColor;

uniform vec4 ourColor;

void main()
{
    //ourColor = vec4(theColor.r, theColor.g, theColor.b, 1.0);
    FragColor = vec4(theColor, 1.0);
}