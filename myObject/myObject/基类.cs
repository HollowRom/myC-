// Kingdee.BOS.Orm.DataEntity.DynamicObject
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq.Expressions;
using Kingdee.BOS.Collections.Generic;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Orm.Exceptions;
using Kingdee.BOS.Orm.Metadata.DataEntity;
using Kingdee.BOS.Resource;
using SmartAssembly.Delegates;
using SmartAssembly.HouseOfCards;

[Serializable]
[DebuggerDisplay("{DynamicObjectType.Name}")]
[DebuggerTypeProxy(typeof(DynamicObjectDebugView))]
public class DynamicObject : DataEntityBase, IDynamicMetaObjectProvider, ICustomTypeDescriptor
{
    [Serializable]
    internal struct ArrayStorage : IDataStorage
    {
        private object[] _values;

        private ArrayStorage(object[] values)
        {
            _values = values;
        }

        public ArrayStorage(DynamicObjectType dt)
        {
            _values = new object[dt.Properties.Count];
        }

        public unsafe object GetLocalValue(DynamicProperty property)
        {
            //IL_001a: Invalid comparison between Unknown and I4
            //IL_0030: Incompatible stack heights: 0 vs 1
            //IL_0037: Incompatible stack heights: 0 vs 1
            //IL_003d: Incompatible stack heights: 0 vs 1
            //IL_0040: Incompatible stack heights: 0 vs 1
            //IL_0043: Incompatible stack heights: 0 vs 1
            //IL_0046: Incompatible stack heights: 0 vs 1
            //IL_0050: Incompatible stack heights: 0 vs 1
            if (false)
            {
                goto IL_001c;
            }
            int num;
            if (3u != 0)
            {
                int ordinal = ((DynamicProperty)/*Error near IL_0037: Stack underflow*/).Ordinal;
                num = (int)/*Error near IL_003a: Stack underflow*/;
                if (0 == 0)
                {
                    IntPtr intPtr = (IntPtr)(void*)((ArrayStorage)/*Error near IL_0018: Stack underflow*/)._values.LongLength;
                    if ((int)/*Error near IL_001c: Stack underflow*/ >= (int)(long)intPtr)
                    {
                        goto IL_001c;
                    }
                }
            }
            goto IL_0027;
        IL_0027:
            return ((ArrayStorage)/*Error near IL_002c: Stack underflow*/)._values[num];
        IL_001c:
            if (2u != 0)
            {
                ((ArrayStorage*)/*Error near IL_004d: Stack underflow*/)->EnsureCapacity((DynamicProperty)/*Error near IL_004d: Stack underflow*/);
            }
            goto IL_0027;
        }

        private void EnsureCapacity(DynamicProperty property)
        {
            //IL_0026: Incompatible stack heights: 0 vs 1
            //IL_002d: Incompatible stack heights: 0 vs 1
            //IL_0034: Incompatible stack heights: 0 vs 1
            //IL_0037: Incompatible stack heights: 0 vs 1
            //IL_003e: Incompatible stack heights: 0 vs 1
            //IL_0045: Incompatible stack heights: 0 vs 1
            //IL_004c: Incompatible stack heights: 0 vs 1
            //IL_004f: Incompatible stack heights: 0 vs 1
            //IL_0056: Incompatible stack heights: 0 vs 1
            DynamicObjectType reflectedType = ((DynamicProperty)/*Error near IL_002d: Stack underflow*/).ReflectedType;
            bool flag = (DynamicObjectType)/*Error near IL_0034: Stack underflow*/ == null;
            if ((int)/*Error near IL_0009: Stack underflow*/ == 0)
            {
                DynamicObjectType reflectedType2 = ((DynamicProperty)/*Error near IL_003e: Stack underflow*/).ReflectedType;
                DynamicPropertyCollection property2 = ((DynamicObjectType)/*Error near IL_0045: Stack underflow*/).Properties;
                int count = ((ReadOnlyCollection<DynamicProperty>)/*Error near IL_004c: Stack underflow*/).Count;
            }
            else
            {
                int ordinal = ((DynamicProperty)/*Error near IL_0056: Stack underflow*/).Ordinal;
            }
            int newSize = (int)/*Error near IL_0018: Stack underflow*/;
            Array.Resize(ref _values, newSize);
        }

        public unsafe void SetLocalValue(DynamicProperty property, object value)
        {
            //IL_0017: Invalid comparison between Unknown and I4
            //IL_0031: Incompatible stack heights: 0 vs 1
            //IL_0038: Incompatible stack heights: 0 vs 1
            //IL_003e: Incompatible stack heights: 0 vs 1
            //IL_0041: Incompatible stack heights: 0 vs 1
            //IL_0044: Incompatible stack heights: 0 vs 1
            //IL_0047: Incompatible stack heights: 0 vs 1
            //IL_0051: Incompatible stack heights: 0 vs 1
            int ordinal = ((DynamicProperty)/*Error near IL_0038: Stack underflow*/).Ordinal;
            while (true)
            {
                int num;
                if (0 == 0)
                {
                    num = (int)/*Error near IL_003b: Stack underflow*/;
                }
                while (0 == 0)
                {
                    IntPtr intPtr = (IntPtr)(void*)((ArrayStorage)/*Error near IL_0015: Stack underflow*/)._values.LongLength;
                    if ((int)/*Error near IL_0019: Stack underflow*/ >= (int)(long)intPtr && 0 == 0)
                    {
                        ((ArrayStorage*)/*Error near IL_004e: Stack underflow*/)->EnsureCapacity((DynamicProperty)/*Error near IL_004e: Stack underflow*/);
                    }
                    ((ArrayStorage)/*Error near IL_0029: Stack underflow*/)._values[num] = value;
                    if (false)
                    {
                        continue;
                    }
                    return;
                }
            }
        }

        public IDataStorage MemberClone()
        {
            //IL_005a: Incompatible stack heights: 0 vs 1
            //IL_0063: Incompatible stack heights: 0 vs 1
            //IL_0066: Incompatible stack heights: 0 vs 2
            //IL_0073: Incompatible stack heights: 0 vs 1
            //IL_0076: Incompatible stack heights: 0 vs 1
            //IL_0079: Incompatible stack heights: 0 vs 1
            object[] array2 = default(object[]);
            if (3u != 0)
            {
                object[] array = new object[((ArrayStorage)/*Error near IL_000a: Stack underflow*/)._values.Length];
                if (0 == 0)
                {
                    array2 = array;
                }
            }
            if (2u != 0)
            {
                object[] value = ((ArrayStorage)/*Error near IL_001f: Stack underflow*/)._values;
                ((Array)/*Error near IL_006d: Stack underflow*/).CopyTo((Array)/*Error near IL_006d: Stack underflow*/, 0);
                goto IL_0024;
            }
            goto IL_0035;
        IL_0040:
            if (6 == 0)
            {
                goto IL_0024;
            }
            int num = num + 1;
            goto IL_0047;
        IL_0024:
            num = 0;
            goto IL_0047;
        IL_0035:
            if (2u != 0 && 3u != 0)
            {
                ((object[])/*Error near IL_0040: Stack underflow*/)[num] = null;
                goto IL_0040;
            }
            goto IL_0047;
        IL_0047:
            if (num < array2.Length)
            {
                if (((object[])/*Error near IL_002e: Stack underflow*/)[/*Error near IL_002e: Stack underflow*/] is ILocaleValue)
                {
                    goto IL_0035;
                }
                goto IL_0040;
            }
            return new ArrayStorage(array2);
        }
    }

    [Serializable]
    internal struct DictionaryDataStorage : IDataStorage
    {
        private ConcurrentDictionary<DynamicProperty, object> _values;

        private DictionaryDataStorage(ConcurrentDictionary<DynamicProperty, object> values)
        {
            _values = values;
        }

        public DictionaryDataStorage(DynamicObjectType dt)
        {
            _values = new ConcurrentDictionary<DynamicProperty, object>();
        }

        public object GetLocalValue(DynamicProperty property)
        {
            //IL_0012: Incompatible stack heights: 0 vs 1
            //IL_0015: Incompatible stack heights: 0 vs 2
            //IL_001c: Incompatible stack heights: 0 vs 1
            //IL_001f: Incompatible stack heights: 0 vs 1
            ConcurrentDictionary<DynamicProperty, object> value2 = ((DictionaryDataStorage)/*Error near IL_0007: Stack underflow*/)._values;
            object value;
            ((ConcurrentDictionary<DynamicProperty, object>)/*Error near IL_001c: Stack underflow*/).TryGetValue((DynamicProperty)/*Error near IL_001c: Stack underflow*/, out value);
            return (object)/*Error near IL_0011: Stack underflow*/;
        }

        public void SetLocalValue(DynamicProperty property, object value)
        {
            //IL_000f: Incompatible stack heights: 0 vs 1
            //IL_0012: Incompatible stack heights: 0 vs 2
            //IL_0015: Incompatible stack heights: 0 vs 1
            ConcurrentDictionary<DynamicProperty, object> value2 = ((DictionaryDataStorage)/*Error near IL_0007: Stack underflow*/)._values;
            ((ConcurrentDictionary<DynamicProperty, object>)/*Error near IL_001c: Stack underflow*/)[(DynamicProperty)/*Error near IL_001c: Stack underflow*/] = (object)/*Error near IL_001c: Stack underflow*/;
        }

        public IDataStorage MemberClone()
        {
            //IL_006f: Incompatible stack heights: 0 vs 1
            //IL_0076: Incompatible stack heights: 0 vs 1
            //IL_0080: Incompatible stack heights: 0 vs 1
            //IL_009d: Incompatible stack heights: 0 vs 1
            //IL_00a9: Incompatible stack heights: 0 vs 1
            //IL_00b3: Incompatible stack heights: 0 vs 1
            ConcurrentDictionary<DynamicProperty, object> concurrentDictionary;
            if (0 == 0)
            {
                new ConcurrentDictionary<DynamicProperty, object>();
                concurrentDictionary = (ConcurrentDictionary<DynamicProperty, object>)/*Error near IL_00a3: Stack underflow*/;
                ((DictionaryDataStorage)/*Error near IL_001a: Stack underflow*/)._values.GetEnumerator();
                using (IEnumerator<KeyValuePair<DynamicProperty, object>> enumerator = /*Error near IL_00b9: Stack underflow*/)
                {
                    while (enumerator.MoveNext())
                    {
                        KeyValuePair<DynamicProperty, object> current = ((IEnumerator<KeyValuePair<DynamicProperty, object>>)/*Error near IL_0076: Stack underflow*/).Current;
                        KeyValuePair<DynamicProperty, object> keyValuePair = (KeyValuePair<DynamicProperty, object>)/*Error near IL_0079: Stack underflow*/;
                        if (7 == 0)
                        {
                            continue;
                        }
                        if (0 == 0)
                        {
                            object value = keyValuePair.Value;
                            if (!(/*Error near IL_0037: Stack underflow*/ is ILocaleValue))
                            {
                                concurrentDictionary[keyValuePair.Key] = keyValuePair.Value;
                                continue;
                            }
                        }
                        if (5u != 0)
                        {
                            concurrentDictionary[keyValuePair.Key] = null;
                        }
                    }
                }
            }
            return new DictionaryDataStorage(concurrentDictionary);
        }
    }

    private readonly DynamicObjectType _dt;

    private IDataStorage _dataStorage;

    [NonSerialized]
    internal static GetString \u001f;

	[DebuggerHidden]
    public DynamicObjectType DynamicObjectType
    {
        get
        {
            //IL_0009: Incompatible stack heights: 0 vs 1
            return ((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0007: Stack underflow*/)._dt;
        }
    }

    protected internal IDataStorage DataStorage
    {
        get
        {
            //IL_0009: Incompatible stack heights: 0 vs 1
            return ((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0007: Stack underflow*/)._dataStorage;
        }
        internal set
        {
            //IL_000b: Incompatible stack heights: 0 vs 1
            //IL_000e: Incompatible stack heights: 0 vs 1
            ((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0009: Stack underflow*/)._dataStorage = (IDataStorage)/*Error near IL_0009: Stack underflow*/;
        }
    }

    public object this[string propertyName]
    {
        get
        {
            //IL_0064: Incompatible stack heights: 0 vs 1
            //IL_006e: Incompatible stack heights: 0 vs 1
            //IL_0074: Incompatible stack heights: 0 vs 1
            //IL_007e: Incompatible stack heights: 0 vs 1
            //IL_0084: Incompatible stack heights: 0 vs 1
            //IL_008a: Incompatible stack heights: 0 vs 1
            //IL_0094: Incompatible stack heights: 0 vs 1
            //IL_009e: Incompatible stack heights: 0 vs 1
            //IL_00a8: Incompatible stack heights: 0 vs 1
            DynamicPropertyCollection property = ((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0007: Stack underflow*/)._dt.Properties;
            DynamicProperty value;
            ((KeyedCollectionBase<string, DynamicProperty>)/*Error near IL_007e: Stack underflow*/).TryGetValue((string)/*Error near IL_007e: Stack underflow*/, out value);
            if ((int)/*Error near IL_0011: Stack underflow*/ != 0)
            {
                ((DynamicProperty)/*Error near IL_0094: Stack underflow*/).GetValueFast((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0094: Stack underflow*/);
                return (object)/*Error near IL_0018: Stack underflow*/;
            }
			\u001f(107396354);
			\u001f(107390104);
            throw new ORMDesignException(message: string.Format(ResManager.LoadKDString(resourceID: \u001f(107390555), systemType: SubSystemType.SL, args: new object[0], description: (string)/*Error near IL_004c: Stack underflow*/), _dt.Name, propertyName), code: (string)/*Error near IL_0062: Stack underflow*/);
        }
        set
        {
            //IL_0081: Incompatible stack heights: 0 vs 1
            //IL_008b: Incompatible stack heights: 0 vs 1
            //IL_0091: Incompatible stack heights: 0 vs 1
            //IL_009b: Incompatible stack heights: 0 vs 1
            //IL_00a1: Incompatible stack heights: 0 vs 1
            //IL_00a7: Incompatible stack heights: 0 vs 1
            //IL_00ad: Incompatible stack heights: 0 vs 1
            //IL_00c1: Incompatible stack heights: 0 vs 1
            DynamicPropertyCollection property = ((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0007: Stack underflow*/)._dt.Properties;
            DynamicProperty value2;
            ((KeyedCollectionBase<string, DynamicProperty>)/*Error near IL_009b: Stack underflow*/).TryGetValue((string)/*Error near IL_009b: Stack underflow*/, out value2);
            if ((int)/*Error near IL_001a: Stack underflow*/ != 0 || 2 == 0)
            {
                ((DynamicProperty)/*Error near IL_00b7: Stack underflow*/).SetValue((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_00b7: Stack underflow*/, (object)/*Error near IL_00b7: Stack underflow*/);
                return;
            }
			\u001f(107396354);
            throw new ORMDesignException(message: string.Format(ResManager.LoadKDString(\u001f(107390104), \u001f(107390555), SubSystemType.SL), _dt.Name, propertyName), code: (string)/*Error near IL_007f: Stack underflow*/);
        }
    }

    public object this[int index]
    {
        get
        {
            //IL_0017: Incompatible stack heights: 0 vs 1
            //IL_001e: Incompatible stack heights: 0 vs 1
            //IL_0021: Incompatible stack heights: 0 vs 1
            //IL_0028: Incompatible stack heights: 0 vs 1
            //IL_002e: Incompatible stack heights: 0 vs 1
            //IL_0031: Incompatible stack heights: 0 vs 1
            //IL_0038: Incompatible stack heights: 0 vs 1
            DynamicPropertyCollection property = ((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0007: Stack underflow*/)._dt.Properties;
            DynamicProperty dynamicProperty2 = ((ReadOnlyCollection<DynamicProperty>)/*Error near IL_0028: Stack underflow*/)[(int)/*Error near IL_0028: Stack underflow*/];
            DynamicProperty dynamicProperty = (DynamicProperty)/*Error near IL_002b: Stack underflow*/;
            ((DynamicProperty)/*Error near IL_0038: Stack underflow*/).GetValueFast((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0038: Stack underflow*/);
            return (object)/*Error near IL_0016: Stack underflow*/;
        }
        set
        {
            //IL_0025: Incompatible stack heights: 0 vs 1
            //IL_002c: Incompatible stack heights: 0 vs 1
            //IL_002f: Incompatible stack heights: 0 vs 1
            //IL_0036: Incompatible stack heights: 0 vs 1
            //IL_003c: Incompatible stack heights: 0 vs 1
            //IL_003f: Incompatible stack heights: 0 vs 1
            //IL_0042: Incompatible stack heights: 0 vs 1
            while (true)
            {
                if (true && 0 == 0)
                {
                    DynamicPropertyCollection property = ((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_000d: Stack underflow*/)._dt.Properties;
                    DynamicProperty dynamicProperty2 = ((ReadOnlyCollection<DynamicProperty>)/*Error near IL_0036: Stack underflow*/)[(int)/*Error near IL_0036: Stack underflow*/];
                    DynamicProperty dynamicProperty = (DynamicProperty)/*Error near IL_0039: Stack underflow*/;
                }
                while (true)
                {
                    ((DynamicProperty)/*Error near IL_0049: Stack underflow*/).SetValue((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0049: Stack underflow*/, (object)/*Error near IL_0049: Stack underflow*/);
                    if (false)
                    {
                        break;
                    }
                    if (false)
                    {
                        continue;
                    }
                    return;
                }
            }
        }
    }

    public object this[DynamicProperty property]
    {
        get
        {
            //IL_001e: Incompatible stack heights: 0 vs 1
            //IL_0025: Incompatible stack heights: 0 vs 1
            //IL_002c: Incompatible stack heights: 0 vs 1
            //IL_002f: Incompatible stack heights: 0 vs 1
            //IL_0032: Incompatible stack heights: 0 vs 1
            //IL_0039: Incompatible stack heights: 0 vs 1
            if (1 == 0 || (int)/*Error near IL_0007: Stack underflow*/ == 0)
            {
				\u001f(107390534);
                new ArgumentNullException((string)/*Error near IL_002c: Stack underflow*/);
                throw /*Error near IL_0016: Stack underflow*/;
            }
            ((DynamicProperty)/*Error near IL_0039: Stack underflow*/).GetValueFast((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0039: Stack underflow*/);
            return (object)/*Error near IL_001d: Stack underflow*/;
        }
        set
        {
            //IL_0023: Incompatible stack heights: 0 vs 1
            //IL_002a: Incompatible stack heights: 0 vs 1
            //IL_0031: Incompatible stack heights: 0 vs 1
            //IL_0034: Incompatible stack heights: 0 vs 1
            //IL_0037: Incompatible stack heights: 0 vs 1
            //IL_003a: Incompatible stack heights: 0 vs 1
            while (true)
            {
                if ((int)/*Error near IL_0004: Stack underflow*/ != 0)
                {
                    if (false)
                    {
                        continue;
                    }
                    ((DynamicProperty)/*Error near IL_0041: Stack underflow*/).SetValue((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0041: Stack underflow*/, (object)/*Error near IL_0041: Stack underflow*/);
                    if (0 == 0)
                    {
                        break;
                    }
                }
				\u001f(107390534);
                new ArgumentNullException((string)/*Error near IL_0031: Stack underflow*/);
                throw /*Error near IL_0013: Stack underflow*/;
            }
        }
    }

    public DynamicObject(DynamicObjectType dt)
    {
        if (dt == null)
        {
            throw new ORMArgInvalidException(\u001f(107396354), ResManager.LoadKDString(\u001f(107390998), \u001f(107390873), SubSystemType.SL));
        }
        if (dt.Flag == DataEntityTypeFlag.Abstract)
        {
            throw new ORMArgInvalidException(\u001f(107396354), string.Format(ResManager.LoadKDString(\u001f(107390852), \u001f(107390243), SubSystemType.SL), dt.Name));
        }
        if (dt.Flag == DataEntityTypeFlag.Interface)
        {
            throw new ORMArgInvalidException(\u001f(107396354), string.Format(ResManager.LoadKDString(\u001f(107390254), \u001f(107390157), SubSystemType.SL), dt.Name));
        }
        _dt = dt;
        _dataStorage = CreateDataStorage();
    }

    public override IDataEntityType GetDataEntityType()
    {
        //IL_0009: Incompatible stack heights: 0 vs 1
        return ((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0007: Stack underflow*/)._dt;
    }

    protected virtual IDataStorage CreateDataStorage()
    {
        //IL_0010: Invalid comparison between Unknown and I4
        //IL_0031: Incompatible stack heights: 0 vs 1
        //IL_0038: Incompatible stack heights: 0 vs 1
        //IL_003f: Incompatible stack heights: 0 vs 1
        //IL_0042: Incompatible stack heights: 0 vs 1
        //IL_0049: Incompatible stack heights: 0 vs 1
        //IL_004c: Incompatible stack heights: 0 vs 1
        //IL_0053: Incompatible stack heights: 0 vs 1
        DynamicPropertyCollection property = ((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0007: Stack underflow*/)._dt.Properties;
        int count = ((ReadOnlyCollection<DynamicProperty>)/*Error near IL_003f: Stack underflow*/).Count;
        if ((int)/*Error near IL_0012: Stack underflow*/ > 200)
        {
            new DictionaryDataStorage(((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0019: Stack underflow*/)._dt);
            return (DictionaryDataStorage)/*Error near IL_0020: Stack underflow*/;
        }
        new ArrayStorage(((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0028: Stack underflow*/)._dt);
        return (ArrayStorage)/*Error near IL_002f: Stack underflow*/;
    }

    public unsafe bool TryGetValue(string propertyName, out object value)
    {
        //IL_0029: Incompatible stack heights: 0 vs 1
        //IL_002c: Incompatible stack heights: 0 vs 1
        //IL_0033: Incompatible stack heights: 0 vs 1
        //IL_0036: Incompatible stack heights: 0 vs 1
        //IL_003d: Incompatible stack heights: 0 vs 1
        //IL_0040: Incompatible stack heights: 0 vs 1
        //IL_0043: Incompatible stack heights: 0 vs 1
        //IL_0046: Incompatible stack heights: 0 vs 1
        //IL_004d: Incompatible stack heights: 0 vs 1
        *(@null*)(IntPtr)/*Error near IL_0004: Stack underflow*/ = null;
        while (true)
        {
            if (2u != 0)
            {
                DynamicPropertyCollection property = ((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_000e: Stack underflow*/)._dt.Properties;
                DynamicProperty value2;
                ((KeyedCollectionBase<string, DynamicProperty>)/*Error near IL_003d: Stack underflow*/).TryGetValue((string)/*Error near IL_003d: Stack underflow*/, out value2);
                if ((int)/*Error near IL_0018: Stack underflow*/ != 0)
                {
                    if (0 == 0)
                    {
                        break;
                    }
                    continue;
                }
            }
            return false;
        }
        ((DynamicProperty)/*Error near IL_004d: Stack underflow*/).GetValueFast((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_004d: Stack underflow*/);
        *(? *)(IntPtr)/*Error near IL_0026: Stack underflow*/ = /*Error near IL_0026: Stack underflow*/;
        return true;
    }

    private unsafe bool TryGetMember(GetMemberBinder binder, out object result)
    {
        //IL_0030: Incompatible stack heights: 0 vs 1
        //IL_0037: Incompatible stack heights: 0 vs 1
        //IL_003a: Incompatible stack heights: 0 vs 1
        //IL_0041: Incompatible stack heights: 0 vs 1
        //IL_0048: Incompatible stack heights: 0 vs 1
        //IL_004b: Incompatible stack heights: 0 vs 1
        //IL_004e: Incompatible stack heights: 0 vs 1
        //IL_0051: Incompatible stack heights: 0 vs 1
        //IL_0058: Incompatible stack heights: 0 vs 1
        int num;
        if (0 == 0)
        {
            if (0 == 0)
            {
                DynamicPropertyCollection property = ((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_000d: Stack underflow*/)._dt.Properties;
                string name = ((GetMemberBinder)/*Error near IL_0041: Stack underflow*/).Name;
                DynamicProperty value;
                ((KeyedCollectionBase<string, DynamicProperty>)/*Error near IL_0048: Stack underflow*/).TryGetValue((string)/*Error near IL_0048: Stack underflow*/, out value);
                if ((int)/*Error near IL_0019: Stack underflow*/ == 0)
                {
                    goto IL_0027;
                }
            }
            ((DynamicProperty)/*Error near IL_0058: Stack underflow*/).GetValueFast((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0058: Stack underflow*/);
            *(? *)(IntPtr)/*Error near IL_0022: Stack underflow*/ = /*Error near IL_0022: Stack underflow*/;
            if (2u != 0)
            {
                num = 1;
                goto IL_0026;
            }
            goto IL_0027;
        }
        goto IL_002a;
    IL_0026:
        return (byte)num != 0;
    IL_002a:
        num = 0;
        if (num == 0)
        {
            return (byte)num != 0;
        }
        goto IL_0026;
    IL_0027:
        result = null;
        goto IL_002a;
    }

    private bool TrySetMember(SetMemberBinder binder, object dataEntity)
    {
        //IL_002c: Incompatible stack heights: 0 vs 1
        //IL_0033: Incompatible stack heights: 0 vs 1
        //IL_0036: Incompatible stack heights: 0 vs 1
        //IL_003d: Incompatible stack heights: 0 vs 1
        //IL_0044: Incompatible stack heights: 0 vs 1
        //IL_0047: Incompatible stack heights: 0 vs 1
        //IL_004a: Incompatible stack heights: 0 vs 1
        //IL_004d: Incompatible stack heights: 0 vs 1
        int num;
        while (true)
        {
            DynamicPropertyCollection property = ((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0007: Stack underflow*/)._dt.Properties;
            string name = ((SetMemberBinder)/*Error near IL_003d: Stack underflow*/).Name;
            DynamicProperty value;
            ((KeyedCollectionBase<string, DynamicProperty>)/*Error near IL_0044: Stack underflow*/).TryGetValue((string)/*Error near IL_0044: Stack underflow*/, out value);
            if ((int)/*Error near IL_0013: Stack underflow*/ != 0 && 3u != 0)
            {
                if (false)
                {
                    continue;
                }
                ((DynamicProperty)/*Error near IL_0054: Stack underflow*/).SetValueFast((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0054: Stack underflow*/, (object)/*Error near IL_0054: Stack underflow*/);
                if (false)
                {
                    continue;
                }
                num = 1;
            }
            else
            {
                num = 0;
                if (num == 0)
                {
                    break;
                }
            }
            return (byte)num != 0;
        }
        return (byte)num != 0;
    }

    DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression parameter)
    {
        //IL_0008: Incompatible stack heights: 0 vs 1
        //IL_000b: Incompatible stack heights: 0 vs 1
        //IL_0012: Incompatible stack heights: 0 vs 1
        new MetaDynamic((Expression)/*Error near IL_0012: Stack underflow*/, (Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0012: Stack underflow*/);
        return (DynamicMetaObject)/*Error near IL_0007: Stack underflow*/;
    }

    AttributeCollection ICustomTypeDescriptor.GetAttributes()
    {
        //IL_000d: Incompatible stack heights: 0 vs 1
        //IL_0014: Incompatible stack heights: 0 vs 1
        //IL_001b: Incompatible stack heights: 0 vs 1
        ICustomTypeDescriptor customTypeDescriptor = ((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0007: Stack underflow*/)._dt.CustomTypeDescriptor;
        ((ICustomTypeDescriptor)/*Error near IL_001b: Stack underflow*/).GetAttributes();
        return (AttributeCollection)/*Error near IL_000c: Stack underflow*/;
    }

    string ICustomTypeDescriptor.GetClassName()
    {
        //IL_000d: Incompatible stack heights: 0 vs 1
        //IL_0014: Incompatible stack heights: 0 vs 1
        //IL_001b: Incompatible stack heights: 0 vs 1
        ICustomTypeDescriptor customTypeDescriptor = ((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0007: Stack underflow*/)._dt.CustomTypeDescriptor;
        ((ICustomTypeDescriptor)/*Error near IL_001b: Stack underflow*/).GetClassName();
        return (string)/*Error near IL_000c: Stack underflow*/;
    }

    string ICustomTypeDescriptor.GetComponentName()
    {
        //IL_000d: Incompatible stack heights: 0 vs 1
        //IL_0014: Incompatible stack heights: 0 vs 1
        //IL_001b: Incompatible stack heights: 0 vs 1
        ICustomTypeDescriptor customTypeDescriptor = ((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0007: Stack underflow*/)._dt.CustomTypeDescriptor;
        ((ICustomTypeDescriptor)/*Error near IL_001b: Stack underflow*/).GetComponentName();
        return (string)/*Error near IL_000c: Stack underflow*/;
    }

    TypeConverter ICustomTypeDescriptor.GetConverter()
    {
        //IL_000d: Incompatible stack heights: 0 vs 1
        //IL_0014: Incompatible stack heights: 0 vs 1
        //IL_001b: Incompatible stack heights: 0 vs 1
        ICustomTypeDescriptor customTypeDescriptor = ((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0007: Stack underflow*/)._dt.CustomTypeDescriptor;
        ((ICustomTypeDescriptor)/*Error near IL_001b: Stack underflow*/).GetConverter();
        return (TypeConverter)/*Error near IL_000c: Stack underflow*/;
    }

    EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
    {
        //IL_000d: Incompatible stack heights: 0 vs 1
        //IL_0014: Incompatible stack heights: 0 vs 1
        //IL_001b: Incompatible stack heights: 0 vs 1
        ICustomTypeDescriptor customTypeDescriptor = ((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0007: Stack underflow*/)._dt.CustomTypeDescriptor;
        ((ICustomTypeDescriptor)/*Error near IL_001b: Stack underflow*/).GetDefaultEvent();
        return (EventDescriptor)/*Error near IL_000c: Stack underflow*/;
    }

    PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
    {
        //IL_000d: Incompatible stack heights: 0 vs 1
        //IL_0014: Incompatible stack heights: 0 vs 1
        //IL_001b: Incompatible stack heights: 0 vs 1
        ICustomTypeDescriptor customTypeDescriptor = ((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0007: Stack underflow*/)._dt.CustomTypeDescriptor;
        ((ICustomTypeDescriptor)/*Error near IL_001b: Stack underflow*/).GetDefaultProperty();
        return (PropertyDescriptor)/*Error near IL_000c: Stack underflow*/;
    }

    object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
    {
        //IL_000f: Incompatible stack heights: 0 vs 1
        //IL_0016: Incompatible stack heights: 0 vs 1
        //IL_0019: Incompatible stack heights: 0 vs 1
        //IL_0020: Incompatible stack heights: 0 vs 1
        ICustomTypeDescriptor customTypeDescriptor = ((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0007: Stack underflow*/)._dt.CustomTypeDescriptor;
        ((ICustomTypeDescriptor)/*Error near IL_0020: Stack underflow*/).GetEditor((Type)/*Error near IL_0020: Stack underflow*/);
        return (object)/*Error near IL_000e: Stack underflow*/;
    }

    EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
    {
        //IL_000f: Incompatible stack heights: 0 vs 1
        //IL_0016: Incompatible stack heights: 0 vs 1
        //IL_0019: Incompatible stack heights: 0 vs 1
        //IL_0020: Incompatible stack heights: 0 vs 1
        ICustomTypeDescriptor customTypeDescriptor = ((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0007: Stack underflow*/)._dt.CustomTypeDescriptor;
        ((ICustomTypeDescriptor)/*Error near IL_0020: Stack underflow*/).GetEvents((Attribute[])/*Error near IL_0020: Stack underflow*/);
        return (EventDescriptorCollection)/*Error near IL_000e: Stack underflow*/;
    }

    EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
    {
        //IL_000d: Incompatible stack heights: 0 vs 1
        //IL_0014: Incompatible stack heights: 0 vs 1
        //IL_001b: Incompatible stack heights: 0 vs 1
        ICustomTypeDescriptor customTypeDescriptor = ((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0007: Stack underflow*/)._dt.CustomTypeDescriptor;
        ((ICustomTypeDescriptor)/*Error near IL_001b: Stack underflow*/).GetEvents();
        return (EventDescriptorCollection)/*Error near IL_000c: Stack underflow*/;
    }

    PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
    {
        //IL_000f: Incompatible stack heights: 0 vs 1
        //IL_0016: Incompatible stack heights: 0 vs 1
        //IL_0019: Incompatible stack heights: 0 vs 1
        //IL_0020: Incompatible stack heights: 0 vs 1
        ICustomTypeDescriptor customTypeDescriptor = ((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0007: Stack underflow*/)._dt.CustomTypeDescriptor;
        ((ICustomTypeDescriptor)/*Error near IL_0020: Stack underflow*/).GetProperties((Attribute[])/*Error near IL_0020: Stack underflow*/);
        return (PropertyDescriptorCollection)/*Error near IL_000e: Stack underflow*/;
    }

    PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
    {
        //IL_000d: Incompatible stack heights: 0 vs 1
        //IL_0014: Incompatible stack heights: 0 vs 1
        //IL_001b: Incompatible stack heights: 0 vs 1
        ICustomTypeDescriptor customTypeDescriptor = ((Kingdee.BOS.Orm.DataEntity.DynamicObject)/*Error near IL_0007: Stack underflow*/)._dt.CustomTypeDescriptor;
        ((ICustomTypeDescriptor)/*Error near IL_001b: Stack underflow*/).GetProperties();
        return (PropertyDescriptorCollection)/*Error near IL_000c: Stack underflow*/;
    }

    object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
    {
        //IL_0004: Incompatible stack heights: 0 vs 1
        return (object)/*Error near IL_0003: Stack underflow*/;
    }

    static DynamicObject()
    {
        //IL_000f: Incompatible stack heights: 0 vs 1
        Type typeFromHandle = typeof(Kingdee.BOS.Orm.DataEntity.DynamicObject);
        Strings.CreateGetStringDelegate((Type)/*Error near IL_0016: Stack underflow*/);
    }
}
