﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using static WinFormsApp123.Token;

namespace WinFormsApp123
{
    class LR
    {
        List<Token> tokens = new List<Token>();
        Stack<Token> lexemStack = new Stack<Token>();
        Stack<int> stateStack = new Stack<int>();
        int nextLex = 0;
        int state = 0;
        bool isEnd = false;
        public List<Troyka> operatsii = new List<Troyka>();
        bool Firt_expr = true;


        public LR(List<Token> vvodtoken)
        {
            tokens = vvodtoken;
        }
        private Token GetLexeme(int nextLex)
        {
            return tokens[nextLex];
        }

        private void Shift()
        {
            lexemStack.Push(GetLexeme(nextLex));
            nextLex++;
        }
        private void GoToState(int state)
        {
            stateStack.Push(state);
            this.state = state;
        }
        private void Reduce(int num, string neterm)
        {
            for (int i = 0; i < num; i++)
            {
                lexemStack.Pop();
                stateStack.Pop();
            }
            state = stateStack.Peek();
            Token k = new Token(TokenType.NETERM);
            k.Value = neterm;
            lexemStack.Push(k);
        }
        private void State0()
        {
            if (lexemStack.Count == 0)
                Shift();
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<программа>":
                            if (nextLex == tokens.Count)
                                isEnd = true;
                                MessageBox.Show("Восходящий анализ завершён без ошибок!");
                            break;
                        default:
                            throw new Exception($"Error!\n String: {nextLex}; Ожидалось: программа, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;
                case TokenType.VAR:
                    GoToState(1);
                    break;
                default:
                    throw new Exception($"Error!\n String: {nextLex}; Ожидалось: Var, но было получено: {lexemStack.Peek().Value}");
            }
        }
        private void State1()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис_опис>":
                            GoToState(2);
                            break;
                        case "<опис>":
                            GoToState(3);
                            break;
                        case "<спис_перем>":
                            GoToState(4);
                            break;
                        default:
                            throw new Exception($"Error!\n String: {nextLex}; Ожидалось: <спис_опис> или <опис> или <спис_перем>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;
                case TokenType.VAR:
                    Shift();
                    break;
                case TokenType.VARIABLE:
                    GoToState(5);
                    break;
                default:
                    throw new Exception($"Error!\n String: {nextLex}; Ожидался: идентификатор, но было получено: {lexemStack.Peek().Value}");
            }
        }
        private void State2()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис_опис>":
                            Shift();
                            break;
                        case "<опис>":
                            GoToState(7);
                            break;
                        case "<спис_перем>":
                            GoToState(4);
                            break;
                        default:
                            throw new Exception($"Error!\n String: {nextLex}; Ожидалось: <опис>, <спис_перем>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;
                case TokenType.BEGIN:
                    GoToState(6);
                    break;
                case TokenType.VARIABLE:
                    GoToState(5);
                    break;
                default:
                    throw new Exception($"Error!\n String: {nextLex}; Ожидались: begin или идентификатор,  но было получено: {lexemStack.Peek().Value}");
            }
        }
        private void State3()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<опис>")
                Reduce(1, "<спис_опис>");
            else
                throw new Exception($"Error!\n String: {nextLex}; Ожидалось: <опис>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        private void State4()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис_перем>":
                            Shift();
                            break;
                        default:
                            throw new Exception($"Error!\n String: {nextLex}; Ожидалось: <спис_перем>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;
                case TokenType.COLON:
                    GoToState(8);
                    break;
                case TokenType.COMMA:
                    GoToState(9);
                    break;
                default:
                    throw new Exception($"Error!\n String: {nextLex}; Ожидались: двоеточие или запятая, но было получено: {lexemStack.Peek().Value}");
            }
        }
        private void State5()
        {
            if (lexemStack.Peek().Type == TokenType.VARIABLE)
                Reduce(1, "<спис_перем>");
            else
                throw new Exception($"Error!\n String: {nextLex}; Ожидалась: идентификатор, но было получено: {lexemStack.Peek().Value}");
        }
        private void State6()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис_опер>":
                            GoToState(10);
                            break;
                        case "<опер>":
                            GoToState(11);
                            break;
                        case "<услов.>":
                            GoToState(12);
                            break;
                        case "<присв>":
                            GoToState(13);
                            break;
                        default:
                            throw new Exception($"Error!\n String: {nextLex}; Ожидалось: <спис_опер>,<опер>, <услов.>, <присв>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;
                case TokenType.BEGIN:
                    Shift();
                    break;
                case TokenType.VARIABLE:
                    GoToState(14);
                    break;
                case TokenType.IF:
                    GoToState(15);
                    break;
                default:
                    throw new Exception($"Error!\n String: {nextLex}; Ожидались: условия или индификато,  но было получено: {lexemStack.Peek().Value}");
            }
        }

        private void State7()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<опис>")
                Reduce(2, "<спис_опис>");
            else
                throw new Exception($"Error!\n String: {nextLex}; Ожидалось: <опис>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        private void State8()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<тип>":
                            GoToState(16);
                            break;
                        default:
                            throw new Exception($"Error!\n String: {nextLex}; Ожидалось: <тип>,  но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;
                case TokenType.COLON:
                    Shift();
                    break;
                case TokenType.INTEGER:
                    GoToState(17);
                    break;
                case TokenType.REAL:
                    GoToState(18);
                    break;
                case TokenType.DOUBLE:
                    GoToState(19);
                    break;
                default:
                    throw new Exception($"Error!\n String: {nextLex}; Ожидалось: тип: integer или real или double,  но было получено: {lexemStack.Peek().Value}");
            }
        }

        private void State9()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.COMMA:
                    Shift();
                    break;
                case TokenType.VARIABLE:
                    GoToState(20);
                    break;
                default:
                    throw new Exception($"Error!\n String: {nextLex}; Ожидалась: идентификатор , но было получено: {lexemStack.Peek().Value}");
            }
        }
        private void State10()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис_опер>":
                            Shift();
                            break;
                        case "<опер>":
                            GoToState(22);
                            break;
                        case "<условн.>":
                            GoToState(12);
                            break;
                        case "<присв>":
                            GoToState(13);
                            break;
                        default:
                            throw new Exception($"Error!\n String: {nextLex}; Ожидалось: <спис_опер>,<опер>, <услов.>,  <присв>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;
                case TokenType.END:
                    GoToState(21);
                    break;
                case TokenType.VARIABLE:
                    GoToState(14);
                    break;
                case TokenType.IF:
                    GoToState(15);
                    break;
                default:
                    throw new Exception($"Error!\n String: {nextLex}; Ожидались: условие или идентификатор, end но было получено: {lexemStack.Peek().Value}");
            }
        }
        private void State11()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<опер>")
                Reduce(1, "<спис_опер>");
            else
                throw new Exception($"Error!\n String: {nextLex}; Ожидалось: <опер>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        private void State12()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<условн.>")
                Reduce(1, "<опер>");
            else
                throw new Exception($"Error!\n String: {nextLex}; Ожидалось: <цикл>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        private void State13()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<присв>")
                Reduce(1, "<опер>");
            else
                throw new Exception($"Error!\n String: {nextLex}; Ожидалось: <присв>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        private void State14()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.VARIABLE:
                    Shift();
                    break;
                case TokenType.ASSIGNMENT:
                    GoToState(23);
                    break;
                default:
                    throw new Exception($"Error!\n String: {nextLex}; Ожидалось:  символ присваивания (:=), но было получено: {lexemStack.Peek().Value}");
            }
        }
        private void State15()
        {
            switch (lexemStack.Peek().Type)
            {
                
                case TokenType.IF:
                    expr();
                    break;
                case TokenType.EXPR:
                    GoToState(24);
                    break;
                default:
                    throw new Exception($"Error!\n String: {nextLex}; Ожидалась: expr,  но было получено: {lexemStack.Peek().Value}");
            }
        }
        private void State16()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<тип>":
                            Shift();
                            break;
                        default:
                            throw new Exception($"Error!\n String: {nextLex}; Ожидалось: <тип>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;
                case TokenType.SEMICOLON:
                    GoToState(25);
                    break;
                default:
                    throw new Exception($"Error!\n String: {nextLex}; Ожидалась: точка с запятой,  но было получено:{lexemStack.Peek().Value}");
            }
        }
        private void State17()
        {
            if (lexemStack.Peek().Type == TokenType.INTEGER)
                Reduce(1, "<тип>");
            else
                throw new Exception($"Error!\n String: {nextLex}; Ожидалось: <тип>, но было получено: {lexemStack.Peek().Value}");
        }
        private void State18()
        {
            if (lexemStack.Peek().Type == TokenType.REAL)
                Reduce(1, "<тип>");
            else
                throw new Exception($"Error!\n String: {nextLex}; Ожидалось: <тип>, но было получено: {lexemStack.Peek().Value}");
        }
        private void State19()
        {
            if (lexemStack.Peek().Type == TokenType.DOUBLE)
                Reduce(1, "<тип>");
            else
                throw new Exception($"Error!\n String: {nextLex}; Ожидалось: <тип>, но было получено: {lexemStack.Peek().Value}");
        }
        private void State20()
        {
            if (lexemStack.Peek().Type == TokenType.VARIABLE)
                Reduce(3, "<спис_перем>");
            else
                throw new Exception($"Error!\n String: {nextLex}; Ожидалась: идентификатор, но было получено: {lexemStack.Peek().Value}");
        }
        private void State21()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.END:
                    Shift();
                    break;
                case TokenType.POINT:
                    GoToState(26);
                    break;
                default:
                    throw new Exception($"Error!\n String: {nextLex}; Ожидалась: точка, но было получено: {lexemStack.Peek().Value}");
            }
        }
        private void State22()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<опер>")
                Reduce(2, "<спис_опер>");
            else
                throw new Exception($"Error!\n String: {nextLex}; Ожидалось: <спис_опер>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        private void State23()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<операнд>":
                            GoToState(27);
                            break;
                        default:
                            throw new Exception($"Error!\n String {nextLex}: Ожидалось: <операнд>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;
                case TokenType.ASSIGNMENT:
                    Shift();
                    break;
                case TokenType.VARIABLE:
                    GoToState(28);
                    break;
                case TokenType.NUMBER:
                    GoToState(29);
                    break;
                default:
                    throw new Exception($"Error!\n String {nextLex}: Ожидался: идентификатор или число, но было получено: {lexemStack.Peek().Value}");
            }
        }
        private void State24()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.EXPR:
                    Shift();
                    //expr();
                    break;
                case TokenType.THEN:
                    GoToState(30);
                    break;
                default:
                    throw new Exception($"Error!\n String {nextLex}: Ожидался: tHen,  но было получено: {lexemStack.Peek().Value}");
            }
        }
        private void State25()
        {
            if (lexemStack.Peek().Type == TokenType.SEMICOLON)
                Reduce(4, "<опис>");
            else
                throw new Exception($"Error!\n String {nextLex}: Ожидалась: точка с запятой, но было получено: {lexemStack.Peek().Value}");
        }
        private void State26()
        {
            if (lexemStack.Peek().Type == TokenType.POINT)
                Reduce(6, "<программа>");
            else
                throw new Exception($"Error!\n String {nextLex}: Ожидалось: точка, но было получено: {lexemStack.Peek().Value}");
        }
        private void State27()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<операнд>":
                            Shift();
                            break;
                        case "<знак>":
                            Shift();
                            break;
                        default:
                            throw new Exception($"Error!\n String {nextLex}: Ожидалось: <знак>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;
                case TokenType.SEMICOLON:
                    GoToState(31);
                    break;
                case TokenType.PLUS:
                    GoToState(33);
                    break;
                case TokenType.MINUS:
                    GoToState(34);
                    break;
                case TokenType.MULTIPLY:
                    GoToState(35);
                    break;
                case TokenType.DIVISION:
                    GoToState(36);
                    break;
                default:
                    throw new Exception($"Error!\n String {nextLex}: Ожидались: точка с запятой или люс или минус или умножить или делить, но было получено: {lexemStack.Peek().Value}");
            }
        }

        private void State28()
        {
            if (lexemStack.Peek().Type == TokenType.VARIABLE)
                Reduce(1, "<операнд>");
            else
                throw new Exception($"Error!\n String {nextLex}: Ожидалась: идеяфикато ,  но было получено:  {lexemStack.Peek().Value}");
        }
        private void State29()
        {
            if (lexemStack.Peek().Type == TokenType.NUMBER)
                Reduce(1, "<операнд>");
            else
                throw new Exception($"Error!\n String {nextLex}: Ожидалось: число,  но было получено: {lexemStack.Peek().Value}");
        }
        private void State30()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<блок_опер>":
                            GoToState(37);
                            break;
                        case "<опер>":
                            GoToState(38);
                            break;
                        case "<условн.>":
                            GoToState(12);
                            break;
                        case "<присв>":
                            GoToState(13);
                            break;
                        default:
                            throw new Exception($"Error!\n String {nextLex}: Ожидалось: <блок_опер>,<опер> ,<условн.>,<присв>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;
                case TokenType.THEN:
                    Shift();
                    break;
                case TokenType.BEGIN:
                    GoToState(39);
                    break;
                case TokenType.VARIABLE:
                    GoToState(14);
                    break;
                case TokenType.IF:
                    GoToState(15);
                    break;
                default:
                    throw new Exception($"Error!\n String {nextLex}: Ожидались: begin, IF или инефикато, но было получено: {lexemStack.Peek().Value}");
            }
        }
        private void State31()
        {
            if (lexemStack.Peek().Type == TokenType.SEMICOLON)
                Reduce(4, "<присв>");
            else
                throw new Exception($"Error!\n String {nextLex}: Ожидалось: <присв>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        private void State32()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<знак>":
                            Shift();
                            break;
                        case "<операнд>":
                            GoToState(40);
                            break;
                        default:
                            throw new Exception($"Error!\n String {nextLex}: Ожидалось: <знак> или <операнд>,  но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;
                case TokenType.VARIABLE:
                    GoToState(28);
                    break;
                case TokenType.NUMBER:
                    GoToState(29);
                    break;
                default:
                    throw new Exception($"Error!\n String {nextLex}: Ожидались: индификато или число, но было получено: {lexemStack.Peek().Value}");
            }

        }
        private void State33()
        {
            if (lexemStack.Peek().Type == TokenType.PLUS)
                Reduce(1, "<знак>");
            else
                throw new Exception($"Error!\n String {nextLex}: Ожидались: <знак>, но было получено: {lexemStack.Peek().Value}");
        }
        private void State34()
        {
            if (lexemStack.Peek().Type == TokenType.MINUS)
                Reduce(1, "<знак>");
            else
                throw new Exception($"Error!\n String {nextLex}: Ожидались: <знак>, но было получено: {lexemStack.Peek().Value}");
        }
        private void State35()
        {
            if (lexemStack.Peek().Type == TokenType.MULTIPLY)
                Reduce(1, "<знак>");
            else
                throw new Exception($"Error!\n String {nextLex}: Ожидались: <знак>, но было получено: {lexemStack.Peek().Value}");
        }
        private void State36()
        {
            if (lexemStack.Peek().Type == TokenType.DIVISION)
                Reduce(1, "<знак>");
            else
                throw new Exception($"Error!\n String {nextLex}: Ожидались: <знак>, но было получено: {lexemStack.Peek().Value}");
        }
        private void State37()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<блок_опер>":
                            Shift();
                            break;
                        default:
                            throw new Exception($"Error!\n String {nextLex}: Ожидалось: <блок_опер>,  но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;
                case TokenType.ELSE:
                    GoToState(41);
                    break;
                default:
                    throw new Exception($"Error!\n String {nextLex}: Ожидались: else, но было получено:  {lexemStack.Peek().Value}");
            }
        }
        private void State38()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<опер>")
                Reduce(1, "<блок_опер>");
            else
                throw new Exception($"Error!\n String {nextLex}: Ожидалось: <опер>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        private void State39()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис_опер>":
                            GoToState(42);
                            break;
                        case "<опер>":
                            GoToState(43);
                            break;
                        case "<условн.>":
                            GoToState(12);
                            break;
                        case "<присв>":
                            GoToState(13);
                            break;
                        default:
                            throw new Exception($"Error!\n String {nextLex}: Ожидалось: <спис_опер>,<опер> ,<условн.>,<присв>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;
                case TokenType.BEGIN:
                    Shift();
                    break;
                case TokenType.VARIABLE:
                    GoToState(14);
                    break;
                case TokenType.IF:
                    GoToState(15);
                    break;
                default:
                    throw new Exception($"Error!\n String {nextLex}: Ожидались: begin, IF или инефикато, но было получено: {lexemStack.Peek().Value}");
            }
        }
        private void State40()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<операнд>":
                            Shift();
                            break;
                        default:
                            throw new Exception($"Error!\n String {nextLex}: Ожидалось: <операнд>,  но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;
                case TokenType.SEMICOLON:
                    GoToState(44);
                    break;
                default:
                    throw new Exception($"Error!\n String {nextLex}: Ожидались: точка с заятой, но было получено:  {lexemStack.Peek().Value}");
            }
        }
        private void State41()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<блок_опер>":
                            GoToState(45);
                            break;
                        case "<опер>":
                            GoToState(49);
                            break;
                        case "<спис_опер>":
                            GoToState(10);
                            break;
                        case "<условн.>":
                            GoToState(12);
                            break;
                        case "<присв>":
                            GoToState(13);
                            break;
                        default:
                            throw new Exception($"Error!\n String {nextLex}: Ожидалось: <блок_опер>,<опер>,<спис_опер>,<условн.>,<присв>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;
                case TokenType.ELSE:
                    Shift();
                    break;
                case TokenType.BEGIN:
                    GoToState(39);
                    break;
                case TokenType.VARIABLE:
                    GoToState(14);
                    break;
                case TokenType.IF:
                    GoToState(15);
                    break;
                default:
                    throw new Exception($"Error!\n String {nextLex}: Ожидались: begin, IF или инефикато, но было получено: {lexemStack.Peek().Value}");
            }
        }
        private void State42()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис_опер>":
                            Shift();
                            break;
                        case "<опер>":
                            GoToState(47);
                            break;
                        case "<условн.>":
                            GoToState(12);
                            break;
                        case "<присв>":
                            GoToState(13);
                            break;
                        default:
                            throw new Exception($"Error!\n String {nextLex}: Ожидалось: <блок_опер>,<опер>,<спис_опер>,<условн.>,<присв>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;
                case TokenType.END:
                    GoToState(46);
                    break;
                case TokenType.VARIABLE:
                    GoToState(14);
                    break;
                case TokenType.IF:
                    GoToState(15);
                    break;
                default:
                    throw new Exception($"Error!\n String {nextLex}: Ожидались: End, IF или инефикато, но было получено: {lexemStack.Peek().Value}");
            }
        }
        private void State43()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<опер>")
                Reduce(1, "<спис_опер>");
            else
                throw new Exception($"Error!\n String {nextLex}: Ожидалось: <опер>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        private void State44()
        {
            if (lexemStack.Peek().Type == TokenType.SEMICOLON)
                Reduce(6, "<присв>");
            else
                throw new Exception($"Error!\n String {nextLex}: Ожидалось: точка с заятой, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        private void State45()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<блок_опер>")
                Reduce(6, "<условн.>");
            else
                throw new Exception($"Error!\n String {nextLex}: Ожидалось: <блок_опер>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        private void State46()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.END:
                    Shift();
                    break;
                case TokenType.SEMICOLON:
                    GoToState(48);
                    break;
                default:
                    throw new Exception($"Error!\n String {nextLex}: Ожидался: точка с заятой,  но было получено: {lexemStack.Peek().Value}");
            }
        }
        private void State47()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<опер>")
                Reduce(2, "<спис_опер>");
            else
                throw new Exception($"Error!\n String {nextLex}: Ожидалось: <опер>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        private void State48()
        {
            if (lexemStack.Peek().Type == TokenType.SEMICOLON)
                Reduce(4, "<блок_опер>");
            else
                throw new Exception($"Error!\n String {nextLex}: Ожидалось: точка с заятой, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        int index = 0;

        private void expr()
        {
            List<Token> t0kens = new List<Token>();
            while (tokens[nextLex].Type != TokenType.THEN)
            {
                t0kens.Add(tokens[nextLex]);
                nextLex++;
            }
            if (Firt_expr == true)
            {
                Bower bower = new Bower(t0kens);
                bower.Start();
                foreach (Troyka z in bower.troyka)
                {
                    operatsii.Add(z);
                }
                index = bower.index;
                Firt_expr = false;

            }
            else
            {
                Bower bower = new Bower(t0kens, index);
                bower.Start();
                foreach (Troyka z in bower.troyka)
                {
                    operatsii.Add(z);
                }
                index = bower.index;

            }
            Token k = new Token(TokenType.EXPR);
            lexemStack.Push(k);
        }
        public static string ConvertLex(TokenType type)
        {
            string s = "";
            switch (type)
            {
                case TokenType.VARIABLE:
                    s = "идентификатор";
                    break;
                case TokenType.NUMBER:
                    s = "литерал";
                    break;
                case TokenType.PLUS:
                    s = "+";
                    break;
                case TokenType.MINUS:
                    s = "-";
                    break;
                case TokenType.MULTIPLY:
                    s = "*";
                    break;
                case TokenType.DIVISION:
                    s = "/";
                    break;
                case TokenType.EQUAL:
                    s = "=";
                    break;
                case TokenType.MORE:
                    s = ">";
                    break;
                case TokenType.LESS:
                    s = "<";
                    break;
                case TokenType.COMMA:
                    s = ",";
                    break;
                case TokenType.LEFTBRACKET:
                    s = "(";
                    break;
                case TokenType.RIGHTBRACKET:
                    s = ")";
                    break;
                case TokenType.INTEGER:
                    s = "integer";
                    break;
                case TokenType.DOUBLE:
                    s = "double";
                    break;
                case TokenType.VAR:
                    s = "var";
                    break;
                case TokenType.REAL:
                    s = "Real";
                    break;
                case TokenType.DO:
                    s = "do";
                    break;
                case TokenType.IF:
                    s = "if";
                    break;
                case TokenType.THEN:
                    s = "then";
                    break;
                case TokenType.ELSE:
                    s = "else";
                    break;
                case TokenType.END:
                    s = "end";
                    break;
                case TokenType.OR:
                    s = "or";
                    break;
                case TokenType.AND:
                    s = "and";
                    break;
                case TokenType.EXPR:
                    s = "Сложное логическое выражение";
                    break;
                default:
                    break;
            }
            return s;
        }
    public void Start()
        {
            try
            {
                stateStack.Push(0);
                while (isEnd != true)
                    switch (state)
                    {
                        case 0:
                            State0();
                            break;
                        case 1:
                            State1();
                            break;
                        case 2:
                            State2();
                            break;
                        case 3:
                            State3();
                            break;
                        case 4:
                            State4();
                            break;
                        case 5:
                            State5();
                            break;
                        case 6:
                            State6();
                            break;
                        case 7:
                            State7();
                            break;
                        case 8:
                            State8();
                            break;
                        case 9:
                            State9();
                            break;
                        case 10:
                            State10();
                            break;
                        case 11:
                            State11();
                            break;
                        case 12:
                            State12();
                            break;
                        case 13:
                            State13();
                            break;
                        case 14:
                            State14();
                            break;
                        case 15:
                            State15();
                            break;
                        case 16:
                            State16();
                            break;
                        case 17:
                            State17();
                            break;
                        case 18:
                            State18();
                            break;
                        case 19:
                            State19();
                            break;
                        case 20:
                            State20();
                            break;
                        case 21:
                            State21();
                            break;
                        case 22:
                            State22();
                            break;
                        case 23:
                            State23();
                            break;
                        case 24:
                            State24();
                            break;
                        case 25:
                            State25();
                            break;
                        case 26:
                            State26();
                            break;
                        case 27:
                            State27();
                            break;
                        case 28:
                            State28();
                            break;
                        case 29:
                            State29();
                            break;
                        case 30:
                            State30();
                            break;
                        case 31:
                            State31();
                            break;
                        case 32:
                            State32();
                            break;
                        case 33:
                            State33();
                            break;
                        case 34:
                            State34();
                            break;
                        case 35:
                            State35();
                            break;
                        case 36:
                            State36();
                            break;
                        case 37:
                            State37();
                            break;
                        case 38:
                            State38();
                            break;
                        case 39:
                            State39();
                            break;
                        case 40:
                            State40();
                            break;
                        case 41:
                            State41();
                            break;
                        case 42:
                            State42();
                            break;
                        case 43:
                            State43();
                            break;
                        case 44:
                            State44();
                            break;
                        case 45:
                            State45();
                            break;
                        case 46:
                            State46();
                            break;
                        case 47:
                            State47();
                            break;
                        case 48:
                            State48();
                            break;
                    }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Восходящий анализ завершён с ошибкой!");
                MessageBox.Show($"{ex.Message}");
            }
        }
}
}