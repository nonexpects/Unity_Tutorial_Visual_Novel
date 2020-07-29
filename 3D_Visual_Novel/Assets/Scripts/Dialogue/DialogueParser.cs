using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    public Dialogue[] Parse(string _CSVFileName)
    {
        List<Dialogue> dialogueList = new List<Dialogue>(); //대사 리스트 생성
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName);

        string[] data = csvData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length;)
        {
            string[] row = data[i].Split(new char[] { ',' });

            Dialogue dialogue = new Dialogue();
            dialogue.name = row[1];
            List<string> contextList = new List<string>();
            List<string> spriteList = new List<string>();
            List<string> voiceList = new List<string>();

            do
            {
                contextList.Add(row[2]);
                spriteList.Add(row[3]);
                voiceList.Add(row[4]);
                if (++i < data.Length)
                {
                    row = data[i].Split(new char[] { ',' });
                }
                else
                {
                    break;
                }
            } while (row[0].ToString() == "");

            dialogue.contexts = contextList.ToArray();
            dialogue.spriteName = spriteList.ToArray();
            dialogue.VoiceName = voiceList.ToArray();
            dialogueList.Add(dialogue);
        }

        return dialogueList.ToArray();
    }

    private void Start()
    {
        Parse("Prologue");
    }
}
