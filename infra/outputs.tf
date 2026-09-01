output "resource_group_name" {
    value = azurerm_resource_group.rg.name
}

output "acr_name" {
    value = azurerm_container_registry.acr.name
}

output "aca_environment_name" {
    value = azurerm_container_app_environment.aca_env.name
}

output "acr_login_server" {
    value = azurerm_container_registry.acr.login_server
}