﻿/**
 * 文件名：EthernetViewModel.cs
 * 说明：以太网实体模型
 * 作者：林安城
 * 更改记录： 
 * -------------------------------------------------------
 * 改动人 	时间 			原因
 * -------------------------------------------------------
 * 林安城 	2013-09-20		创建文件
 * -------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigEditor.Core.ViewModels
{
    /// <summary>
    /// 以太网实体模型
    /// </summary>
    public class EthernetViewModel
    {
        public EthernetViewModel()
        {
        }

        /// <summary>
        /// 设备列表
        /// </summary>
        public List<DeviceViewModel> Devices { get; set; }
    }
}