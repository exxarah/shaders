#version 330 core

uniform vec2 u_Resolution;
uniform vec2 u_Mouse;
uniform float u_Time;

float plot(vec2 st) {
    return smoothstep(0.02, 0.0, abs(st.y - st.x));
}

void main() {
    vec2 st = gl_FragCoord.xy/u_Resolution;
    
    float y = st.x;
    
    vec3 color = vec3(y);
    
    // Plot a line
    float pct = plot(st);
    color = (1.0-pct)*color+pct*vec3(0.0, 1.0, 0.0);
    
    gl_FragColor = vec4(color, 1.0);
}
