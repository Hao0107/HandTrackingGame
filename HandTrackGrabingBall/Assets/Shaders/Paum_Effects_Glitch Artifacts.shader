Shader "Paum/Effects/Glitch Artifacts" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_InputEffect ("Effect", 2D) = "white" {}
		_BlockSize ("Intensity", Float) = 1
		_iTime ("Time", Float) = 0
	}
	SubShader {
		LOD 100
		Tags { "RenderType" = "Opaque" }
		Pass {
			LOD 100
			Tags { "RenderType" = "Opaque" }
			GpuProgramID 17309
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float2 texcoord : TEXCOORD0;
				float4 position : SV_POSITION0;
			};
			struct fout
			{
				float3 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			// $Globals ConstantBuffers for Fragment Shader
			float _BlockSize;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _InputEffect;
			sampler2D _MainTex;
			
			// Keywords: 
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                o.texcoord.xy = v.texcoord.xy;
                tmp0 = v.vertex.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.xxxx + tmp0;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.zzzz + tmp0;
                tmp0 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp1 = tmp0.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp1 = unity_MatrixVP._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp1 = unity_MatrixVP._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp0.wwww + tmp1;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                float4 tmp3;
                tmp0.xy = _Time.yy * float2(1234.0, 3543.0);
                tmp0.xy = floor(tmp0.xy);
                tmp0.z = _ScreenParams.y * 0.0019531;
                tmp1.xyz = inp.texcoord.xyx * _ScreenParams.xyy;
                tmp0.zw = tmp1.xy / tmp0.zz;
                tmp1.x = tmp1.z * 0.3333333;
                tmp1.x = frac(tmp1.x);
                tmp1.xy = tmp1.xx > float2(0.333, 0.666);
                tmp0 = tmp0 * float4(0.015625, 0.015625, 0.0625, 0.0625);
                tmp0.zw = floor(tmp0.zw);
                tmp0.xy = tmp0.zw * float2(0.015625, 0.015625) + tmp0.xy;
                tmp1.zw = frac(tmp0.xy);
                tmp1.zw = tmp1.zw - float2(0.5, 0.5);
                tmp1.zw = tmp1.zw * float2(0.03, 0.03) + inp.texcoord.xy;
                tmp0.z = 0.0;
                tmp2 = tex2D(_InputEffect, tmp0.yz);
                tmp0 = tex2D(_InputEffect, tmp0.xy);
                tmp0.xy = tmp0.xy * _BlockSize.xx;
                tmp0.zw = tmp2.zy * _BlockSize.xx;
                tmp2.x = sin(_Time.y);
                tmp2.x = tmp2.x * 43758.55;
                tmp2.x = frac(tmp2.x);
                tmp2.xyz = tmp2.xxx * float3(0.2, 0.5, 0.14);
                tmp2.w = tmp0.w < tmp2.y;
                tmp0.zw = tmp0.zw * float2(2.5, 1.75);
                tmp0.zw = tmp0.zw < tmp2.yy;
                tmp2.yz = tmp0.xy < tmp2.xz;
                tmp0.x = tmp0.y * 1.5;
                tmp0.x = tmp0.x < tmp2.x;
                tmp0.x = uint1(tmp0.w) | uint1(tmp0.x);
                tmp0.y = uint1(tmp2.w) | uint1(tmp2.y);
                tmp0.yw = tmp0.yy ? tmp1.zw : inp.texcoord.xy;
                tmp3 = tex2D(_MainTex, tmp0.yw);
                tmp3.xz = tmp2.zz ? tmp3.yy : tmp3.xz;
                tmp2.y = dot(tmp3.xyz, float3(1.0, 1.0, 1.0));
                tmp2.xz = float2(0.0, 0.0);
                tmp0.yzw = tmp0.zzz ? tmp2.xyz : tmp3.xyz;
                tmp1.xzw = tmp1.xxx ? float3(0.0, 3.0, 0.0) : float3(3.0, 0.0, 0.0);
                tmp1.xyz = tmp1.yyy ? float3(0.0, 0.0, 3.0) : tmp1.xzw;
                tmp1.xyz = tmp0.yzw * tmp1.xyz;
                o.sv_target.xyz = tmp0.xxx ? tmp1.xyz : tmp0.yzw;
                return o;
			}
			ENDCG
		}
	}
}