using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(PostProcessOutlineRenderer), PostProcessEvent.BeforeStack, "Roystan/Post Process Outline")]
public sealed class PostProcessOutline : PostProcessEffectSettings
{
    public IntParameter scale = new IntParameter { value = 1 };

    public FloatParameter depthThreshold = new FloatParameter { value = 1.5f };

    [Range(0, 1)]
    public FloatParameter normalThreshhold = new FloatParameter { value = 0.4f };

    [Range(0, 1)]
    public FloatParameter depthNormalThreshold = new FloatParameter { value = 0.5f };
    public FloatParameter depthNormalThresholdScale = new FloatParameter { value = 7 };

    public ColorParameter color = new ColorParameter { value = Color.black };
}

public sealed class PostProcessOutlineRenderer : PostProcessEffectRenderer<PostProcessOutline>
{

    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Roystan/Outline Post Process"));
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);

        Matrix4x4 clipToView = GL.GetGPUProjectionMatrix(context.camera.projectionMatrix, true).inverse;

        sheet.properties.SetMatrix("_ClipToView", clipToView);

        sheet.properties.SetFloat("_Scale", settings.scale);

        sheet.properties.SetFloat("_DepthThreshold", settings.depthThreshold);

        sheet.properties.SetFloat("_NormalThreshold", settings.normalThreshhold);

        sheet.properties.SetFloat("_DepthNormalThreshold", settings.depthNormalThreshold);

        sheet.properties.SetFloat("_DepthNormalThreshholdScale", settings.depthNormalThresholdScale);

        sheet.properties.SetColor("_Color", settings.color);
    }
}