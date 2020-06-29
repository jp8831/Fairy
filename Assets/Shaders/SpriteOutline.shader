Shader "Custom/SpriteOutline"
{
    Properties
    {
        [HideInInspector]_MainTex ("Texture", 2D) = "white" {}
		_Thickness ("Outline Thickness", Range (0, 0.015)) = 0.0
		[HDR]_OutlineColor ("Outline Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
			float4 _MainTex_ST;

			float _Thickness;
			float4 _OutlineColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 textureColor = tex2D(_MainTex, i.uv) * i.color;

				fixed outlineX = tex2D (_MainTex, float2 (i.uv.x + _Thickness, i.uv.y)).a + tex2D (_MainTex, float2 (i.uv.x - _Thickness, i.uv.y)).a;
				fixed outlineY = tex2D (_MainTex, float2 (i.uv.x, i.uv.y + _Thickness)).a + tex2D (_MainTex, float2 (i.uv.x, i.uv.y - _Thickness)).a;
				fixed outline = saturate (outlineX + outlineY) - textureColor.a;

				fixed4 color;
				color.rgb = textureColor.rgb * textureColor.a;
				color.a = textureColor.a;
				color += _OutlineColor * saturate (outline);

                return color;
            }
            ENDCG
        }
    }
}
