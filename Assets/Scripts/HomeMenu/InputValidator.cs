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
            inputField.text = "7777";
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
            foreach (char c in inputField.text)
            {
                if (!Char.IsDigit(c))
                {
                    inputField.text = inputField.text.Remove(inputField.text.LastIndexOf(c));
                }
                else if (int.Parse(inputField.text) > 65535)
                {
                    inputField.text = "65535";
                }
            }

           
        }

        // USERNAME
        if (InputType == TypeOfInput.Username)
        {
            
            foreach (char c in inputField.text)
            {
                if (!Char.IsLetter(c))
                {
                    if (!Char.IsDigit(c))
                    {
                        inputField.text = inputField.text.Remove(inputField.text.LastIndexOf(c));
                    }
                }
            }
            

            if(inputField.text.Length > 25)
            {
                string tempText = "";

                
                for(int c = 0; c < inputField.text.Length; c++)
                {
                    if(c < 25)
                    {
                        tempText += inputField.text[c];
                    }
                }

                inputField.text = tempText;


            }
        }
    }
}
