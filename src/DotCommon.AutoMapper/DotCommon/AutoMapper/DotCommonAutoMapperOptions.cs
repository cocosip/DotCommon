using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DotCommon.Collections;

namespace DotCommon.AutoMapper
{
    /// <summary>
    /// AutoMapper configuration options class for managing AutoMapper configurations and validations
    /// </summary>
    public class DotCommonAutoMapperOptions
    {
        /// <summary>
        /// List of AutoMapper configurators, storing a collection of configuration action delegates
        /// </summary>
        public List<Action<IDotCommonAutoMapperConfigurationContext>> Configurators { get; }

        /// <summary>
        /// List of Profile types that need validation
        /// </summary>
        public ITypeList<Profile> ValidatingProfiles { get; set; }

        /// <summary>
        /// Initializes a new instance of the DotCommonAutoMapperOptions class
        /// </summary>
        public DotCommonAutoMapperOptions()
        {
            Configurators = [];
            ValidatingProfiles = new TypeList<Profile>();
        }

        /// <summary>
        /// Adds all mapping configurations from the assembly containing the specified module
        /// </summary>
        /// <typeparam name="TModule">Module type used to determine which assembly to scan</typeparam>
        /// <param name="validate">Whether to validate all Profiles in the assembly</param>
        public void AddMaps<TModule>(bool validate = false)
        {
            var assembly = typeof(TModule).Assembly;

            Configurators.Add(context =>
            {
                context.MapperConfiguration.AddMaps(assembly);
            });

            if (validate)
            {
                var profileTypes = assembly
                    .DefinedTypes
                    .Where(type => typeof(Profile).IsAssignableFrom(type) && !type.IsAbstract && !type.IsGenericType);

                foreach (var profileType in profileTypes)
                {
                    ValidatingProfiles.Add(profileType);
                }
            }
        }

        /// <summary>
        /// Adds the specified Profile configuration
        /// </summary>
        /// <typeparam name="TProfile">The Profile type to add</typeparam>
        /// <param name="validate">Whether to validate this Profile</param>
        public void AddProfile<TProfile>(bool validate = false)
            where TProfile : Profile, new()
        {
            Configurators.Add(context =>
            {
                context.MapperConfiguration.AddProfile<TProfile>();
            });

            if (validate)
            {
                ValidateProfile(typeof(TProfile));
            }
        }

        /// <summary>
        /// Sets whether to validate the specified Profile type
        /// </summary>
        /// <typeparam name="TProfile">The Profile type to set validation for</typeparam>
        /// <param name="validate">True to add to validation list, false to remove from validation list</param>
        public void ValidateProfile<TProfile>(bool validate = true)
            where TProfile : Profile
        {
            ValidateProfile(typeof(TProfile), validate);
        }

        /// <summary>
        /// Sets whether to validate the specified Profile type
        /// </summary>
        /// <param name="profileType">The Profile type to set validation for</param>
        /// <param name="validate">True to add to validation list, false to remove from validation list</param>
        public void ValidateProfile(Type profileType, bool validate = true)
        {
            if (validate)
            {
                ValidatingProfiles.AddIfNotContains(profileType);
            }
            else
            {
                ValidatingProfiles.Remove(profileType);
            }
        }
    }
}
