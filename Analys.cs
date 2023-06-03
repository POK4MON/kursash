namespace WinFormsApp123
{
      public struct Lex
        {
            public string Lexema;
            public string Type;
            public Lex(string L, string Type)
            {
                Lexema = L;
                this.Type = Type;
            }
        }
        public class Analys
        {
            string buff = string.Empty;
            int lenght = 0;
            char curS;
            string stroka;
            List<Lex> strokes = new List<Lex>();
            public List<Lex> lexes { get { return strokes; } }
            public Analys(string s)
            {
                stroka = s;
            }

            string div = "+-*/=><,()\n ";
            string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            public void Razbor()
            {
                for (int i = 0; i < stroka.Length; i++)
                {
                    curS = stroka[i];
                    if (buff == string.Empty)
                    {
                        lenght = 0;
                        if (char.IsLetter(curS))
                        {
                            if (alphabet.Contains(curS))
                            {
                                buff += curS;
                                lenght++;
                                if (i == stroka.Length - 1)
                                {
                                    strokes.Add(new Lex(buff, "идентификатор"));
                                    buff = string.Empty;
                                }
                            }
                            else
                                throw new Exception("Подмножество поддерживает только буквы латинского алфавита.");
                        }
                        else if (char.IsDigit(curS))
                        {
                            buff += curS;
                            if (i == stroka.Length - 1)
                            {
                                strokes.Add(new Lex(buff, "литерал"));
                                buff = string.Empty;
                            }
                        }
                        else
                        {
                            if (div.Contains(curS))
                            {
                                if (curS != ' ')
                                    strokes.Add(new Lex(Convert.ToString(curS), "разделитель"));
                            }
                            else
                                throw new Exception($"Разделитель {curS} не поддерживается подмножеством.");
                        }
                    }
                    else
                    {
                        if (char.IsLetter(buff[0]))
                        {
                            if (char.IsLetter(curS))
                            {
                                if (char.IsLetter(curS) && alphabet.Contains(curS))
                                {
                                    buff += curS;
                                    lenght++;
                                    if (i == stroka.Length - 1)
                                    {
                                        if (lenght > 8)
                                            throw new Exception("Длина идентификатора превышает 8 символов.");
                                        strokes.Add(new Lex(buff, "идентификатор"));
                                        buff = string.Empty;
                                    }
                                }
                                else
                                    throw new Exception("Подмножество поддерживает только буквы латинского алфавита.");
                            }
                            else if (char.IsDigit(curS))
                            {
                                buff += curS;
                                lenght++;
                                if (i == stroka.Length - 1)
                                {
                                    if (lenght > 8)
                                        throw new Exception("Длина идентификатора превышает 8 символов.");
                                    strokes.Add(new Lex(buff, "идентификатор"));
                                    buff = string.Empty;
                                }
                            }
                            else
                            {
                                if (lenght > 8)
                                    throw new Exception("Длина идентификатора превышает 8 символов.");
                                strokes.Add(new Lex(buff, "идентификатор"));
                                buff = string.Empty;
                                if (div.Contains(curS))
                                {
                                    if (curS != ' ')
                                        strokes.Add(new Lex(Convert.ToString(curS), "разделитель"));
                                }
                                else
                                    throw new Exception($"Разделитель {curS} не поддерживается подмножеством.");
                            }
                        }
                        else
                        {
                            if (char.IsDigit(curS))
                            {
                                buff += curS;
                                if (i == stroka.Length - 1)
                                {
                                    strokes.Add(new Lex(buff, "литерал"));
                                    buff = string.Empty;
                                }
                            }
                            else
                            {
                                if (char.IsLetter(curS))
                                    throw new Exception($"Следующим символом литерала не может быть символ {curS}.");
                                strokes.Add(new Lex(buff, "литерал"));
                                buff = string.Empty;
                                if (div.Contains(curS))
                                {
                                    if (curS != ' ')
                                        strokes.Add(new Lex(Convert.ToString(curS), "разделитель"));
                                }
                                else
                                    throw new Exception($"Разделитель {curS} не поддерживается подмножеством");
                            }
                        }
                    }
                }
            }
        }
    }