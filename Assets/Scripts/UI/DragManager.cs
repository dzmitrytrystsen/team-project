using UnityEngine;

public class DragManager : MonoBehaviour
{
    public static DragManager instance;
    public LayerMask layer;
    public bool draged;

    private void Start()
    {
        instance = this;
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (draged)
        {
            if (Physics.Raycast(ray, out hit, 100, layer))
            {
                transform.position = hit.point + new Vector3(0.0f, 0.5f, 0.0f);
            }
        }
    }

    private void OnMouseDrag()
    {
        draged = true;
    }

    private void OnMouseUp()
    {
        draged = false;
    }
}
