using System.Data;
using System.Text.RegularExpressions;

namespace DoctorGu
{
    public class CLang
    {
        /// <summary>
        /// VB의 Choose 함수와 같은 기능을 구현함. 0부터 시작됨.
        /// </summary>
        /// <param name="Index">0부터 시작하는 Index</param>
        /// <param name="Choice">경합을 벌일 모든 값</param>
        /// <returns><paramref name="Index"/>번째에 위치한 <paramref name="Choice"/>의 값</returns>
        /// <example>
        /// 다음은 목록 중 2번째의 값을 선택합니다.
        /// <code>
        /// string[] aAll = new string[] { "가", "나", "다" };
        /// Console.WriteLine(CMath.Choose(1, aAll)); // "나"
        /// 
        /// Console.WriteLine(CMath.Choose(1, "가", "나", "다")); // "나"
        /// </code>
        /// </example>
        public static string Choose(int Index, params string[] Choice)
        {
            return Choice[Index];
        }
        public static Enum Choose(int Index, params Enum[] Choice)
        {
            return Choice[Index];
        }
        public static T Choose<T>(int Index, params T[] Choice)
        {
            return Choice[Index];
        }
        public static int Choose(int Index, params int[] Choice)
        {
            return Choice[Index];
        }
        public static object Choose(int Index, params object[] Choice)
        {
            return Choice[Index];
        }

        /// <summary>
        /// Same function with in operator in SQL statement.
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="AnyValue"></param>
        /// <example>
        /// bool b = CLang.In("a", "a", "b", "c"); //true
        /// bool b = CLang.In("x", "a", "b", "c"); //false
        /// </example>
        /// <returns></returns>
        public static bool In(object Value, params object[] AnyValue)
        {
            for (int i = 0; i < AnyValue.Length; i++)
            {
                // == operator compares by reference.
                // So Enum type will result false even if same value was compared.
                // So Equal used to compare by value.
                if (AnyValue[i] == null)
                    return (Value == null);
                else if (AnyValue[i].Equals(Value))
                    return true;
            }

            return false;
        }
        public static bool InIgnoreCase(string Value, params string[] AnyValue)
        {
            for (int i = 0; i < AnyValue.Length; i++)
            {
                if (AnyValue[i] == null)
                    return (Value == null);
                else if (string.Compare(Value, AnyValue[i], true) == 0)
                    return true;
            }

            return false;
        }
        public static bool In<T>(T Value, params T[] AnyValue)
        {
            for (int i = 0; i < AnyValue.Length; i++)
            {
                // == operator compares by reference.
                // So Enum type will result false even if same value was compared.
                // So Equal used to compare by value.
                if (AnyValue[i] == null)
                    return (Value == null);
                else if (AnyValue[i].Equals(Value))
                    return true;
            }

            return false;
        }
        /// <summary>
        /// Compare with XAnd.
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="AnyValue"></param>
        /// <returns></returns>
        public static bool InX(Enum Value, params Enum[] AnyValue)
        {
            for (int i = 0; i < AnyValue.Length; i++)
            {
                if ((Convert.ToInt32(Value) & Convert.ToInt32(AnyValue[i])) == Convert.ToInt32(AnyValue[i]))
                    return true;
            }

            return false;
        }
        public static bool InX<T>(T Value, params T[] AnyValue)
        {
            for (int i = 0; i < AnyValue.Length; i++)
            {
                if ((Convert.ToInt32(Value) & Convert.ToInt32(AnyValue[i])) == Convert.ToInt32(AnyValue[i]))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// In은 하나라도 같은 값이 있으면 true를 리턴하지만 Out은 하나라도 틀린 값이 있으면 true를 리턴함.
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="AnyValue"></param>
        /// <example>
        /// bool b = CFindRep.Out("a", "x", "a", "x"); //true
        /// bool b = CFindRep.Out("x", "x", "x", "x"); //false
        /// </example>
        /// <returns></returns>
        public static bool Out(object Value, params object[] AnyValue)
        {
            for (int i = 0; i < AnyValue.Length; i++)
            {
                //== operator를 사용하면 참조 비교하므로
                //Enum 형식과 같이 같은 값이라도 다른 개체를 참조하면
                //같지 않다고 나옴.
                if (AnyValue[i] == null)
                    return (Value != null);
                else if (!AnyValue[i].Equals(Value))
                    return true;
            }

            return false;
        }

        public static bool Between(TimeSpan Value, TimeSpan From, TimeSpan To)
        {
            return (Value >= From) && (Value <= To);
        }
        public static bool Between(int Value, int From, int To)
        {
            return (Value >= From) && (Value <= To);
        }
        public static bool Between(long Value, long From, long To)
        {
            return (Value >= From) && (Value <= To);
        }
        public static bool Between(double Value, double From, double To)
        {
            return (Value >= From) && (Value <= To);
        }
        public static bool Between(DateTime Value, DateTime From, DateTime To)
        {
            return (Value >= From) && (Value <= To);
        }
        /// <summary>
        /// 아스키코드 순이 아닌 문자열 정렬순을 기준으로 함.
        /// 예를 들어 아스키코드 순이라면 A, C, b이지만 정렬순이라면 A, b, C임.
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// Console.WriteLine(CLang.Between("b", "a", "b")); //true
        /// Console.WriteLine(CLang.Between("c", "a", "b")); //false
        /// Console.WriteLine(CLang.Between("b", "A", "C")); //true
        /// Console.WriteLine(CLang.Between("b", "A", "C", true)); //true
        /// Console.WriteLine(CLang.Between("a", "A", "C")); //false
        /// Console.WriteLine(CLang.Between("A", "A", "C")); //true
        /// Console.WriteLine(CLang.Between("a", "A", "C", true)); //true
        /// ]]>
        /// </example>
        public static bool Between(string Value, string From, string To, bool IgnoreCase)
        {
            return ((string.Compare(Value, From, IgnoreCase) >= 0) && (string.Compare(Value, To, IgnoreCase) <= 0));
        }
        public static bool Between(string Value, string From, string To)
        {
            return ((string.Compare(Value, From) >= 0) && (string.Compare(Value, To) <= 0));
        }

        /// <summary>
        /// Act like VB Like function
        /// </summary>
        /// <param name="ValueSrc"></param>
        /// <param name="ValueDest"></param>
        /// <returns></returns>
        public static bool Like(string ValueSrc, string ValueDest, bool IgnoreCase)
        {
            ValueDest =
                "^" + Regex
                .Escape(ValueDest)
                .Replace(@"\*", ".*")
                .Replace(@"\?", ".") + "$";
            Regex r = new Regex(ValueDest, (IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None));
            return r.IsMatch(ValueSrc);
        }

        //http://blog.logiclabz.com/c/evaluate-function-in-c-net-as-eval-function-in-javascript.aspx
        public static double EvaluateDoubleUsingXPath(string Expression)
        {
            return Convert.ToDouble(EvaluateUsingXPath(Expression));
        }
        /// <example>
        /// <![CDATA[
        /// string Expression = "? > 3 and ? < 5";
        /// string Value = "4";
        /// bool b = CLang.EvaluateBooleanUsingXPath(Expression.Replace("?", Value));
        /// ]]>
        /// </example>
        public static bool EvaluateBooleanUsingXPath(string Expression)
        {
            return Convert.ToBoolean(EvaluateUsingXPath(Expression));
        }
        public static string EvaluateStringUsingXPath(string Expression)
        {
            return EvaluateUsingXPath(Expression).ToString();
        }
        private static object EvaluateUsingXPath(string Expression)
        {
            string XPath = new System.Text.RegularExpressions.Regex(@"([\+\-\*])")
                .Replace(Expression, " ${1} ")
                .Replace("/", " div ")
                .Replace("%", " mod ");
            return new System.Xml.XPath.XPathDocument(new System.IO.StringReader("<r/>")).CreateNavigator().Evaluate(XPath);
        }

        public static bool EvaludateBooleanUsingDataTable(string Expression)
        {
            var dt = new DataTable();
            var col = new DataColumn("Eval", typeof(bool), Expression);
            dt.Columns.Add(col);
            dt.Rows.Add(false);
            return (bool)dt.Rows[0]["Eval"];
        }
    }
}
