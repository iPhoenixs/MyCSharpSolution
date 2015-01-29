using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CalcWinFrom
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        public static int iReservedIndex = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            string strFromTxtbox = textBox1.Text.Trim();

            int iReserveIndex   = CountSetOfNumber(strFromTxtbox);
            iReservedIndex      = iReserveIndex;

            string[] setOfChar      = new string[iReserveIndex+1];
            string[] setOfSymbol    = new string[iReserveIndex];
            int iLoopCount = 0;

            StringBuilder sbdSet = new StringBuilder();
            string previousChar = "";
            foreach (char c in strFromTxtbox)
            {
               
               if(CompareSymbol(c.ToString()) == false)
               {
                   sbdSet.Append(c.ToString());
                   //MessageBox.Show("sbdSet add C = "+ sbdSet.ToString());
               }
               else
               {
                   setOfSymbol[iLoopCount] = c.ToString();

                   setOfChar[iLoopCount] = sbdSet.ToString();
                   //for (int i = 0; i <= iLoopCount;i++ )
                   //{
                   //    MessageBox.Show("setOfChar["+i.ToString()+"] ="+setOfChar[i].ToString());
                   //    MessageBox.Show("setOfSymbol[" + i.ToString() + "] =" + setOfSymbol[iLoopCount].ToString());
                   //}

                   sbdSet = new StringBuilder();
                   //MessageBox.Show("sbdSet after set = "+ sbdSet.ToString());
                   iLoopCount += 1;
               }
               previousChar = c.ToString(CultureInfo.InvariantCulture);     
            }
            setOfChar[iLoopCount] = sbdSet.ToString();
            float[] setOfNumber = ConvertStringToFloat(setOfChar);

            lblResult.Text = CalcManagement(setOfNumber, setOfSymbol).ToString();
        }

        private float CalcManagement(float[] _SetOfNumber, string[] _SetOfSymbol)
        {
            CalcParameter clc = new CalcParameter();
            clc = CalMultiplyDevide(_SetOfNumber, _SetOfSymbol);
            float result = CalPlusMinus(clc.SetOfNumber, clc.SetOfSymbol);

            return result;
        }

        
        private CalcParameter CalMultiplyDevide(float[] _SetOfNumber, string[] _SetOfSymbol)
        {
            CalcParameter calcParameter = new CalcParameter();
            
            int iSymbolIndexCount = _SetOfSymbol.Count();
            float[] SetOfNumber = _SetOfNumber;
            string[] SetOfSymbol = _SetOfSymbol;
            float tempResult = 0;
            int iSymbolIndex = 0;
            int iNumberIndex = 0;


            // 4/2+3/2*4
            for (int i = 0; i <= iSymbolIndexCount; i++)
            {
                foreach (var item in SetOfSymbol)
                {

                    if ((item == "/") || (item == "*"))
                    {
                        if (item == "/")
                        {
                            tempResult = CalcDevide(SetOfNumber[iNumberIndex], SetOfNumber[iNumberIndex + 1]);
                            SetOfNumber = reArrangeNumber(SetOfNumber, tempResult, iNumberIndex);
                            SetOfSymbol = reArrangeSymbol(SetOfSymbol, iSymbolIndex);                            
                            goto OUTLOOP;
                        }
                        if (item == "*")
                        {
                            tempResult = CalcMultiply(SetOfNumber[iNumberIndex], SetOfNumber[iNumberIndex + 1]);
                            SetOfNumber = reArrangeNumber(SetOfNumber, tempResult, iNumberIndex);
                            SetOfSymbol = reArrangeSymbol(SetOfSymbol, iSymbolIndex);                            
                            goto OUTLOOP;
                        }
                    }

                    iSymbolIndex += 1;
                    iNumberIndex += 1;
                }
            OUTLOOP:
                {                   
                    iSymbolIndex = 0;
                    iNumberIndex = 0;                    
                }
            }


            calcParameter.SetOfSymbol = SetOfSymbol;
            calcParameter.SetOfNumber = SetOfNumber;

            return calcParameter;
        }

        private float CalPlusMinus(float[] _SetOfNumber, string[] _SetOfSymbol)
        {          
            int iSymbolIndexCount = _SetOfSymbol.Count();
            float[] SetOfNumber = _SetOfNumber;
            string[] SetOfSymbol = _SetOfSymbol;
            float tempResult = 0;
            int iSymbolIndex = 0;
            int iNumberIndex = 0;

            for (int i = 0; i <= iSymbolIndexCount; i++)
            {
                foreach (var item in SetOfSymbol)
                {
                    if ((item == "-") || (item == "+"))
                    {
                        if (item == "-")
                        {
                            tempResult = CalcMinus(SetOfNumber[iNumberIndex], SetOfNumber[iNumberIndex + 1]);
                            SetOfNumber = reArrangeNumber(SetOfNumber, tempResult, iNumberIndex);
                            SetOfSymbol = reArrangeSymbol(SetOfSymbol, iSymbolIndex);
                            goto OUT2ndLOOP;
                        }
                        else if (item == "+")
                        {
                            tempResult = CalcPlus(SetOfNumber[iNumberIndex], SetOfNumber[iNumberIndex + 1]);
                            SetOfNumber = reArrangeNumber(SetOfNumber, tempResult, iNumberIndex);
                            SetOfSymbol = reArrangeSymbol(SetOfSymbol, iSymbolIndex);
                            goto OUT2ndLOOP;
                        }
                    }

                    iSymbolIndex += 1;
                    iNumberIndex += 1;
                }
            OUT2ndLOOP:
                {                    
                    iSymbolIndex = 0;
                    iNumberIndex = 0;
                }
            }

           

            return tempResult;
        }
        

        private string[] reArrangeSymbol(string[] oldSetSymbol,int iIndex)
        {
            int iCountSymbol = oldSetSymbol.Count();
            string[] tempNewSymbol = new string[iCountSymbol - 1];

            for (int i = 0; i < iCountSymbol - 1; i++)
            {
                if (i < iIndex)
                {
                    tempNewSymbol[i] = oldSetSymbol[i];
                }
                else if (i >= iIndex)
                {
                    tempNewSymbol[i] = oldSetSymbol[i+1];
                }               
            }
            return tempNewSymbol;
        }
        private float[] reArrangeNumber(float[] oldSetNumber,float newValue,int iIndex)
        {
            int iCountOldNumberIndex = oldSetNumber.Count();
            float[] tempNewNumber = new float[iCountOldNumberIndex-1];              

            for(int i = 0;i<iCountOldNumberIndex-1;i++)
            {
                if(i < iIndex)
                {
                    tempNewNumber[i] = oldSetNumber[i];
                }
                else if(i == iIndex)
                {
                    tempNewNumber[i] = newValue;
                }
                else if (i > iIndex)
                {
                    tempNewNumber[i] = oldSetNumber[i + 1];
                }                
            }
            return tempNewNumber;
        }
        private float[] ConvertStringToFloat(string[] setOfNumber)
        {
            float[] _setOfNumber = new float[setOfNumber.Count()];
            int iLoop = 0;
            foreach(var item in setOfNumber)
            {
                try
                {
                    _setOfNumber[iLoop] = (float)Convert.ToDouble(item.ToString());
                    
                }
                catch 
                {
                    _setOfNumber[iLoop] = 0;
                }
                iLoop += 1;
            }

            return _setOfNumber;
        }       
        private float CalcPlus(float firstValue,float secondValue)
        {
            float fResult = 0;

            fResult = firstValue + secondValue;

            return fResult;
        }
        private float CalcMinus(float firstValue, float secondValue)
        {
            float fResult = 0;

            fResult = firstValue - secondValue;

            return fResult;
        }
        private float CalcMultiply(float firstValue, float secondValue)
        {
            float fResult = 0;

            fResult = firstValue * secondValue;

            return fResult;
        }
        private float CalcDevide(float firstValue, float secondValue)
        {
            float fResult = 0;

            fResult = firstValue / secondValue;

            return fResult;
        }
        private bool CompareSymbol(string prmChar)
        {
            string[] sSymbol = { "+", "-", "*", "/" };
            bool bResult = false;

            for(int i = 0; i < 4; i++)
            {
                if(prmChar.ToString() == sSymbol[i].ToString())
                {
                    bResult = true;
                    //MessageBox.Show(prmChar + "  >>-->  " + sSymbol[i].ToString());                    
                    return bResult;
                }
                else
                {
                    bResult = false;
                }
            }

            return bResult;

        }
        private int CountSetOfNumber(string prmChar)
        {
            int iCount = 0;

            foreach (char c in prmChar)
            {
                if (CompareSymbol(c.ToString()) == true)
                {
                    iCount += 1;
                }
                else
                {

                }
            }
            return iCount;
        }
    }

    public class CalcParameter
    {        
        public string[] SetOfSymbol { get; set; }
        public float[] SetOfNumber { get; set; }

    }
}
