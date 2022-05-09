Shader "Custom/SurfaceShader double side lerp"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Color2 ("Diffuse Color 2", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _MainTex2 ("Diffuse map 2", 2D) = "white" {}
        _Cutoff("Alpha cutoff", Range(0,1)) = 0.5



	    _Power("Power", Float) = 1 
        _Multiplier("Multiplier", Float) = 1
	    _Buffer("Buffer", Float) = 0  
        //_Buffer("Buffer", Range(-10,10)) = 0  

        _TexNoise ("Noise", 2D) = "black" {}
        _NoiseOffset("NoiseOffset", Range(-1,1)) = 0

        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Cull Off

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows // alpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _TexNoise;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_TexNoise;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _Color2;
        float _Cutoff;
        float _Power;
        float _Buffer;
        float _Multiplier;
        float _NoiseOffset;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            float4 layer_1 = pow(tex2D (_MainTex, IN.uv_MainTex),_Power) *  _Color * _Multiplier + float4 (_Buffer,_Buffer,_Buffer,0);
            float4 noiseIn = tex2D (_TexNoise, IN.uv_TexNoise) + _NoiseOffset;
            noiseIn = clamp(noiseIn,0,1);
            //float4 c = lerp(layer_1,_Color2,_Noise);
            float4 c = lerp(layer_1,_Color2,noiseIn);
            
            //float ca = tex2D(_MainTex, IN.uv_MainTex).a;

            //float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex)) * (_NoiseOffset * (1,1) + tex2D(_TexNoise, TRANSFORM_TEX(i.uv0, _TexNoise))) + tex2D(_MainTex2,TRANSFORM_TEX(i.uv0, _MainTex2)) * ((1,1) - (_NoiseOffset * (1,1) + tex2D(_TexNoise, TRANSFORM_TEX(i.uv0, _TexNoise))));

            o.Albedo = c.rgb;



            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            //o.Alpha = c.a;
            o.Alpha = 1;
/*
            if (ca > _Cutoff)
                o.Alpha = c.a;
            else
                o.Alpha = 0;
*/
            
        }

        //#pragma vertex vert
        //#pragma fragment frag

/*
        float4 frag(Input i) : COLOR
        {
            float4 tex = tex2Dproj(_MainTex, i.uv_MainTex);
            
            tex.a = 1 - tex.a;
            if (i.uv.w < 0)
            {
                tex = float4(0,0,0,1);
            }
            
            return tex;
        }
*/



        ENDCG
    }
    FallBack "Diffuse"
}
