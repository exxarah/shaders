#version 330 core

#define MAX_ITERATIONS 100

uniform vec2 u_Resolution;
uniform vec2 u_Mouse;
uniform float u_Time;

// https://www.youtube.com/watch?v=NGMRB4O922I
// https://arukiap.github.io/fractals/2019/06/02/rendering-the-mandelbrot-set-with-shaders.html
// https://physicspython.wordpress.com/2020/02/16/visualizing-the-mandelbrot-set-using-opengl-part-1/

int get_iterations()
{
    float real = (gl_FragCoord.x / u_Resolution.x - 0.75) * 3;
    float imag = (gl_FragCoord.y / u_Resolution.y - 0.5) * 3;

    int iterations = 0;
    float const_real = real;
    float const_imag = imag;

    while (iterations < MAX_ITERATIONS)
    {
        float tmp_real = real;
        real = (real * real - imag * imag) + const_real;
        imag = (2.0 * tmp_real * imag) + const_imag;

        float dist = real * real + imag * imag;

        if (dist > 4.0)
        break;

        ++iterations;
    }
    return iterations;
}

vec4 return_color()
{
    int iter = get_iterations();
    /*
    // Uncomment me to only have the outline
    if (iter == MAX_ITERATIONS)
    {
        gl_FragDepth = 0.0f;
        return vec4(0.0f, 0.0f, 0.0f, 1.0f);
    }
    */

    float iterations = float(iter) / MAX_ITERATIONS;
    return vec4(iterations, iterations, iterations, 1.0f);
}

void main()
{
    gl_FragColor = return_color();
}