using System.Collections.Generic;
using UnityEngine;

public class Info
{
    public string name;
    public string description;
    public int type;
}

public class ListSort : MonoBehaviour
{
    public void Set(List<Info> list)
    {
        list.Sort((item1, item2) =>
        {
            if (item1 == null) return -1;
            if (item2 == null) return 1;

            return item1.name.CompareTo(item2.name);
        });
    }
}
