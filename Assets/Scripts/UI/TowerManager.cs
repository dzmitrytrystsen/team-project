using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject tower;
    GameObject instance;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (instance == null)
        {
            instance = Instantiate(tower, transform.position, Quaternion.identity);
            instance.GetComponent<DragManager>().draged = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        instance.GetComponent<DragManager>().draged = false;
        instance = null;
    }
}
