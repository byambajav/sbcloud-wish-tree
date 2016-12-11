using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnitySpeechToText.Utilities;
using UnitySpeechToText.Services;

public class SpeechRecognizer: MonoBehaviour {

    /// <summary>
    /// Store for SpeechToTextService property
    /// </summary>
    [SerializeField]
    SpeechToTextService m_SpeechToTextService;

    /// <summary>
    /// All the final results that have already been determined in the current recording session
    /// </summary>
    string m_PreviousFinalResults;
    /// <summary>
    /// Whether this object has stopped recording and is waiting for the last final text result of the current session.
    /// Here "last" means that there will be no more results this session, and "final" means the result is fixed
    /// ("final" is used for the sake of consistency).
    /// </summary>
    bool m_WaitingForLastFinalResultOfSession;
    /// <summary>
    /// Whether the last speech-to-text result received was a final (rather than interim) result
    /// </summary>
    bool m_LastResultWasFinal;
    string m_ResultsText;

    /// <summary>
    /// Delegate for recording timeout
    /// </summary>
    Action m_OnRecordingTimeout;
    /// <summary>
    /// Delegate for receiving the last text result
    /// </summary>
    Action<SpeechRecognizer> m_OnReceivedLastResponse;

    // public string WISHTREE_SERVER_URL = "https://c2438ab0.ngrok.io/";
    public string WISHTREE_SERVER_URL = "https://69422f4c.ngrok.io/";
    public string SANTA_OGG_PATH = "file:///Users/byambajav/git-repos/wish-tree/unity/snowflake3d4angles/Assets/santa.ogg";

    public Transform firework1;
    public Transform firework2;

    /// <summary>
    /// The specific speech-to-text service to use
    /// </summary>
    public SpeechToTextService SpeechToTextService
    {
        set
        {
            m_SpeechToTextService = value;
            RegisterSpeechToTextServiceCallbacks();
        }
    }

    void Start()
    {
        print ("Starting " + Time.time);

        RegisterSpeechToTextServiceCallbacks();
        // Start function WaitAndStartRecording as a coroutine.
        StartCoroutine(WaitAndStartRecording(3, 10));

        print ("Before WaitAndStartRecording Finishes " + Time.time);

        firework1.gameObject.SetActive(false);
        firework2.gameObject.SetActive(false);
    }

    private IEnumerator WaitAndStartRecording(float startWaitTime, float stopWaitTime) {
        yield return new WaitForSeconds(startWaitTime);
        print("WaitAndStartRecording " + Time.time);

        // play santa
        StartCoroutine(DownloadAndPlay(SANTA_OGG_PATH));
        yield return new WaitForSeconds(5);

        // Then start recording
        StartRecording();

        // reserve stop
        StartCoroutine(WaitAndStopRecording(stopWaitTime));
    }


    private IEnumerator WaitAndStopRecording(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        print("WaitAndStopRecording " + Time.time);
        StopRecording();
    }


    /// <summary>
    /// Starts recording audio for the speech-to-text service widget if not already recording.
    /// </summary>
    void StartRecording()
    {
        Debug.Log("StartRecording - SpeechRecognizer");
        if (!m_SpeechToTextService.IsRecording)
        {
            Debug.Log("!m_IsRecording");
            Debug.Log("Start recording");
            m_WaitingForLastFinalResultOfSession = false;
            m_LastResultWasFinal = false;
            m_PreviousFinalResults = "";
            m_ResultsText = m_PreviousFinalResults;
            m_SpeechToTextService.StartRecording();
        } else {
            Debug.Log("Already m_IsRecording");
        }
    }


    /// <summary>
    /// Stops recording audio for each speech-to-text service widget if already recording. Also schedules a wrap-up of the
    /// current comparison session to happen after the responses timeout.
    /// </summary>
    void StopRecording()
    {
        Debug.Log("StopRecording - SpeechRecognizer");
        if (m_SpeechToTextService.IsRecording)
        {
            Debug.Log("!m_IsRecording");
            Debug.Log("Stop recording");
            if (m_LastResultWasFinal)
            {
                ProcessEndResults();
            }
            else
            {
                m_WaitingForLastFinalResultOfSession = true;
            }
            m_SpeechToTextService.StopRecording();
        } else {
            Debug.Log("Already !m_IsRecording");
        }
    }


    /// <summary>
    /// Registers callbacks with the SpeechToTextService.
    /// </summary>
    void RegisterSpeechToTextServiceCallbacks()
    {
        if (m_SpeechToTextService != null)
        {
            Debug.Log("RegisterSpeechToTextServiceCallbacks: service is not null!");
            m_SpeechToTextService.RegisterOnError(OnSpeechToTextError);
            m_SpeechToTextService.RegisterOnTextResult(OnTextResult);
            m_SpeechToTextService.RegisterOnRecordingTimeout(OnSpeechToTextRecordingTimeout);
        } else {
            Debug.Log("RegisterSpeechToTextServiceCallbacks: service is null!");
        }
    }

    /// <summary>
    /// Unregisters callbacks with the SpeechToTextService.
    /// </summary>
    void UnregisterSpeechToTextServiceCallbacks()
    {
        if (m_SpeechToTextService != null)
        {
            m_SpeechToTextService.UnregisterOnError(OnSpeechToTextError);
            m_SpeechToTextService.UnregisterOnTextResult(OnTextResult);
            m_SpeechToTextService.UnregisterOnRecordingTimeout(OnSpeechToTextRecordingTimeout);
        }
    }

    /// <summary>
    /// Function that is called when a speech-to-text result is received.
    /// </summary>
    /// <param name="result">The speech-to-text result</param>
    void OnTextResult(SpeechToTextResult result)
    {
        Debug.Log("OnTextResult: " + result.TextAlternatives[0].Text);

        // this just uses the first alternative
        m_LastResultWasFinal = result.IsFinal;
        if (result.IsFinal)
        {
            m_PreviousFinalResults += result.TextAlternatives[0].Text;
            m_ResultsText = m_PreviousFinalResults;
            Debug.Log(m_SpeechToTextService.GetType().ToString() + " final result");
            if (m_WaitingForLastFinalResultOfSession)
            {
                m_WaitingForLastFinalResultOfSession = false;
                ProcessEndResults();
            }
        }
    }


    /// <summary>
    /// Does any final processing necessary for the results of the last started session
    /// </summary>
    void ProcessEndResults()
    {
        Debug.Log(m_SpeechToTextService.GetType().ToString() + " got last response");
        Debug.Log(SpeechToTextServiceString() + ": " + m_ResultsText);
        if (m_OnReceivedLastResponse != null)
        {
            m_OnReceivedLastResponse(this);
        }

        // send the results to the WishTree Server
        StartCoroutine(SendRequestToWishTreeServer(m_ResultsText));
    }

    IEnumerator SendRequestToWishTreeServer(string message)
    {
        Debug.Log("Going to send a request to WishTreeServer");
        string url = WISHTREE_SERVER_URL + "api/wishmessage?serial=wish-tree-01&message=" + WWW.EscapeURL(message);
        Debug.Log("url: " + url);
        WWW www = new WWW(url);
        yield return www;
        Debug.Log("WishTreeServer Response: " + www.text);
        StartCoroutine(DownloadAndPlay(WISHTREE_SERVER_URL + www.text));
        StartCoroutine(Play());
    }

    IEnumerator DownloadAndPlay(string url)
    {
        WWW www = new WWW(url);
        yield return www;

        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = www.audioClip;
        audio.Play();
    }

    /// <summary>
    /// Function that is called when an error occurs. If this object is waiting for
    /// a last response, then this error is treated as the last "result" of the current session.
    /// </summary>
    /// <param name="text">The error text</param>
    void OnSpeechToTextError(string text)
    {
        Debug.Log(" error: " + text);
        m_PreviousFinalResults += "[Error: " + text + "] ";
        m_ResultsText = m_PreviousFinalResults;
        if (m_WaitingForLastFinalResultOfSession)
        {
            m_WaitingForLastFinalResultOfSession = false;
            if (m_OnReceivedLastResponse != null)
            {
                m_OnReceivedLastResponse(this);
            }
        }
    }

    /// <summary>
    /// Function that is called when the recording times out.
    /// </summary>
    void OnSpeechToTextRecordingTimeout()
    {
        SmartLogger.Log(DebugFlags.SpeechToTextWidgets, SpeechToTextServiceString() + " call timeout");
        if (m_OnRecordingTimeout != null)
        {
            m_OnRecordingTimeout();
        }
    }

    /// <summary>
    /// Returns a string representation of the type of speech-to-text service used by this object.
    /// </summary>
    /// <returns>String representation of the type of speech-to-text service used by this object</returns>
    public string SpeechToTextServiceString()
    {
        return m_SpeechToTextService.GetType().ToString();
    }

    /// <summary>
    /// Adds a function to the received last response delegate.
    /// </summary>
    /// <param name="action">Function to register</param>
    public void RegisterOnReceivedLastResponse(Action<SpeechRecognizer> action)
    {
        m_OnReceivedLastResponse += action;
    }

    /// <summary>
    /// Removes a function from the received last response delegate.
    /// </summary>
    /// <param name="action">Function to unregister</param>
    public void UnregisterOnReceivedLastResponse(Action<SpeechRecognizer> action)
    {
        m_OnReceivedLastResponse -= action;
    }

    IEnumerator Play()
    {
        firework1.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        firework2.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        firework1.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        firework2.gameObject.SetActive(false);
    }
}
