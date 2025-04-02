using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlitchFeature : ScriptableRendererFeature
{

    public static GlitchFeature Instance;

    [System.Serializable]
    public class GlitchSettings
    {
        public Material glitchMaterial;
    }

    public GlitchSettings settings = new GlitchSettings();

    public bool glitchEnabled = true;

    class GlitchPass : ScriptableRenderPass
    {
        public Material glitchMaterial;

        private RenderTargetIdentifier source;
        private RenderTargetHandle tempTexture;

        public GlitchPass(Material material)
        {
            glitchMaterial = material;
            tempTexture.Init("_TemporaryColorTexture");
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            source = renderingData.cameraData.renderer.cameraColorTarget;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (glitchMaterial == null)
                return;
            if (!renderingData.cameraData.camera.CompareTag("Glitch"))
                return;

            CommandBuffer cmd = CommandBufferPool.Get("GlitchPass");
            RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
            desc.depthBufferBits = 0;

            cmd.GetTemporaryRT(tempTexture.id, desc);
            cmd.Blit(source, tempTexture.Identifier(), glitchMaterial);
            cmd.Blit(tempTexture.Identifier(), source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tempTexture.id);
        }
    }

    GlitchPass glitchPass;

    public override void Create()
    {
        Instance = this;
        glitchPass = new GlitchPass(settings.glitchMaterial)
        {
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (!glitchEnabled)
            return;
        if (settings.glitchMaterial == null)
            return;

        renderer.EnqueuePass(glitchPass);
    }
}
