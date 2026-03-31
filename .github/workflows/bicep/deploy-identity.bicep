param environment string
param appResourceGroup string
param name string

resource appUserAssignedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2024-11-30' = {
  name: name
  location: resourceGroup(appResourceGroup).location
}

resource environmentKeyvault 'Microsoft.KeyVault/vaults@2025-05-01' existing = {
  name: 'kv-sming-${environment}-001'
}

resource keyvaultPolicy 'Microsoft.KeyVault/vaults/accessPolicies@2025-05-01' = {
  properties: {
    accessPolicies: [
      {
        objectId: appUserAssignedIdentity.properties.principalId
        permissions: {
          secrets: [
            'get'
            'list'
          ]
        }
        tenantId: tenant().tenantId
      }
    ]
  }
  parent: environmentKeyvault
  name: 'add'
}
