﻿using Library.Data.Managers;

namespace Library.Data
{
    /// <summary>
    /// DI-Aware Abstract Factory.
    /// Fetches Each Data Manager On-Demand (aka Lazy Loading).
    /// Keeps The Calling Object Very Light.
    /// </summary>
    public interface IDataRepositoryFactory
    {
         T Create<T>() where T : IDataRepository;
    }
}
