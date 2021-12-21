using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;


public class InputValidator : MonoBehaviour
{
    public enum TypeOfInput
    {
        IP,
        Username,
        Port
    }

    public TypeOfInput InputType;
    private TMP_InputField inputField;

    private void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        

        if(InputType == TypeOfInput.Port)
        {
            inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
            inputField.text = "7777";
        }else if(InputType == TypeOfInput.Username)
        {
            inputField.contentType = TMP_InputField.ContentType.Alphanumeric;
            inputField.characterLimit = 25;
        }
        else if(InputType== TypeOfInput.IP)
        {
            inputField.contentType = TMP_InputField.ContentType.Alphanumeric;
        }

        inputField.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    private void ValueChangeCheck()
    {
        //IP
        if(InputType == TypeOfInput.IP)
        {
            int counter = 0;
            foreach (char c in inputField.text)
            {
                //is not 0-9 or dot
                if (!Char.IsDigit(c))
                {
                    if (c != '.')
                    {
                        inputField.text = inputField.text.Remove(counter);
                        counter--;
                    }
                }
                else
                {
                    //Max 3 nummern nacheinander
                    if(counter > 2)
                    {
                        if(Char.IsDigit(inputField.text[counter-1]) && Char.IsDigit(inputField.text[counter - 2]) && Char.IsDigit(inputField.text[counter - 3]))
                        {
                            inputField.text = inputField.text.Remove(counter);
                            counter--;
                        }
                    }
                }

                //no double dot
                if(c == '.' && counter > 0)
                {
                    if(inputField.text[counter-1] == '.')
                    {
                        inputField.text = inputField.text.Remove(counter);
                        counter--;
                    }
                }

                counter++;
            }
            //max dots = 3
            if (inputField.text.Split(".").Length-1 > 3)
            {
                inputField.text = inputField.text.Remove(inputField.text.LastIndexOf("."));
            }

        }
        
        if(InputType == TypeOfInput.Port)
        {//0  - 65535
            
            if (inputField.text[0] == '0')
            {
                inputField.text = "";
            }

            if (int.Parse(inputField.text) > 65535)
            {
                inputField.text = "65535";
            }
        }
    }
    
    public string GetHelpText()
    {
        string helpText = "no help text defined";

        if(InputType == TypeOfInput.IP)
        {
            helpText = "Only numeric IP-Addresses";
        }else if(InputType == TypeOfInput.Username)
        {
            helpText = "Only Numbers and Letters";
        }else if(InputType == TypeOfInput.Port)
        {
            helpText = "Only Numbers (max: 65535)";
        }

        return helpText;
    }

    public string GetValidatorTypeName()
    {
        string name = "text";

        if (InputType == TypeOfInput.IP)
        {
            name = "IP-Address";
        }
        else if (InputType == TypeOfInput.Username)
        {
            name = "Username";
        }
        else if (InputType == TypeOfInput.Port)
        {
            name = "Port";
        }

        return name;
    }
}
