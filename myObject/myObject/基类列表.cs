// Kingdee.BOS.Orm.DataEntity.DynamicObjectCollection
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Orm.Exceptions;
using Kingdee.BOS.Orm.Metadata.DataEntity;
using Kingdee.BOS.Resource;
using SmartAssembly.Delegates;
using SmartAssembly.HouseOfCards;

[Serializable]
public class DynamicObjectCollection : DataEntityCollection<DynamicObject>, IListSource
{
    //private sealed class DynamicObjectBindingList : BindingList<DynamicObject>, ITypedList
    //{
    //    private DynamicObjectCollection _col;

    //    public DynamicObjectBindingList(DynamicObjectCollection col)
    //        : base((IList<DynamicObject>)col)
    //    {
    //        _col = col;
    //        _col.ListChanged += _col_ListChanged;
    //        base.AllowNew = true;
    //    }

    //    private void _col_ListChanged(object sender, ListChangedEventArgs e)
    //    {
    //        //IL_0008: Incompatible stack heights: 0 vs 1
    //        //IL_000b: Incompatible stack heights: 0 vs 1
    //        ((BindingList<DynamicObject>)/*Error near IL_0012: Stack underflow*/).OnListChanged((ListChangedEventArgs)/*Error near IL_0012: Stack underflow*/);
    //    }

    //    protected override void OnAddingNew(AddingNewEventArgs e)
    //    {
    //        //IL_002f: Incompatible stack heights: 0 vs 1
    //        //IL_0032: Incompatible stack heights: 0 vs 1
    //        //IL_003c: Incompatible stack heights: 0 vs 1
    //        //IL_0043: Incompatible stack heights: 0 vs 1
    //        //IL_0046: Incompatible stack heights: 0 vs 1
    //        //IL_0049: Incompatible stack heights: 0 vs 1
    //        //IL_0050: Incompatible stack heights: 0 vs 1
    //        while (8u != 0 && 5u != 0)
    //        {
    //            if (3u != 0)
    //            {
    //                ((BindingList<DynamicObject>)/*Error near IL_0039: Stack underflow*/).OnAddingNew((AddingNewEventArgs)/*Error near IL_0039: Stack underflow*/);
    //            }
    //            while (0 == 0)
    //            {
    //                object newObject = ((AddingNewEventArgs)/*Error near IL_0043: Stack underflow*/).NewObject;
    //                if ((int)/*Error near IL_0018: Stack underflow*/ == 0)
    //                {
    //                    if (2u != 0)
    //                    {
    //                        ((DynamicObjectBindingList)/*Error near IL_0024: Stack underflow*/)._col._collectionItemPropertyType.CreateInstance();
    //                        ((AddingNewEventArgs)/*Error near IL_0057: Stack underflow*/).NewObject = (object)/*Error near IL_0057: Stack underflow*/;
    //                        return;
    //                    }
    //                    continue;
    //                }
    //                return;
    //            }
    //        }
    //    }

    //    public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
    //    {
    //        //IL_000f: Incompatible stack heights: 0 vs 1
    //        //IL_0016: Incompatible stack heights: 0 vs 1
    //        //IL_001d: Incompatible stack heights: 0 vs 1
    //        //IL_0024: Incompatible stack heights: 0 vs 1
    //        DynamicObjectType dynamicCollectionItemPropertyType = ((DynamicObjectBindingList)/*Error near IL_0007: Stack underflow*/)._col.DynamicCollectionItemPropertyType;
    //        ICustomTypeDescriptor customTypeDescriptor = ((DynamicObjectType)/*Error near IL_001d: Stack underflow*/).CustomTypeDescriptor;
    //        ((ICustomTypeDescriptor)/*Error near IL_0024: Stack underflow*/).GetProperties();
    //        return (PropertyDescriptorCollection)/*Error near IL_000e: Stack underflow*/;
    //    }

    //    public string GetListName(PropertyDescriptor[] listAccessors)
    //    {
    //        //IL_000d: Incompatible stack heights: 0 vs 1
    //        //IL_0014: Incompatible stack heights: 0 vs 1
    //        //IL_001b: Incompatible stack heights: 0 vs 1
    //        DynamicObjectType dynamicCollectionItemPropertyType = ((DynamicObjectBindingList)/*Error near IL_0007: Stack underflow*/)._col.DynamicCollectionItemPropertyType;
    //        string name = ((DynamicMetadata)/*Error near IL_001b: Stack underflow*/).Name;
    //        return (string)/*Error near IL_000c: Stack underflow*/;
    //    }
    //}

    //private readonly DynamicObjectType _collectionItemPropertyType;

    //private IList<DynamicObject> _deleteRows;

    //[NonSerialized]
    //private BindingList<DynamicObject> _bingingList;

    [NonSerialized]
    internal static GetString \u001d;

	public DynamicObjectType DynamicCollectionItemPropertyType
    {
        get
        {
            //IL_0009: Incompatible stack heights: 0 vs 1
            return ((DynamicObjectCollection)/*Error near IL_0007: Stack underflow*/)._collectionItemPropertyType;
        }
    }

    public IList<DynamicObject> DeleteRows
    {
        get
        {
            //IL_001b: Incompatible stack heights: 0 vs 1
            //IL_001e: Incompatible stack heights: 0 vs 1
            //IL_0025: Incompatible stack heights: 0 vs 1
            //IL_0028: Incompatible stack heights: 0 vs 1
            if (((DynamicObjectCollection)/*Error near IL_0007: Stack underflow*/)._deleteRows == null)
            {
                new List<DynamicObject>();
                ((DynamicObjectCollection)/*Error near IL_0012: Stack underflow*/)._deleteRows = (IList<DynamicObject>)/*Error near IL_0012: Stack underflow*/;
            }
            return ((DynamicObjectCollection)/*Error near IL_0019: Stack underflow*/)._deleteRows;
        }
        //private set
        //{
        //    //IL_000b: Incompatible stack heights: 0 vs 1
        //    //IL_000e: Incompatible stack heights: 0 vs 1
        //    ((DynamicObjectCollection)/*Error near IL_0009: Stack underflow*/)._deleteRows = (IList<DynamicObject>)/*Error near IL_0009: Stack underflow*/;
        //}
    }

    public IList<DynamicObject> InsertRows
    {
        get
        {
            //IL_004a: Incompatible stack heights: 0 vs 1
            //IL_0051: Incompatible stack heights: 0 vs 1
            //IL_0057: Incompatible stack heights: 0 vs 1
            //IL_006d: Incompatible stack heights: 0 vs 1
            //IL_0079: Incompatible stack heights: 0 vs 1
            //IL_0083: Incompatible stack heights: 0 vs 1
            IList<DynamicObject> list;
            if (0 == 0)
            {
                new List<DynamicObject>();
                list = (IList<DynamicObject>)/*Error near IL_0073: Stack underflow*/;
                ((Collection<DynamicObject>)/*Error near IL_0083: Stack underflow*/).GetEnumerator();
                IEnumerator<DynamicObject> enumerator = (IEnumerator<DynamicObject>)/*Error near IL_0089: Stack underflow*/;
                try
                {
                    if (8 == 0)
                    {
                        goto IL_0038;
                    }
                    if (false)
                    {
                    }
                    goto IL_003f;
                IL_003f:
                    bool num;
                    DynamicObject item = default(DynamicObject);
                    do
                    {
                        num = enumerator.MoveNext();
                        do
                        {
                            if (num)
                            {
                                DynamicObject current = ((IEnumerator<DynamicObject>)/*Error near IL_0051: Stack underflow*/).Current;
                                item = (DynamicObject)/*Error near IL_0054: Stack underflow*/;
                                num = ((DataEntityBase)/*Error near IL_002e: Stack underflow*/).DataEntityState.FromDatabase;
                                continue;
                            }
                            return list;
                        }
                        while (false);
                    }
                    while (num);
                    goto IL_0038;
                IL_0038:
                    list.Add(item);
                    goto IL_003f;
                }
                finally
                {
                    if (enumerator != null && 2u != 0)
                    {
                        enumerator.Dispose();
                    }
                }
            }
            return list;
        }
    }

    public IList<DynamicObject> UpdateRows
    {
        get
        {
            //IL_0057: Incompatible stack heights: 0 vs 1
            //IL_005e: Incompatible stack heights: 0 vs 1
            //IL_0064: Incompatible stack heights: 0 vs 1
            //IL_007a: Incompatible stack heights: 0 vs 1
            //IL_0086: Incompatible stack heights: 0 vs 1
            //IL_0090: Incompatible stack heights: 0 vs 1
            new List<DynamicObject>();
            IList<DynamicObject> list = (IList<DynamicObject>)/*Error near IL_0080: Stack underflow*/;
            ((Collection<DynamicObject>)/*Error near IL_0090: Stack underflow*/).GetEnumerator();
            IEnumerator<DynamicObject> enumerator = (IEnumerator<DynamicObject>)/*Error near IL_0096: Stack underflow*/;
            try
            {
                while (6u != 0)
                {
                    bool num = enumerator.MoveNext();
                    while (true)
                    {
                        if (num)
                        {
                            DynamicObject dynamicObject;
                            if (8u != 0)
                            {
                                DynamicObject current = ((IEnumerator<DynamicObject>)/*Error near IL_005e: Stack underflow*/).Current;
                                dynamicObject = (DynamicObject)/*Error near IL_0061: Stack underflow*/;
                                if (false)
                                {
                                    return list;
                                }
                                num = ((DataEntityBase)/*Error near IL_002b: Stack underflow*/).DataEntityState.FromDatabase;
                                if (false)
                                {
                                    continue;
                                }
                                if (!num || !dynamicObject.DataEntityState.DataEntityDirty)
                                {
                                    break;
                                }
                            }
                            list.Add(dynamicObject);
                            break;
                        }
                        return list;
                    }
                }
                return list;
            }
            finally
            {
                if (enumerator != null)
                {
                    goto IL_0069;
                }
                goto IL_006f;
            IL_006f:
                if (false)
                {
                    goto IL_0069;
                }
                goto end_IL_0066;
            IL_0069:
                enumerator.Dispose();
                goto IL_006f;
            end_IL_0066:;
            }
        }
    }

    bool IListSource.ContainsListCollection
    {
        get
        {
            return false;
        }
    }

    public DynamicObjectCollection(DynamicObjectType dynamicItemPropertyType, object parent = null)
        : base(parent)
    {
        if (dynamicItemPropertyType == null)
        {
            throw new ORMArgInvalidException(\u001d(107396350), ResManager.LoadKDString(\u001d(107394715), \u001d(107395073), SubSystemType.SL));
        }
        _collectionItemPropertyType = dynamicItemPropertyType;
        DeleteRows = new List<DynamicObject>();
    }

    public DynamicObjectCollection(DynamicObjectType dynamicItemPropertyType, object parent, IList<DynamicObject> list)
        : base(parent, list)
    {
        if (dynamicItemPropertyType == null)
        {
            throw new ORMArgInvalidException(\u001d(107396350), ResManager.LoadKDString(\u001d(107394715), \u001d(107395073), SubSystemType.SL));
        }
        _collectionItemPropertyType = dynamicItemPropertyType;
        DeleteRows = new List<DynamicObject>();
    }

    protected override void RemoveItem(int index)
    {
        //IL_002f: Incompatible stack heights: 0 vs 1
        //IL_0032: Incompatible stack heights: 0 vs 1
        //IL_0039: Incompatible stack heights: 0 vs 1
        //IL_003f: Incompatible stack heights: 0 vs 1
        //IL_0046: Incompatible stack heights: 0 vs 1
        //IL_004d: Incompatible stack heights: 0 vs 1
        //IL_0050: Incompatible stack heights: 0 vs 1
        //IL_0057: Incompatible stack heights: 0 vs 1
        while (true)
        {
            DynamicObject item;
            if (uint.MaxValue != 0)
            {
                DynamicObject dynamicObject = ((Collection<DynamicObject>)/*Error near IL_0039: Stack underflow*/)[(int)/*Error near IL_0039: Stack underflow*/];
                item = (DynamicObject)/*Error near IL_003c: Stack underflow*/;
                DataEntityState dataEntityState = ((DataEntityBase)/*Error near IL_0046: Stack underflow*/).DataEntityState;
                bool fromDatabase = ((DataEntityState)/*Error near IL_004d: Stack underflow*/).FromDatabase;
                if ((int)/*Error near IL_0013: Stack underflow*/ != 0)
                {
                    goto IL_0013;
                }
            }
            goto IL_0023;
        IL_0013:
            if (3u != 0)
            {
                if (false)
                {
                    continue;
                }
                IList<DynamicObject> deleteRow = ((DynamicObjectCollection)/*Error near IL_0057: Stack underflow*/).DeleteRows;
                ((ICollection<DynamicObject>)/*Error near IL_0023: Stack underflow*/).Add(item);
                goto IL_0023;
            }
            goto IL_002a;
        IL_0023:
            base.RemoveItem(index);
            goto IL_002a;
        IL_002a:
            if (true)
            {
                break;
            }
            goto IL_0013;
        }
    }

    protected override void ClearItems()
    {
        //IL_003c: Incompatible stack heights: 0 vs 1
        //IL_0043: Incompatible stack heights: 0 vs 1
        //IL_0049: Incompatible stack heights: 0 vs 1
        //IL_0050: Incompatible stack heights: 0 vs 1
        //IL_0057: Incompatible stack heights: 0 vs 1
        //IL_0071: Incompatible stack heights: 0 vs 1
        //IL_007b: Incompatible stack heights: 0 vs 1
        ((Collection<DynamicObject>)/*Error near IL_007b: Stack underflow*/).GetEnumerator();
        IEnumerator<DynamicObject> enumerator = (IEnumerator<DynamicObject>)/*Error near IL_0081: Stack underflow*/;
        try
        {
            while (enumerator.MoveNext())
            {
                if (2 == 0)
                {
                    continue;
                }
                DynamicObject item;
                if (0 == 0)
                {
                    DynamicObject current = ((IEnumerator<DynamicObject>)/*Error near IL_0043: Stack underflow*/).Current;
                    item = (DynamicObject)/*Error near IL_0046: Stack underflow*/;
                    DataEntityState dataEntityState = ((DataEntityBase)/*Error near IL_0050: Stack underflow*/).DataEntityState;
                    bool fromDatabase = ((DataEntityState)/*Error near IL_0057: Stack underflow*/).FromDatabase;
                    if ((int)/*Error near IL_0022: Stack underflow*/ == 0)
                    {
                        continue;
                    }
                }
                if (0 == 0)
                {
                    DeleteRows.Add(item);
                }
            }
        }
        finally
        {
            if (0 == 0 && enumerator != null)
            {
                goto IL_005f;
            }
            goto IL_0065;
        IL_005f:
            enumerator.Dispose();
            goto IL_0065;
        IL_0065:
            if (false)
            {
                goto IL_005f;
            }
        }
        base.ClearItems();
    }

    IList IListSource.GetList()
    {
        //IL_001d: Incompatible stack heights: 0 vs 1
        //IL_0020: Incompatible stack heights: 0 vs 1
        //IL_0023: Incompatible stack heights: 0 vs 1
        //IL_002a: Incompatible stack heights: 0 vs 1
        //IL_002d: Incompatible stack heights: 0 vs 1
        if (((DynamicObjectCollection)/*Error near IL_0007: Stack underflow*/)._bingingList == null)
        {
            new DynamicObjectBindingList((DynamicObjectCollection)/*Error near IL_002a: Stack underflow*/);
            ((DynamicObjectCollection)/*Error near IL_0014: Stack underflow*/)._bingingList = (BindingList<DynamicObject>)/*Error near IL_0014: Stack underflow*/;
        }
        return ((DynamicObjectCollection)/*Error near IL_001b: Stack underflow*/)._bingingList;
    }

    static DynamicObjectCollection()
    {
        //IL_000f: Incompatible stack heights: 0 vs 1
        Type typeFromHandle = typeof(DynamicObjectCollection);
        Strings.CreateGetStringDelegate((Type)/*Error near IL_0016: Stack underflow*/);
    }
}
