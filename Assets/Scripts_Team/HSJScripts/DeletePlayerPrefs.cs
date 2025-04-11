using UnityEngine;

public class DeletePlayerPrefs : MonoBehaviour
{
    public void deletePP() {
        PlayerPrefs.DeleteAll();
    }
}
