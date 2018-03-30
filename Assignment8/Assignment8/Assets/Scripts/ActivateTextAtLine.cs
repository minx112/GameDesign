using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTextAtLine : MonoBehaviour
{
    public TextAsset theText;

    public int startingLine;
    public int endLine;

    public TextBoxManager theTextBox;
    private bool waitForPress;

    public bool requireButtonPress;
    public bool randomLine;
    public int randomLineNum;
    public bool destroyWhenActivated;

	// Use this for initialization
	void Start ()
    {
        theTextBox = FindObjectOfType<TextBoxManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (waitForPress && Input.GetKeyDown(KeyCode.E))
        {
            if (randomLine)
            {
                randomLineNum = (startingLine - 1) + Mathf.CeilToInt(Random.value * ((endLine + 1) - startingLine));

                theTextBox.ReloadScript(theText);
                theTextBox.currentLine = randomLineNum - 1;
                theTextBox.endAtLine = randomLineNum;
                theTextBox.EnableTextBox();
            }
            else
            {
                theTextBox.ReloadScript(theText);
                theTextBox.currentLine = startingLine;
                theTextBox.endAtLine = endLine;
                theTextBox.EnableTextBox();
            }

            if (destroyWhenActivated)
            {
                Destroy(gameObject);
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            if(requireButtonPress)
            {
                waitForPress = true;
                return;
            }

            theTextBox.ReloadScript(theText);
            theTextBox.currentLine = startingLine;
            theTextBox.endAtLine = endLine;
            theTextBox.EnableTextBox();

            if(destroyWhenActivated)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            waitForPress = false;
        }
    }
}
