param app_config_name string
param app_insights_name string
param container_app_name string
param container_app_image_name string
param container_app_image_tag string
param cpu string
param database_connection_string string
param environment_name string
param environment_resource_group_name string
param latest_revision_no string
param max_replicas int
param memory string
param min_replicas int
param service_type string
param reservation_svc_target_branch string = ''
param ppl_svc_target_branch string = ''
param broken_svc_target_branch string = ''

var revisionNo = replace(container_app_image_tag, '.', '')

var ingressSection object | null = service_type == 'api'
? {
  external: true
  targetPort: 8080
  exposedPort: 0
  transport: 'Auto'
  traffic: [
    {
      weight: 100
      latestRevision: true
    }
  ]
  allowInsecure: false
  clientCertificateMode: 'Ignore'
  stickySessions: {
    affinity: 'none'
  }
} : null

@secure()
param container_app_environment_id string
@secure()
param ghcr_username string
@secure()
param ghcr_token string

resource appInsights 'Microsoft.Insights/components@2020-02-02' existing = {
  name: app_insights_name
}

resource appConfig 'Microsoft.AppConfiguration/configurationStores@2024-06-01' existing = {
  name: app_config_name
  scope: resourceGroup(environment_resource_group_name)
}

var newRevisionSuffixNo = startsWith(latest_revision_no, 'v${revisionNo}')
  ? contains(latest_revision_no, '-')
    ? format('{0}-{1}', revisionNo, int(split(latest_revision_no, '-')[1]) + 1)
    : '${revisionNo}-0'
  : revisionNo

resource container_app 'Microsoft.App/containerapps@2026-01-01' = {
  name: container_app_name
  location: 'UK South'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    environmentId: container_app_environment_id
    configuration: {
      secrets: [
        {
          name: 'ghcr-token'
          value: ghcr_token
        }
        {
          name: 'app-insights-endpoint'
          value: appInsights.properties.ConnectionString
        }
        {
          name: 'app-config-endpoint'
          value: appConfig.properties.endpoint
        }
      ]
      activeRevisionsMode: 'Single'
      ingress: ingressSection
      registries: [
        {
          server: 'ghcr.io'
          username: ghcr_username
          passwordSecretRef: 'ghcr-token'
        }
      ]
      maxInactiveRevisions: 100
      identitySettings: []
    }
    template: {
      containers: [
        {
          image: 'ghcr.io/sming-code/${container_app_image_name}:${container_app_image_tag}'
          name: container_app_name
          resources: {
            cpu: json(cpu)
            memory: memory
          }
          env: [
            {
              name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
              secretRef: 'app-insights-endpoint'
            }
            {
              name: 'App_Config_Endpoint'
              secretRef: 'app-config-endpoint'
            }
            {
              name: 'Database__ConnectionString'
              value: database_connection_string
            }
            {
              name: 'Tag_Environment'
              value: environment_name
            }
            {
              name: 'Service_Name'
              value: 'Customer Service ${toUpper(substring(service_type, 0, 1))}${substring(service_type, 1)}'
            }
            {
              name: 'reservation_svc_target_branch'
              value: reservation_svc_target_branch
            }
            {
              name: 'ppl_svc_target_branch'
              value: ppl_svc_target_branch
            }
            {
              name: 'broken_svc_target_branch'
              value: broken_svc_target_branch
            }
          ]
        }
      ]
      revisionSuffix: 'v${newRevisionSuffixNo}'
      scale: {
        minReplicas: min_replicas
        maxReplicas: max_replicas
        cooldownPeriod: 300
        pollingInterval: 30
      }
    }
  }
}
