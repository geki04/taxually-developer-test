﻿using Taxually.TechnicalTest.Contracts;

namespace Taxually.TechnicalTest.Interfaces
{
    /// <summary>
    /// Ideally these interfaces should have been placed in a dedicated Interfaces project
    /// </summary>
    public interface IVatRegistrationValidator
    {
        bool Validate(VatRegistrationRequest request);
    }
}
