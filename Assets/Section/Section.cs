using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class Section : MonoBehaviour
{
    public Column[] columns;
    public Animator[] columnAnimators = new Animator[12];   //Public so I can see in inspector (bug)
    private Animator animator;

    public float[] columnSaturations;

    public bool isWaving = false;
    public bool waved = false;
    public int columnIndex = 0;
    public bool tooLate = false;

    private float timer;
    public float columnDelay = 0.4f;
    public float acceptanceTime = 3;

    public bool changeColor = true;

    public enum TeamColor { red, yellow, blue, Max };   //Max just to mark enum size
    public TeamColor myTeamColor = TeamColor.red;

    [SerializeField]
    private TextMeshProUGUI myScoreText = null;
    public int myScoreNum;

    public bool isMyTurn = false;

    private void Start()
    {
        animator = GetComponent<Animator>();

        myTeamColor = (TeamColor)Random.Range(0, (int)TeamColor.Max);

        int i = 0;
        foreach (Column column in columns)  //Clap and set color
        {
            column.setMyColor(myTeamColor);
            columnAnimators[i] = column.GetComponent<Animator>();
            columnAnimators[i].SetBool("Clap", true);
            columnAnimators[i].SetInteger("ClapIndex", Random.Range(0, 2));
            i++;
        }//Clap

        animator.SetBool("GamePlaying", SectionMaker.sectionMaker.isPlaying);
    }

    public void setMyScore(int myScore)
    {
        myScoreNum = myScore;
        myScoreText.text = myScoreNum.ToString();
    }

    void Update()
    {
        if (!Manager.manager.gameSet)
            return;

        if (isWaving)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else {

                //If did last column
                if (columnIndex == columns.Length - 1)
                {
                    isWaving = false;
                    waved = true;
                    StartCoroutine(tooFar());
                }

                columnAnimators[columnIndex].SetTrigger("Wave");
                if (changeColor)
                    columns[columnIndex].setSaturation(columnSaturations[columnIndex]);
                timer = columnDelay;
                columnIndex++;
            }
        }
    }

    public void pushButton(TeamColor whichButton)
    {
        if (!isMyTurn || isWaving || waved)
            return;

        if (whichButton == myTeamColor)
        {
            SectionMaker.sectionMaker.tapped(this.gameObject);
        }
        else {
            if (myScoreNum == 1)
                return;
            SectionMaker.sectionMaker.lose("You tapped the wrong button");
        }
    }

    IEnumerator tooFar()
    {
        yield return new WaitForSeconds(columnDelay*acceptanceTime);
        tooLate = true;
    }

    public void stopClapping()
    {
        if (columnAnimators == null)
            return;

        foreach (Animator animator in columnAnimators)  //the crowds
        {
            animator.SetBool("Clap", false);
        }
    }

    public void lose()
    {
        animator.SetTrigger("Fail");    //the button
        foreach (Animator animator in columnAnimators)  //the crowds
        {
            animator.SetBool("Clap", false);

            int failAnim = Random.Range(0, 4);

            switch (failAnim)
            {
                case 0:
                    break;
                case 1:
                    animator.SetTrigger("Fail01");
                    break;
                case 2:
                    animator.SetTrigger("Fail02");
                    break;
                case 3:
                    animator.SetTrigger("Fail03");
                    break;
            }
        }
    }

    public void wave()
    {
        animator.SetBool("BeGone", true);
        columnIndex = 0;
        timer = 0;
        isWaving = true;
        isMyTurn = false;
    }
}
