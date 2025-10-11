using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int SelectedCar
    {
        get => PlayerPrefs.GetInt("SelectedCar");
        set => PlayerPrefs.SetInt("SelectedCar", value);
    }

}
