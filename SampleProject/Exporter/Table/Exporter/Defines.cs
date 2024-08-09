using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Table.Exporter
{
    class Defines
    {
        public enum KeyType
        {
            None = 0,
            Single = 1,
            Multi = 2,
        }

        public enum Usage
        {
            None = 0,
            Client = 1,
            Server = 2,
            Both = 3,
        }

        public class Element
        {
            public string mFieldName = "";
            public string mDataType = "";
            public string mValueType = "";
            public string mDataTypeDef_C = "";
            public string mDataTypeDef_C_Unity = "";
            public string mDataTypeDef_S = "";
            public string mTypeSize = "";
            public long mValueMax = 0;
            public KeyType mKeyType = KeyType.None;
            public Usage mUsage = Usage.None;
            public bool mArray = false;

            public string mValue = ""; // Config 테이블 용도

            public int mIndexInData = -1;

            public void SetDataTypeDefinition()
            {
                if (mDataType == "enum") { mDataTypeDef_C = mArray ? string.Format("TArray<{0}>", mValueType) : string.Format("{0}", mValueType); }
                else if (mDataType == "struct") { mDataTypeDef_C = mArray ? string.Format("TArray<{0}>", mValueType) : string.Format("{0}", mValueType); }
                else if (mDataType == "string") { mDataTypeDef_C = mArray ? "TArray<FString>" : "FString"; }
                else if (mDataType == "name") { mDataTypeDef_C = "FName"; }
                else if (mDataType == "json2")
                {
                    mDataTypeDef_C = string.Format("TMap<{0}, uint32>", mValueType);
                }
                else { mDataTypeDef_C = mArray ? string.Format("TArray<{0}>", mDataType) : mDataType; }


                if (mDataType == "enum") { mDataTypeDef_C_Unity = mArray ? string.Format("{0}[]", mValueType) : string.Format("{0}", mValueType); }
                else if (mDataType == "struct") { mDataTypeDef_C_Unity = mArray ? string.Format("{0}[]", mValueType) : string.Format("{0}", mValueType); }
                else if (mDataType == "string") { mDataTypeDef_C_Unity = mArray ? "string[]" : "string"; }
                else if (mDataType == "vector2") { mDataTypeDef_C_Unity = mArray ? "Vector2[]" : "Vector2"; }
                else if (mDataType == "vector3") { mDataTypeDef_C_Unity = mArray ? "Vector3[]" : "Vector3"; }
                else if (mDataType == "color") { mDataTypeDef_C_Unity = mArray ? "Color[]" : "Color"; }
                else { mDataTypeDef_C_Unity = mArray ? string.Format("{0}[]", Util.ConvertTypeUnity(mDataType)) : Util.ConvertTypeUnity(mDataType); }


                if (mDataType == "enum") { mDataTypeDef_S = mArray ? string.Format("std::vector<{0}>", mValueType) : string.Format("{0}", mValueType); }
                else if (mDataType == "struct") { mDataTypeDef_S = mArray ? string.Format("std::vector<{0}>", mValueType) : string.Format("{0}", mValueType); }
                else if (mDataType == "string") { mDataTypeDef_S = mArray ? "std::vector<std::wstring>" : "std::wstring"; }
                else if (mDataType == "vector2") { mDataTypeDef_S = mArray ? "std::vector<Vector2>" : "Vector2"; }
                else if (mDataType == "vector3") { mDataTypeDef_S = mArray ? "std::vector<Vector3>" : "Vector3"; }
                else if (mDataType == "color") { mDataTypeDef_S = mArray ? "std::vector<Color>" : "Color"; }
                else if (mDataType == "name") { mDataTypeDef_S = "std::wstring"; }
                else if (mDataType == "json2")
                {
                    mDataTypeDef_S = string.Format("std::map<{0}, uint32>", mValueType);
                }
                else { mDataTypeDef_S = mArray ? string.Format("std::vector<{0}>", mDataType) : mDataType; }

                mTypeSize = GetSize(mDataType, mValueType);
            }

            static public string GetSize(string data_type, string value_type)
            {
                switch (data_type)
                {
                    case "int8": case "uint8": case "bool": return "1";
                    case "int16": case "uint16": return "2";
                    case "int32": case "uint32": case "float": return "4";
                    case "int64": case "uint64": return "8";
                    case "vector2": return "8";
                    case "vector3": return "12";
                    case "color": return "16";
                    case "enum": return string.Format("sizeof({0})", value_type);
                    case "struct": return string.Format("sizeof({0})", value_type);
                }
                return string.Format("sizeof({0})", value_type);
            }
        }


        public string mTableName = "";
        public Element mMultiKeyElement = null;
        public Element mSingleKeyElement = null;
        public List<Element> mElements = new List<Element>();


        public void AddElement(Element elem)
        {
            if (mElements.Find(x => x.mFieldName.CompareTo(elem.mFieldName) == 0) != null)
            {
                Log.E("Define FieldName is duplicate : {0}", elem.mFieldName);
                return;
            }

            elem.SetDataTypeDefinition();

            mElements.Add(elem);

            SetKeyTypeElement(elem);
        }

        public void SetElementIndex(string elem_name, int idx)
        {
            Element elem = mElements.Find(x => x.mFieldName.CompareTo(elem_name) == 0);
            if (elem != null)
            {
                elem.mIndexInData = idx;
            }
        }

        private void SetKeyTypeElement(Element elem)
        {
            if (elem.mKeyType == KeyType.None)
            {
                return;
            }

            if (elem.mArray == true || elem.mDataType == "struct" || elem.mDataType == "json2")
            {
                // error
                return;
            }

            if (elem.mKeyType == KeyType.Multi)
            {
                if (mMultiKeyElement != null)
                {
                    // error
                }
                else
                {
                    mMultiKeyElement = elem;
                }
            }

            if (elem.mKeyType == KeyType.Single)
            {
                if (mSingleKeyElement != null)
                {
                    // error
                }
                else
                {
                    mSingleKeyElement = elem;
                }
            }
        }
    }
}
