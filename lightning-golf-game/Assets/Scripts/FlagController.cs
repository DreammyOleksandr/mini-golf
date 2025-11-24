using UnityEngine;
using System.Collections;

public class FlagController : MonoBehaviour
{
    [Header("Animation Settings")]
    public float disappearDuration = 1.5f;
    public AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
    public AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
    
    [Header("Animation Type")]
    public bool useScaleAnimation = true;
    public bool useFadeAnimation = true;
    
    [Header("Debug")]
    public bool showDebugInfo = false;
    
    private Vector3 originalScale;
    private Renderer[] renderers;
    private Material[] originalMaterials;
    private Material[] fadeMaterials;
    private bool isDisappearing = false;
    
    void Awake()
    {
        originalScale = transform.localScale;
        SetupFadeMaterials();
    }
    
    void OnEnable()
    {
        HoleTrigger.OnLevelCompleted += HandleLevelCompleted;
    }
    
    void OnDisable()
    {
        HoleTrigger.OnLevelCompleted -= HandleLevelCompleted;
    }
    
    private void SetupFadeMaterials()
    {
        renderers = GetComponentsInChildren<Renderer>();
        
        if (useFadeAnimation && renderers.Length > 0)
        {
            originalMaterials = new Material[renderers.Length];
            fadeMaterials = new Material[renderers.Length];
            
            for (int i = 0; i < renderers.Length; i++)
            {
                originalMaterials[i] = renderers[i].material;
                
                fadeMaterials[i] = new Material(originalMaterials[i]);
                
                if (fadeMaterials[i].HasProperty("_Mode"))
                {
                    fadeMaterials[i].SetFloat("_Mode", 2);
                    fadeMaterials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    fadeMaterials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    fadeMaterials[i].SetInt("_ZWrite", 0);
                    fadeMaterials[i].DisableKeyword("_ALPHATEST_ON");
                    fadeMaterials[i].EnableKeyword("_ALPHABLEND_ON");
                    fadeMaterials[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    fadeMaterials[i].renderQueue = 3000;
                }
                
                renderers[i].material = fadeMaterials[i];
            }
        }
    }
    
    private void HandleLevelCompleted()
    {
        if (isDisappearing) return;
        
        if (showDebugInfo)
        {
            Debug.Log("Starting flag disappear animation");
        }
        
        StartCoroutine(DisappearAnimation());
    }
    
    private IEnumerator DisappearAnimation()
    {
        isDisappearing = true;
        float elapsedTime = 0f;
        
        while (elapsedTime < disappearDuration)
        {
            float progress = elapsedTime / disappearDuration;
            
            if (useScaleAnimation)
            {
                float scaleMultiplier = scaleCurve.Evaluate(progress);
                transform.localScale = originalScale * scaleMultiplier;
            }
            
            if (useFadeAnimation && fadeMaterials != null)
            {
                float alpha = fadeCurve.Evaluate(progress);
                
                for (int i = 0; i < fadeMaterials.Length; i++)
                {
                    if (fadeMaterials[i].HasProperty("_Color"))
                    {
                        Color color = fadeMaterials[i].color;
                        color.a = alpha;
                        fadeMaterials[i].color = color;
                    }
                    
                    if (fadeMaterials[i].HasProperty("_BaseColor"))
                    {
                        Color baseColor = fadeMaterials[i].GetColor("_BaseColor");
                        baseColor.a = alpha;
                        fadeMaterials[i].SetColor("_BaseColor", baseColor);
                    }
                }
            }
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        if (useScaleAnimation)
        {
            transform.localScale = Vector3.zero;
        }
        
        if (useFadeAnimation && fadeMaterials != null)
        {
            for (int i = 0; i < fadeMaterials.Length; i++)
            {
                if (fadeMaterials[i].HasProperty("_Color"))
                {
                    Color color = fadeMaterials[i].color;
                    color.a = 0f;
                    fadeMaterials[i].color = color;
                }
                
                if (fadeMaterials[i].HasProperty("_BaseColor"))
                {
                    Color baseColor = fadeMaterials[i].GetColor("_BaseColor");
                    baseColor.a = 0f;
                    fadeMaterials[i].SetColor("_BaseColor", baseColor);
                }
            }
        }
        
        if (showDebugInfo)
        {
            Debug.Log("Flag disappear animation completed");
        }
        
        gameObject.SetActive(false);
    }
    
    public void ResetFlag()
    {
        isDisappearing = false;
        transform.localScale = originalScale;
        
        if (useFadeAnimation && fadeMaterials != null)
        {
            for (int i = 0; i < fadeMaterials.Length; i++)
            {
                if (fadeMaterials[i].HasProperty("_Color"))
                {
                    Color color = fadeMaterials[i].color;
                    color.a = 1f;
                    fadeMaterials[i].color = color;
                }
                
                if (fadeMaterials[i].HasProperty("_BaseColor"))
                {
                    Color baseColor = fadeMaterials[i].GetColor("_BaseColor");
                    baseColor.a = 1f;
                    fadeMaterials[i].SetColor("_BaseColor", baseColor);
                }
            }
        }
        
        gameObject.SetActive(true);
        
        if (showDebugInfo)
        {
            Debug.Log("Flag reset to original state");
        }
    }
}