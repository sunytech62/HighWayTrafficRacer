using UnityEngine;
using UnityEngine.EventSystems;

public class HR_UI_MobileDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler/*, IDragHandler, IEndDragHandler*/
{
    //[SerializeField] HR_Camera_Showroom showroomCamera;
    [SerializeField] GarageCam garageCam;

    private void Awake()
    {
        if (!garageCam) garageCam = FindFirstObjectByType<GarageCam>();
        //  if (!garageCam) showroomCamera = FindFirstObjectByType<HR_Camera_Showroom>();
    }
    /* public void OnDrag(PointerEventData data)
     {
         garageCam.OnPointerDown();
         if (showroomCamera) showroomCamera.OnDrag(data);
     }
     public void OnEndDrag(PointerEventData data)
     {
         garageCam.PnPointerUp();
     }*/

    public void OnPointerUp(PointerEventData eventData)
    {
        garageCam.OnPointerUp();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        garageCam.OnPointerDown();
    }
}
