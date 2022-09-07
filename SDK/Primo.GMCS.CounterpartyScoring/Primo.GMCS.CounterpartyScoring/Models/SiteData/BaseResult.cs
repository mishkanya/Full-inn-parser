﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primo.GMCS.CounterpartyScoring.Models.SiteData
{
    [Serializable]
    public abstract class BaseResult
    {
        public bool IsFind;
        public bool HasError;
        public DateTime CheckTime { get; private set; }
        public BaseResult()
        {
            CheckTime = DateTime.Now;
        }
    }
}