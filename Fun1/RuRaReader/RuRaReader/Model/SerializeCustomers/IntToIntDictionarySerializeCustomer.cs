using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SuperJson;
using SuperJson.Objects;

namespace RuRaReader.Model.SerializeCustomers
{
    public class IntToIntDictionarySerializeCustomer : SerializeCustomer
    {
        public override bool UseCustomer(object obj, Type declaredType)
        {
            return obj.GetType() == typeof (Dictionary<int, int>);
        }

        public override SuperToken Serialize(object obj, Type declaredType, SuperJsonSerializer serializer)
        {
            var typed = (Dictionary<int, int>) obj;
            var result = new SuperObject();

            result.TypedValue["$type"] = new SuperString("IntToIntDictionary");
            foreach (var key in typed.Keys)
            {
                result.TypedValue[key.ToString()] = new SuperNumber(typed[key]);
            }

            return result;
        }
    }

    public class IntToIntDictionaryDeserializeCustomer : DeserializeCustomer
    {
        public override bool UseCustomer(SuperToken obj, Type declaredType)
        {
            return obj is SuperObject && ((SuperObject)obj).TypedValue.ContainsKey("$type") && 
                (((SuperObject) obj).TypedValue["$type"] as SuperString)?.TypedValue == "IntToIntDictionary";
        }

        public override object Deserialize(SuperToken obj, SuperJsonSerializer serializer)
        {
            var result = new Dictionary<int, int>();
            var objObj = (SuperObject) obj;
            foreach (var key in objObj.TypedValue.Keys)
            {
                if (key.StartsWith("$"))
                    continue;

                result.Add(int.Parse(key), (int)((SuperNumber) objObj.TypedValue[key]).TypedValue);
            }

            return result;
        }
    }
}