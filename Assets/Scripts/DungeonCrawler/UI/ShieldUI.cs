using TMPro;
using UnityEngine;

namespace DungeonCrawler.UI
{
    public class ShieldUI : MonoBehaviour
    {
        [SerializeField] private Transform shieldIcon;
        [SerializeField] private TextMeshPro shieldText;

        private void Start()
        {
            Defence defence = GetComponentInParent<Defence>();
            defence.OnDefenceChanged += UpdateDefence;
            UpdateDefence(defence);
        }

        private void UpdateDefence(Defence defence)
        {
            shieldIcon.gameObject.SetActive(defence.CurrentDefence > 0);
            shieldText.gameObject.SetActive(defence.CurrentDefence > 0);
            shieldText.text = defence.CurrentDefence.ToString();
        }
    }
}