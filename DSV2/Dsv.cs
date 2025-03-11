using _a = System.Text.StringBuilder;
using _b = System.Data.DataTable;
using _c = System.Data.DataColumn;
using _d = System.Data.DataRow;
using _e = System.String;
using _f = System.Collections.Generic.List<string>;
using _g = System.Linq.Enumerable;
using _h = System.Char;
using _i = System.Boolean;
using _j = System.Int64;
using _k = System.Int32;
using _l = System.Array;

namespace DSV2; 

public static class Dsv {
    public static _e[] ToStringArr(_b table) => A(table);
    private static _e[] A(_b a) {
        _f b = [C([.. a.Columns.OfType<_c>().Select(c => c.Caption)])];
        foreach (_d row in a.Rows) {
            b.Add(C([.. row.ItemArray.Cast<_e>()]));
        }
        return [.. b];
    }
    public static _b ToDataTable(_e[] dsv, _i hasHeaders) => B(dsv, hasHeaders);
    private static _b B(_e[] d, _i e) {
        _e[]? f = D(d[0]);
        using _b g = new();
        if (f is not null) {
            if (e)
                g.Columns.AddRange([.. f.Select(r => new _c(r))]);
            else {
                g.Columns.AddRange([.. _g.Range(0, f.Length).Select(h => new _c($"{DD.H(CC.ee)}{h}"))]);
                _ = g.Rows.Add(f);
            }
        }
        foreach (var i in d[1..]) {
            _e[]? j = D(i);
            if (j is not null)
                _ = g.Rows.Add(j);
        }
        return g;
    }
    public static _b ToDataTable(string dsv) => B([dsv], false);
    private static _e C(_e[] k) {
        _h? l = DD.I(CC.dd);
        if (l is null)
            return _e.Empty;
        char m = l.Value;
        if (k[0].All(c => c == m))
            return _e.Empty;
        var n = AA.F();
        var o = (_h)n;
        _a p = new();
        foreach (var q in k[..^1]) {
            if ((q.StartsWith(m) && q.EndsWith(m)) || !q.Contains(o)) 
                p.Append($"{q}{o}");
            else 
                p.Append($"{m}{q}{m}{o}");
        }
        var r = k[^1];
        if ((r.StartsWith(m) && r.EndsWith(m)) || !r.Contains(o))
            p.Append(r);
        else
            p.Append($"{m}{r}{m}");
        
        return _e.Join(p.ToString(), AA.G(n));
    }
    private static _e[]? D(_e s) {
        _h? t = DD.I(CC.dd);
        if (t is null)
            return null;
        if (s.Length < (_j)AA.BB.E8)
            return null;
        _h u = t.Value;
        _e v = s[^(_k)AA.BB.E8..];
        _e w = s[..(_k)AA.BB.E8];
        _h x = AA.cc[v];
        _e[] y = w.Split(x);
        _e[] z;
        _k aa = 0;
        _i bb = false;
        z = new _e[y.Length];
        foreach (_e cc in y) {
            if (cc == u.ToString()) {
                z[aa] += x;
                if (bb) {
                    bb = false;
                    aa++;
                } else {
                    bb = true;
                }
                continue;
            }
            if (cc.Count(c => c == u) % 2 == 1) {
                if (bb) {
                    if (cc[0] == u) {
                        z[aa] += cc[1..];
                    } else {
                        bb = false;
                        z[aa] += cc[0..^1];
                        aa++;
                    }
                    continue;
                } else {
                    z[aa] += cc[1..];
                    bb = true;
                    continue;
                }
            }
            z[aa] += cc;
            if (!bb) {
                aa++;
            }
        }
        if (bb)
            return null;
        _l.Resize(ref z, aa);
        return z;
    }


}