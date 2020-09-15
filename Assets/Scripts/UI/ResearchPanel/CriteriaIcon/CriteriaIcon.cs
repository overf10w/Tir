using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class CriteriaIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Transform _tooltip;
        [SerializeField] private Vector2 _offset;
        //[SerializeField] private TextMeshProUGUI _criteriaText;

        private string _stat;
        private string _statsList;
        private string _value;

        public void Init(string stat, string statsList, string value)
        {
            _stat = stat;
            _statsList = statsList;
            _value = value;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _tooltip.gameObject.SetActive(true);
            CriteriaIconTooltip criteriaIconTooltip = _tooltip.gameObject.GetComponent<CriteriaIconTooltip>();
            criteriaIconTooltip.StatText.text = _stat;
            criteriaIconTooltip.StatsListText.text = _statsList;
            criteriaIconTooltip.ValueText.text = _value;
            Vector2 scaledOffset = new Vector2(_offset.x * transform.lossyScale.x, _offset.y * transform.lossyScale.y);
            _tooltip.position = new Vector2(transform.position.x + scaledOffset.x, transform.position.y + scaledOffset.y);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _tooltip.gameObject.SetActive(false);
        }
    }
}