//
// StructsAndEnums.cs
//
// Hand-maintained enums for the Salesforce Marketing Cloud MobilePush
// framework (MarketingCloudSDK) 11.0.2, sourced from
// MarketingCloudSDK+Constants.h. Ported from the 8.1.3-era binding by header
// diff; never regenerated with Objective Sharpie.
//
// Dropped at v11: AuthEventType (its declaring header, SFMCEvent.h, was
// removed from the framework).
//
using ObjCRuntime;

namespace MarketingCloudSDK
{
	// typedef NS_ENUM(NSUInteger, configureError)
	[Native]
	public enum configureError : ulong
	{
		firstConfigureErrorIndex = 0,
		configureNoError = firstConfigureErrorIndex,
		configureInvalidAppIDError,
		configureInvalidAccessTokenError,
		configureUnableToReadRandomError,
		configureDatabaseAccessError,
		configureUnableToKeyDatabaseError,
		configureCCKeyDerivationPBKDFError,
		configureCCSymmetricKeyWrapError,
		configureCCSymmetricKeyUnwrapError,
		configureKeyChainError,
		configureUnableToReadCertificateError,
		configureRunOnceSimultaneouslyError,
		configureRunOnceError,
		configureInvalidLocationAndProximityError,
		configureSimulatorBlobError,
		configureKeyChainInvalidError,
		configureIncorrectConfigurationCallingSequenceError,
		configureMissingConfigurationFileError,
		configureInvalidConfigurationFileError,
		configureInvalidConfigurationIndexError,
		configureFailedFrameworkCreationError,
		configureInvalidAppEndpointError,
		lastConfigureErrorIndex = configureInvalidAppEndpointError
	}
}
