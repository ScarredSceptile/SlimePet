using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Tamagotchi : MonoBehaviour
{
    private const string fileName = "/gamesave.save";

    [System.Serializable]
    public class StringUnityEvent : UnityEvent<string>
    {

    }

    [System.Serializable]
    public class IntUnityEvent : UnityEvent<int>
    {

    }

    [SerializeField]
    private StringUnityEvent _slimeGone;
    [SerializeField]
    private IntUnityEvent _slimeDaysAlive;

    [SerializeField]
    private int _hunger;
    [SerializeField]
    private int _cleanliness;
    [SerializeField]
    private int _happiness;
    [SerializeField]
    private long _lastHungerDrop;
    [SerializeField]
    private long _lastCleanlinessDrop;
    [SerializeField]
    private long _lastHappinessDrop;
    [SerializeField]
    private long _timeStarted;
    private int days;

    [SerializeField]
    private GameObject _hungerStat;
    [SerializeField]
    private GameObject _cleanStat;
    [SerializeField]
    private GameObject _happyStat;

    private Image _hungerImage;
    private Image _cleanlinessImage;
    private Image _happinessImage;

    private Text _hungerText;
    private Text _cleanlinessText;
    private Text _happinessText;

    private Animator _anim;

    [SerializeField]
    private SpriteRenderer _dirt;
    private int _slimeColor;

    [SerializeField]
    private Text _aliveText;

    // Start is called before the first frame update
    void Start()
    {
        days = 0;
        UpdateDaysAlive();

        _anim = gameObject.GetComponent<Animator>();

        _hungerImage = _hungerStat.GetComponentInChildren<Image>();
        _hungerText = _hungerImage.GetComponentInChildren<Text>();

        _cleanlinessImage = _cleanStat.GetComponentInChildren<Image>();
        _cleanlinessText = _cleanlinessImage.GetComponentInChildren<Text>();

        _happinessImage = _happyStat.GetComponentInChildren<Image>();
        _happinessText = _happinessImage.GetComponentInChildren<Text>();

        LoadGame();
    }

    // Update is called once per frame
    void Update()
    {
        CheckTimer();
        CheckStatus();
    }

    private void StandardValues()
    {
        _hunger = 10;
        _cleanliness = 10;
        _happiness = 10;
        _lastHungerDrop = CurrentTime();
        _lastCleanlinessDrop = CurrentTime();
        _lastHappinessDrop = CurrentTime();
        _timeStarted = CurrentTime();

        Color tmp = _dirt.color;
        tmp.a = 0f;
        _dirt.color = tmp;

        _slimeColor = (int)Random.Range(0.0f, 7.99f);

        _anim.SetInteger("Color", _slimeColor);

    }

    public void SaveGame()
    {
        Save save = new Save(_hunger, _cleanliness, _happiness, _lastHungerDrop, _lastCleanlinessDrop, _lastHappinessDrop, _timeStarted, _slimeColor);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + fileName);
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Saved");
    }

    public void SaveQuit()
    {
        SaveGame();
        MenuHandler handler = new MenuHandler();
        handler.quitGame();
    }

    public void DeleteSave()
    {
        File.Delete(Application.persistentDataPath + fileName);
    }

    private void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + fileName, FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            _hunger = save._hunger;
            _cleanliness = save._cleanliness;
            _happiness = save._happiness;
            _lastHungerDrop = save._hungerTime;
            _lastCleanlinessDrop = save._cleanlinessTime;
            _lastHappinessDrop = save._happinessTime;
            _timeStarted = save._timeStarted;
            _slimeColor = save._color;

            _anim.SetInteger("Color", _slimeColor);

            UpdateHunger();
            UpdateCleanliness();
            UpdateHappiness();

            CheckTimer();
            CheckStatus();
        }
        else
        {
            StandardValues();
            Debug.Log("No game save");
        }
    }

    private void CheckTimer()
    {
        int hungerDrops, cleanDrops, happyDrops, daysAlive;

        //Drops every 4 hours
        hungerDrops = (int)((CurrentTime() - _lastHungerDrop) / (3600 * 4));
        //Drops every 5 hours
        cleanDrops = (int)((CurrentTime() - _lastCleanlinessDrop) / (3600 * 5));
        //Drops every 3 hours
        happyDrops = (int)((CurrentTime() - _lastHappinessDrop) / (3600 * 3));

        daysAlive = (int)((CurrentTime() - _timeStarted) / (3600 * 24));

        if (hungerDrops != 0)
        {
            _lastHungerDrop += hungerDrops * (3600 * 4);
            _hunger -= hungerDrops;
            UpdateHunger();
        }

        if (cleanDrops != 0)
        {
            _lastCleanlinessDrop += cleanDrops * (3600 * 5);
            _cleanliness -= cleanDrops;
            UpdateCleanliness();
        }

        if (happyDrops != 0)
        {
            _lastHappinessDrop += happyDrops * (3600 * 3);
            _happiness -= happyDrops;
            UpdateHappiness();
        }

        if (daysAlive != days)
        {
            days = daysAlive;
            UpdateDaysAlive();
        }
    }

    private void CheckStatus()
    {
        if (_hunger <= 0)
        {
            long timeOfDeath = 3600 * 4 * _hunger;
            int ageOfDeath = (int)(((CurrentTime() + timeOfDeath) - _timeStarted) / (3600 * 24));
            _slimeDaysAlive.Invoke(ageOfDeath);
            _slimeGone.Invoke("Hunger");
        }

        if (_cleanliness <= 0)
        {
            long timeOfLeaving = 3600 * 5 * _cleanliness;
            int ageOfLeaving = (int)(((CurrentTime() + timeOfLeaving) - _timeStarted) / (3600 * 24));
            _slimeDaysAlive.Invoke(ageOfLeaving);
            _slimeGone.Invoke("Cleanliness");
        }

        if (_happiness <= 0)
        {
            long timeOfLeaving = 3600 * 3 * _happiness;
            int ageOfLeaving = (int)(((CurrentTime() + timeOfLeaving) - _timeStarted) / (3600 * 24));
            _slimeDaysAlive.Invoke(ageOfLeaving);
            _slimeGone.Invoke("Happiness");
        }
    }

    public void InterractedWith(string type)
    {
        switch (type)
        {
            case "Hunger":
                _hunger += 2;
                if (_hunger > 10) _hunger = 10;
                UpdateHunger();
                break;
            case "Cleanliness":
                _cleanliness += 2;
                if (_cleanliness > 10) _cleanliness = 10;
                UpdateCleanliness();
                break;
            case "Happiness":
                _happiness += 2;
                if (_happiness > 10) _happiness = 10;
                UpdateHappiness();
                break;
            default: break;
        }
    }

    private void UpdateHunger()
    {
        _hungerImage.fillAmount = _hunger / 10f;
        _hungerText.text = $"{_hunger}/10";
    }

    private void UpdateCleanliness()
    {
        _cleanlinessImage.fillAmount = _cleanliness / 10f;
        _cleanlinessText.text = $"{_cleanliness}/10";

        Color tmp = _dirt.color;
        if (_cleanliness == 10) tmp.a = 0f;
        else
        {
            tmp.a = 1 - (_cleanliness / 10f);
        }
        _dirt.color = tmp;
    }

    private void UpdateHappiness()
    {
        _happinessImage.fillAmount = _happiness / 10f;
        _happinessText.text = $"{_happiness}/10";
    }

    private void UpdateDaysAlive()
    {
        _aliveText.text = $"Days Alive: {days}";
    }

    public void ResetSlime()
    {
        DeleteSave();
        StandardValues();
        UpdateHunger();
        UpdateCleanliness();
        UpdateHappiness();
        UpdateDaysAlive();
    }

    //Returns current time in seconds
    private long CurrentTime()
    {
        //Each second is 10 000 000 ticks
        return System.DateTime.Now.Ticks / 10000000;
    }
}
