using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FishingMinigame : MonoBehaviour
{
    public RectTransform fishTransform;
    public RectTransform catcherTransform;

    public bool isFishOverlapping;

    public Slider successSlider;
    private float successIncrement = 15f;
    private float failDecrement = 12f;
    private float successThreshold = 100f;
    private float failThreshold = -100f;
    private float successCounter = 0f;

    private void Update()
    {
        // Kiểm tra xem cá có nằm trong vùng catcher hay không
        isFishOverlapping = CheckOverlapping(fishTransform, catcherTransform);

        // Tính toán tiến trình bắt cá
        OverlappingCalculatiion();

        // Hủy minigame khi nhấn chuột trái
        if (Input.GetMouseButtonDown(0))
        {
            CancelMinigame();
        }
    }

    private void OverlappingCalculatiion()
    {
        // Nếu cá nằm trong vùng catcher, tăng thanh tiến trình
        if (isFishOverlapping)
        {
            successCounter += successIncrement * Time.deltaTime;
        }
        else
        {
            successCounter -= failDecrement * Time.deltaTime;
        }

        // Giới hạn giá trị trong khoảng từ failThreshold đến successThreshold
        successCounter = Mathf.Clamp(successCounter, failThreshold, successThreshold);

        // Chỉ cập nhật thanh tiến trình nếu có thay đổi
        if (successSlider.value != successCounter)
        {
            successSlider.value = successCounter;
        }

        // Kiểm tra điều kiện thắng/thua
        if (successCounter >= successThreshold)
        {
            FishingSystem.Instance.EndMinigame(true);
            ResetMinigame();
        }
        else if (successCounter <= failThreshold)
        {
            FishingSystem.Instance.EndMinigame(false);
            ResetMinigame();
        }
    }

    private bool CheckOverlapping(RectTransform rect1, RectTransform rect2)
    {
        Vector3[] corners1 = new Vector3[4];
        Vector3[] corners2 = new Vector3[4];

        rect1.GetWorldCorners(corners1);
        rect2.GetWorldCorners(corners2);

        Rect r1 = new Rect(corners1[0].x, corners1[0].y, rect1.rect.width, rect1.rect.height);
        Rect r2 = new Rect(corners2[0].x, corners2[0].y, rect2.rect.width, rect2.rect.height);

        return r1.Overlaps(r2);
    }

    private void CancelMinigame()
    {
        Debug.Log("Minigame Canceled!");

        // Đặt lại thanh trạng thái
        successCounter = 0;
        successSlider.value = 0;

        // Kết thúc minigame với thất bại
        FishingSystem.Instance.EndMinigame(false);

        // Hủy mồi câu
        FishingSystem.Instance.CancelFishing();
    }

    private void ResetMinigame()
    {
        StartCoroutine(ResetSuccessCounter());
    }

    IEnumerator ResetSuccessCounter()
    {
        while (Mathf.Abs(successCounter) > 0.1f)
        {
            successCounter = Mathf.Lerp(successCounter, 0, Time.deltaTime * 5);
            successSlider.value = successCounter;
            yield return null;
        }
        successCounter = 0;
        successSlider.value = 0;
    }
}
