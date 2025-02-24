using UnityEngine;
using System.Collections;

public class ThunderEffect : MonoBehaviour
{
    private AudioSource thunderAudio; // Audio của sét
    private bool isRaining = false; // Kiểm tra mưa có đang xảy ra không

    private void Awake()
    {
        thunderAudio = GetComponent<AudioSource>(); // Lấy AudioSource trên GameObject
    }

    public void StartRain() // Gọi khi mưa bắt đầu
    {
        isRaining = true;
        PlayThunder(); // Phát âm thanh sét ngay khi mưa bắt đầu
        StartCoroutine(ThunderRoutine()); // Bắt đầu phát sét ngẫu nhiên
    }

    public void StopRain() // Gọi khi mưa kết thúc
    {
        isRaining = false;
        StopCoroutine(ThunderRoutine());
    }

    private void PlayThunder()
    {
        if (!thunderAudio.isPlaying)
        {
            thunderAudio.Play(); // Phát âm thanh sét nếu chưa phát
        }
    }

    private IEnumerator ThunderRoutine()
    {
        while (isRaining)
        {
            yield return new WaitForSeconds(Random.Range(10f, 30f)); // Ngẫu nhiên mỗi 10-30 giây
            PlayThunder();
        }
    }
}
