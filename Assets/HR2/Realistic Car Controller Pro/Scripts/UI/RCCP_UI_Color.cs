using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller Pro/UI/Modification/RCCP UI Color Button")]
public class RCCP_UI_Color : RCCP_UIComponent
{
    public PickedColor _pickedColor = PickedColor.Orange;
    public enum PickedColor { Orange, Red, Green, Blue, Black, White, Cyan, Magenta, Pink, Yellow }

    /* private void Awake()
     {
         GetComponent<Button>().onClick.AddListener(OnClick);
     }


     public void OnClick()
     {

         //  Finding the player vehicle.
         RCCP_CarController playerVehicle = RCCPSceneManager.activePlayerVehicle;

         //  If no player vehicle found, return.
         if (!playerVehicle)
             return;

         //  If player vehicle doesn't have the customizer component, return.
         if (!playerVehicle.Customizer)
             return;

         if (!playerVehicle.Customizer.PaintManager)
             return;

         //  Color.
         Color selectedColor = new Color();

         switch (_pickedColor)
         {

             case PickedColor.Orange:
                 selectedColor = Color.red + (Color.green / 2f);
                 break;

             case PickedColor.Red:
                 selectedColor = Color.red;
                 break;

             case PickedColor.Green:
                 selectedColor = Color.green;
                 break;

             case PickedColor.Blue:
                 selectedColor = Color.blue;
                 break;

             case PickedColor.Black:
                 selectedColor = Color.black;
                 break;

             case PickedColor.White:
                 selectedColor = Color.white;
                 break;

             case PickedColor.Cyan:
                 selectedColor = Color.cyan;
                 break;

             case PickedColor.Magenta:
                 selectedColor = Color.magenta;
                 break;

             case PickedColor.Pink:
                 selectedColor = new Color(1, 0f, .5f);
                 break;

             case PickedColor.Yellow:
                 selectedColor = new Color(1, 1f, 0f);
                 break;

         }

         playerVehicle.Customizer.PaintManager.Paint(selectedColor);

     }
    */


    public Color GetColor()
    {
        switch (_pickedColor)
        {

            case PickedColor.Orange:
                return Color.red + (Color.green / 2f);
                break;

            case PickedColor.Red:
                return Color.red;
                break;

            case PickedColor.Green:
                return Color.green;
                break;

            case PickedColor.Blue:
                return Color.blue;
                break;

            case PickedColor.Black:
                return Color.black;
                break;

            case PickedColor.White:
                return Color.white;
                break;

            case PickedColor.Cyan:
                return Color.cyan;
                break;

            case PickedColor.Magenta:
                return Color.magenta;
                break;

            case PickedColor.Pink:
                return new Color(1, 0f, .5f);
                break;

            case PickedColor.Yellow:
                return new Color(1, 1f, 0f);
                break;

            default:
                return Color.white;

        }
    }
}
