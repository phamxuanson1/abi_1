using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image imageFill;
    [SerializeField] Vector3 offset;

    float hp;
    float maxHp;
    private Transform target;

    // Hàm này được gọi để setup ban đầu
    public void OnInit(float maxHp, Transform target)
    {
        this.target = target;
        this.maxHp = maxHp;
        this.hp = maxHp;

        // Cần đảm bảo imageFill không null trước khi tương tác
        if (imageFill != null)
        {
            imageFill.fillAmount = 1;
        }
    }

    public void SetNewHp(float hp)
    {
        this.hp = hp;
    }

    void Update()
    {
        // QUAN TRỌNG: Chỉ chạy đống này nếu target không bị null 
        // và imageFill đã được kéo vào
        if (target != null && imageFill != null && maxHp > 0)
        {
            imageFill.fillAmount = Mathf.Lerp(imageFill.fillAmount, hp / maxHp, Time.deltaTime * 5f);
            transform.position = target.position + offset;
        }
    }
}