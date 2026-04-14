using UnityEngine;

public class SwipeController : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private float swipeThreshold = 50f;
    private bool dragInterruptedSwipe = false;

    void Update()
    {
        // 1. Dokunma başladığında
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
            dragInterruptedSwipe = false;
        }

        // 2. Dokunma devam ederken bir item tutulursa swipe'ı iptal et
        if (DragAndDrop.IsAnyItemDragging)
        {
            dragInterruptedSwipe = true;
        }

        // 3. Dokunma bittiğinde, eğer hiçbir item tutulmadıysa kaydır
        if (Input.GetMouseButtonUp(0) && !dragInterruptedSwipe)
        {
            DetectSwipe(Input.mousePosition);
        }
    }

    void DetectSwipe(Vector2 endPosition)
    {
        float xDistance = endPosition.x - startTouchPosition.x;

        if (Mathf.Abs(xDistance) > swipeThreshold)
        {
            if (xDistance < 0) StageManager.Instance.MoveNext();
            else StageManager.Instance.MoveBack();
        }
    }
}