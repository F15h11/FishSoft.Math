﻿namespace FishSoft.Math.Converter {
    public class StringToMathTerm {
        private char[] expressionArr;
        private List<string> operantsStr;
        private List<char> operators;
        private List<double> operants;
        private string exceptionMessage = "Error: ";

        public MathTerm Term;
        public StringToMathTerm(string input) {
            string temp = input + ' ';
            expressionArr = temp.ToCharArray();
            operantsStr = new List<string>();
            operators = new List<char>();
            operants = new List<double>();

            Term = getTerm();
        }
        MathTerm getTerm() {
            if(!isValidForNow()) {
                return new MathTerm(exceptionMessage);
            }

            if(!operandsAreValid()) {
                return new MathTerm(exceptionMessage);
            }

            return new MathTerm(operators.ToArray(), operants.ToArray());
        }
        bool isValidForNow() {
            char currentChar;                 //Char at index i in expressionArr
            string currentOperant = "";   
            bool getOperant = true;           //True = program searches operants //False = program searches operators
            bool atFirstChar = true;          //True = index i is at first char of possible operant or less //False = index is behind the first char of a possible operant
                                              //Switches when index i is at a char thats either a sign (-) or a number
            bool operantRequired = true;      //True = operant is required //False = ..its obvious
                                              //Switches from true to false when a operant is added
                                              //If its true after the for loop this method will return null
            bool digitRequired = false;      //if next char must be a digit
            bool containsDotYet = false;    //Switches true when a dot is added to currentOperant and false when currentOperant is beeing resetted 

            if(expressionArr.Length == 1) {
                exceptionMessage += "Input string was empty";
                return false;
            }

            for(int i = 0; i < expressionArr.Length; i++) {
                currentChar = expressionArr[i];
                #region Get Operant
                if(getOperant) {
                    if(atFirstChar) {
                        if(Character.IsDigit(currentChar)) {
                            atFirstChar = false;
                            currentOperant += currentChar;
                        }
                        else if(Character.IsMinus(currentChar)) {       //Minus will be used as sign if at first char of a operant
                            atFirstChar = false;
                            currentOperant += currentChar;
                            digitRequired = true;
                        }
                        else if(Character.IsOperator(currentChar) & !Character.IsMinus(currentChar)) {
                            exceptionMessage += "Unexpected operator found";
                            return false;
                        }
                        else if(Character.IsDot(currentChar)) {
                            exceptionMessage += "Unexpected dot found";
                            return false;
                        }
                        else if(Character.IsComma(currentChar)) {
                            exceptionMessage += "Unexpected comma found";
                        }
                        else if(Character.IsLetter(currentChar)) {
                            exceptionMessage += "Letter found.";
                            return false;
                        }
                    }
                    else if(!atFirstChar) {      //Checks for things that shouldnt be in the middle of a number and dots
                        if(Character.IsDigit(currentChar)) {   //Add number if char is number
                            if(digitRequired) {
                                digitRequired = false;
                            }
                            currentOperant += currentChar;
                        }
                        else if(Character.IsDot(currentChar)) {
                            if(containsDotYet) {
                                exceptionMessage += "Operant has to many dots/commas.";
                                return false;
                            }
                            else if(digitRequired) {
                                return false;
                            }
                            else {
                                containsDotYet = true;
                                digitRequired = true;
                                currentOperant += currentChar;
                            }
                        }
                        else if(Character.IsComma(currentChar)) {
                            if(containsDotYet) {
                                exceptionMessage += "Operant has to many dots/commas";
                                return false;
                            }
                            else if(digitRequired) {
                                return false;
                            }
                            else {
                                containsDotYet = true;
                                digitRequired = true;
                                currentOperant += '.';
                            }
                        }
                        else if(Character.IsSpace(currentChar)) {
                            if(digitRequired) {
                                exceptionMessage = "Required number not found";
                                return false;
                            }
                            operantsStr.Add(currentOperant);
                            currentOperant = "";
                            getOperant = false;
                            atFirstChar = true;
                            operantRequired = false;
                            containsDotYet = false;
                            continue;
                        }
                        else if(Character.IsOperator(currentChar)) {
                            if(digitRequired) {
                                exceptionMessage += "Unexpected Operator found";
                                return false;
                            }
                            operantsStr.Add(currentOperant);
                            currentOperant = "";
                            getOperant = false;
                            atFirstChar = true;
                            operantRequired = false;
                            containsDotYet = false;
                        }
                        else if(Character.IsLetter(currentChar)) {
                            exceptionMessage += "Letter found";
                            return false;
                        }
                    }
                }
                #endregion
                #region Get Operator
                if(!getOperant) {
                    if(Character.IsOperator(currentChar)) {
                        operators.Add(currentChar);
                        getOperant = true;
                        atFirstChar = true;
                        operantRequired = true;
                        continue;
                    }
                    if(Character.IsDigit(currentChar)) {
                        exceptionMessage += "Unexpected Number found";
                        return false;
                    }
                    else if(Character.IsLetter(currentChar)) {
                        exceptionMessage += "Letter found";
                        return false;
                    }
                }
                #endregion
            }
            if(operantsStr.Count() == 0) {
                exceptionMessage += "Input didnt contain any operants";
                return false;
            }
            if(operantRequired) {
                exceptionMessage += "Required operant not found";
                return false;
            }
            else {
                return true;
            }
        }
        bool operandsAreValid() {
            foreach(var item in operantsStr) {
                try {
                    operants.Add(Convert.ToDouble(item));
                }
                catch {
                    exceptionMessage = "Invalid operant found";
                    return false;
                }
            }
            return true;
        }
        public override string ToString() {
            string str = "\n";

            str += $"{nameof(expressionArr)}(char[]):\n";
            for(int i = 0;i < expressionArr.Length;i++) {
                str += $" {i}: {expressionArr[i]}\n";
            }
            str += $"{nameof(operantsStr)}(List<string>):\n";
            for(int i = 0;i < operantsStr.Count();i++) {
                str += $" {i}: {operantsStr[i]}\n";
            }
            str += $"{nameof(operators)}(List<char>):\n";
            for(int i = 0;i < operators.Count();i++) {
                str += $"\t{i}: {operators[i]}\n";
            }
            str += $"{nameof(operants)}(List<double>):\n";
            for(int i = 0;i < operants.Count();i++) {
                str += $" {i}: {operants[i]}\n";
            }
            return str;
        }
    }
}
