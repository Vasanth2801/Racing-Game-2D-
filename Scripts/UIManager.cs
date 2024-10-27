using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI lapText; // Assign LapText in the Inspector
    public TextMeshProUGUI positionText; // Assign PositionText in the Inspector

    // Method to update the lap count in the UI
    public void UpdateLapCount(int currentLap, int maxLap)
    {
        lapText.text = "Lap: " + currentLap + "/" + maxLap;
    }

    // Method to update the player's position in the UI
    public void UpdatePosition(int position)
    {
        positionText.text = "Position: " + position;
    }
}
