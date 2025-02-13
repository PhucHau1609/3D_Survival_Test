using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FishingRod : MonoBehaviour
{
    public bool isEquipped;
    public bool isFishingAvailable;
    public bool isCasted;
    public bool isPulling;

    Animator animator;
    public GameObject baitPrefab;
    Transform baitPosition;
    GameObject baitReference;

    private void Start()
    {
        animator = GetComponent<Animator>();
        isEquipped = true;
    }

    private void OnEnable()
    {
        FishingSystem.OnFishingEnd += HandleFishingEnd;
    }

    private void OnDestroy()
    {
        FishingSystem.OnFishingEnd -= HandleFishingEnd;
    }

    public void HandleFishingEnd()
    {
        if (baitReference != null)
        {
            Destroy(baitReference);
            baitReference = null;
        }
    }

    void Update()
    {
        if (isEquipped)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("FishingArea"))
                {
                    isFishingAvailable = true;

                    // Vung cần (ném mồi) khi nhấn chuột trái nếu chưa ném mồi
                    if (Input.GetMouseButtonDown(0) && !isCasted && !isPulling)
                    {
                        WaterSource source = hit.collider.gameObject.GetComponent<FishingArea>().waterSource;
                        StartCoroutine(CastRod(hit.point, source));
                    }
                }
                else
                {
                    isFishingAvailable = false;
                }
            }
            else
            {
                isFishingAvailable = false;
            }
        }

        // Hủy thao tác câu cá khi nhấn chuột trái nếu đã ném mồi và có cá cắn câu
        if (isCasted && FishingSystem.Instance.isThereABite && Input.GetMouseButtonDown(0))
        {
            CancelFishing();
        }

        // Kéo cần câu khi nhấn chuột phải nếu có cá cắn câu
        if (isCasted && Input.GetMouseButtonDown(1) && FishingSystem.Instance.isThereABite)
        {
            PullRod();
        }

        // Hiển thị cảnh báo khi có cá cắn câu
        if (FishingSystem.Instance.isThereABite && baitReference != null)
        {
            baitReference.transform.Find("Alert").gameObject.SetActive(true);
        }
    }

    IEnumerator CastRod(Vector3 targetPosition, WaterSource source)
    {
        isCasted = true;
        animator.SetTrigger("Cast");

        // Tạo độ trễ giữa animation và khi mồi xuất hiện trong nước
        yield return new WaitForSeconds(1f);

        GameObject instantiatedBait = Instantiate(baitPrefab);
        instantiatedBait.transform.position = new Vector3(targetPosition.x, targetPosition.y + 1.0f, targetPosition.z);

        baitPosition = instantiatedBait.transform;
        baitReference = instantiatedBait;

        // Bắt đầu logic cá cắn câu
        FishingSystem.Instance.StartFishing(source);
    }

    private void PullRod()
    {
        animator.SetTrigger("Pull");
        isCasted = false;
        isPulling = true;

        // Kích hoạt minigame câu cá
        FishingSystem.Instance.SetHasPulled();
    }

    private void CancelFishing()
    {
        Debug.Log("Fishing Canceled!");

        isCasted = false;
        isPulling = false;

        // Hủy mồi câu nếu đã có
        if (baitReference != null)
        {
            Destroy(baitReference);
            baitReference = null;
        }

        // Đặt lại trạng thái trong FishingSystem
        FishingSystem.Instance.CancelFishing();
    }
}