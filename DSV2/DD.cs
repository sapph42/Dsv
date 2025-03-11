using _n = System.Text.StringBuilder;

namespace DSV2;
internal class DD {
    internal static string H(string[] vals) {
        _n sb = new();
        foreach (var val in vals) {
            if (AA.cc.TryGetValue(val, out char thisChar))
                sb.Append(thisChar);
        }
        return sb.ToString();
    }
    internal static char? I(string val) {
        if (AA.cc.TryGetValue(val, out char thisChar))
            return thisChar;
        return null;
    }
}
