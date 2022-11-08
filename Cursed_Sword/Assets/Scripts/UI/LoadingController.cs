using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{
    //SceneManager.LoadScene("Gaia_Room");
    [SerializeField] private GameObject[] othersUiElements;
    [SerializeField] private Image selectedPage;
    [SerializeField] private GameObject[] helpTexts;
    [SerializeField] private Animator loadingTextAnim;
    [SerializeField] private Animator loadingFrameAnim;
    [SerializeField] private GameObject[] prints;
    [SerializeField] private GameObject rightPage;
    [SerializeField] private GameObject leftPage;
    [SerializeField] private GameObject mainObj;
    [SerializeField] private GameObject loadingTxtObj;

    public InputMaster input;
    AsyncOperation loadAsync;

    private bool[] page;
    private bool playUpdate = false;
    private bool waitHelpAnim = false;
    private bool pageSelect = false;
    private bool endLoad = false;
    private bool waitEndLoad = false;
    private bool[] alreadyTriggered;
    private bool waitingInput = false;
    private bool dontSelectButton = false;
    private bool dontWaitInput = false;
    private bool canLoad = false;
    private bool loadOneTime = false;

    private int pageIndex = 0;

    [SerializeField] private float[] selectXPos;
    private float timer = 0.5f;

    private void Awake()
    {
        input = new InputMaster();

        page = new bool[8];
        alreadyTriggered = new bool[2];

        for (int i = 0; i < page.Length; i++)
        {
            if (i == 0)
                page[i] = true;

            page[i] = false;
        }

        for (int i = 0; i < alreadyTriggered.Length; i++)
            alreadyTriggered[i] = false;

        ////////////////////////

        input.SecondaryUI.Submit.performed += ctx => LoadEnd();
        input.SecondaryUI.RightTab.performed += ctx => PageAscend();
        input.SecondaryUI.LeftTab.performed += ctx => PageDescend();
    }

    private void Update()
    {
        if (playUpdate && !canLoad)
        {
            if (!pageSelect)
            {
                pageSelect = true;
                PageSelect();
                loadingTxtObj.SetActive(true);
            }

            StartCoroutine("LoadAsync");
        }

        if (waitHelpAnim)
        {
            if (timer <= 0)
            {
                playUpdate = true;
                FindObjectOfType<AudioManager>().PlaySound("ChoiceHover");
                waitHelpAnim = false;
            }

            else
                timer -= Time.deltaTime;
        }

        if (waitEndLoad)
        {
            if (timer <= 0)
            {
                waitEndLoad = false;
                canLoad = true;
            }

            else
                timer -= Time.deltaTime;
        }
    }

    public void StartLoading()
    {
        foreach (GameObject go in othersUiElements)
            go.SetActive(false);

        mainObj.SetActive(true);
        waitHelpAnim = true;
    }

    private void PageSelect()
    {
        if (page[0])
        {
            leftPage.SetActive(false);
            selectedPage.rectTransform.anchoredPosition = new Vector2(selectXPos[0], selectedPage.rectTransform.anchoredPosition.y);

            for (int i = 0; i < prints.Length; i++)
            {
                if (i == 0)
                {
                    prints[i].SetActive(true);
                    helpTexts[i].SetActive(true);
                }

                else
                {
                    prints[i].SetActive(false);
                    helpTexts[i].SetActive(false);
                }
            }
        }

        else if (page[1])
        {
            leftPage.SetActive(true);
            selectedPage.rectTransform.anchoredPosition = new Vector2(selectXPos[1], selectedPage.rectTransform.anchoredPosition.y);

            for (int i = 0; i < prints.Length; i++)
            {
                if (i == 1)
                {
                    prints[i].SetActive(true);
                    helpTexts[i].SetActive(true);
                }

                else
                {
                    prints[i].SetActive(false);
                    helpTexts[i].SetActive(false);
                }
            }
        }

        else if (page[2])
        {
            selectedPage.rectTransform.anchoredPosition = new Vector2(selectXPos[2], selectedPage.rectTransform.anchoredPosition.y);

            for (int i = 0; i < prints.Length; i++)
            {
                if (i == 2)
                {
                    prints[i].SetActive(true);
                    helpTexts[i].SetActive(true);
                }

                else
                {
                    prints[i].SetActive(false);
                    helpTexts[i].SetActive(false);
                }
            }
        }

        else if (page[3])
        {
            selectedPage.rectTransform.anchoredPosition = new Vector2(selectXPos[3], selectedPage.rectTransform.anchoredPosition.y);

            for (int i = 0; i < prints.Length; i++)
            {
                if (i == 3)
                {
                    prints[i].SetActive(true);
                    helpTexts[i].SetActive(true);
                }

                else
                {
                    prints[i].SetActive(false);
                    helpTexts[i].SetActive(false);
                }
            }
        }

        else if (page[4])
        {
            selectedPage.rectTransform.anchoredPosition = new Vector2(selectXPos[4], selectedPage.rectTransform.anchoredPosition.y);

            for (int i = 0; i < prints.Length; i++)
            {
                if (i == 4)
                {
                    prints[i].SetActive(true);
                    helpTexts[i].SetActive(true);
                }

                else
                {
                    prints[i].SetActive(false);
                    helpTexts[i].SetActive(false);
                }
            }
        }

        else if (page[5])
        {
            selectedPage.rectTransform.anchoredPosition = new Vector2(selectXPos[5], selectedPage.rectTransform.anchoredPosition.y);

            for (int i = 0; i < prints.Length; i++)
            {
                if (i == 5)
                {
                    prints[i].SetActive(true);
                    helpTexts[i].SetActive(true);
                }

                else
                {
                    prints[i].SetActive(false);
                    helpTexts[i].SetActive(false);
                }
            }
        }

        else if (page[6])
        {
            rightPage.SetActive(true);
            selectedPage.rectTransform.anchoredPosition = new Vector2(selectXPos[6], selectedPage.rectTransform.anchoredPosition.y);

            for (int i = 0; i < prints.Length; i++)
            {
                if (i == 6)
                {
                    prints[i].SetActive(true);
                    helpTexts[i].SetActive(true);
                }

                else
                {
                    prints[i].SetActive(false);
                    helpTexts[i].SetActive(false);
                }
            }
        }

        else if (page[7])
        {
            rightPage.SetActive(false);
            selectedPage.rectTransform.anchoredPosition = new Vector2(selectXPos[7], selectedPage.rectTransform.anchoredPosition.y);

            for (int i = 0; i < prints.Length; i++)
            {
                if (i == 7)
                {
                    prints[i].SetActive(true);
                    helpTexts[i].SetActive(true);
                }

                else
                {
                    prints[i].SetActive(false);
                    helpTexts[i].SetActive(false);
                }
            }
        }
    }

    public void PageAscend()
    {
        if (pageIndex < 7)
        {
            FindObjectOfType<AudioManager>().PlaySound("ChoiceHover");
            pageIndex++;

            for (int i = 0; i < page.Length; i++)
            {
                if (i == pageIndex)
                    page[i] = true;

                else
                    page[i] = false;
            }

            PageSelect();
        }

    }

    public void PageDescend()
    {
        if (pageIndex > 0)
        {
            FindObjectOfType<AudioManager>().PlaySound("ChoiceHover");
            pageIndex--;

            for (int i = 0; i < page.Length; i++)
            {
                if (i == pageIndex)
                    page[i] = true;

                else
                    page[i] = false;
            }

            PageSelect();
        }
    }

    public void LoadEnd()
    {
        if (waitingInput)
        {
            waitingInput = false;
            endLoad = true;
            dontSelectButton = true;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private IEnumerator LoadAsync()
    {
        if (!loadOneTime)
        {
            loadAsync = SceneManager.LoadSceneAsync("Gaia_Room");
            loadAsync.allowSceneActivation = false;
            loadOneTime = true;
        }

        while(!loadAsync.isDone)
        {
            if (loadAsync.progress >= 0.9f && !loadAsync.allowSceneActivation)
            {
                if (!dontWaitInput)
                {
                    waitingInput = true;
                    dontWaitInput = true;
                }

                if (!alreadyTriggered[0])
                {
                    loadingTextAnim.SetTrigger("Press");
                    alreadyTriggered[0] = true;
                }

                if (endLoad) // pressed the button
                {
                    if (!alreadyTriggered[1])
                    {
                        loadingTextAnim.SetTrigger("Loaded");
                        loadingFrameAnim.SetTrigger("Play");
                        waitEndLoad = true;
                        timer = 3;
                        alreadyTriggered[1] = true;
                    }
                }

                if (canLoad)
                    loadAsync.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
