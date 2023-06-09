using System.Data;
using System.IO;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using static System.Windows.Forms.LinkLabel;
using static WinFormsApp123.Token;

namespace WinFormsApp123
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _Form1 = this;
        }

        public List<string> forToken = new List<string>();
        public List<string> listBuf = new List<string>();
        public List<char> forChar = new List<char>();
        List<Token> tokens = new List<Token>();
        List<string> lexemes = new List<string>();
        string str;
        string type;
        char ch;
        Token token;
        private void button1_Click(object sender, EventArgs e)
        {
            string text = "";
            try
            {
                using (StreamReader fs = new StreamReader(textBox1.Text))
                {
                    while (true)
                    {
                        string temp = fs.ReadLine();
                        if (temp == null) break;
                        text += temp;
                        text += " \n  ";
                    }
                    richTextBox1.Text = text;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("���� �� ������!");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            lexemes = new List<string>();
            string subText = "";

            foreach (char s in richTextBox1.Text)
            {
                    if (Lexems.IsIDVariable(subText) && (s == '.' || s == ' ' || s == ',' || s == '<' || s == ':' || s == '>' || s == ';' || s == '+' || s == '-' || s == '*' || s == '/'))
                    {
                        lexemes.Add(subText + " - �������������;");
                        subText = "";
                    }
                    else if (Lexems.IsLiteral(subText) && (s == ' ' || s == ';' || s == ')'))
                    {
                        lexemes.Add(subText + " - ���������;");
                        subText = "";
                    }
                    else if (Lexems.IsSeparator(subText) && (s == ' ' || s == ')' || char.IsDigit(s) || char.IsLetter(s)))
                    {
                        if (subText != "\n")
                        {
                            lexemes.Add(subText + " - �����������;");
                        }
                        subText = "";
                    }
                else if (subText == Environment.NewLine || subText == " ")
                {
                    subText = "";
                }
                subText += s;
                
            }
            richTextBox2.Clear();

            int i = 0;
            foreach (string lexem in lexemes)
            {
                i++;
                richTextBox2.Text += i + " " + lexem + Environment.NewLine;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox3.Clear();

            listBuf = new List<string>();
            forToken = new List<string>();
            forChar = new List<char>();
            int i = 0;
            string subText = " ";
            foreach (char s in richTextBox1.Text)
            {
                    if (Lexems.IsOperator(subText) && (s == '.' || s == ':' || s == ',' || s == ' ' || s == '(' || s == ' ' || s == '<' || s == '>' || s == ';'))
                    {
                        i++;
                        listBuf.Add(subText + " ");
                        forToken.Add("I");
                        forChar.Add(' ');

                        subText = "";
                    }
                    else if (Lexems.IsLiteral(subText) && (s == ' ' || s == ';' || s == ')'))
                    {
                        i++;
                        listBuf.Add(subText + " ");
                        forToken.Add("D");
                        forChar.Add(' ');

                        subText = "";
                    }
                    else if (Lexems.IsSeparator(subText) && (s == ' ' || s == ')' || char.IsDigit(s) || char.IsLetter(s)))
                    {
                        i++;
                        if (subText != "\n")
                        {
                            listBuf.Add(subText + " ");
                            forToken.Add("R");
                            forChar.Add(s);
                        }
                        subText = "";
                    }

                    else if (Lexems.IsIDVariable(subText) && !Lexems.IsOperator(subText) && (s == '.' || s == ' ' || s == ',' || s == '<' || s == ':' || s == '>' || s == ';' || s == '+' || s == '-' || s == '*' || s == '/'))
                    {
                        i++;
                        listBuf.Add(subText + " ");
                        forToken.Add("P");
                        forChar.Add(' ');
                        subText = "";
                    }

                    else if (subText == Environment.NewLine || subText == " ")
                    {
                        subText = "";
                    }
                    subText += s;
            }
            tokens.Clear();
            for (i = 0; i < listBuf.Count; i++)
            {
                str = listBuf[i].Split(' ')[0];
                type = forToken[i];

                if (type == "I")
                {
                    try
                    {
                        if (Token.IsSpecialWord(str))
                        {
                            Token token = new Token(Token.SpecialWords[str]);
                            token.Value = str;
                            tokens.Add(token);
                        }
                    }
                    catch (Exception)
                    {
                        richTextBox3.Text += $"�������������� ������ � ������ ������������ �����\n";
                    }
                }
                else if (type == "D")
                {
                    try
                    {
                        token = new Token(Token.TokenType.NUMBER);
                        token.Value = str;
                        tokens.Add(token);
                        continue;
                    }
                    catch (Exception)
                    {
                        richTextBox3.Text += $"�������������� ������ � ������ ��������";
                    }
                }
                else if (type == "P")
                {
                    try
                    {
                        token = new Token(Token.TokenType.VARIABLE);
                        token.Value = str;
                        tokens.Add(token);
                        continue;
                    }
                    catch (Exception)
                    {
                        richTextBox3.Text += $"�������������� ������ � ������ ����������";
                    }
                }
                else if (type == "R")
                {
                    try
                    {
                        if (Token.IsSpecialSymbol(str))
                        {
                            token = new Token(Token.SpecialSymbols[str]);
                            token.Value = str;
                            tokens.Add(token);
                            continue;
                        }
                    }
                    catch (Exception)
                    {
                        richTextBox3.Text += $"�������������� ������ � ������ ������������";
                    }
                }
            }
            Token.PrintTokens(richTextBox3, tokens);
        }
        private void button5_Click(object sender, EventArgs e)
        {

            LR recognizer2 = new LR(tokens);
            recognizer2.Start();
            listBox1.Items.Clear();
                listBox1.Items.Add("��������� �������� �������1 �������2");
                recognizer2.Start();
                int index = 0;
                foreach (Troyka z in recognizer2.operatsii)
                {
                    listBox1.Items.Add($"m{index} {z.deystvie.Value} {z.operand1.Value} {z.operand2.Value}");
                    index++;
                }
            lexemes.Clear();
            tokens.Clear();
        }
        public void richTextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        public static Form1 _Form1;

        private void button4_Click(object sender, EventArgs e)
        {
            LL recognizer1 = new LL(tokens);
            recognizer1.Start();
            lexemes.Clear();
            tokens.Clear();

            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Clear();
                tokens.Clear();
                foreach (Token a in tokens)
                {
                    if (a.Type == TokenType.IDENTIFIER || a.Type == TokenType.LITERAL)
                        listBox1.Items.Add($"{a.Type}    :    {a.Value}");
                    else
                        listBox1.Items.Add($"{a.Type}");
                }
                LR rule = new LR(tokens);
                listBox1.Items.Clear();
                listBox1.Items.Add("��������� �������� �������1 �������2");
                rule.Start();
                int index = 0;
                foreach (Troyka z in rule.operatsii)
                {
                    listBox1.Items.Add($"m{index} {z.deystvie.Value} {z.operand1.Value} {z.operand2.Value}");
                    index++;
                }
                MessageBox.Show("������ ��������");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"������: {ex.Message}");
                MessageBox.Show($"Error! {ex.Message}");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }
}
