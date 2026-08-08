using UnityEngine;

public class SaveButton : MonoBehaviour
{
    public int Type;
    public int TypeNumber;

    public void SaveButtons()
    {
        PlayerPrefs.SetInt("Type", Type);
        PlayerPrefs.SetInt("TypeNumber", TypeNumber);

        Debug.Log("Type : " + Type);
        Debug.Log("TypeNumber : " + TypeNumber);
    }
}