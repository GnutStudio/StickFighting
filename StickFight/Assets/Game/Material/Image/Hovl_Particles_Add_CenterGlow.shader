Shader "Hovl/Particles/Add_CenterGlow"
{
  Properties
  {
    _MainTex ("MainTex", 2D) = "white" {}
    _Noise ("Noise", 2D) = "white" {}
    _Flow ("Flow", 2D) = "white" {}
    _Mask ("Mask", 2D) = "white" {}
    _SpeedMainTexUVNoiseZW ("Speed MainTex U/V + Noise Z/W", Vector) = (0,0,0,0)
    _DistortionSpeedXYPowerZ ("Distortion Speed XY Power Z", Vector) = (0,0,0,0)
    _Emission ("Emission", float) = 2
    _Color ("Color", Color) = (0.5,0.5,0.5,1)
    [Toggle] _Usecenterglow ("Use center glow?", float) = 0
    [MaterialToggle] _Usedepth ("Use depth?", float) = 0
    [MaterialToggle] _Usecustomrandom ("Use Custom Random?", float) = 0
    _Depthpower ("Depth power", float) = 1
    [Enum(Cull Off,0, Cull Front,1, Cull Back,2)] _CullMode ("Culling", float) = 0
    [Enum(One,1,OneMinuSrcAlpha,6)] _Blend2 ("Blend mode subset", float) = 1
    [HideInInspector] _texcoord ("", 2D) = "white" {}
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "PreviewType" = "Plane"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "PreviewType" = "Plane"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      ZWrite Off
      Cull Off
      ColorMask RGB
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      //uniform float4 _Time;
      uniform float4 _MainTex_ST;
      uniform float _Usecenterglow;
      uniform float4 _SpeedMainTexUVNoiseZW;
      uniform float4 _DistortionSpeedXYPowerZ;
      uniform float4 _Flow_ST;
      uniform float4 _Mask_ST;
      uniform float4 _Noise_ST;
      uniform float4 _Color;
      uniform float _Emission;
      uniform float _Usecustomrandom;
      uniform sampler2D _Mask;
      uniform sampler2D _Flow;
      uniform sampler2D _MainTex;
      uniform sampler2D _Noise;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float4 color :COLOR0;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 color :COLOR0;
          float4 texcoord :TEXCOORD0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 color :COLOR0;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          out_v.vertex = UnityObjectToClipPos(in_v.vertex);
          out_v.color = in_v.color;
          out_v.texcoord = in_v.texcoord;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 u_xlat0_d;
      float4 u_xlat16_0;
      float4 u_xlat1_d;
      float4 u_xlat16_1;
      float4 u_xlat2;
      float4 u_xlat16_2;
      float4 u_xlat3;
      float2 u_xlat8;
      float2 u_xlat16_8;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.xy = TRANSFORM_TEX(in_f.texcoord.xy, _MainTex);
          u_xlat0_d.xy = ((_Time.yy * _SpeedMainTexUVNoiseZW.xy) + u_xlat0_d.xy);
          u_xlat8.xy = TRANSFORM_TEX(in_f.texcoord.xy, _Flow);
          u_xlat8.xy = ((_Time.yy * _DistortionSpeedXYPowerZ.xy) + u_xlat8.xy);
          u_xlat16_8.xy = tex2D(_Flow, u_xlat8.xy).xy;
          u_xlat1_d.xy = TRANSFORM_TEX(in_f.texcoord.xy, _Mask);
          u_xlat16_1 = tex2D(_Mask, u_xlat1_d.xy);
          u_xlat8.xy = (u_xlat16_8.xy * u_xlat16_1.xy);
          u_xlat0_d.xy = (((-u_xlat8.xy) * _DistortionSpeedXYPowerZ.zz) + u_xlat0_d.xy);
          u_xlat16_0 = tex2D(_MainTex, u_xlat0_d.xy);
          u_xlat2.xy = TRANSFORM_TEX(in_f.texcoord.xy, _Noise);
          u_xlat2.xy = ((float2(float2(_Usecustomrandom, _Usecustomrandom)) * in_f.texcoord.ww) + u_xlat2.xy);
          u_xlat2.xy = ((_Time.yy * _SpeedMainTexUVNoiseZW.zw) + u_xlat2.xy);
          u_xlat16_2 = tex2D(_Noise, u_xlat2.xy);
          u_xlat3 = (u_xlat16_0 * u_xlat16_2);
          u_xlat3 = (u_xlat3 * _Color);
          u_xlat3 = (u_xlat3 * in_f.color);
          u_xlat0_d = (u_xlat16_0.wwww * u_xlat3);
          u_xlat0_d = (u_xlat16_2.wwww * u_xlat0_d);
          u_xlat0_d = (u_xlat0_d * _Color.wwww);
          u_xlat0_d = (u_xlat0_d * in_f.color.wwww);
          u_xlat2.x = ((-in_f.texcoord.z) + 1);
          u_xlat2 = (u_xlat16_1 + (-u_xlat2.xxxx));
          #ifdef UNITY_ADRENO_ES3
          u_xlat2 = min(max(u_xlat2, 0), 1);
          #else
          u_xlat2 = clamp(u_xlat2, 0, 1);
          #endif
          u_xlat1_d = (u_xlat16_1 * u_xlat2);
          #ifdef UNITY_ADRENO_ES3
          u_xlat1_d = min(max(u_xlat1_d, 0), 1);
          #else
          u_xlat1_d = clamp(u_xlat1_d, 0, 1);
          #endif
          u_xlat1_d = ((u_xlat0_d * u_xlat1_d) + (-u_xlat0_d));
          u_xlat0_d = ((float4(_Usecenterglow, _Usecenterglow, _Usecenterglow, _Usecenterglow) * u_xlat1_d) + u_xlat0_d);
          u_xlat0_d = (u_xlat0_d * float4(_Emission, _Emission, _Emission, _Emission));
          out_f.color = u_xlat0_d;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
