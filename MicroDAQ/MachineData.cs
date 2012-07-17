﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MicroDAQ
{
    class MachineData : JonLibrary.OPC.Machine
    {
        public List<DataItem> Items = null;
        public Dictionary<int, DataItem> ItemPair = null;
        public MachineData(string name, string[] dataHead, string[] data)
            : base()
        {
            this.Name = Name;
            ItemCtrl = dataHead;
            ItemStatus = data;
            int count = (dataHead.Length < data.Length) ? (dataHead.Length) : (data.Length);
            Items = new List<DataItem>();
            ItemPair = new Dictionary<int, DataItem>();
            for (int i = 0; i < count; i++)
                Items.Add(new DataItem());
        }



        internal protected override bool Connect(string OPCServerIP)
        {
            bool success = true;
            success &= PLC.Connect("OPC.SimaticNET", OPCServerIP);
            success &= PLC.AddGroup(GROUP_NAME_CTRL, 1, 0);
            success &= PLC.AddItems(GROUP_NAME_CTRL, ItemCtrl);
            success &= PLC.AddGroup(GROUP_NAME_STATE, 1, 0);
            success &= PLC.AddItems(GROUP_NAME_STATE, ItemStatus);
            PLC.SetState(GROUP_NAME_CTRL, true);
            PLC.SetState(GROUP_NAME_STATE, true);
            ConnectionState = (success) ? (ConnectionState.Open) : (ConnectionState.Closed);
            return success;
        }


        protected override void PLC_DataChange(string groupName, int[] item, object[] value, short[] Qualities)
        {
            base.PLC_DataChange(groupName, item, value, Qualities);
            switch (groupName)
            {
                case GROUP_NAME_CTRL:

                    for (int i = 0; i < item.Length; i++)
                    {
                        ushort[] val = null;
                        if (value[i] != null)
                        {
                            val = (ushort[])value[i];
                            this.Items[item[i]].ID = val[0];
                            this.Items[item[i]].Type = (DataType)val[1];
                            this.Items[item[i]].State = (DataState)val[2];
                            UpdateItemPair(this.Items[item[i]].ID, this.Items[item[i]]);
                        }
                    }
                    break;
                case GROUP_NAME_STATE:
                    for (int i = 0; i < item.Length; i++)
                    {
                        if (value[i] != null)
                        {
                            this.Items[item[i]].Value = (float)value[i];
                        }
                    }
                    break;
            }
            OnStatusChannge();
        }

        private void UpdateItemPair(int key, DataItem item)
        {
            if (!ItemPair.ContainsKey(key))
            { ItemPair.Add(key, item); }
            ItemPair[key] = item;
        }
    }
}
