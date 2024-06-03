// 이 셰이더는 메쉬를 미리 지정된 칼라로 채우는 셰이더입니다 
Shader "ExampleS/URPUnlitShaderBasic"
{
    // 유니티 셰이더의 프로퍼티 블럭(인터페이스를 만드는 곳)입니다. 지금은 비워놨습니다. 
    // 왜냐하면 프레그먼트 (픽셀) 셰이더 코드에서 출력 칼라를 걍 정의해 놨기 때문입니다 
    Properties
    {
        // _TestFloat("테스트 플롯이라뇨", Float) = 0
        [Toggle]_TestFloat("테스트 플롯레인지", Float) = 0
        _TestFloat2("테스트 플롯레인지", Range(0,1)) = 0
        _TestTex("테스트 텍스쳐다뇨", 2D) = "white"{}
        _TestCUBE("테스트 큐브맵이다뇨", Cube) = ""{}
        _TestColor("테스트 칼라다뇨", Color) = (1,1,1,1)
        _TestVector("테스트 벡터라뇨", Vector) = (1,1,1,1)
    }

    // 섭셰이더 블럭에 셰이더 코드가 들어 있습니다 
    SubShader
    {
        // 섭셰이더 태그는 언제 어떤 조건에서 섭셰이더 블럭을 정의하는지 또는 
        //패스가 실행되는지를 정의합니다. (그냥 일종의 설정용 태그란 말입니다.) 
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            // HLSL(High Level Shader Language) 코드 블럭입니다. SRP는 HLSL를 사용합니다 
            HLSLPROGRAM
            // 여기는 버텍스 셰이더의 이름을 정의하고요
            #pragma vertex vert
            // 여기는 프레그먼트(픽셀) 셰이더의 이름을 정의합니다. 
            #pragma fragment frag

            // Core.hlsl 파일에는 자주 사용되는 HLSL 메크로나 함수가 정의되어 있습니다. 
            // 그리고 이렇게 #include를 사용하면 다른 HLSL 파일들을 참조할 수 있습니다. 
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // 이 구조체에는 어떤 변수가 들어 있는지 정의되어 있습니다. 
            // 이 예제에서는 Attributes 구조체를 버텍스 셰이더의 인풋 구조체로 사용하고 있습니다. 
            struct Attributes
            {
                // positionOS 변수는 오브젝트 스페이스의 버텍스 포지션을 가지고 있습니다 
                float4 positionOS   : POSITION;
            };

            struct Varyings
            {
                // 이 구조체의 포지션 변수는 반드시 SV_POSITION 시멘틱을 가지고 있어야 합니다. 
                float4 positionHCS  : SV_POSITION;
            };

            // 버텍스 셰이더는 Varyings의 요소로 정의됩니다. 
            // 버텍스 셰이더의 타입은 반드시 출력해 주는 구조체의 타입과 일치해야 합니다. 
            Varyings vert(Attributes IN)
            {
                //  Varyings 구조체로 출력(OUT) 선언을 해줍니다. 
                Varyings OUT;
                // TransformObjectToHClip 함수는 오브젝트 좌표계의 버텍스 포지션을 
                // 클립스페이스로 변환해줍니다. 
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                // output을 리턴해 줍니다. 
                return OUT;
            }

            // 프레그먼트 (픽셀) 셰이더 정의입니다. 
            half4 frag() : SV_Target
            {
                // 색상 정의하고 리턴해 줍니다. 
                half4 customColor = half4(0.5, 0, 0, 1);
                return customColor;
            }
            ENDHLSL
        }
    }
}