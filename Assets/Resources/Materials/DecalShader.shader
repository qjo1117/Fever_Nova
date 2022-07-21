Shader "Tutorial/054_UnlitDynamicDecal"{
	// �ν����� �󿡼� ������ �� �ֵ��� �� ǥ��
	Properties{
		[HDR] _Color("Tint", Color) = (0, 0, 0, 1)
		_MainTex("Texture", 2D) = "white" {}
	}

		SubShader{
		// �ش� ���̴��� ������ ���׸����� ������ �����ϸ�, �� ���׸����� ������ ���� ���� �տ� �������˴ϴ�.
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent-400" "DisableBatching" = "True"}

		// ���İ��� ���� ȥ�� (blend)
		Blend SrcAlpha OneMinusSrcAlpha
		// �������� �־ zbuffer�� �� �� ����.
		ZWrite off

		Pass{
			CGPROGRAM

			// ������ ���̴� ��� include
			#include "UnityCG.cginc"

			// vertex shader, fragment (pixel) shader ����Ѵٰ� ����
			#pragma vertex vert
			#pragma fragment frag

			// Texture�� Texture�� Transform (��ġ����)
			sampler2D _MainTex;
			float4 _MainTex_ST;

			// �ؽ����� ����
			fixed4 _Color;

			// ���� ������ ������ global texture
			sampler2D_float _CameraDepthTexture;

			// Vertex Shader�� �д� Mesh Data
			struct appdata {
				float4 vertex : POSITION;
			};

			// �����Ͷ����� : 3d ���� ���� -> 2d�� �ȼ��� ǥ������
			// vertex���� fragment (pixel) shader�� ���޵ǰ�, �����Ͷ������� ���� �����Ǵ� ������
			struct v2f {
				float4 position : SV_POSITION;
				float4 screenPos : TEXCOORD0;
				float3 ray : TEXCOORD1;
			};

			// object space?? , world space??, clip space??

			// vertex shader �Լ�
			v2f vert(appdata v) {
				v2f o;
				// �ùٸ��� ������ �ɼ� �ֵ��� ���� ��ġ�� object space���� clip space�� ��ȯ
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.position = UnityWorldToClipPos(worldPos);
				// ī�޶�� ���� ������ ray(�Ÿ�?,���Ͱ�?) �� ���Ѵ�.
				o.ray = worldPos - _WorldSpaceCameraPos;
				// ��ȯ�� Clip Position���� Screen Position�� ���Ѵ�.
				o.screenPos = ComputeScreenPos(o.position);
				return o;
			}

			float3 getProjectedObjectPos(float2 screenPos, float3 worldRay) {
				// Screen Position�� �ش�Ǵ� ������ depth���� depthBuffer�� texture �̿��Ͽ� �˾Ƴ���.
				float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, screenPos);
				depth = Linear01Depth(depth) * _ProjectionParams.z;
				// decal�� �׷��� ��ġ�� �ٶ󺸴� ���̰� 1 ¥���� ray�� �����´�.
				worldRay = normalize(worldRay);
				// ������� 3���� ���� ī�޶��� forward vector�� ��Ÿ���⋚����, worldRay�� 3�������� ������ ���͸� dot product�ϸ� worldRay��
				// ������ �Ÿ��� �� �� �ִ�.
				worldRay /= dot(worldRay, -UNITY_MATRIX_V[2].xyz);
				// World position�� Object position�� �籸���Ѵ�.
				float3 worldPos = _WorldSpaceCameraPos + worldRay * depth;
				float3 objectPos = mul(unity_WorldToObject, float4(worldPos,1)).xyz;
				// discard pixels where any component is beyond +-0.5
				clip(0.5 - abs(objectPos));
				// -0.5 | 0.5 �� �Ǿ��ִ� ������ 0 | 1 �� �ٲ۴�. (clip space�� 0 | 1 �� �Ǿ��ֱ� ������ �ƴұ�?)
				objectPos += 0.5;
				return objectPos;
			}

			// fragment (pixel) shader �Լ�
			fixed4 frag(v2f i) : SV_TARGET{
				//unstretch screenspace uv and get uvs from function
				float2 screenUv = i.screenPos.xy / i.screenPos.w;
				float2 uv = getProjectedObjectPos(screenUv, i.ray).xz;
				//read the texture color at the uv coordinate
				  fixed4 col = tex2D(_MainTex, uv);
				  //multiply the texture color and tint color
				  col *= _Color;
				  //return the final color to be drawn on screen
				  return col;
			}

			  ENDCG
			}

		}
}

	