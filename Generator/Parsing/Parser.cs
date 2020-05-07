using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Generator.Parsing
{
    public class Parser
    {
        /*названия блоков*/
        private const string _storage = "ХРАНИЛИЩЕ_ОБЪЕКТОВ";
        private const string _template = "ШАБЛОННЫЙ_ВИД";
        private const string _solution = "РЕШЕНИЕ";
        private const string _service = "СЛУЖЕБНОЕ";
        private const string _tests = "ТЕСТОВЫЕ_ДАННЫЕ";
        /*настройки в блоке СЛУЖЕБНОЕ*/
        private const string _arithmSigns = "знаки_арифм";
        private const string _arithmFuncs = "функции_арифм";

        private List<DataContainer> _storageData;
        private List<DataContainer> _testData;
        private string _templateStr, _code;
        public GenData GenData;

        public Parser()
        {
            _storageData = new List<DataContainer>();
        }
        // основной метод класса
        public GenData Read(string fileName)
        {
            _code = "";
            _templateStr = "";
            string lines = "----";
            try
            {
                string text;
                // чтение файла
                using (StreamReader s = new StreamReader(fileName)) text = s.ReadToEnd();
                // разделение файла наблоки
                Regex r = new Regex($"{lines}(\\W*)\n");
                text = r.Replace(text, lines);
                var splitedText = text.Split(lines);
                // обработка блоков
                foreach (string str in splitedText)
                {
                    var s = str.Trim(' ', '\n', '\r');
                    if (s.Length == 0) continue;
                    var pos = s.IndexOf('\n');
                    var head = s.Substring(0, pos);
                    var body = s.Remove(0, pos);
                    Parse(head, body);
                }

                GenData = new GenData(_storageData, _templateStr, _code, _testData);
            }
            catch (Exception ex)
            {
                throw new Exception($"Возникла ошибка во время парсинга фалйа! Сообщение об ошибке = [{ex.Message}] ");
            }

            return GenData;
        }

        private void Parse(string head, string body)
        {
            switch (head.Trim(' ', '\r').ToUpper())
            {
                case _storage:
                    GetStorageData(body);
                    break;
                case _template:
                    GetTemplateData(body);
                    break;
                case _service:
                    GetServiceData(body);
                    break;
                case _solution:
                    GetCode(body);
                    break;
                case _tests:
                    GetTestsD(body);
                    break;
                default:
                    break;
            }
        }


        public (string, string) GetAssociativeValues(string str)
        {
            return GetQuotedValues(str, '[', ']');
        }

        public (string, string) GetQuotedValues(string s, char bracketS, char bracketE)
        {
            string key = default, value = default;
            var start = s.IndexOf(bracketS);
            if (start >= 0)
            {
                var end = FindStringEnd(new StringBuilder(s), start, bracketE);
                if (end < 0) throw new Exception($"{s} - отсутствует символ окончания {bracketE}");
                key = s.Substring(start + 1, end - start - 1).Trim();
                value = s.Remove(start, end - start+1);
                //return s.Substring(st + 1, end - st-1).Trim();
            }

            return (key, value);
        }

        public int FindStringEnd(StringBuilder s, int pos, char separator)
        {
            //const char separator = '\"';
            pos++;
            for (;pos < s.Length; pos++)
            {
                if (s[pos] == separator)
                {
                    if (pos > 0 && s[pos - 1] == '\\')
                    {
                        s[pos - 1] = ' ';
                    }
                    else
                    {
                        return pos;
                    }
                }
            }
            return -1;
        }

        private void HandleOfSymbol(StringBuilder s, int i, char super)
        {
            if (i > 0 && s[i - 1] == '\\')
                s = s.Remove(i - 1, 1);
            else
                s[i] = super;
        }
        // получения списка значений разделенных символом separator
        public string[] GetSeparatedValues(string str, char separator, char super)
        {
            StringBuilder s = new StringBuilder(str);

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == separator)
                {
                    HandleOfSymbol(s, i, super);
                }
                else if (s[i] == '\"')
                {
                    i = FindStringEnd(s, i, '\"');
                    if (i < 0) throw new Exception($"Не получилось найти окончание строки: [{s.ToString()}]");
                }
            }

            return s.ToString().Split(super, StringSplitOptions.RemoveEmptyEntries);
        }
        // получение списка аргументов передавемых в функции
        public string[] GetSeparatedArgs(string str)
        {
            const char separator = ',';
            const char super = '■';
            int i_start = str.IndexOf('(') + 1;
            if (i_start < 0) throw new Exception($"Не получилось найти окончание функции: [{str}]");
            StringBuilder s = new StringBuilder(str.Substring(i_start));
            int counter = 1;

            for (int i = 0; i < s.Length; i++)
            {
                switch (s[i])
                {
                    case '(':
                        counter++;
                        break;
                    case ')':
                        counter--;
                        if (counter < 1)
                        {
                            s = s.Remove(i, s.Length - i);
                            i = s.Length;
                        }
                        break;
                    case '\"':
                        i = FindStringEnd(s, i, '\"');
                        if (i < 0) throw new Exception($"Не получилось найти окончание строки: [{str}]");
                        break;
                    case separator:
                        if (counter == 1)
                        {
                            HandleOfSymbol(s, i, super);
                        }
                        break;
                }
            }

            if (counter > 0) throw new Exception($"Не получилось найти окончание функции: [{str}]");

            return s.ToString().Split(super, StringSplitOptions.RemoveEmptyEntries);
        }
        // получение данных из строки body
        private List<DataContainer> GetDCFromBody(string body)
        {
            List<DataContainer> sd = new List<DataContainer>();
            var lines = GetSeparatedValues(body, ';', '■');
            
            foreach (string v in lines)
            {
                List<string> data = new List<string>();

                var parts = v.Split(':', 2);
                if (parts.Length < 2) continue;
                var rightPart = GetSeparatedValues(parts[1], '|', '■');
                foreach (var s in rightPart)
                {
                    if (s.Length == 0) continue;
                    data.Add(s.Trim(' ', '\n', '\r'));
                }

                if (data.Count > 0)
                {
                    sd.Add(new DataContainer(parts[0].Trim(' ', '\r', '\n'), data));
                }
            }

            return sd;
        }
        // обработка блока ХРАНИЛИЩЕ_ОБЪЕКТОВ
        private void GetStorageData(string body)
        {
            _storageData = GetDCFromBody(body);
        }
        // обработка блока ШАБЛОННЫЙ_ВИД
        private void GetTemplateData(string body)
        {
            _templateStr = body.Trim(' ', '\r', '\n');
        }
        // обработка блока РЕШЕНИЕ
        private void GetCode (string body)
        {
            _code = body.Trim(' ', '\r', '\n');
        }
        // обработка блока СЛУЖЕБНОЕ
        private void GetServiceData(string body)
        {
            var sd = GetDCFromBody(body);

            foreach (var d in sd)
            {
                switch (d.Name.ToLower())
                {
                    case _arithmFuncs:
                        Elems.SetFuncs(d.Data);
                        break;
                    case _arithmSigns:
                        Elems.SetSigns(d.Data);
                        break;
                    default:
                        break;
                }
            }
        }
        // обработка блока ТЕСТОВЫЕ_ДАННЫЕ
        private void GetTestsD(string body)
        {
            _testData = GetDCFromBody(body);
        }

        public override string ToString()
        {
            string s = string.Empty;

            foreach (var d in _storageData)
            {
                Console.WriteLine(d);
            }

            return s;
        }

    }
}