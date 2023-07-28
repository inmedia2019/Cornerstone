using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cornerstone.Code
{
    public class TextHelper
    {
        /// <summary>
        /// 获取默认值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetCustomValue(string value, string defaultValue)
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// 截取指定长度的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetSubString(string value, int length, bool ellipsis = false)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            if (value.Length > length)
            {
                value = value.Substring(0, length);
                if (ellipsis)
                {
                    value += "...";
                }
            }
            return value;
        }

        /// <summary>
        /// 字符串转指定类型数组
        /// </summary>
        /// <param name="value"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static T[] SplitToArray<T>(string value, char split)
        {
            T[] arr = value.Split(new string[] { split.ToString() }, StringSplitOptions.RemoveEmptyEntries).CastSuper<T>().ToArray();
            return arr;
        }
        /// <summary>
        /// 判断是否有交集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public static bool IsArrayIntersection<T>(List<T> list1, List<T> list2)
        {
            List<T> t = list1.Distinct().ToList();

            var exceptArr = t.Except(list2).ToList();

            if (exceptArr.Count < t.Count)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        /// <summary>

        /// 在指定的字符串列表CnStr中检索符合拼音索引字符串

        /// </summary>

        /// <param name="CnStr">汉字字符串</param>

        /// <returns>相对应的汉语拼音首字母串</returns>

        public static string GetSpellCode(string CnStr)
        {
            string strTemp = GetCharSpellCode(CnStr.Substring(0, 1));
            return strTemp;

        }
        /// <summary>
        /// 得到一个汉字的拼音第一个字母，如果是一个英文字母则直接返回大写字母
        /// </summary>
        /// <param name="CnChar">单个汉字</param>
        /// <returns>单个大写字母</returns>

        public static string GetCharSpellCode(string CnChar)
        {
            long GBCode;
            byte[] ZW = System.Text.Encoding.Default.GetBytes(CnChar);
            //如果是字母，则直接返回
            if (ZW.Length == 1)
            {
                return CnChar.ToUpper();
            }
            else
            {
                Encoding gb2312 = Encoding.GetEncoding("gb2312");
                byte[] unicodeBytes = Encoding.Unicode.GetBytes(CnChar);
                byte[] gb2312Bytes = Encoding.Convert(Encoding.Unicode, gb2312, unicodeBytes);
                GBCode = Convert.ToInt32(
             String.Format("{0:D2}", Convert.ToInt16(gb2312Bytes[0]) - 160)
             + String.Format("{0:D2}", Convert.ToInt16(gb2312Bytes[1]) - 160)
             );
            }
            if (GBCode >= 1601 && GBCode < 1637) return "A";
            if (GBCode >= 1637 && GBCode < 1833) return "B";
            if (GBCode >= 1833 && GBCode < 2078) return "C";
            if (GBCode >= 2078 && GBCode < 2274) return "D";
            if (GBCode >= 2274 && GBCode < 2302) return "E";
            if (GBCode >= 2302 && GBCode < 2433) return "F";
            if (GBCode >= 2433 && GBCode < 2594) return "G";
            if (GBCode >= 2594 && GBCode < 2787) return "H";
            if (GBCode >= 2787 && GBCode < 3106) return "J";
            if (GBCode >= 3106 && GBCode < 3212) return "K";
            if (GBCode >= 3212 && GBCode < 3472) return "L";
            if (GBCode >= 3472 && GBCode < 3635) return "M";
            if (GBCode >= 3635 && GBCode < 3722) return "N";
            if (GBCode >= 3722 && GBCode < 3730) return "O";
            if (GBCode >= 3730 && GBCode < 3858) return "P";
            if (GBCode >= 3858 && GBCode < 4027) return "Q";
            if (GBCode >= 4027 && GBCode < 4086) return "R";
            if (GBCode >= 4086 && GBCode < 4390) return "S";
            if (GBCode >= 4390 && GBCode < 4558) return "T";
            if (GBCode >= 4558 && GBCode < 4684) return "W";
            if (GBCode >= 4684 && GBCode < 4925) return "X";
            if (GBCode >= 4925 && GBCode < 5249) return "Y";
            if (GBCode >= 5249 && GBCode <= 5589) return "Z";
            if (GBCode >= 5601 && GBCode <= 8794)
            {
                String CodeData = "cjwgnspgcenegypbtwxzdxykygtpjnmjqmbsgzscyjsyyfpggbzgydywjkgaljswkbjqhyjwpdzlsgmr"
                 + "ybywwccgznkydgttngjeyekzydcjnmcylqlypyqbqrpzslwbdgkjfyxjwcltbncxjjjjcxdtqsqzycdxxhgckbphffss"
                 + "pybgmxjbbyglbhlssmzmpjhsojnghdzcdklgjhsgqzhxqgkezzwymcscjnyetxadzpmdssmzjjqjyzcjjfwqjbdzbjgd"
                 + "nzcbwhgxhqkmwfbpbqdtjjzkqhylcgxfptyjyyzpsjlfchmqshgmmxsxjpkdcmbbqbefsjwhwwgckpylqbgldlcctnma"
                 + "eddksjngkcsgxlhzaybdbtsdkdylhgymylcxpycjndqjwxqxfyyfjlejbzrwccqhqcsbzkymgplbmcrqcflnymyqmsqt"
                 + "rbcjthztqfrxchxmcjcjlxqgjmshzkbswxemdlckfsydsglycjjssjnqbjctyhbftdcyjdgwyghqfrxwckqkxebpdjpx"
                 + "jqsrmebwgjlbjslyysmdxlclqkxlhtjrjjmbjhxhwywcbhtrxxglhjhfbmgykldyxzpplggpmtcbbajjzyljtyanjgbj"
                 + "flqgdzyqcaxbkclecjsznslyzhlxlzcghbxzhznytdsbcjkdlzayffydlabbgqszkggldndnyskjshdlxxbcghxyggdj"
                 + "mmzngmmccgwzszxsjbznmlzdthcqydbdllscddnlkjyhjsycjlkohqasdhnhcsgaehdaashtcplcpqybsdmpjlpcjaql"
                 + "cdhjjasprchngjnlhlyyqyhwzpnccgwwmzffjqqqqxxaclbhkdjxdgmmydjxzllsygxgkjrywzwyclzmcsjzldbndcfc"
                 + "xyhlschycjqppqagmnyxpfrkssbjlyxyjjglnscmhcwwmnzjjlhmhchsyppttxrycsxbyhcsmxjsxnbwgpxxtaybgajc"
                 + "xlypdccwqocwkccsbnhcpdyznbcyytyckskybsqkkytqqxfcwchcwkelcqbsqyjqcclmthsywhmktlkjlychwheqjhtj"
                 + "hppqpqscfymmcmgbmhglgsllysdllljpchmjhwljcyhzjxhdxjlhxrswlwzjcbxmhzqxsdzpmgfcsglsdymjshxpjxom"
                 + "yqknmyblrthbcftpmgyxlchlhlzylxgsssscclsldclepbhshxyyfhbmgdfycnjqwlqhjjcywjztejjdhfblqxtqkwhd"
                 + "chqxagtlxljxmsljhdzkzjecxjcjnmbbjcsfywkbjzghysdcpqyrsljpclpwxsdwejbjcbcnaytmgmbapclyqbclzxcb"
                 + "nmsggfnzjjbzsfqyndxhpcqkzczwalsbccjxpozgwkybsgxfcfcdkhjbstlqfsgdslqwzkxtmhsbgzhjcrglyjbpmljs"
                 + "xlcjqqhzmjczydjwbmjklddpmjegxyhylxhlqyqhkycwcjmyhxnatjhyccxzpcqlbzwwwtwbqcmlbmynjcccxbbsnzzl"
                 + "jpljxyztzlgcldcklyrzzgqtgjhhgjljaxfgfjzslcfdqzlclgjdjcsnclljpjqdcclcjxmyzftsxgcgsbrzxjqqcczh"
                 + "gyjdjqqlzxjyldlbcyamcstylbdjbyregklzdzhldszchznwczcllwjqjjjkdgjcolbbzppglghtgzcygezmycnqcycy"
                 + "hbhgxkamtxyxnbskyzzgjzlqjdfcjxdygjqjjpmgwgjjjpkjsbgbmmcjssclpqpdxcdyykypcjddyygywchjrtgcnyql"
                 + "dkljczzgzccjgdyksgpzmdlcphnjafyzdjcnmwescsglbtzcgmsdllyxqsxsbljsbbsgghfjlwpmzjnlyywdqshzxtyy"
                 + "whmcyhywdbxbtlmswyyfsbjcbdxxlhjhfpsxzqhfzmqcztqcxzxrdkdjhnnyzqqfnqdmmgnydxmjgdhcdycbffallztd"
                 + "ltfkmxqzdngeqdbdczjdxbzgsqqddjcmbkxffxmkdmcsychzcmljdjynhprsjmkmpcklgdbqtfzswtfgglyplljzhgjj"
                 + "gypzltcsmcnbtjbhfkdhbyzgkpbbymtdlsxsbnpdkleycjnycdykzddhqgsdzsctarlltkzlgecllkjljjaqnbdggghf"
                 + "jtzqjsecshalqfmmgjnlyjbbtmlycxdcjpldlpcqdhsycbzsckbzmsljflhrbjsnbrgjhxpdgdjybzgdlgcsezgxlblg"
                 + "yxtwmabchecmwyjyzlljjshlgndjlslygkdzpzxjyyzlpcxszfgwyydlyhcljscmbjhblyjlycblydpdqysxktbytdkd"
                 + "xjypcnrjmfdjgklccjbctbjddbblblcdqrppxjcglzcshltoljnmdddlngkaqakgjgyhheznmshrphqqjchgmfprxcjg"
                 + "dychghlyrzqlcngjnzsqdkqjymszswlcfqjqxgbggxmdjwlmcrnfkkfsyyljbmqammmycctbshcptxxzzsmphfshmclm"
                 + "ldjfyqxsdyjdjjzzhqpdszglssjbckbxyqzjsgpsxjzqznqtbdkwxjkhhgflbcsmdldgdzdblzkycqnncsybzbfglzzx"
                 + "swmsccmqnjqsbdqsjtxxmbldxcclzshzcxrqjgjylxzfjphymzqqydfqjjlcznzjcdgzygcdxmzysctlkphtxhtlbjxj"
                 + "lxscdqccbbqjfqzfsltjbtkqbsxjjljchczdbzjdczjccprnlqcgpfczlclcxzdmxmphgsgzgszzqjxlwtjpfsyaslcj"
                 + "btckwcwmytcsjjljcqlwzmalbxyfbpnlschtgjwejjxxglljstgshjqlzfkcgnndszfdeqfhbsaqdgylbxmmygszldyd"
                 + "jmjjrgbjgkgdhgkblgkbdmbylxwcxyttybkmrjjzxqjbhlmhmjjzmqasldcyxyqdlqcafywyxqhz";
                String _gbcode = GBCode.ToString();
                int pos = (Convert.ToInt16(_gbcode.Substring(0, 2)) - 56) * 94 + Convert.ToInt16(_gbcode.Substring(_gbcode.Length - 2, 2));
                return CodeData.Substring(pos - 1, 1);
            };
            return (" ");
        }

    }
}
