using UnityEngine;
using System.Collections;
using TMPro; // 文字を表示するためにこれを追加！

public class CheekAction : MonoBehaviour
{
    // 全ての頬で共有される「合計回数」
    private static int totalPuniCount = 0;

    // 画面上の文字を表示する部品を紐付けるための枠
    [SerializeField] TextMeshProUGUI countDisplay;

    [Header("揺れの設定")]
    [SerializeField] float duration = 0.4f;
    [SerializeField] float scalePower = 0.15f;
    [SerializeField] float movePower = 0.03f;

    private Vector3 originalScale;
    private Vector3 originalPosition;
    private bool isAnimating = false;

    void Start()
    {
        originalScale = transform.localScale;
        originalPosition = transform.localPosition;

        // 最初に現在の回数を表示（0回）
        UpdateDisplayText();
    }

    void OnMouseDown()
    {
        // クリックされたら回数を増やす
        totalPuniCount++;

        // 画面の文字を更新する
        UpdateDisplayText();

        if (!isAnimating)
        {
            StartCoroutine(JiggleEffect());
        }
    }

    // 表示を更新する専用の命令（同じことを二回書かないために分けます）
    void UpdateDisplayText()
    {
        if (countDisplay != null)
        {
            countDisplay.text = "あなたは「" + totalPuniCount + "」回らうらちゃんをぷにりました\n" + totalPuniCount + " ぷに";
        }
    }

    IEnumerator JiggleEffect()
    {
        isAnimating = true;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;
            float damping = 1.0f - percent;
            float scaleX = Mathf.Sin(elapsed * 25f) * scalePower * damping;
            transform.localScale = originalScale + new Vector3(scaleX, -scaleX, 0);
            float moveX = Mathf.Sin(elapsed * 30f) * movePower * damping;
            transform.localPosition = originalPosition + new Vector3(moveX, 0, 0);
            yield return null;
        }
        transform.localScale = originalScale;
        transform.localPosition = originalPosition;
        isAnimating = false;
    }
}