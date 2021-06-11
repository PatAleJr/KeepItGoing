using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionMaker : MonoBehaviour
{
    //Variables
    #region
    public static SectionMaker sectionMaker;

    [SerializeField]
    private GameObject sectionPrefab = null;

    public List<GameObject> sections;   //Holds existing sections (4)
    public Transform spawnPoint = null;
    public Vector3 spaceBetweenSections;

    public Transform[] firstSectionPoints;

    [SerializeField]
    private Animator tutorial = null;
    [SerializeField]
    private GameObject hiscorePrefab = null;

    private GameObject hiscore = null;

    private Section.TeamColor firstColor;
    private int sectionScore = 1;

    public float xTapRange = 1f;
    public GameObject currentSection;
    private GameObject previousSection = null;

    [SerializeField]
    private float spaceBetweenColumns = 0;

    public int columnsPassedToAccept = 8;

    [Header("Camera")]
    public float firstAccelX;
    public float secondAccelX;
    public float firstAccelRate;
    public float secondAccelRate;

    public float firstVelocity;     //Diference from 1
    public float secondVelocity;    //Diference from 1
    public float desiredVelocity;   //Actual value

    public float waveOffset;
    public float screenSpeed;

    [Header("Game Info")]
    public float destroyX = -10f;
    public float columnDelay = 1f;
    public int score = 0;
    public int numBalls = 0;
    public bool isPlaying;
    public bool lost;
    public float startSpeed = 1.5f;
    public float currentSpeed;
    public float n_currentSpeed;

    [Header("Speed Change")]
    //Changes speed at constant rate
    public float n_speedChange = 0.1f;  //Change in normal speed per second

    private float nextAccelChange = 0f;
    public float minAccelChangeFrequency = 3f;
    public float maxAccelChangeFrequency = 3f;
    [SerializeField]
    private int scoreForCrazy = 20;  //Score at which game accelerates weirdly

    private bool changingAccel;
    private bool revertingSpeed;
    private bool holdingSpeed = false;
    private float targetSpeed;

    [SerializeField]
    private float speedChangeRate;
    [SerializeField]
    private float holdTime = 2f;
    [SerializeField]
    private float maxSpeedDelta = 2f;
    [SerializeField]
    private float minSpeedDelta = 0.4f;
    [SerializeField]
    private float minAccelTime = 0.5f;
    [SerializeField]
    private float maxAccelTime = 1f;

    [Header("Balls")]
    [SerializeField]
    private GameObject incrementPrefab = null;

    #endregion

    private void Awake()
    {
        if (SectionMaker.sectionMaker == null)
        {
            SectionMaker.sectionMaker = this;
        }
        else
        {
            if (SectionMaker.sectionMaker != this)
            {
                Destroy(SectionMaker.sectionMaker);
                SectionMaker.sectionMaker = this;
            }
        }
    }

    void Start()
    {
        setGame();
    }

    public void setGame()
    {
        //Resets vars
        score = 0;
        numBalls = 0;
        sectionScore = 1;
        lost = false;
        isPlaying = false;
        currentSpeed = startSpeed;
        columnDelay = spaceBetweenColumns / currentSpeed;
        changingAccel = false;
        revertingSpeed = false;
        holdingSpeed = false;
        n_currentSpeed = currentSpeed;
        screenSpeed = 1;

        //Destroy all existing sections
        foreach (GameObject section in sections)
            Destroy(section);
        sections.Clear();

        //Makes initial sections
        for (int i = 0; i < firstSectionPoints.Length; i++)
        {
            sections.Add(Instantiate(sectionPrefab));
            sections[i].transform.position = firstSectionPoints[i].position;

            Section sectionScript = sections[i].GetComponent<Section>();
            sectionScript.columnDelay = columnDelay;
            sectionScript.setMyScore(sectionScore);

            if (sectionScore == Manager.highscore)
                hiscore = Instantiate(hiscorePrefab, sections[i].transform);

            sectionScore++;
        }

        StartCoroutine(setFirstColor());  //Gets key of first section (button which triggers game)
        currentSection = sections[0];

        //Activate tutorial
        tutorial.SetBool("Disable", false);
    }

    IEnumerator setFirstColor()
    {
        yield return new WaitForEndOfFrame();
        firstColor = sections[0].GetComponent<Section>().myTeamColor;
        sections[0].GetComponent<Section>().isMyTurn = true;
    }

    void Update()
    {
        if (lost)
            return;

        if (isPlaying)
        {
            //Scroll
            #region
            float baseScrollSpeed = spaceBetweenColumns / columnDelay;
            int columnIndex = previousSection.GetComponent<Section>().columnIndex;
            float waveX = (previousSection.transform.position.x - 3) + (spaceBetweenColumns * columnIndex);
            waveX += waveOffset;

            if (previousSection != null)
            {
                //Slow down sections
                if (waveX < -secondAccelX)
                {
                    if (screenSpeed > 1 - secondVelocity)
                        screenSpeed -= secondAccelRate;
                }
                else if (waveX < -firstAccelX)
                {
                    float lowestAllowed = 1 - firstVelocity;    //Lowest speed allowed when wave is in this area

                    if (screenSpeed > lowestAllowed)
                    {
                        screenSpeed -= firstAccelRate;
                        if (screenSpeed < lowestAllowed)    //Clamps so doesnt jitter between accel and decel
                            screenSpeed = lowestAllowed;
                    }

                    if (screenSpeed < lowestAllowed)
                    {
                        screenSpeed += firstAccelRate;
                        if (screenSpeed > lowestAllowed)    //Clamps so doesnt jitter between accel and decel
                            screenSpeed = lowestAllowed;
                    }
                }

                //Speed up sections
                if (waveX > secondAccelX)
                {
                    if (screenSpeed < 1 + secondVelocity)
                        screenSpeed += secondAccelRate;
                    
                }
                else if (waveX > firstAccelX)
                {
                    float fastestAllowed = 1 + firstVelocity;   //Fastest speed allowed when wave is in this area. Approach this

                    if (screenSpeed < fastestAllowed)   
                    {
                        screenSpeed += firstAccelRate;
                        if (screenSpeed > fastestAllowed)   //Clamps so doesnt jitter between accel and decel
                            screenSpeed = fastestAllowed;
                    }
                    if (screenSpeed > fastestAllowed)
                    {
                        screenSpeed -= firstAccelRate;
                        if (screenSpeed < fastestAllowed)   //Clamps so doesnt jitter between accel and decel
                            screenSpeed = fastestAllowed;
                    }
                }

                if (waveX < firstAccelX && waveX > -firstAccelX)    //If in middle, try to get to 1
                {
                    if (screenSpeed < desiredVelocity)
                    {
                        screenSpeed += firstAccelRate;
                        if (screenSpeed > desiredVelocity)   //Clamps so doesnt jitter between accel and decel
                            screenSpeed = desiredVelocity;
                    }
                    else if (screenSpeed > desiredVelocity) {
                        screenSpeed -= firstAccelRate;
                        if (screenSpeed < desiredVelocity)
                            screenSpeed = desiredVelocity;
                    }
                }
            }

            float spd = baseScrollSpeed * screenSpeed;
            Vector3 moveVector = new Vector2(-spd * Time.deltaTime, 0);
             
            //Move sections
            foreach (GameObject section in sections)
            {
                //section.transform.Translate(moveVector);
                Vector3 pos = section.transform.position;
                Vector3 wantedPos = new Vector3(pos.x-spd, pos.y, pos.z);
                section.transform.position = Vector3.Lerp(pos, wantedPos, Time.deltaTime);
            }

            //Check if lost (scrolled too far)
            if (score > 0)
            {
                int previousIndex = sections.IndexOf(currentSection) - 1;
                Section prevSection = sections[previousIndex].GetComponent<Section>();
                if (prevSection.tooLate)  //if finished waiving, its too late
                {
                    lose("You didn't tap soon enough");
                }
            }

            //Delete and create if scrolled out of screen
            if (sections[0].transform.position.x <= destroyX)
            {
                destroyAndReplaceSection();
            }
            #endregion

            n_currentSpeed += n_speedChange*Time.deltaTime;

            //Change acceleration
            if (Time.time >= nextAccelChange && score > scoreForCrazy && !changingAccel && !revertingSpeed && !holdingSpeed)
            {
                changeAccel();
            }

            //Change speed
            if (changingAccel || revertingSpeed)
            {
                if (Mathf.Abs(currentSpeed - targetSpeed) <= 0.1f)
                {
                    currentSpeed = targetSpeed;
                    if (changingAccel)
                    {
                        changingAccel = false;
                        holdingSpeed = true;
                        StartCoroutine(revertTimer());  
                    }
                    else {
                        revertingSpeed = false;
                        nextAccelChange = Time.time + Random.Range(minAccelChangeFrequency, maxAccelChangeFrequency);
                    }
                }
                else if (!holdingSpeed)
                {
                    currentSpeed += speedChangeRate * Time.deltaTime;
                }                
            }
            else if (!changingAccel && !revertingSpeed && !holdingSpeed)
            {
                currentSpeed = n_currentSpeed;
            }


            columnDelay = spaceBetweenColumns / currentSpeed;
            foreach (GameObject section in sections)
            {
                section.GetComponent<Section>().columnDelay = columnDelay;
            }
        }
    }

    IEnumerator revertTimer()
    {
        yield return new WaitForSeconds(holdTime);
        revertSpeed();
    }

    void changeAccel()
    {
        targetSpeed = n_currentSpeed;
        float accelTime = Random.Range(minAccelTime, maxAccelTime);

        bool faster = Random.Range(-1, 1) < 0 ? true : false;
        if (faster)
        {
            targetSpeed += Random.Range(minSpeedDelta, maxSpeedDelta);
        }
        else
        {
            targetSpeed -= Random.Range(minSpeedDelta, maxSpeedDelta);
        }

        float speedDif = targetSpeed - currentSpeed;
        speedChangeRate = speedDif / accelTime;
        changingAccel = true;
    }

    void revertSpeed()
    {
        float accelTime = Random.Range(minAccelTime, maxAccelTime);
        targetSpeed = n_currentSpeed + (n_speedChange * accelTime);
        float speedDif = targetSpeed - currentSpeed;
        speedChangeRate = speedDif / accelTime;
        revertingSpeed = true;
        holdingSpeed = false;
    }

    public void lose(string reason)
    {
        lost = true;
        isPlaying = false;
        Manager.manager.lose(score, reason, numBalls);

        currentSection.GetComponent<Section>().lose();
        foreach (GameObject section in sections)
            section.GetComponent<Section>().stopClapping(); //Make everyone stop clapping
    }

    void destroyAndReplaceSection()
    {
        Destroy(sections[0].gameObject);
        sections.RemoveAt(0);

        GameObject newSection = Instantiate(sectionPrefab);
        Vector2 newPos = sections[sections.Count - 1].transform.position + spaceBetweenSections;
        newSection.transform.position = newPos;
        sections.Add(newSection);

        Section sectionScript = newSection.GetComponent<Section>();
        sectionScript.columnDelay = columnDelay;
        sectionScript.setMyScore(sectionScore);

        if (sectionScore == Manager.highscore)
        {
            hiscore = Instantiate(hiscorePrefab, newSection.transform);
        }

        sectionScore++;
    }

    public void pushButton(int colorIndex)
    {
        if (!Manager.manager.gameSet)
            return;
        if (lost)
            return;

        Section.TeamColor buttonColor = (Section.TeamColor)colorIndex;

        if (buttonColor == firstColor && !isPlaying && score == 0)
        {
            isPlaying = true;
            Manager.manager.startedPlaying();

            foreach (GameObject sec in sections)
            {
                sec.GetComponent<Animator>().SetBool("GamePlaying", true);
            }
        }

        currentSection.GetComponent<Section>().pushButton(buttonColor);
    }

    public void tapped(GameObject whichSection)
    {
        int sectionIndex = sections.IndexOf(whichSection);
        Section section = whichSection.GetComponent<Section>();

        bool passed = false;

        if (score == 0)
        {
            passed = true;
            tutorial.SetBool("Disable", true);  //Deactivate tutorial
        }
        else
        {
            Section prevSection = sections[sectionIndex - 1].GetComponent<Section>();
            if (prevSection.columnIndex > columnsPassedToAccept)
            {
                passed = true;
            } else {
                passed = false;
            }
        }

        if (passed)
        {
            //Passed
            whichSection.GetComponent<Section>().wave();

            previousSection = currentSection;
            currentSection = sections[sectionIndex + 1];
            StartCoroutine(setNextSectionTurn());   //Delay so doesnt click for next section
            score++;
        }
        else {
            lose("You tapped too soon");
        }
    }

    public void incrementScore()    //When tapped ball
    {
        numBalls++;

        foreach (GameObject section in sections)
        {
            Section secScript = section.GetComponent<Section>();
            if (secScript.waved == false && secScript.isWaving == false)
                Instantiate(incrementPrefab, section.transform);

            secScript.setMyScore(secScript.myScoreNum+1);
        }
        sectionScore++;
        score++;

        if (hiscore != null)
            hiscore.GetComponent<HighscoreLine>().changeScore();
    }

    IEnumerator setNextSectionTurn()
    {
        yield return new WaitForEndOfFrame();
        currentSection.GetComponent<Section>().isMyTurn = true;
    }
}
