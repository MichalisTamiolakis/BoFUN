using System;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class BoFUNSpeech : MonoBehaviour
{
    //string[] m_Keywords = new string[]
    //{
    //    "Ok Bo create a game"
    //};

    GrammarRecognizer m_Recognizer;

    // Start is called before the first frame update
    void Start()
    {
        m_Recognizer = new GrammarRecognizer(Application.streamingAssetsPath + "/SRGS/BoFUNGrammar.xml", ConfidenceLevel.Low);
        m_Recognizer.OnPhraseRecognized += Recognized;
    }

    public void Listen()
    {
        m_Recognizer.Start();

    }

    public void StopListening()
    {
        m_Recognizer.Stop();
    }

    void Recognized(PhraseRecognizedEventArgs args)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
        builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
        builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(builder.ToString());

        bool hasSemantics = args.semanticMeanings != null;
        if (args.semanticMeanings != null)
        {
            foreach (SemanticMeaning sm in args.semanticMeanings)
            {
                string newSm = sm.key;
                foreach (string val in sm.values)
                    Debug.Log(newSm + " " + val);
                //semantics.Add(newSm);
            }
        }
    }
}
