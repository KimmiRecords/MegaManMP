using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI myText;

    public void UpdateText(Dictionary<PlayerModel, int> playersAndScores)
    {
        string totalText = "";

        foreach (KeyValuePair<PlayerModel, int> pair in playersAndScores)
        {
            string playerLine = "Player " + pair.Key.gameObject.GetHashCode() + ": " + playersAndScores[pair.Key] + " /// ";
            totalText += playerLine;
            //Debug.Log("agregue la player line " + playerLine + " al totaltext");
        }

        myText.text = totalText;
    }
}
