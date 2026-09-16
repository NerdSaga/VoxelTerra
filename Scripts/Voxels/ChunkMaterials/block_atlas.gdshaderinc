uniform sampler2D atlasTexture : filter_nearest, source_color;
uniform vec2 atlasPixelSize = vec2(512, 512);
uniform vec2 atlasTileSize = vec2(16, 16);

//vec2 tileUV(vec2 uv, vec2 tilePosition) {
	//return uv * vec2(atlasTileSize.x / atlasPixelSize.x, atlasTileSize.y / atlasPixelSize.y);
//}

vec4 sampleBlockAtlas(vec2 uv, vec2 tilePosition) {
	vec2 tileUV = uv * vec2(atlasTileSize.x / atlasPixelSize.x, atlasTileSize.y / atlasPixelSize.y);
	return texture(atlasTexture, tileUV).rgba; 
}