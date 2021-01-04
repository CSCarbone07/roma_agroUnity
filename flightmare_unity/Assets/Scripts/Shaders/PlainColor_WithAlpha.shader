Shader "Plant/Plain Color With Alpha"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		[HideInInspector]_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
		
		_plantBend("Bend", Vector) = (0,0,0,0)
		_bendOffset("Bend Offset", float) = 0
		_positionOffset("Position Offset", Vector) = (0,0,0,0)

    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" }
		//Tags { "Queue" = "Geometry" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _Color;
			uniform vector _plantBend;
			uniform float _bendOffset;
			uniform vector _positionOffset;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				v.vertex.xyz = v.vertex.xyz + ((1 - (pow(o.uv.y, _bendOffset)))*(pow(o.uv.y, _bendOffset)) * _plantBend) + _positionOffset;

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				fixed4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv, _MainTex));
				clip(_MainTex_var.a - 0.5);
				//fixed4 col= tex2D(_MainTex, i.uv);
				fixed4 col; // = _Color * tex2D(_MainTex, i.uv).a; //tex2D(_Color, i.uv); //
				


				col.rgb = _Color.rgb; 
				col.a = tex2D(_MainTex, i.uv).a;
				clip(col.a - 0.5);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
