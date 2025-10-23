#import "Base3D.wgsl"

@vertex
fn vs_main(input: VertexInput) -> VertexResult {
    var result = vs_process(input);
    return result;
}

@fragment
fn fs_main(input: VertexResult) -> @location(0) vec4f {
    return fs_process(input);
}