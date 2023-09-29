using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.ComponentModel;
using System.Drawing;

public class SkillChooseController : MonoBehaviour
{
    [Header("Description Texts")]
    [SerializeField] private GameObject LowflightDescription;
    [SerializeField] private GameObject earthquakeDescription;
    [SerializeField] private GameObject fireupDescription;
    [SerializeField] private GameObject laserDescription;

    [Header("Black Selected")]
    [SerializeField] private GameObject blackSelected1;
    [SerializeField] private GameObject blackSelected2;

    [Header("Selected Icons")]
    [SerializeField] private GameObject lowflightSelected;
    [SerializeField] private GameObject earthquakeSelected;
    [SerializeField] private GameObject fireupSelected;
    [SerializeField] private GameObject laserSelected;

    [Header("Buttons")]
    public GameObject lowflightButton;
    [SerializeField] private GameObject earthquakeButton;
    [SerializeField] private GameObject fireupButton;
    [SerializeField] private GameObject laserButton;

    [Header("Selection")]
    [SerializeField] private GameObject selection;
    [SerializeField] private Image selectionImage;

    [Header("Description Window")]
    [SerializeField] private GameObject descWindow;
    [SerializeField] private GameObject toselectObj;
    [SerializeField] private Image descWindowImage;

    [Header("Description Window X Position")]
    [SerializeField] private float windowXPos1 = 0;
    [SerializeField] private float windowXPos2 = 0;
    [SerializeField] private float windowXPos3 = 0;
    [SerializeField] private float windowXPos4 = 0;

    [Header("Selected Skill 2 X Position")]
    [SerializeField] private float secondSkillXPos = 0;

    [Header("Confirm Text")]
    public Text confirmText;

    [Header("Selected")]
    [SerializeField] private GameObject selectedText;

    [Header("Confirm Window")]
    [SerializeField] private GameObject confirmWindow;

    [Header("Confirm Controller")]
    [SerializeField] private ConfirmController confirmController;

    [Header("Description Texts")]
    [SerializeField] private Text lowflightDesc;
    [SerializeField] private Text earthquakeDesc;
    [SerializeField] private Text fireupDesc;
    [SerializeField] private Text laserDesc;

    [Header("Block Selection")]
    [SerializeField] private GameObject blockSelection;

    private int alreadyPlayed; // 0 = false, 1 - true;

    ///////////////////////////////////////////////////////////////

    public static string skill1;
    public static string skill2;

    [HideInInspector] public string confirmString; 
    private string skill1Text;
    private string skill2Text;
    private string lowflightDescString;
    private string earthquakeDescString;
    private string fireupDescString;
    private string laserDescString;

    private Animator anim;

    [HideInInspector] public GameObject currentSelectedButton;

    [HideInInspector] public bool endAnim = false;
    private bool playUpdate;
    private bool[] alreadySelected;
    private bool[] alreadySubmited;
    private bool confirmCalled = false;
    private bool confirmDelay = true;

    private int skillCount = 0;

    private float confirmDelayTimer = 1.5f;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        skill1 = "";
        skill2 = "";

        if (alreadyPlayed == 0)
        {
            alreadyPlayed = 1;
            PlayerPrefs.SetInt("AlreadyPlayed", alreadyPlayed);
            PlayerPrefs.Save();
        }

        alreadySelected = new bool[4];
        for (int i = 0; i < alreadySelected.Length; i++)
            alreadySelected[i] = false;

        alreadySubmited = new bool[4];
        for (int i = 0; i < alreadySelected.Length; i++)
            alreadySubmited[i] = false;

        lowflightDescString = "<color=white>Makes a dash towards the side you are facing in, making you </color> " + "<color=yellow>invulnerable </color>" + "<color=white>for that time and causing </color>" + "<color=yellow>20 </color>" + "<color=white>damage to the enemies that you pass by.</color>";
        //lowflightDescString = "<color=white>Se dispare na dire��o em que est� olhando, o tornando </color> " + "<color=yellow>invulner�vel </color>" + "<color=white>nesse tempo, e causando </color>" + "<color=yellow>20 </color>" + "<color=white>de dano nos inimigos que voc� atravessa.</color>";
        lowflightDesc.text = lowflightDescString;

        earthquakeDescString = "<color=white>Makes you pound the ground, causing </color> " + "<color=yellow>40 </color>" + "<color=white>damage in your </color>" + "<color=yellow>area.</color>";
        //earthquakeDescString = "<color=white>Faz voc� socar o ch�o, causando </color> " + "<color=yellow>40 </color>" + "<color=white>de dano na sua </color>" + "<color=yellow>�rea.</color>";
        earthquakeDesc.text = earthquakeDescString;

        fireupDescString = "<color=white>Increases your basic attack damage by </color> " + "<color=yellow>75 percent </color>" + "<color=white>for </color>" + "<color=yellow>10 seconds.</color>";
        //fireupDescString = "<color=white>Aumenta o dano do seu ataque b�sico em </color> " + "<color=yellow>75 porcento </color>" + "<color=white>por </color>" + "<color=yellow>10 segundos.</color>";
        fireupDesc.text = fireupDescString;

        laserDescString = "<color=white>Creates a laser beam in front of you, towards the side you are facing in, covering the entire </color> " + "<color=yellow>horizontal </color>" + "<color=white>direction, causing </color>" + "<color=yellow>25 </color>" + "<color=white>damage on every enemy that touches it.</color>";
        //laserDescString = "<color=white>Cria um raio laser � sua frente, na dire��o em que voc� est� olhando, cobrindo inteiramente a dire��o </color> " + "<color=yellow>horizontal </color>" + "<color=white>, causando </color>" + "<color=yellow>25 </color>" + "<color=white>de dano em qualquer inimigo que o encoste.</color>";
        laserDesc.text = laserDescString;
    }

    private void Update()
    {
        if (endAnim)
        {
            selection.SetActive(true);
            descWindow.SetActive(true);

            currentSelectedButton = lowflightButton;
            selection.SetActive(true);

            descWindow.SetActive(true);
            descWindow.transform.position = new Vector2(windowXPos1, 253.9f);

            playUpdate = true;
            endAnim = false;
            LowFLightSelect();
        }

        if (playUpdate)
        {
            EventSystem.current.SetSelectedGameObject(currentSelectedButton);
        }

        if (skillCount >= 2 && confirmDelay)
        {

            if (confirmDelayTimer <= 0)
                confirmCalled = true;

            else
                confirmDelayTimer -= Time.deltaTime;
        }

        if (confirmCalled)
        {
            confirmText.text = confirmString;
            confirmCalled = false;
            confirmDelay = false;
            selectedText.SetActive(false);
            descWindow.SetActive(false);
            confirmController.ChooseSkillCalled();
        }
    }

    #region Skill Select

    public void LowFLightSelect()
    {
        if (!alreadySelected[0])
        {
            FindObjectOfType<AudioManager>().PlaySound("ChoiceHover");
            for (int i = 0; i < alreadySelected.Length; i++)
            {
                if (i == 0)
                    alreadySelected[i] = true;
                else
                    alreadySelected[i] = false;
            }
        }

        if (alreadySubmited[0])
        {
            selectedText.GetComponent<Text>().rectTransform.anchoredPosition = new Vector2(selectedText.GetComponent<Text>().rectTransform.anchoredPosition.x, -12.66f);
            selectedText.SetActive(true);
            toselectObj.SetActive(false);
        }

        else
        {
            selectedText.SetActive(false);
            toselectObj.SetActive(true);
        }

        currentSelectedButton = lowflightButton;
        selectionImage.rectTransform.anchoredPosition = new Vector2(windowXPos1, -59.89f);

        LowflightDescription.SetActive(true);
        earthquakeDescription.SetActive(false);
        fireupDescription.SetActive(false);
        laserDescription.SetActive(false);

        descWindowImage.rectTransform.anchoredPosition = new Vector2(windowXPos1, 253.9f);
    }

    public void EarthquakeSelect()
    {
        if (!alreadySelected[1])
        {
            FindObjectOfType<AudioManager>().PlaySound("ChoiceHover");
            for (int i = 0; i < alreadySelected.Length; i++)
            {
                if (i == 1)
                    alreadySelected[i] = true;
                else
                    alreadySelected[i] = false;
            }
        }

        if (alreadySubmited[1])
        {
            selectedText.GetComponent<Text>().rectTransform.anchoredPosition = new Vector2(selectedText.GetComponent<Text>().rectTransform.anchoredPosition.x, 23.42f);
            selectedText.SetActive(true);
            toselectObj.SetActive(false);
        }

        else
        {
            selectedText.SetActive(false);
            toselectObj.SetActive(true);
        }

        currentSelectedButton = earthquakeButton;
        selectionImage.rectTransform.anchoredPosition = new Vector2(windowXPos2, -59.89f);

        LowflightDescription.SetActive(false);
        earthquakeDescription.SetActive(true);
        fireupDescription.SetActive(false);
        laserDescription.SetActive(false);

        descWindowImage.rectTransform.anchoredPosition = new Vector2(windowXPos2, 253.9f);
    }

    public void FireupSelect()
    {
        if (!alreadySelected[2])
        {
            FindObjectOfType<AudioManager>().PlaySound("ChoiceHover");
            for (int i = 0; i < alreadySelected.Length; i++)
            {
                if (i == 2)
                    alreadySelected[i] = true;
                else
                    alreadySelected[i] = false;
            }
        }

        if (alreadySubmited[2])
        {
            selectedText.GetComponent<Text>().rectTransform.anchoredPosition = new Vector2(selectedText.GetComponent<Text>().rectTransform.anchoredPosition.x, 23.42f);
            selectedText.SetActive(true);
            toselectObj.SetActive(false);
        }

        else
        {
            selectedText.SetActive(false);
            toselectObj.SetActive(true);
        }

        currentSelectedButton = fireupButton;
        selectionImage.rectTransform.anchoredPosition = new Vector2(windowXPos3, -59.89f);

        LowflightDescription.SetActive(false);
        earthquakeDescription.SetActive(false);
        fireupDescription.SetActive(true);
        laserDescription.SetActive(false);

        descWindowImage.rectTransform.anchoredPosition = new Vector2(windowXPos3, 253.9f);
    }

    public void LaserSelect()
    {
        if (!alreadySelected[3])
        {
            FindObjectOfType<AudioManager>().PlaySound("ChoiceHover");
            for (int i = 0; i < alreadySelected.Length; i++)
            {
                if (i == 3)
                    alreadySelected[i] = true;
                else
                    alreadySelected[i] = false;
            }
        }

        if (alreadySubmited[3])
        {
            selectedText.GetComponent<Text>().rectTransform.anchoredPosition = new Vector2(selectedText.GetComponent<Text>().rectTransform.anchoredPosition.x, -28.56f);
            selectedText.SetActive(true);
            toselectObj.SetActive(false);
        }

        else
        {
            selectedText.SetActive(false);
            toselectObj.SetActive(true);
        }

        currentSelectedButton = laserButton;
        selectionImage.rectTransform.anchoredPosition = new Vector2(windowXPos4, -59.89f);

        LowflightDescription.SetActive(false);
        earthquakeDescription.SetActive(false);
        fireupDescription.SetActive(false);
        laserDescription.SetActive(true);

        descWindowImage.rectTransform.anchoredPosition = new Vector2(windowXPos4, 253.9f);
    }

    #endregion

    ////////////////////////////////////////////

    #region Skill Submit

    public void LowFlightChoosed()
    {
        if (!alreadySubmited[0])
        {
            FindObjectOfType<AudioManager>().PlaySound("Lowflight");
            lowflightSelected.SetActive(true);

            alreadySubmited[0] = true;

            if (skillCount == 0)
            {
                skill1 = "lowflight";
                skill1Text = "Low Flight";

                blackSelected1.SetActive(true);
                blackSelected1.transform.position = new Vector2(lowflightButton.transform.position.x, lowflightButton.transform.position.y);
            }

            else if (skillCount == 1)
            {
                skill2 = "lowflight";
                skill2Text = "Low Flight";

                lowflightSelected.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(secondSkillXPos, -334.1f);

                blackSelected2.SetActive(true);
                blackSelected2.transform.position = new Vector2(lowflightButton.transform.position.x, lowflightButton.transform.position.y);

                //confirmString = "<color=white>Tem certeza que deseja usar </color>" + "<color=yellow>" + skill1Text + "</color>" + "<color=white> e </color>" + "<color=yellow>" + skill2Text + "</color>" + "<color=white> como suas habilidades?</color>";
                confirmString = "<color=white>Are you sure you want to use </color>" + "<color=yellow>" + skill1Text + "</color>" + "<color=white> and </color>" + "<color=yellow>" + skill2Text + "</color>" + "<color=white> as your skills?</color>";

                playUpdate = false;
                blockSelection.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
            }
            skillCount++;
            LowFLightSelect();
        }
        else
            ChooseError("lowflight");
    }
    public void EarthquakeChoosed()
    {
        if (!alreadySubmited[1])
        {
            FindObjectOfType<AudioManager>().PlaySound("Earthquake");
            earthquakeSelected.SetActive(true);
            alreadySubmited[1] = true;
            if (skillCount == 0)
            {
                skill1 = "earthquake";
                skill1Text = "Earthquake";
                blackSelected1.SetActive(true);
                blackSelected1.transform.position = new Vector2(earthquakeButton.transform.position.x, earthquakeButton.transform.position.y);
            }
            else if (skillCount == 1)
            {
                skill2 = "earthquake";
                skill2Text = "Earthquake";
                earthquakeSelected.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(secondSkillXPos, -334.1f);
                blackSelected2.SetActive(true);
                blackSelected2.transform.position = new Vector2(earthquakeButton.transform.position.x, earthquakeButton.transform.position.y);
                //confirmString = "<color=white>Tem certeza que deseja usar </color>" + "<color=yellow>" + skill1Text + "</color>" + "<color=white> e </color>" + "<color=yellow>" + skill2Text + "</color>" + "<color=white> como suas habilidades?</color>";
                confirmString = "<color=white>Are you sure you want to use </color>" + "<color=yellow>" + skill1Text + "</color>" + "<color=white> and </color>" + "<color=yellow>" + skill2Text + "</color>" + "<color=white> as your skills?</color>";
                playUpdate = false;
                blockSelection.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
            }
            skillCount++;
            EarthquakeSelect();
        }
        else
            ChooseError("earthquake");
    }
    public void FireupChoosed()
    {
        if (!alreadySubmited[2])
        {
            FindObjectOfType<AudioManager>().PlaySound("Fireup");
            fireupSelected.SetActive(true);
            alreadySubmited[2] = true;
            if (skillCount == 0)
            {
                skill1 = "fireup";
                skill1Text = "Fireup";
                blackSelected1.SetActive(true);
                blackSelected1.transform.position = new Vector2(fireupButton.transform.position.x, fireupButton.transform.position.y);
            }
            else if (skillCount == 1)
            {
                skill2 = "fireup";
                skill2Text = "Fireup";
                fireupSelected.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(secondSkillXPos, -334.1f);
                blackSelected2.SetActive(true);
                blackSelected2.transform.position = new Vector2(fireupButton.transform.position.x, fireupButton.transform.position.y);
                //confirmString = "<color=white>Tem certeza que deseja usar </color>" + "<color=yellow>" + skill1Text + "</color>" + "<color=white> e </color>" + "<color=yellow>" + skill2Text + "</color>" + "<color=white> como suas habilidades?</color>";
                confirmString = "<color=white>Are you sure you want to use </color>" + "<color=yellow>" + skill1Text + "</color>" + "<color=white> and </color>" + "<color=yellow>" + skill2Text + "</color>" + "<color=white> as your skills?</color>";
                playUpdate = false;
                blockSelection.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
            }
            skillCount++;
            FireupSelect();
        }
        else
            ChooseError("fireup");
    }
    public void LaserChoosed()
    {
        if (!alreadySubmited[3])
        {
            FindObjectOfType<AudioManager>().PlaySound("Laser");
            laserSelected.SetActive(true);
            alreadySubmited[3] = true;
            if (skillCount == 0)
            {
                skill1 = "laser";
                skill1Text = "Laser";

                blackSelected1.SetActive(true);
                blackSelected1.transform.position = new Vector2(laserButton.transform.position.x, laserButton.transform.position.y);
            }

            else if (skillCount == 1)
            {
                skill2 = "laser";
                skill2Text = "Laser";

                laserSelected.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(secondSkillXPos, -334.1f);

                blackSelected2.SetActive(true);
                blackSelected2.transform.position = new Vector2(laserButton.transform.position.x, laserButton.transform.position.y);

                //confirmString = "<color=white>Tem certeza que deseja usar </color>" + "<color=yellow>" + skill1Text + "</color>" + "<color=white> e </color>" + "<color=yellow>" + skill2Text + "</color>" + "<color=white> como suas habilidades?</color>";
                confirmString = "<color=white>Are you sure you want to use </color>" + "<color=yellow>" + skill1Text + "</color>" + "<color=white> and </color>" + "<color=yellow>" + skill2Text + "</color>" + "<color=white> as your skills?</color>";

                playUpdate = false;
                blockSelection.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
            }

            skillCount++;
            LaserSelect();
        }

        else
            ChooseError("laser");
    }

    #endregion

    public void ChooseError(string skill)
    {
        FindObjectOfType<AudioManager>().PlaySound("Error");

        if (skill == "lowflight")
            LowFLightSelect();

        else if (skill == "earthquake")
            EarthquakeSelect();

        else if (skill == "fireup")
            FireupSelect();

        else if (skill == "laser")
            LaserSelect();
    }

    public void NoConfirm()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(lowflightButton);

        descWindow.SetActive(true);
        selectedText.SetActive(false);
        blockSelection.SetActive(false);

        skill1 = "";
        skill2 = "";
        skill1Text = "";
        skill2Text = "";

        blackSelected1.SetActive(false);
        blackSelected2.SetActive(false);

        skillCount = 0;
        confirmCalled = false;
        confirmDelay = true;
        playUpdate = true;
        confirmDelayTimer = 1.5f;

        lowflightSelected.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(-104.345f, -334.1f);
        earthquakeSelected.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(-104.345f, -334.1f);
        fireupSelected.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(-104.345f, -334.1f);
        laserSelected.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(-104.345f, -334.1f);

        lowflightSelected.SetActive(false);
        earthquakeSelected.SetActive(false);
        fireupSelected.SetActive(false);
        laserSelected.SetActive(false);

        for (int i = 0; i < alreadySubmited.Length; i++)
        {
            alreadySubmited[i] = false;
        }
    }
}
