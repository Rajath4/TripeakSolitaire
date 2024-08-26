using TMPro;
using UnityEngine;

public class LeaderboardContentItem : MonoBehaviour
{
    [@SerializeField] private TMP_Text rankText;
    [@SerializeField] private TMP_Text nameText;
    [@SerializeField] private TMP_Text highScoreText;

    public void SetData(int rank, string name, int highScore)
    {
        rankText.text = rank.ToString();
        nameText.text = name;
        highScoreText.text = highScore.ToString();
    }
}