using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PSX : ScriptableRendererFeature
{
    class PSXRenderPass : ScriptableRenderPass
    {
        private Material psxMaterial;
        private RenderTargetHandle tempTexture;

        public PSXRenderPass(Material material)
        {
            psxMaterial = material;
            tempTexture.Init("_TemporaryColorTexture");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            // Получаем целевую текстуру в безопасном контексте
            RenderTargetIdentifier source = renderingData.cameraData.renderer.cameraColorTargetHandle;
            CommandBuffer cmd = CommandBufferPool.Get("PSX Effect");

            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.depthBufferBits = 0;

            cmd.GetTemporaryRT(tempTexture.id, opaqueDesc);
            Blit(cmd, source, tempTexture.Identifier());
            Blit(cmd, tempTexture.Identifier(), source, psxMaterial);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    [System.Serializable]
    public class PSXSettings
    {
        public Material psxMaterial;
    }

    public PSXSettings settings = new PSXSettings();
    PSXRenderPass renderPass;

    public override void Create()
    {
        renderPass = new PSXRenderPass(settings.psxMaterial)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(renderPass);
    }
}
