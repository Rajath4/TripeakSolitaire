using TMPro;
using UnityEngine;

public class LeaderboardContentItem:MonoBehaviour{
    public TMP_Text rankText;
    public TMP_Text nameText;
    public TMP_Text highScoreText;

    public void SetData(int rank, string name, int highScore)
    {
        rankText.text = rank.ToString();
        nameText.text = name;
        highScoreText.text = highScore.ToString();
    }
}