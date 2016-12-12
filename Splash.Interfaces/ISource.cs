﻿using Splash.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splash.Interfaces
{
    public interface ISource
    {
        /// <summary>
        /// Emits, getting handled by registered handlers, as well as downstream sources.
        /// 
        /// Notes: returned result will not capture changes made by downstream nodes.
        /// </summary>
        /// <typeparam name="TEventData">event data type</typeparam>
        /// <param name="eventData">event data</param>
        /// <returns>output processed event data after processors have been applied to this source.</returns>
        TEventData Emit<TEventData>(TEventData eventData, ResultMode resultMode = ResultMode.OriginOnlyResult)
            where TEventData : class, ICloneable;

        /// <summary>
        /// Registers an event processor on the stream.
        /// The processor will be run after existing event processors.
        /// </summary>
        /// <typeparam name="TEventData"></typeparam>
        /// <param name="eventProcessor"></param>
        void Register<TEventData>(EventProcessor<TEventData> eventProcessor)
            where TEventData : ICloneable;

        /// <summary>
        /// Links the source to a downstream source.
        /// </summary>
        /// <param name="source">downstream source</param>
        void FlowInto(ISourceNode source);
    }
}
