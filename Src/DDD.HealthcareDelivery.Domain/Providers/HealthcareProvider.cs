﻿using System.Collections.Generic;
using Conditions;

namespace DDD.HealthcareDelivery.Domain.Providers
{
    using Common.Domain;
    using Core.Domain;

    public abstract class HealthcareProvider 
        : ValueObject, IStateObjectConvertible<HealthcareProviderState>
    {

        #region Constructors

        protected HealthcareProvider(int identifier,
                                     FullName fullName,
                                     PractitionerLicenseNumber licenseNumber,
                                     SocialSecurityNumber socialSecurityNumber = null,
                                     ContactInformation contactInformation = null,
                                     string speciality = null,
                                     string displayName = null)
        {
            Condition.Requires(identifier, nameof(identifier)).IsGreaterThan(0);
            Condition.Requires(fullName, nameof(fullName)).IsNotNull();
            Condition.Requires(licenseNumber, nameof(licenseNumber)).IsNotNull();
            this.Identifier = identifier;
            this.FullName = fullName;
            this.LicenseNumber = licenseNumber;
            this.SocialSecurityNumber = socialSecurityNumber;
            this.ContactInformation = contactInformation;
            if (!string.IsNullOrWhiteSpace(speciality))
                this.Speciality = speciality;
            this.DisplayName = string.IsNullOrWhiteSpace(displayName) ? fullName.AsFormattedName() : displayName;
        }

        #endregion Constructors

        #region Properties

        public ContactInformation ContactInformation { get; }

        public FullName FullName { get; }

        public string DisplayName { get; }

        public int Identifier { get; }

        public PractitionerLicenseNumber LicenseNumber { get; }

        public SocialSecurityNumber SocialSecurityNumber { get; }

        public string Speciality { get; }

        #endregion Properties

        #region Methods

        public override IEnumerable<object> EqualityComponents()
        {
            yield return this.Identifier;
            yield return this.FullName;
            yield return this.LicenseNumber;
            yield return this.SocialSecurityNumber;
            yield return this.ContactInformation;
            yield return this.Speciality;
            yield return this.DisplayName;
        }

        public virtual HealthcareProviderState ToState()
        {
            return new HealthcareProviderState
            {
                Identifier = this.Identifier,
                FullName = this.FullName.ToState(),
                LicenseNumber = this.LicenseNumber.Number, 
                SocialSecurityNumber = this.SocialSecurityNumber?.Number,
                ContactInformation = this.ContactInformation == null ? 
                                     new ContactInformationState() // EF6 complex types cannot be null
                                     : this.ContactInformation.ToState(), 
                Speciality = this.Speciality,
                DisplayName = this.DisplayName
            };
        }

        public override string ToString()
        {
            var format = "{0} [identifier={1}, fullName={2}, licenseNumber={3}, socialSecurityNumber={4}, contactInformation={5}, speciality={6}, displayName={7}]";
            return string.Format(format, this.GetType().Name, this.Identifier, this.FullName, this.LicenseNumber, this.SocialSecurityNumber, this.ContactInformation, this.Speciality, this.DisplayName);
        }

        #endregion Methods

    }
}