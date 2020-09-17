using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Transform ToolTip { get; set; }

        public PlayerStat Skill { get; set; }

        [SerializeField] private Vector2 _offset;

        public void OnPointerEnter(PointerEventData eventData)
        {
            ToolTip.gameObject.SetActive(true);
            ToolTip.gameObject.GetComponent<SkillIconTooltip>().Init(Skill);

            Vector2 scaledOffset = new Vector2(_offset.x * transform.lossyScale.x, _offset.y * transform.lossyScale.y);
            ToolTip.position = new Vector2(transform.position.x + scaledOffset.x, transform.position.y + scaledOffset.y);
            Debug.Log("Hello friend");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ToolTip.gameObject.SetActive(false);
            Debug.Log("Bye friend");
        }
    }
}