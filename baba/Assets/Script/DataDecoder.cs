/*
https://github.com/konbraphat51/UnityPythonConnectionModules
Author: Konbraphat51
License: Boost Software License (BSL1.0)
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PythonConnection
{
    public class DataDecoder : MonoBehaviour
    {
        /// <summary>
        /// Dictionary of data type name to actual C# class type reference
        /// </summary>
        private Dictionary<Type, UnityEvent<DataClass>> correspondingEvents;

        /// <summary>
        /// Convert from data_type name to actual C# class type reference
        /// </summary>
        /// <returns>Dictionary of data type names to C# types</returns>
        protected Dictionary<string, Type> DataToType()
        {
            // ここに既存の DataClass をマッピング
            return new Dictionary<string, Type>()
            {
                {"ExampleType", typeof(DataClass)} // データタイプ名とC#の型をマッピング
            };
        }

        // Constructor
        public DataDecoder()
        {
            // Get pre-defined data type
            Dictionary<string, Type> dataToType = DataToType();

            // Initialize `correspondingEvents`
            PrepareEvents(dataToType.Values.ToArray());
        }

        public void DecodeAndReport(string dataTypeName, string dataJson)
        {
            // Get data type
            Type dataType = DataToType()[dataTypeName];

            // Convert json to data class
            DataClass data = JsonUtility.FromJson(dataJson, dataType) as DataClass;

            // Report data
            correspondingEvents[dataType].Invoke(data);
        }

        /// <summary>
        /// Register new callback when data received
        /// </summary>
        /// <param name="dataType">Type of the data class</param>
        /// <param name="callback">Callback when data received</param>
        public void RegisterAction(Type dataType, UnityAction<DataClass> callback)
        {
            correspondingEvents[dataType].AddListener(callback);
        }

        /// <summary>
        /// Unregister callback when data received
        /// </summary>
        /// <param name="dataType">Type of the data class</param>
        /// <param name="callback">Callback when data received</param>
        public void RemoveAction(Type dataType, UnityAction<DataClass> callback)
        {
            correspondingEvents[dataType].RemoveListener(callback);
        }

        private void PrepareEvents(Type[] types)
        {
            // Prepare events
            correspondingEvents = new Dictionary<Type, UnityEvent<DataClass>>();
            foreach (Type type in types)
            {
                correspondingEvents.Add(type, new UnityEvent<DataClass>());
            }
        }
    }
}
