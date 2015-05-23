using UnityEngine;
using System.Collections;
using Parse;
using UnityEngine.UI;
using System.Threading.Tasks;

public class ParseSample : MonoBehaviour
{

    public Text numberText;
    public Text textField;

    private int Num
    {
        get { return int.Parse(this.numberText.text); }
        set { this.numberText.text = value.ToString(); }
    }

    public void SaveText()
    {
        StartCoroutine("Save");
    }

    IEnumerator Save()
    {

        var gameScore = new ParseObject("GameScore");
        gameScore["score"] = this.Num;
        var saveTask = gameScore.SaveAsync();
        while (!saveTask.IsCompleted)
        {
            yield return null;
        }

        string log = "Saved " + gameScore["score"].ToString() + " " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        this.AppendLog(log);
    }


    public void LoadText()
    {
        StartCoroutine("Load");
    }


    IEnumerator Load()
    {

        var query = new ParseQuery<ParseObject>("GameScore")
           .OrderByDescending("createdAt");

        var queryTask = query.FirstOrDefaultAsync();
        while (!queryTask.IsCompleted)
        {
            yield return null;
        }
        var gameScore = queryTask.Result;

        if (gameScore == null)
        {
            string log = "not found";
            this.AppendLog(log);
        }
        else
        {
            string log = "Loaded " + gameScore["score"].ToString() + " " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            this.AppendLog(log);
        }
    }

    private void AppendLog(string text)
    {

        string oldtext = this.textField.text;
        string newText = string.Format("{0}\n{1}", oldtext, text);

        this.textField.text = newText;
    }


    public void Increment()
    {
        this.Num++;
    }
    public void Decrement()
    {
        this.Num--;
    }
}


