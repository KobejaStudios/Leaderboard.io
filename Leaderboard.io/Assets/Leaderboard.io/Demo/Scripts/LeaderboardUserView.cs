using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUserView : MonoBehaviour
{
    [SerializeField] private Text _placeText;
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _scoreText;

    public void SetText(int place, string userName, int score)
    {
        _placeText.text = place.ToString();
        _nameText.text = userName;
        _scoreText.text = score.ToString();
    }
}
