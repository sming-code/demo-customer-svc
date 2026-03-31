param appName string
param location string
param environment string

@secure()
param registryPasswordSecretUrl string

resource environmentKeyVault 'Microsoft.KeyVault/vaults@2025-05-01' existing = {
  name: 'kv-sming-$(environment)-001'
}

resource uai 'Microsoft.ManagedIdentity/userAssignedIdentities@2024-11-30' existing = {
  
}

resource containerApp 'Microsoft.App/containerApps@2025-07-01' = {
  name: appName
  location: location
  identity: {
    type: 'SystemAssigned,UserAssigned'
    userAssignedIdentities: {
      '{uaiId}': {}
    }
  }
}

output resourceGroupId string = newRG.id
