
using System.Collections.Generic; using System;
using System.Text;

namespace BrainDuelsLib.delegates
{
    public delegate void Action();
	public delegate void Action<T>(T obj);
    public delegate void Action<T, U>(T obj1, U obj2);
    public delegate void Action<T, U, V>(T obj1, U obj2, V obj3);
    public delegate void Action<T, U, V, Z>(T obj1, U obj2, V obj3, Z obj4);
}
