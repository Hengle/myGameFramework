//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
 
using Framework; 
using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
 
public class ItemTableDB : BaseTableDB<ItemTable>
{
    public static readonly ItemTableDB instance=new ItemTableDB();
}
 
// Generated from: ItemTable.csv
public class ItemTable : TableData
{

     public int ItemId;
     public string ItemName;
     public string ItemDes;
 
    public override int Key
    {
        get
        {
            return ItemId; 
        }
    }
 
    public override void Decode(byte[] byteArr, ref int bytePos)
    {

         ReadInt32(ref byteArr,ref bytePos,out ItemId);
         ReadString(ref byteArr,ref bytePos,out ItemName);
         ReadString(ref byteArr,ref bytePos,out ItemDes);
 
    }
 
}
