using UnityEngine;
using UnityEngine.UI;
public class CombatText : MonoBehaviour
{
    [SerializeField] Text hpText;

    public void OnInit(float damege)
    {
        hpText.text = damege.ToString();
        Invoke(nameof(OnDespawn), 1f);
    }

    public void OnDespawn()
    {
        Destroy(gameObject);
    }
}