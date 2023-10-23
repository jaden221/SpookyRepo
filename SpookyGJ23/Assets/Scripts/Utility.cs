using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    /// <summary>
    /// Finds First empty spot in array and adds value to it
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="arrayOfThings"></param>
    /// <param name="thingToAdd"></param>
    public static void ArrayAdd<T>(ref T[] arrayOfThings, T thingToAdd)
    {
        for (int i = 0; i < arrayOfThings.Length; i++)
        {
            if (arrayOfThings[i] == null)
            {
                arrayOfThings[i] = thingToAdd;
                break;
            }
        }
    }

    /// <summary>
    /// Returns -1 is there are no spots left
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="arrayOfThings"></param>
    /// <param name="thingToAdd"></param>
    /// <returns></returns>
    public static int ArrayAddReturnIndex<T>(ref T[] arrayOfThings, T thingToAdd)
    {
        for (int i = 0; i < arrayOfThings.Length; i++)
        {
            if (arrayOfThings[i] == null)
            {
                arrayOfThings[i] = thingToAdd;
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Returns -1 is there are no spots left
    /// </summary>
    /// <param name="arrayOfThings"></param>
    /// <param name="thingToAdd"></param>
    /// <returns></returns>
    public static int ArrayAddReturnIndex(ref float[] arrayOfThings, float thingToAdd)
    {
        for (int i = 0; i < arrayOfThings.Length; i++)
        {
            if (arrayOfThings[i] == 0)
            {
                arrayOfThings[i] = thingToAdd;
                return i;
            }
        }

        return -1;
    }

    public static bool ArrayContains<T>(T item, ref T[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (EqualityComparer<T>.Default.Equals(items[i], item)) return true;
        }
        return false;
    }

    public static void ArrayClear<T>(ref T[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = default(T);
        }
    }

    public static bool IsArrayEmpty<T>(ref T[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = default(T);
            if (items[i] != null)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Returns first empty spot in array, returns -1 if no empty spots
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static int ReturnFirstEmptyIndex<T>(ref T[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null) return i;
        }
        return -1;
    }

    /// <summary>
    /// If there is no child with the tag, it will return the parent transform as default, so check if result isn't itself
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="_tag"></param>
    /// <returns></returns>
    public static Transform GetChildWithTag(Transform parent, string _tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag)
            {
                return child;
            }
        }
        Debug.Log(parent.name + " Could Not Find Child With Tag: " + _tag + ", Returning Parent Obj");
        return parent;
    }
}