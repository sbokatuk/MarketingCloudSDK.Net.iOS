// AppGroupSDK is Salesforce-internal plumbing (app-group-internal-sdk): the shared-storage layer
// MobilePush v11 loads at runtime. Nothing in it is meant to be called from application code -
// the repository name says internal, and the MobilePush integration docs never mention its
// types - so this package ships the xcframework without projecting any of its API.
//
// The file exists because a binding project with no ObjcBindingApiDefinition does not build; the
// package tests assert this assembly stays EMPTY of public AppGroupSDK types, so a surface
// cannot appear here unreviewed.
