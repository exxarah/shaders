#version 330 core

# define PI 3.14159265359

uniform vec2 u_Resolution;
uniform vec2 u_Mouse;
uniform float u_Time;

float plot(vec2 st, float pct) {
    return smoothstep( pct-0.02, pct, st.y) - smoothstep( pct, pct+0.02, st.y);
}

void main() {
    vec2 st = gl_FragCoord.xy/u_Resolution;
    
    st.x += u_Time;
    float y = (sin(st.x) + 1.0) * 0.5;
    
    vec3 color = vec3(y);
    
    // Plot a line
    float pct = plot(st, y);
    color = (1.0-pct)*color+pct*vec3(0.0, 1.0, 0.0);
    
    gl_FragColor = vec4(color, 1.0);
}
