using UnityEngine;
using System.Collections;
using TMPro;

public class CheekAction : MonoBehaviour
{
    // 将来的にSpineのアニメーションコンポーネントへの参照をここに追加予定
    // public SkeletonAnimation spineAnimation; 

    public float jiggleDuration = 0.5f;
    public float jiggleStrength = 0.1f;
    public TextMeshProUGUI countDisplay;

    private Vector3 originalScale;
    private bool isAnimating = false;
    private int pokeCount = 0;
    private const string POKE_COUNT_KEY = "PokeCount";

    void Start()
    {
        originalScale = transform.localScale;
        
        // 保存されたカウントを読み込む
        pokeCount = PlayerPrefs.GetInt(POKE_COUNT_KEY, 0);
        UpdateCountDisplay();
    }

    void OnMouseDown()
    {
        // 演出中はクリックを受け付けない（連打防止をしたい場合）
        // if (isAnimating) return; 

        // ロジック: カウントアップと保存
        IncrementPokeCount();

        // 演出: アニメーション再生
        PlayPokeAnimation();

        // 演出: サウンド再生
        PlayPokeSound();
    }

    private void IncrementPokeCount()
    {
        pokeCount++;
        PlayerPrefs.SetInt(POKE_COUNT_KEY, pokeCount);
        PlayerPrefs.Save();
        UpdateCountDisplay();
    }

    private void UpdateCountDisplay()
    {
        if (countDisplay != null)
        {
            countDisplay.text = pokeCount + "ぷに";
        }
    }

    // --- Future Proofing Hooks ---

    /// <summary>
    /// 頬を突ついたときのアニメーションを再生する
    /// </summary>
    private void PlayPokeAnimation()
    {
        // 現在はTween的な処理で揺らしているが、
        // 将来的にはここでSpineのアニメーションを再生する処理に差し替える
        // ex: spineAnimation.AnimationState.SetAnimation(0, "poke", false);
        
        if (!isAnimating)
        {
            StartCoroutine(JiggleEffect());
        }
    }

    /// <summary>
    /// 頬を突ついたときの効果音を再生する
    /// </summary>
    private void PlayPokeSound()
    {
        // 将来的なオーディオ実装用プレースホルダー
        // ex: AudioManager.PlaySE("poke_sound");
    }

    // --- Legacy Animation (To be replaced by Spine) ---

    private IEnumerator JiggleEffect()
    {
        isAnimating = true;
        float elapsed = 0f;

        while (elapsed < jiggleDuration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / jiggleDuration;
            
            // シンプルなサイン波で縮んだり伸びたりさせる
            float scaleMultiplier = 1.0f + Mathf.Sin(percent * Mathf.PI * 2) * jiggleStrength * (1.0f - percent);
            
            transform.localScale = originalScale * scaleMultiplier;
            yield return null;
        }

        transform.localScale = originalScale;
        isAnimating = false;
    }
}