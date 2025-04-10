using UnityEngine;

public class DeletePlayerPrefs : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void deletePP() {
        PlayerPrefs.DeleteAll();
    }
}
