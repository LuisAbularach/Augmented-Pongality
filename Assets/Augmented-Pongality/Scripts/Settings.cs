using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{

    public Text txt_maxscore;
    public Text txt_oobounds;

    public Slider slMaxScore;
    public Slider slBoundsTimer;
    int maxScore;
    int oobounds; // out of bounds

    bool visible = false;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("maxScore")) {
            maxScore = PlayerPrefs.GetInt("maxScore");
            txt_maxscore.text = maxScore.ToString();
            slMaxScore.value = maxScore;
        }
        else {
            maxScore = 15;
            txt_maxscore.text = "15";
            slMaxScore.value = maxScore;
        }

        if(PlayerPrefs.HasKey("OutofBoundsTimer")) {
            oobounds = PlayerPrefs.GetInt("OutofBoundsTimer");
            txt_oobounds.text = oobounds.ToString();
            slBoundsTimer.value = oobounds;
        }
        else {
            oobounds = 5;
            txt_oobounds.text = "5";
            slBoundsTimer.value = oobounds;
        }

        gameObject.SetActive(visible);
    }

    public void moveSlider(Slider sl)
    {
        switch(sl.name)
        {
            case "sl_maxScore":
                maxScore = Mathf.FloorToInt(sl.value);
                PlayerPrefs.SetInt("maxScore",maxScore);
                txt_maxscore.text = maxScore.ToString();
                break;
            case "sl_OOBounds":
                oobounds = Mathf.FloorToInt(sl.value);
                PlayerPrefs.SetInt("OutofBoundsTimer",oobounds);
                txt_oobounds.text = oobounds.ToString();
                break;
        }
    }

    public void OnSettingsPressed()
    {
        visible = !visible;
        gameObject.SetActive(visible);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
