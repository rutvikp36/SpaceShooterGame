using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MainUIController : MonoBehaviour
{


    private static MainUIController instance;

    public static MainUIController Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<MainUIController>() as MainUIController;
            return instance;
        }
    }

    // Start is called before the first frame update
    [SerializeField]
    public GameObject[] airPlaneModels;
    public float rotateSpeed = 5f;

    [SerializeField]
    public Button nextButton,
                    previousButton,
                    SelectButton;


    [SerializeField]
    public GameObject menuCam;
    [SerializeField]
    float timeToMoveCam = 2f;

    [SerializeField]
    public GameObject changePlanePanel,
                      MainMenuPanel,
                      InGameRunningPanel,
                      InGamePausePanel,
                      LoadingPanel,
                      GameOverPanel;

    [SerializeField]
    public TMP_Text loadingNumber,
                    GameOverScore,
                    InGameScore,
                    InGameNoOfBulletsText;


    int currentSelection = 0;
    float camPos = 0;


    private void Awake()
    {
        nextButton.onClick.AddListener(delegate { NextButtonClicked(); });
        previousButton.onClick.AddListener(delegate { PreviousButtonClicked(); });
        onUiStart();
    }

    void Update()
    {
        InGameScore.text = "Score : " + MainGameController.Instance.Score;
    }


    // ------------------------------Helper - Functions ---------------------------------------------------------------------



    void onUiStart()
    {
        camPos = 12 * currentSelection;
        menuCam.transform.localPosition = new Vector3(camPos, menuCam.transform.localPosition.y, menuCam.transform.localPosition.z);
        foreach (GameObject Plane in airPlaneModels)
        {
            Plane.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, rotateSpeed, 0);
        }
        setButtonsActiveOnPlaneSelect();
        MainMenuPanel.SetActive(true);
        changePlanePanel.SetActive(false);
        GameOverPanel.SetActive(false);
        GameOverScore.text = "Score : 0";
    }

    public void gameOverUI()
    {
        GameOverScore.text = "Score : " +  MainGameController.Instance.Score;
        InGameRunningPanel.SetActive(false);
        GameOverPanel.SetActive(true);
    }

    public IEnumerator LoadingScreen()
    {
        InGamePausePanel.SetActive(false);
        GameOverPanel.SetActive(false);
        InGameRunningPanel.SetActive(false);
        LoadingPanel.SetActive(true);

        Time.timeScale = 0;
        int i = 3;
        while(i > 0)
        {
            loadingNumber.text = "" + i;
            i--;
            yield return new WaitForSecondsRealtime(1f);
        }
        LoadingPanel.SetActive(false);
        InGameRunningPanel.SetActive(true);
        Time.timeScale = 1;
    }

    private void setButtonsActiveOnPlaneSelect()
    {
        if (currentSelection == 0)
        {
            previousButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(true);
        }
        else if (currentSelection == airPlaneModels.Length - 1)
        {
            previousButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(false);
        }
        else
        {
            nextButton.gameObject.SetActive(true);
            previousButton.gameObject.SetActive(true);
        }
    }

    private IEnumerator SmoothCameraLerp()
    {
        Vector3 startingPos = menuCam.transform.localPosition;
        Vector3 finalPos = new Vector3(camPos, menuCam.transform.localPosition.y, menuCam.transform.localPosition.z);
        nextButton.interactable = false;
        previousButton.interactable = false;
        SelectButton.interactable = false;
        float elapsedTime = 0;
        while (elapsedTime < timeToMoveCam)
        {
            menuCam.transform.localPosition = Vector3.Lerp(startingPos, finalPos, (elapsedTime / timeToMoveCam));
            elapsedTime += Time.deltaTime;
            yield return null;

        }
        nextButton.interactable = true;
        previousButton.interactable = true;
        SelectButton.interactable = true;
    }

    // ------------------------------------ UIButtonClicked - Functions-------------------------------------------------------------



    public void NextButtonClicked()
    {
        Debug.Log("nextClicked");
        currentSelection++;
        camPos = 12 * currentSelection;
        StartCoroutine(SmoothCameraLerp());
        setButtonsActiveOnPlaneSelect();
    }
    public void PreviousButtonClicked()
    {
        currentSelection--;
        camPos = 12 * currentSelection;
        StartCoroutine(SmoothCameraLerp());
        menuCam.transform.Translate(new Vector3(camPos, 0,0),Space.Self);
        setButtonsActiveOnPlaneSelect();
    }




    public void selectPlaneClicked()
    {
        MainMenuPanel.SetActive(true);
        changePlanePanel.SetActive(false);
    }
    public void changeAirPlaneClicked()
    {
        setButtonsActiveOnPlaneSelect();
        MainMenuPanel.SetActive(false);
        changePlanePanel.SetActive(true);
    }

    public void startMode1Clicked()
    {
        MainGameController.Instance.startMode1(currentSelection);

    }
    public void startMode2Clicked()
    {
        MainGameController.Instance.startMode2(currentSelection);
        
    }

    public void pauseClicked()
    {
        InGameRunningPanel.SetActive(false);
        InGamePausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void resumeClicked()
    {
        InGameRunningPanel.SetActive(true);
        InGamePausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void MainMenuClicked()
    {
        onUiStart();
        MainGameController.Instance.OnGameStart();
    }


}
