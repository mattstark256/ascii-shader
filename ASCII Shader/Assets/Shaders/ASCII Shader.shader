Shader "Image Effect Shaders/ASCII Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_CharacterTex("Character Texture", 2D) = "white" {}

		_CellSize("Cell Size", Vector) = (8, 12, 0, 0)
		_CharacterCount("Character Count", int) = 10
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
			sampler2D _CharacterTex;
			float4 _CellSize;
			int _CharacterCount;

            fixed4 frag (v2f i) : SV_Target
            {
				// Get pixel coordinates
				float2 pixelCoOrds = round(float2(i.uv.x * (_ScreenParams.x - 1), i.uv.y * (_ScreenParams.y - 1)));

				// Get the coordinates relative to the character
				float2 characterCoOrds = float2(pixelCoOrds.x % _CellSize.x, pixelCoOrds.y % _CellSize.y);

				// Get the character's position
				float2 characterPos = float2((pixelCoOrds.x - characterCoOrds.x) / _CellSize.x, (pixelCoOrds.y - characterCoOrds.y) / _CellSize.y);

				// Sample the camera texture, taking an average of four points
				float2 texCoOrds1 = characterPos * _CellSize / _ScreenParams.xy;
				float2 texCoOrds2 = (characterPos + float2(0.5, 0)) * _CellSize / _ScreenParams.xy;
				float2 texCoOrds3 = (characterPos + float2(0.5, 0.5)) * _CellSize / _ScreenParams.xy;
				float2 texCoOrds4 = (characterPos + float2(0, 0.5)) * _CellSize / _ScreenParams.xy;
				fixed4 centerCol =(
					tex2D(_MainTex, texCoOrds1) +
					tex2D(_MainTex, texCoOrds2) +
					tex2D(_MainTex, texCoOrds3) +
					tex2D(_MainTex, texCoOrds4)) / 4;

				// Use the sampled color to get a brightness value from 0 to _CharacterCount - 1
				float brightness = (centerCol.r + centerCol.g + centerCol.b) / 3;
				brightness *= 0.8f; // handle HDR?
				brightness += ((characterPos.x + characterPos.y) % 2 * 0.5 - 0.25) / _CharacterCount; // Dither
				brightness = clamp(brightness, 0, 1);
				brightness = (brightness == 1) ? _CharacterCount - 1 : floor(brightness * _CharacterCount);

				// Sample the character texture
				float2 characterTexPixel = float2(brightness * _CellSize.x + characterCoOrds.x, characterCoOrds.y);
				float2 characterUV = float2((characterTexPixel.x + 0.5) / (_CellSize.x * _CharacterCount), (characterTexPixel.y + 0.5) / _CellSize.y);
				fixed4 col = tex2D(_CharacterTex, characterUV);

                return col;
            }
            ENDCG
        }
    }
}
