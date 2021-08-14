Shader "Unlit/Waves"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
                float  distance : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float distance : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex.y=0.001;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.vertex.xz;
                o.distance = v.distance;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float AddWave(v2f i, float timeOffset)
            {
               float time = _Time.x+timeOffset;
                
                float wave = (sin(i.distance+time*10)-0.9)*10;

                i.uv.x+=time;
                
                float gradient = saturate(2-i.distance/2);

                gradient*=tex2D(_MainTex,i.uv*0.01);
                
                wave = lerp(0,wave,gradient);

                return saturate(wave);
            }

            
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the tex ture
                clip(i.distance);

                float wave = 0;

                wave+=AddWave(i,0);
                wave+=AddWave(i,2.32872);
                wave+=AddWave(i,-10.9023);
                

wave= wave>0.5?1:0;

clip(wave-0.5);
                
                return 1;
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
