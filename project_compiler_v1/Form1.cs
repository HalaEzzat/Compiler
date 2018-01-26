using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//new
using System.Collections;
using System.Collections.Specialized;
using System.IO;

public delegate bool checkMethods(string s);

namespace project_compiler_v1
{
    public partial class Form1 : Form
    {
        public comment Comment;
        private static string[] reservedWord = { "bool", "break", "continue", "class", "else", "extends", "using", "false", "for", "if", "int", "new", "null", "return", "string", "true", "void", "while", "switch", "case" };
        private static string[] specialCharacters = {".","{","}","[" ,"]", "," ,";", "(", ")","=","-", "+","*","/",">","<","%",":","\'","\\","<=",">=","==" ,"!=","and","or" ,"not" };
       
        ArrayList reservedWordResult = new ArrayList();
        ArrayList specialCharactersResult = new ArrayList();
        ArrayList DigitsResult = new ArrayList();
        ArrayList IdentifiersResult = new ArrayList();
       // ArrayList othersResult = new ArrayList(); 
       static string chosenFile;

        private string[] getLines;
        private string[] fileLines;

        static int currentCharCount;
        static  bool resultUsingFlag = false ;
        static string T_Desc = "";
        static string T_Content = "";
        static int x;

        public Form1()
        {
            InitializeComponent();
            Comment = new comment();
        }
        private void button1_Click(object sender, EventArgs e)
        {

            get_tokens(richTextBox1.Lines);



            foreach (string x in reservedWordResult)
            {
                listBox4.Items.Add(x);
            }

            foreach (string x in specialCharactersResult)
            {
                listBox5.Items.Add(x);
            }

            foreach (string x in DigitsResult)
            {
                listBox6.Items.Add(x);
            }


            foreach (string x in IdentifiersResult)
            {
                listBox7.Items.Add(x);
            }

            MessageBox.Show("Matched Tokens count in this program is : '" + MergeMatchedArrayResult().ToString() + "' !");
        }
        private void button2_Click(object sender, EventArgs e)
        {

            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "Cpp Files (*.cpp)|*.cpp";


            // Call the ShowDialog method to show the dialog box.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                chosenFile = openFileDialog1.FileName.ToString();
                DisplayFile();
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {

            listBox4.Items.Clear();
            listBox5.Items.Clear();
            listBox6.Items.Clear();
            listBox7.Items.Clear();
            richTextBox1.Clear();
            richTextBox2.Clear(); 
            ListViewDetails.Items.Clear();


            IdentifiersResult.Clear();
            reservedWordResult.Clear();
            specialCharactersResult.Clear();
            DigitsResult.Clear();
            


            chosenFile = " ";
            T_Desc = "";
            T_Content = "";
            currentCharCount = 0;

            /*
              foreach(string s in getLines)
                   s="";
              */
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(richTextBox1.Text))
            {


                listBox4.Items.Clear();
                listBox5.Items.Clear();
                listBox6.Items.Clear();
                listBox7.Items.Clear();
                richTextBox1.Clear();
                richTextBox2.Clear(); 
                ListViewDetails.Items.Clear();


                IdentifiersResult.Clear();
                reservedWordResult.Clear();
                specialCharactersResult.Clear();
                DigitsResult.Clear();


                chosenFile = " ";
                T_Desc = "";
                T_Content = "";
                currentCharCount = 0;
            }
        }

        void DisplayFile()
        {
            StringCollection linesCollection = ReadFileIntoStringCollection();
            getLines = new string[linesCollection.Count];
            linesCollection.CopyTo(getLines, 0);
            this.richTextBox2.Lines = getLines;


        }
        StringCollection ReadFileIntoStringCollection()
        {
            const int MaxBytes = 65536;
            StreamReader sr = new StreamReader(chosenFile);
            StringCollection result = new StringCollection();
            int nBytesRead = 0;
            string nextLine;

            while ((nextLine = sr.ReadLine()) != null)
            {

                nBytesRead += nextLine.Length;
                if (nBytesRead > MaxBytes)
                    break;
                result.Add(nextLine);
            }
            sr.Close();
            return result;
        }

        public char CurrentChar(char[] array, int count)
        {
            return array[count];
        }
        public char NextChar(char[] array, int count)
        {
            if (count < array.Length - 1)
                return array[count + 1];
            else
                return '@';
        }
        public char PrevChar(char[] array, int count)
        {
            if (count <= array.Length - 1)
                return array[count -1];
            else
                return '@';
        }
       
        public List<string> ClearWhiteSpaceInLine(string line)
        {
            List<string> strArr = new List<string>();
            strArr = SplitFun(line);
            return strArr;
        }
        List<string> SplitFun(string line)
        {
            List<string> ar = new List<string>();
            string tmp = "";
            for (int i = 0; i < line.Length; i++)
            {
                //case1
                if (line[i] != ' ')
                {
                    tmp = tmp + line[i];

                    //case4(end of string)
                    if (i == line.Length - 1)
                    {
                        ar.Add(tmp);
                        tmp = "";
                    }
                    continue;
                }

                //case2
                if (i != 0 && line[i] == ' ' && line[i - 1] != ' ')
                {
                    ar.Add(tmp);
                    tmp = "";
                    continue;
                }

                //case3(str1 str2    str3)
                else
                    continue;
            }



       //    foreach(string s in ar)
         //       MessageBox.Show(s);

            return ar;
        }
        public int MergeMatchedArrayResult()
        {

            ArrayList MergedArrResult= new ArrayList();

            for (int i = 0; i < reservedWordResult.Count; i++)
            {
                MergedArrResult.Add(reservedWordResult[i]);
            }

            for (int i = 0; i < specialCharactersResult.Count; i++)
            {
                MergedArrResult.Add(specialCharactersResult[i]);
            }

            for (int i = 0; i < DigitsResult.Count; i++)
            {
                MergedArrResult.Add(DigitsResult[i]);
            }

            for (int i = 0; i < IdentifiersResult.Count; i++)
            {
                MergedArrResult.Add(IdentifiersResult[i]);
            }

            return MergedArrResult.Count;
        }
       private void usingKeyWord(List<string> splitedLine)
        {
          // foreach(string s in splitedLine)
           //MessageBox.Show(splitedLine[0]);

           if (splitedLine[0] == "using")
           {
               string str = "";

               for (int i = 1; i < splitedLine.Count; i++)
               {
                   if (i == 1)

                       str += splitedLine[1];
                   else
                       str += " " + splitedLine[i];
               }

               string path = "";
               bool flag1 = false;
               bool flag2 = false;


               if ( str!="" && str[0] == '(' && str[1] == '\'')
               {

                   for (int i = 2; i < str.Length - 2; i++)
                   {
                       path += str[i];
                       flag1 = true;
                   }

                   if (str[str.Length - 2] == '\'' && str[str.Length - 1] == ')')
                   {
                       flag2 = true;
                   }


                   //check flags
                   try
                   {
                       if (flag1 == true && flag2 == true)
                       {
                           chosenFile = path;
                           //MessageBox.Show("path is exist:" + chosenFile);              
                           StringCollection linesCollection = ReadFileIntoStringCollection();
                           fileLines = new string[linesCollection.Count];
                           linesCollection.CopyTo(fileLines, 0);
                           this.richTextBox2.Lines = fileLines;
                           get_tokens(this.fileLines);
                           resultUsingFlag = true;
                       }

                   }
                   catch (FileNotFoundException)
                   {
                       MessageBox.Show("The file is not found in the specified location !");

                   }
                   catch (Exception ex)
                   {
                       MessageBox.Show(ex.Message);

                   }
               }

               else
               {
                   MessageBox.Show("Error1 in writing sytax of 'using' keyword!  ");
               }

           }
           else
           {
               MessageBox.Show("Error2 in writing sytax of 'using' keyword!  ");
           }
            
        }

        public string GetWhileInt(int count, checkMethods check, char[] array, string startStr)
        {
            checkMethods checkDeleget = check;
            string temp = "";
            temp += startStr;
            count++;
            count++;
            //if doesnt reach to the end of arr of characters and this char is number
            while (count < array.Length && checkDeleget(array[count].ToString()))
            {
                temp += array[count];
                count++;

            }
            currentCharCount = count;
            return temp;
        }
        public string GetWhileKeyword(int count, checkMethods check, char[] array, char startChar)
        {
            checkMethods checkDeleget = check;
            string temp = "";
            temp += startChar;
            count++;

            while (count < array.Length && checkRestOfKeyWordOrIdentOrNo(array[count].ToString()))
            {
                temp += array[count];
                count++;
                if (checkDeleget(temp) == true)
                    break;

            }


            currentCharCount = count;
            return temp;
        }
        
        static bool checkReservedWord(string token)
        {
            bool flag = false;
            foreach (string x in reservedWord)
            {
                if (x==token)
                {
                    flag = true;
                   
                }
            }
            return flag;
        }
        static bool checkSpecialCharacters(string token)
        {
            bool flag = false;
            foreach (string x in specialCharacters)
            {
                if (x == token)
                {
                    flag = true;
                }
            }
            return flag;
        }
        static bool checkDigits(string value)
        {
            char[] c = value.ToCharArray();

            for (int i = 0; i < c.Length; i++)
            {
                if (!(char.IsDigit(c[i])))
                    return false;
            }
            return true;
         }
        static bool checkIsStartOfKeyWordOrIdentOrNo(string value)
        {

            if ((char.IsLetterOrDigit(value[0])) || (value[0] == '_'))
                return true;
            return false;

        }
        static bool checkRestOfKeyWordOrIdentOrNo(string value)
        {
            // ^ indicate letter or digit
            //      dot net perls 867-5309?!
            //      ^^^ ^^^ ^^^^^ ^^^ ^^^^

   
            if (   (char.IsLetterOrDigit(value[0]))  ||  (value[0] == '_')  )
                    return true;
                 return false;
          
        }  
        static bool checkValidIdentifier(string value)
        {
            if (char.IsDigit(value[0]))   
                return false;
            return true;
        }

        void get_tokens(string[] strArray)
        {
            int _currentLine = 0;
            List<string> Code = Comment.IgnoreComments(strArray);

            //traverse each line
            foreach (string item in Code)
            {

                List<string> splitedLine = ClearWhiteSpaceInLine(item);


                //traverse each token in a specific line
                for (int i = 0; i < splitedLine.Count; i++)
                {

                    char[] charToken = splitedLine[i].ToCharArray();
                    currentCharCount = 0;
                    //traverse each char in a specific token
                    for (int j = 0; j < charToken.Length; j++)
                    {

                        if (currentCharCount >= charToken.Length)
                            break;
                        char currentChar = CurrentChar(charToken, currentCharCount);


                        if (checkIsStartOfKeyWordOrIdentOrNo(currentChar.ToString()) == true)
                        {
                            //MessageBox.Show("keyOrident : " + currentChar.ToString() + " count " + currentCharCount);

                            string keywordOrIdentOrNumber = "";
                            checkMethods keywordCheck = new checkMethods(checkReservedWord);
                            keywordOrIdentOrNumber += GetWhileKeyword(currentCharCount, keywordCheck, charToken, currentChar);

                            if (checkReservedWord(keywordOrIdentOrNumber))
                            {

                                if (keywordOrIdentOrNumber == "using")
                                {

                                    usingKeyWord(splitedLine);
                                    reservedWordResult.Add(keywordOrIdentOrNumber);
                                    T_Desc = "ReverseWord";
                                    T_Content = keywordOrIdentOrNumber;

                                    if (resultUsingFlag)
                                    {
                                        // x = (splitedLine.Count - 1) - i;
                                        i = splitedLine.Count - 1;
                                    }
                                }
                                else
                                {

                                    reservedWordResult.Add(keywordOrIdentOrNumber);
                                    T_Desc = "ReverseWord";
                                }
                            }
                            else if (checkDigits(keywordOrIdentOrNumber))
                            {

                                DigitsResult.Add(keywordOrIdentOrNumber);
                                T_Desc = "Constant";

                            }


                            else if (checkValidIdentifier(keywordOrIdentOrNumber))
                            {

                                if ((new[] { "and", "or", "not" }.Contains(keywordOrIdentOrNumber)))
                                {
                                    specialCharactersResult.Add(keywordOrIdentOrNumber);
                                    T_Desc = "Symbol";

                                }
                                else
                                {
                                    IdentifiersResult.Add(keywordOrIdentOrNumber);
                                    T_Desc = "Identifier";

                                }

                            }
                            else 
                            {
                                T_Desc = "lexical Errors";
                                MessageBox.Show("lexical Errors '" + keywordOrIdentOrNumber + "'! at line:" + _currentLine);
                            }


                            T_Content = keywordOrIdentOrNumber;
                        }//end checkIsStartOfKeyWordOrIdentOrNo


                        else if (checkSpecialCharacters(currentChar.ToString()) == true)
                        {



                            if ((new[] { '<', '>', '=', '!' }.Contains(currentChar)) && NextChar(charToken, currentCharCount) == '=')
                            {
                                currentCharCount++;
                                currentCharCount++;
                                specialCharactersResult.Add(currentChar.ToString() + "=");
                                T_Desc = "Symbol";
                                T_Content = currentChar.ToString();
                            }

                            else if ((new[] { '+', '-' }.Contains(currentChar)) && (char.IsDigit(NextChar(charToken, currentCharCount))))
                            {

                                string AssignAndStartNo = currentChar.ToString() + NextChar(charToken, currentCharCount);
                                string intNumber = "";
                                checkMethods numberCheck = new checkMethods(checkDigits);
                                intNumber += GetWhileInt(currentCharCount, numberCheck, charToken, AssignAndStartNo);
                                DigitsResult.Add(intNumber);

                                T_Desc = "Constant";
                                T_Content = intNumber;
                            }
                         /*   else if ((currentChar == '.') && (char.IsLetter(PrevChar(charToken, currentCharCount))))
                            {
                                currentCharCount++;
                                specialCharactersResult.Add(currentChar.ToString());
                                T_Desc = "Symbol";
                                T_Content = currentChar.ToString();
                            }
                          */
                            else
                            {
                                currentCharCount++;
                                specialCharactersResult.Add(currentChar.ToString());
                                T_Desc = "Symbol";
                                T_Content = currentChar.ToString();
                            }

                        }//check special symbols

                        else
                        {

                            T_Desc = "Illegal characters";
                            T_Content = currentChar.ToString();
                            MessageBox.Show("Illegal characters '" + currentChar.ToString() + "' ! at line:" + _currentLine);
                        }


                        ListViewItem List_item = new ListViewItem(_currentLine.ToString());
                        List_item.SubItems.Add(T_Desc);
                        List_item.SubItems.Add(T_Content);
                        ListViewDetails.Items.Add(List_item);


                    }//end for char
                }//end for token

                
                _currentLine++;


            }//end foreach lines

          
        }//end get_tokens() fun

        
         
    
    }//end class
}//end name space
