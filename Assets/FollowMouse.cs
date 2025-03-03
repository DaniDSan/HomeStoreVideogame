using UnityEngine;

public class FollowMouse : MonoBehaviour {
    private RectTransform rectTransform;

    void Start() {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update() {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            Input.mousePosition,
            null,
            out mousePos
        );

        rectTransform.anchoredPosition = mousePos + new Vector2(15f, 15f);
    }
}