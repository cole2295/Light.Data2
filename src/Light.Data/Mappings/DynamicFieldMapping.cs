﻿using System;
using System.Reflection;

namespace Light.Data
{
    abstract class DynamicFieldMapping : FieldMapping
    {
        public static DynamicFieldMapping CreateDynmaicFieldMapping(PropertyInfo property, DynamicCustomMapping mapping)
        {
            DynamicFieldMapping fieldMapping;
            Type type = property.PropertyType;
            TypeInfo typeInfo = type.GetTypeInfo();
            string fieldName = property.Name;
            if (typeInfo.IsGenericType) {
                Type frameType = type.GetGenericTypeDefinition();
                if (frameType.FullName == "System.Nullable`1") {
                    Type[] arguments = typeInfo.GetGenericArguments();
                    type = arguments[0];
                    typeInfo = type.GetTypeInfo();
                }
            }

            if (type.IsArray) {
                throw new LightDataException(string.Format(SR.DataMappingUnsupportFieldType, property.DeclaringType, property.Name, type));
            }
            else if (type.IsGenericParameter || typeInfo.IsGenericTypeDefinition) {
                throw new LightDataException(string.Format(SR.DataMappingUnsupportFieldType, property.DeclaringType, property.Name, type));
            }
            else if (typeInfo.IsEnum) {
                DynamicEnumFieldMapping enumFieldMapping = new DynamicEnumFieldMapping(type, fieldName, mapping);
                fieldMapping = enumFieldMapping;
            }
            else {
                TypeCode code = Type.GetTypeCode(type);
                if (code == TypeCode.Empty) {
                    throw new LightDataException(string.Format(SR.DataMappingUnsupportFieldType, property.DeclaringType, property.Name, type));
                }
                else if (code == TypeCode.Object) {
                    DynamicObjectFieldMapping objectFieldMapping = new DynamicObjectFieldMapping(type, fieldName, mapping);
                    fieldMapping = objectFieldMapping;
                }
                else {
                    DynamicPrimitiveFieldMapping primitiveFieldMapping = new DynamicPrimitiveFieldMapping(type, fieldName, mapping);
                    fieldMapping = primitiveFieldMapping;
                }
            }
            return fieldMapping;
        }

        protected DynamicFieldMapping(Type type, string fieldName, DynamicCustomMapping mapping, bool isNullable)
            : base(type, fieldName, fieldName, mapping, isNullable, null)
        {

        }
    }
}

