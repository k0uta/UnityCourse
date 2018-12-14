using UnityEngine;
using UnityEngine.UI;

public class PlayerResultView : MonoBehaviour {

    public PlayerResult playerResult;

    public Text rankText;

    public Text playerNameText;

    public Text scoreText;

	void Start () {
        rankText.text = string.Format("#{0}", playerResult.rank);
        playerNameText.text = playerResult.playerName;
        scoreText.text = string.Format("x {0}", playerResult.score);
    }
}
