Shader "Unlit/Gradient"
{
Properties
    {
        _TopColour("Top Gradient Colour: ", Color) = (1,1,1,1)
        _BottomColour("Bottom Gradient Colour: ", Color) = (0,0,0,0)
        _coef("Coefficient", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

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
                // We output the colour from each vertex for the fragment shader.
                float4 colour : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _TopColour;
            fixed4 _BottomColour;
            float _coef;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // Interpolate the colour between each vertex.
                o.colour = lerp(_BottomColour, _TopColour, pow(v.uv.y,_coef ));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // The colour will be interpolated from the vertex colours.
                return i.colour;
            }
            ENDCG
        }
    }}
