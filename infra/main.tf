resource "azurerm_resource_group" "rg" {
    name = var.resource_group_name
    location = var.location
}

resource "azurerm_log_analytics_workspace" "law" {
    name = "${{var.aca_environment_name}}-law"
    location = azurerm_resource_group.rg.location
    resource_group_name = azurerm_resource_group.rg.name
    sku = "PerGB2018"
    retention_in_days = 30
}

resource "azurerm_container_registry" "acr" {
    name = var.acr_name
    resource_group_name = azurerm_resource_group.rg.name
    location = azurerm_resource_group.rg.location
    sku = "Basic"
    admin_enabled = true
}

resource "azurerm_container_app_environment" "aca_env" {
    name = var.aca_environment_name
    location = azurerm_resource_group.rg.location
    resource_group_name = azurerm_resource_group.rg.name
    log_analytics_workspace_id = azurerm_log_analytics_workspace.law.id
    log_analytics_workspace_client_id = azurerm_log_analytics_workspace.law.workspace_id
    log_analytics_workspace_client_secret = azurerm_log_analytics_workspace.law.primary_shared_key
}