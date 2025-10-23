struct VertexInput {
    @location(0) position: vec3f,
    @location(1) normal: vec3f,
    @location(2) color: vec3f,
    @location(3) uv: vec2f,
}

struct VertexResult {
    @builtin(position) position: vec4f,
    @location(0) normal: vec3f,
    @location(1) color: vec3f,
    @location(2) uv: vec2f,
}

struct Perspective {
    model: mat4x4f,
    view: mat4x4f,
}

@group(0) @binding(0)
var<uniform> perspective: Perspective;

@group(1) @binding(0)
var colorTexture: texture_2d<f32>;

@group(1) @binding(1)
var colorSampler: sampler;

fn vs_process(input: VertexInput) -> VertexResult {
    var result: VertexResult;
    result.position = perspective.view * perspective.model * vec4f(input.position, 1.0);
    result.normal = input.normal;
    result.color = input.color;
    result.uv = input.uv;
    return result;
}

fn fs_process(input: VertexResult) -> vec4f {
    var color: vec3f = textureSample(colorTexture, colorSampler, input.uv).rgb;
    return vec4f(color, 1.0);
}