﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Core.Entities.Base
{
    public interface IEntityBase<TId>
    {
        //TId Id { get; set; }
        TId Id { get;}
    }
}
