using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DebugGame : MonoBehaviour
{
    public static event Action onDataChange;

    [SerializeField] private TextMeshProUGUI totalMoneyText;
    [SerializeField] private TextMeshProUGUI printText;
    [SerializeField] private GameObject inputFieldPrefab;
    [SerializeField] private GameObject inputArrayFieldPrefab;

    [SerializeField] private Transform parentInputField;
    private GameObject[] inputFieldInstances;
    private GameData gameData;
    private Type type;
    private FieldInfo[] fieldInfos;

    int totalBoolArrays;
    bool[][] boolArrays;

    private void Awake()
    {
        gameData = new GameData();
        gameData = GameManager.Instance.GetGameData();
        type = gameData.GetType();
        fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

        CreateInputFields();

    }

    private void CreateInputFields()
    {
        //Debug.Log("Create Input Fields");
        int lenght = fieldInfos.Length;
        inputFieldInstances = new GameObject[lenght];

        for (int i = 0; i < lenght; i++)
        {
            Type fieldType = fieldInfos[i].FieldType;
            if (fieldType == typeof(bool[]))
            {
                inputFieldInstances[i] = Instantiate(inputArrayFieldPrefab, parentInputField.position, Quaternion.identity);
                inputFieldInstances[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = fieldInfos[i].ToString();
                inputFieldInstances[i].transform.SetParent(parentInputField);
                object value = fieldInfos[i].GetValue(gameData);
                if (value is Array arr)
                {
                    //Debug.Log("The Array size: " + arr.Length);
                }

            }
            else
            {
                inputFieldInstances[i] = Instantiate(inputFieldPrefab, parentInputField.position, Quaternion.identity);
                inputFieldInstances[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = fieldInfos[i].ToString();
                inputFieldInstances[i].transform.SetParent(parentInputField);
                //Debug.Log("Current Input Field: " + i + ": " + fieldInfos[i].ToString());
                inputFieldInstances[i].transform.GetChild(0).name = "" + i;
            }

        }

    }

    private void DetermineArrays()
    {
        for (int i = 0; i < fieldInfos.Length; i++)
        {
            //totalBoolArrays
            Type fieldType = fieldInfos[i].FieldType;
            if (fieldType == typeof(bool[]))
            {
                totalBoolArrays++;
                bool[] tempArr = new bool[4];
                boolArrays = new bool[totalBoolArrays][];
                object value = fieldInfos[i].GetValue(gameData);
                if (value is Array arr)
                {
                    for (int j = 0; j < totalBoolArrays; j++)
                    {
                        boolArrays[j] = tempArr;
                    }
                }
            }
            Debug.Log("Total Bools[] in game data: " + totalBoolArrays);
        }
    }

    private void PrintArr(bool[] arr)
    {
        foreach (var item in arr)
        {
            Debug.Log("item: " + item);
        }
    }

    public void ApplyFieldChanges()
    {
        //int newValue = int.Parse(inputFieldInstances[0].transform.GetChild(1).GetComponent<TMP_InputField>().text);
        //Debug.Log("New Value: " + newValue);
        for (int i = 0; i < fieldInfos.Length; i++)
        {
            Type fieldType = fieldInfos[i].FieldType;
            string inputValue = "";
            bool[] inputArrayValue = new bool[4];
            object value = fieldInfos[i].GetValue(gameData);
            if (value is Array arr)
            {
                inputArrayValue = new bool[arr.Length];
                for (int j = 0; j < arr.Length; j++)
                {
                    inputArrayValue[j] = false;
                }
            }

            if (fieldType == typeof(bool[]))
            {
                PrintArr(inputArrayValue);
                fieldInfos[i].SetValue(gameData, inputArrayValue);
            }
            else
            {
                inputValue = inputFieldInstances[i].transform.GetChild(1).GetComponent<TMP_InputField>().text;
            }

            //Debug.Log("input Value: " + string.IsNullOrEmpty(inputValue));
            if (!string.IsNullOrEmpty(inputValue))
            {
                if (fieldType == typeof(int))
                {
                    int newValue = int.Parse(inputValue);
                    Debug.Log(i + " Current Value: " + fieldInfos[i].GetValue(gameData) + " New Value: " + newValue);
                    fieldInfos[i].SetValue(gameData, newValue);
                    Debug.Log(i + " Value: " + fieldInfos[i].GetValue(gameData));

                }
                else if (fieldType == typeof(float))
                {
                    float newValue = float.Parse(inputValue);
                    Debug.Log(i + " Current Value: " + fieldInfos[i].GetValue(gameData) + " New Value: " + newValue);
                    fieldInfos[i].SetValue(gameData, newValue);
                    Debug.Log(i + " Value: " + fieldInfos[i].GetValue(gameData));

                }
                else if (fieldType == typeof(string))
                {
                    string newValue = inputValue;
                    Debug.Log(i + " Current Value: " + fieldInfos[i].GetValue(gameData) + " New Value: " + newValue);
                    fieldInfos[i].SetValue(gameData, newValue);
                    Debug.Log(i + " Value: " + fieldInfos[i].GetValue(gameData));

                }
                else if (fieldType == typeof(bool))
                {
                    Debug.Log(i + " Value: " + fieldInfos[i].GetValue(gameData));
                    //bool newValue = int.Parse(inputFieldInstances[i].transform.GetChild(1).GetComponent<TMP_InputField>().text);
                    //Debug.Log("Current Value: " + fieldInfos[i].GetValue(gameData) + " New Value: " + newValue);
                }
                else if (fieldType == typeof(double))
                {
                    double newValue = float.Parse(inputValue);
                    Debug.Log(i + " Current Value: " + fieldInfos[i].GetValue(gameData) + " New Value: " + newValue);
                    fieldInfos[i].SetValue(gameData, newValue);
                    Debug.Log(i + " Value: " + fieldInfos[i].GetValue(gameData));
                }

            }
        }

        GameManager.Instance.SetGameData(gameData);
        onDataChange?.Invoke();
        //fieldInfos[0].SetValue(gameData, 432);
        //Debug.Log("Value in 0: " + fieldInfos[0].GetValue(gameData));

    }

    public void PrintGameData()
    {
        //gameData = GameManager.Instance.GetGameData();
        string printData = "";
        //bool[] bools = new bool[4];

        for (int i = 0; i < fieldInfos.Length; i++)
        {
            Type fieldType = fieldInfos[i].FieldType;
            if (fieldType == typeof(bool[]))
            {
                object value = fieldInfos[i].GetValue(gameData);
                //object value = fieldInfos[i].GetValue(gameData);
                if (value is bool[] arr)
                {
                    printData += fieldInfos[i].Name + ": " + "\n";
                    for (int j = 0; j < arr.Length; j++)
                    {
                        printData += "Item " + j + ": " + arr[j] + "\n";
                    }
                }
            }
            else
            {
                printData += fieldInfos[i].Name + ": " + fieldInfos[i].GetValue(gameData) + "\n";
            }
        }
        printText.text = printData;
    }

    #region BasicDebugFeatures

    public void GoNextLevel()
    {
        int level = SceneManager.GetActiveScene().buildIndex + 1;
        GameManager.Instance.NextLevel(level);
    }

    public void SaveGame()
    {
        GameManager.Instance.SaveGame();
    }

    public void LoadGame()
    {
        GameManager.Instance.LoadGame();
    }

    public void ResetGameData()
    {
        GameManager.Instance.ResetGameData();
        GameManager.Instance.NextLevel(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion

}

/*
 * 
 * 
 */
